using System;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialPresenter
{
    private readonly LevelDataSet _levelDataSet;
    private readonly TutorialNarrator _narrator;
    private int _currentStepIndex;

    public TutorialPresenter(
        LevelDataSet dataSet,
        TutorialNarrator narrator)
    {
        _levelDataSet = dataSet;
        _narrator = narrator;
    }

    public async void StartTutorial()
    {
        await _narrator.Play("0_0");
        for (_currentStepIndex = 0; _currentStepIndex < _levelDataSet.StepData.Count; _currentStepIndex++)
        {
            var step = _levelDataSet.ReturnRequiredStep(_currentStepIndex);

            var go = step.interactableObject;
            go.SetActive(true);

            var interactable = go.GetComponent<IInteractable>();
            var tutorialObject = go.GetComponent<ITutorial>();

            if (interactable != null && tutorialObject != null)
            {
                tutorialObject.EnableInteraction(true);

                await WaitForInteraction(interactable);

                await PlayStepAnimation(step, tutorialObject);

                tutorialObject.EnableInteraction(false);
            }
            else if (tutorialObject != null)
            {
                await PlayStepAnimation(step, tutorialObject);

                if (step.canInteractAfterStep)
                    tutorialObject.EnableInteraction(true);
            }

            await _narrator.Play(step.narratorText);

            CollectSymbolIfExists(tutorialObject);

            if (step.delayBeforeNextStep > 0)
                await Task.Delay(TimeSpan.FromSeconds(step.delayBeforeNextStep));
        }
    }

    private async Task PlayStepAnimation(TutorialStepData step,ITutorial tutorialObject)
    {
        var sprites = _levelDataSet.ReturnRequiredSprites(step.animationId);
        if (sprites != null)
            await tutorialObject.PlayAnimationAsync(sprites);
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

    private void CollectSymbolIfExists(ITutorial tutorialObject)
    {
        if (tutorialObject is ISymbolToCollect symbolObj)
        {
            symbolObj.CollectSymbol();
        }
    }
}
