using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class ScrollDragAndDropMiniGame : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private List<CanvasGroup> groupsToTurnOn;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private List<Sprite> spritesToAnimateWhenOpen;
    [SerializeField] private List<Sprite> spritesToAnimateWhenClose;
    [SerializeField] private Image mainSpriteForBackground;
    [SerializeField] private CanvasGroup japanMapImage;
    [SerializeField] private List<LetterSlot> letterSlots;
    [SerializeField] private CanvasGroup objectsCanvasGroup;

    private event UnityAction _eventToPopUpWhenFInish;
    private bool isFinishedMiniGame = false;
    private RoadMapPresenter _roadMapPresenter;

    [Inject]
    public void Construct(RoadMapPresenter roadMapPresenter)
    {
        _roadMapPresenter = roadMapPresenter;
    }
    public async Task Initialize(UnityAction eventForMiniGameFinish, Func<Task> functionToFireWhenOpened)
    {
        foreach (var group in groupsToTurnOn)
        {
            group.alpha = 0f;
            group.gameObject.SetActive(true);
        }

        objectsCanvasGroup.alpha = 0f;
        japanMapImage.alpha = 0f;
        japanMapImage.gameObject.SetActive(false);
        _eventToPopUpWhenFInish = eventForMiniGameFinish;
        gameObject.SetActive(true);
        exitButton.gameObject.SetActive(false);
        exitButton.onClick.AddListener(() => _ = WaitForScrollToCloseAndFinish());

        await PlayAnimationOfScroll(spritesToAnimateWhenOpen,1f);

        await MiniGameAppearance(1f);

        if (functionToFireWhenOpened != null)
            await functionToFireWhenOpened?.Invoke();
    }

    private void Update()
    {
        if (letterSlots.All(s => s.IsCorrect) && !isFinishedMiniGame)
            FinishMiniGameAndShowMap();
    }

    public async void FinishMiniGameAndShowMap()
    {
        RevealWordUnlocked();
        japanMapImage.gameObject.SetActive(true);
        isFinishedMiniGame = true;
        await MiniGameAppearance(0f);

        foreach (var group in groupsToTurnOn) group.gameObject.SetActive(false);

        var seq = DOTween.Sequence();
        seq.Append(japanMapImage.DOFade(1f, fadeDuration));
        await seq.AsyncWaitForCompletion();
        exitButton.gameObject.SetActive(true);
    }

    private void RevealWordUnlocked()
    {
        string word = "";
        foreach (var symbol in letterSlots)
            word += symbol.ExpectedSymbol;

        _roadMapPresenter.RevealWord(word);
    }

    private async Task MiniGameAppearance(float alphaValue)
    {
        var seq = DOTween.Sequence();

        for (int i = 0; i < groupsToTurnOn.Count; i++)
        {
            seq.Append(groupsToTurnOn[i].DOFade(alphaValue, fadeDuration));
        }
        await seq.AsyncWaitForCompletion();
        seq.Kill();
    }

    private async Task PlayAnimationOfScroll(List<Sprite> spritesToAnimate, float fadeAlphaValue)
    {
        if (spritesToAnimate == null || spritesToAnimate.Count == 0) return;

        var seq = DOTween.Sequence();
        seq.Append(objectsCanvasGroup.DOFade(fadeAlphaValue, 1f));
        await seq.AsyncWaitForCompletion();
        seq.Kill();

        foreach (var frame in spritesToAnimate)
        {
            mainSpriteForBackground.sprite = frame;
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
        }

    }

    private async Task WaitForScrollToCloseAndFinish()
    {
        japanMapImage.gameObject.SetActive(false);
        await PlayAnimationOfScroll(spritesToAnimateWhenClose, 0f);
        gameObject.SetActive(false);
        _eventToPopUpWhenFInish.Invoke();

    }
}
