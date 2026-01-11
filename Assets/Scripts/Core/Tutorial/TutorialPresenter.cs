using System;
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
        await _narrator.Play(_levelDataSet.TutorialInitialText);

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

    private async Task ProcessStep(TutorialStepData step)
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

        // 1️⃣ Ждём интеракции, если есть
        if (interactable != null && tutorialObject != null)
        {
            tutorialObject.EnableInteraction(true);
            await WaitForInteraction(interactable);
            tutorialObject.EnableInteraction(false);
        }

        if (step.isChangingLightning)
            await _lightHandler.OnTutorialStepChanged(step.stepId);
        // 2️⃣ Проигрываем анимацию
        if (tutorialObject != null)
        {
            var sprites = _levelDataSet.ReturnRequiredSprites(step.animationId);
            if (sprites != null)
                await tutorialObject.PlayAnimationAsync(sprites);
        }

        if (step.isStepEndingWithShake)
            _camera.PlayTutorialShake();

        // 3️⃣ Нарратор
        if (!string.IsNullOrEmpty(step.narratorText))
            await _narrator.Play(step.narratorText);


        // 4️⃣ Собираем символ, если можно
        if (step.canCollectSymbol && tutorialObject is ISymbolToCollect symbolObj)
            symbolObj.CollectSymbol();

        // 5️⃣ Delay
        if (step.delayBeforeNextStep > 0)
            await Task.Delay(TimeSpan.FromSeconds(step.delayBeforeNextStep));
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
