using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.Android.Gradle;

public class BookView : MonoBehaviour
{
    [SerializeField] RectTransform bookObject;
    [SerializeField] RectTransform miniBookObject;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] SymbolPageView leftPage;
    [SerializeField] SymbolPageView rightPage;
    [SerializeField] Button nextPageButton;
    [SerializeField] Button previousPageButton;
    [SerializeField] Transform gridWithSymbols;
    [SerializeField] private Image bookImage;
    [SerializeField] private List<Sprite> openBookSprites;
    [SerializeField] private List<Sprite> closeBookSprites;
    [SerializeField] private List<Sprite> nextPageSprites;
    [SerializeField] private List<Sprite> previousPageSprites;
    [SerializeField] private float spriteChangeDuration = 0.1f;

    public void ShowMiniBookButton() => miniBookObject.gameObject.SetActive(true);
    public void HideMiniBookButton() => miniBookObject.gameObject.SetActive(false);

    public event Action OnBookOpened;

    private bool isOpen = false;
    private bool _isAnimating = false;

    public SymbolPageView LeftPage => leftPage;
    public SymbolPageView RightPage => rightPage;

    private void Awake()
    {
        if (miniBookObject.TryGetComponent(out Button button))
            button.onClick.AddListener(OpenBook);
    }

    public void LinkPage(SymbolPageModel left, SymbolPageModel right)
    {
        leftPage.BindData(left);
        rightPage.BindData(right);
    }

    public void SetButtonActions(Func<Task> nextPressed, Func<Task> previousPressed)
    {
        nextPageButton.onClick.RemoveAllListeners();
        previousPageButton.onClick.RemoveAllListeners();

        nextPageButton.onClick.AddListener(async () => await nextPressed());
        previousPageButton.onClick.AddListener(async () => await previousPressed());
    }

    public void SetButtonsState(bool canNext, bool canPrev)
    {
        nextPageButton.interactable = canNext;
        previousPageButton.interactable = canPrev;
    }

    public void OpenBook()
    {
        if (isOpen) return;

        isOpen = true;
        miniBookObject.gameObject.SetActive(false);

        // ѕоказываем контейнер книги
        bookObject.gameObject.SetActive(true);
        Vector2 targetAnchoredPos = Vector2.zero; // центр Canvas
        Vector3 targetScale = Vector3.one;
        Vector2 miniAnchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, miniBookObject.position, null, out miniAnchoredPos);

        bookObject.anchoredPosition = miniAnchoredPos;
        bookObject.localScale = miniBookObject.localScale;

        // —тавим в позицию мини-книги и минимальный масштаб
        bookObject.position = miniBookObject.transform.position;
        bookObject.localScale = Vector3.one * 0.2f; // маленька€ книга

        // јнимаци€ увеличени€ и перемещени€ к центру
        bookObject.DOAnchorPos(targetAnchoredPos, 1f).SetEase(Ease.OutCubic);
        bookObject.DOScale(targetScale, 1f).SetEase(Ease.OutCubic)
            .OnComplete(async () =>
            {
                await PlayOpenBookAsync();
                ChangeUiActiveStatus(true);
                OnBookOpened?.Invoke();
                // тут можно запускать анимацию раскрыти€ книги
            });

    }

    public Task PlayNextPageAsync() =>
    PlayBookAnimation(nextPageSprites);

    public Task PlayPreviousPageAsync() =>
        PlayBookAnimation(previousPageSprites);

    public Task PlayOpenBookAsync() =>
        PlayBookAnimation(openBookSprites);

    public async Task PlayBookAnimation(List<Sprite> frames)
    {
        if (_isAnimating) return;

        ChangeUiActiveStatus(false);
        await PlayAnimationAsync(frames);
        ChangeUiActiveStatus(true);
    }

    public async Task PlayAnimationAsync(List<Sprite> frames)
    {

        if (frames == null || frames.Count == 0) return;

        _isAnimating = true;

        foreach (var frame in frames)
        {
            bookImage.sprite = frame;
            await Task.Delay(TimeSpan.FromSeconds(spriteChangeDuration));
        }

        _isAnimating = false;
    }

    public void ChangeUiActiveStatus(bool status)
    {
        leftPage.gameObject.SetActive(status);
        rightPage.gameObject.SetActive(status);
        nextPageButton.gameObject.SetActive(status);
        previousPageButton.gameObject.SetActive(status);
    }
}
