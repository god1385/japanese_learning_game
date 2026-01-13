using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialPresenter
{
    private readonly LevelDataSet _levelDataSet;
    private readonly TutorialNarrator _narrator;
    private readonly GameCameraUtilities _camera;
    private readonly LevelLightningHandler _lightHandler;
    private int _currentStepIndex;

    public TutorialPresenter(
        LevelDataSet dataSet,
        TutorialNarrator narrator,
        LevelLightningHandler lightHandler,
        GameCameraUtilities camera)
    {
        _camera = camera;
        _levelDataSet = dataSet;
        _narrator = narrator;
        _lightHandler = lightHandler;
    }

    public async Task StartTutorial()
    {
        _currentStepIndex = 0;

        // Начальный нарратор
        await _narrator.PlaySequence(_levelDataSet.TutorialInitialText);

        while (_currentStepIndex < _levelDataSet.StepData.Count)
        {
            var step = _levelDataSet.ReturnRequiredStep(_currentStepIndex);
            if (step == null)
            {
                _currentStepIndex++;
                continue;
            }

            await ProcessStep(step);
            _currentStepIndex++;
        }
    }

    private async Task ProcessStep(TutorialStep step)
    {
        var go = step.interactableObject;
        ITutorial tutorialObject = null;
        IInteractable interactable = null;

        if (go != null)
        {
            tutorialObject = go.GetComponent<ITutorial>();
            interactable = go.GetComponent<IInteractable>();
            go.SetActive(true);
        }

        if (step.actionsOrder != null)
        {
            foreach (var actionType in step.actionsOrder)
            {
                switch (actionType)
                {
                    case StepActionType.WaitForInteraction:
                        if (interactable != null && tutorialObject != null)
                        {
                            tutorialObject.EnableInteraction(true);
                            await WaitForInteraction(interactable);
                            tutorialObject.EnableInteraction(false);

                            if (interactable is ITutorialAwaitable awaitable)
                            {
                                if (step.narratorTextIfWaitForInteraction.Count > 0)
                                {
                                    awaitable.SetActionAfterInteraction(() => _narrator.PlaySequence(step.narratorTextIfWaitForInteraction));
                                }

                                await awaitable.WaitForCompletionAsync();
                            }
                        }
                        break;
                    case StepActionType.ChangeLightning:
                        if (step.isChangingLightning)
                            await _lightHandler.OnTutorialStepChanged(step.stepId);
                        break;

                    case StepActionType.NarratorText:
                        if (step.narratorText.Count > 0)
                            await _narrator.PlaySequence(step.narratorText);
                        break;

                    case StepActionType.CollectSymbol:
                        if (step.canCollectSymbol && tutorialObject is ISymbolToCollect symbolObj)
                            await symbolObj.CollectSymbol();
                        break;

                    case StepActionType.PlayAnimation:
                        if (tutorialObject != null)
                        {
                            var sprites = _levelDataSet.ReturnRequiredSprites(step.animationId);
                            if (sprites != null)
                                await tutorialObject.PlayAnimationAsync(sprites);
                        }
                        break;

                    case StepActionType.PlayShake:
                        if (step.isStepEndingWithShake)
                            _camera.PlayTutorialShake();
                        break;
                    case StepActionType.FinishTutorial:
                        {
                            await FadeUi(_levelDataSet.UiElementsToFadeInTheEnd);
                            if (step.narratorText.Count > 0)
                                await _narrator.PlaySequence(step.narratorText);
                            break;
                        }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        if (step.delayBeforeNextStep > 0)
            await Task.Delay(TimeSpan.FromSeconds(step.delayBeforeNextStep));
    }

    private async Task FadeUi(List<CanvasGroup> uiElements)
    {
        var seq = DOTween.Sequence();
        foreach (var uiElement in uiElements)
        {
            seq.Join(uiElement.DOFade(0, 1f));
        }

        await seq.AsyncWaitForCompletion();
    }

    private Task WaitForInteraction(IInteractable interactable)
    {
        var tcs = new TaskCompletionSource<bool>();

        void Handler()
        {
            interactable.OnInteracted -= Handler;
            tcs.SetResult(true);
        }

        interactable.OnInteracted += Handler;
        return tcs.Task;
    }
}
