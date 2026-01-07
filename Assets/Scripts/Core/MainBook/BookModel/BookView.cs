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
    [SerializeField] private RectTransform bookObject;
    [SerializeField] private RectTransform miniBookObject;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private SymbolPageView leftPage;
    [SerializeField] private SymbolPageView rightPage;
    [SerializeField] private Button closeBookButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Transform gridWithSymbols;
    [SerializeField] private Image bookImage;
    [SerializeField] private List<Sprite> openBookSprites;
    [SerializeField] private List<Sprite> closeBookSprites;
    [SerializeField] private List<Sprite> nextPageSprites;
    [SerializeField] private List<Sprite> previousPageSprites;
    [SerializeField] private float spriteChangeDuration = 0.1f;

    public void ShowMiniBookButton() => miniBookObject.gameObject.SetActive(true);
    public void HideMiniBookButton() => miniBookObject.gameObject.SetActive(false);

    public event Action OnBookOpened;
    public event Action OnBookClosed;

    private bool _isOpen = false;
    private bool _isAnimating = false;
    private Button _miniBookButton;

    public SymbolPageView LeftPage => leftPage;
    public SymbolPageView RightPage => rightPage;

    public bool IsOpen => _isOpen;

    private void Awake()
    {
        if (miniBookObject.TryGetComponent(out Button button))
        {
            _miniBookButton = button;
            _miniBookButton.onClick.AddListener(async () => await OpenBook());
        }

        closeBookButton.onClick.AddListener(CloseBook);
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

    public async Task OpenBook()
    {
        if (_isOpen) return;

        _isOpen = true;
        miniBookObject.gameObject.SetActive(false);

        // Показываем контейнер книги
        bookObject.gameObject.SetActive(true);
        Vector2 targetAnchoredPos = Vector2.zero; // центр Canvas
        Vector3 targetScale = Vector3.one;
        Vector2 miniAnchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, miniBookObject.position, null, out miniAnchoredPos);

        bookObject.anchoredPosition = miniAnchoredPos;

        // Ставим в позицию мини-книги и минимальный масштаб
        bookObject.position = miniBookObject.transform.position;
        bookObject.localScale = Vector3.one * 0.2f; // маленькая книга

        var moveTween = bookObject.DOAnchorPos(targetAnchoredPos, 1f).SetEase(Ease.OutCubic);
        var scaleTween = bookObject.DOScale(targetScale, 1f).SetEase(Ease.OutCubic);

        // Ожидаем завершения **любой** из анимаций
        await DOTween.Sequence()
            .Join(moveTween)
            .Join(scaleTween)
            .AsyncWaitForCompletion();

        // После завершения
        await PlayOpenBookAsync();
        ChangeUiActiveStatus(true);
        OnBookOpened?.Invoke();

    }

    public async void CloseBook()
    {
        if (!_isOpen || _isAnimating) return;

        _isOpen = false;
        ChangeUiActiveStatus(false);
        await PlayBookAnimation(closeBookSprites, true);

        Vector3 targetScale = miniBookObject.localScale;
        Vector2 miniAnchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, miniBookObject.position, null, out miniAnchoredPos);
        bookObject.DOAnchorPos(miniAnchoredPos, 1f).SetEase(Ease.OutCubic);
        bookObject.DOScale(targetScale, 1f).SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                bookObject.gameObject.SetActive(false);
                miniBookObject.gameObject.SetActive(true);
                OnBookClosed.Invoke();
            });
    }

    public Task PlayNextPageAsync() =>
    PlayBookAnimation(nextPageSprites, false);

    public Task PlayPreviousPageAsync() =>
        PlayBookAnimation(previousPageSprites, false);

    public Task PlayOpenBookAsync() =>
        PlayBookAnimation(openBookSprites, false);

    public async Task PlayBookAnimation(List<Sprite> frames, bool isCloseAnimation)
    {
        if (_isAnimating) return;

        ChangeUiActiveStatus(false);
        await PlayAnimationAsync(frames);

        if (!isCloseAnimation) ChangeUiActiveStatus(true);
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
        closeBookButton.gameObject.SetActive(status);
    }

    public void ActionDispose()
    {
        nextPageButton.onClick.RemoveAllListeners();
        previousPageButton.onClick.RemoveAllListeners();
        _miniBookButton.onClick.RemoveAllListeners();
        closeBookButton.onClick.RemoveAllListeners();
    }
}
