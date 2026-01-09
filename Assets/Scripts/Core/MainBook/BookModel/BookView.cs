using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookView : MonoBehaviour
{
    [Header("Book UI")]
    [SerializeField] private RectTransform bookObject;
    [SerializeField] private RectTransform miniBookObject;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private SymbolPageView leftPage;
    [SerializeField] private SymbolPageView rightPage;
    [SerializeField] private Button closeBookButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Image bookImage;

    [Header("Sprites")]
    [SerializeField] private List<Sprite> openBookSprites;
    [SerializeField] private List<Sprite> closeBookSprites;
    [SerializeField] private List<Sprite> nextPageSprites;
    [SerializeField] private List<Sprite> previousPageSprites;
    [SerializeField] private float spriteChangeDuration = 0.1f;

    [Header("Symbol Pop-up")]
    [SerializeField] private CanvasGroup symbolPopUp;
    [SerializeField] private Image symbolImage;
    [SerializeField] private float popUpMoveDuration = 1f;
    [SerializeField] private float popUpBounceDuration = 0.5f;
    [SerializeField] private int bounceLoops = 4;

    public void ShowMiniBookButton() => miniBookObject.gameObject.SetActive(true);
    public void HideMiniBookButton() => miniBookObject.gameObject.SetActive(false);

    private bool _isOpen = false;
    private bool _isAnimating = false;
    private Button _miniBookButton;

    public SymbolPageView LeftPage => leftPage;
    public SymbolPageView RightPage => rightPage;
    public bool IsOpen => _isOpen;

    public event Action OnBookOpened;
    public event Action OnBookClosed;

    private void Awake()
    {
        if (miniBookObject.TryGetComponent(out Button button))
        {
            _miniBookButton = button;
            _miniBookButton.onClick.AddListener(async () => await OpenBook());
        }

        closeBookButton.onClick.AddListener(async () => await CloseBook());
    }

    private async Task AnimateRect(RectTransform rect, Vector2 targetPos, Vector3 targetScale, float duration, Ease ease = Ease.OutCubic)
    {
        await DOTween.Sequence()
            .Join(rect.DOAnchorPos(targetPos, duration).SetEase(ease))
            .Join(rect.DOScale(targetScale, duration).SetEase(ease))
            .AsyncWaitForCompletion();
    }

    public async Task PlayUnlockSymbolAnimation(SymbolData symbolData)
    {
        var root = symbolPopUp.GetComponent<RectTransform>();
        var defaultScale = root.localScale;
        var centerPosition = root.anchoredPosition;
        float canvasHalfHeight = canvasRect.rect.height / 2f;
        float popupHalfHeight = root.rect.height / 2f;
        Vector2 _startPos = new Vector2(centerPosition.x,-canvasHalfHeight - popupHalfHeight);
        float bounceHeight = root.rect.height * 0.05f;

        symbolPopUp.alpha = 0f;
        root.localScale = Vector3.one * 0.9f;
        root.anchoredPosition = _startPos;

        symbolImage.sprite = symbolData.icon;
        symbolPopUp.gameObject.SetActive(true);

        var seq = DOTween.Sequence();

        // Fade сразу, но пусть длится так же, как движение, чтобы не было рывка
        seq.Insert(0f, symbolPopUp.DOFade(1f, popUpMoveDuration));

        // Одновременно двигаем и масштабируем
        seq.Insert(0f, root.DOAnchorPos(centerPosition, popUpMoveDuration).SetEase(Ease.OutCubic));
        seq.Insert(0f, root.DOScale(1f, popUpMoveDuration).SetEase(Ease.OutBack));

        seq.Append(root.DOAnchorPosY(centerPosition.y + bounceHeight, popUpBounceDuration)
        .SetEase(Ease.InOutSine)
        .SetLoops(bounceLoops, LoopType.Yoyo));

        Vector3 targetScale = miniBookObject.localScale;
        Vector2 miniAnchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, miniBookObject.position, null, out miniAnchoredPos);
        var moveTween = root.DOAnchorPos(miniAnchoredPos, 1f).SetEase(Ease.OutCubic);
        var scaleTween = root.DOScale(targetScale, 1f).SetEase(Ease.OutCubic);

        seq.Append(moveTween).Join(scaleTween);

        await seq.AsyncWaitForCompletion();

        symbolPopUp.gameObject.SetActive(false);
        root.anchoredPosition = centerPosition;
        root.localScale = defaultScale;
    }

    public async Task HighlightUnlockedSymbol(SymbolPageView pageView)
    {
        var rect = pageView.SymbolText.GetComponent<RectTransform>();

        // Сохраняем оригинальный масштаб
        Vector3 originalScale = rect.localScale;

        // Краткий "pop"
        var seq = DOTween.Sequence();
        seq.Append(rect.DOScale(originalScale * 1.1f, 1).SetEase(Ease.OutBack));
        var symbolText = pageView.SymbolText;
        if (symbolText != null)
        {
            Color originalColor = symbolText.color;
            seq.Join(symbolText.DOColor(Color.yellow, 1).SetLoops(2, LoopType.Yoyo));
        }

        seq.Append(rect.DOScale(originalScale, 1).SetEase(Ease.InBack));
        await seq.AsyncWaitForCompletion();
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

        nextPageButton.onClick.AddListener(() => _ = nextPressed());
        previousPageButton.onClick.AddListener(() => _ = previousPressed());
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

        Vector2 targetAnchoredPos = Vector2.zero; // центр Canvas
        Vector3 targetScale = Vector3.one;
        Vector2 miniAnchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, miniBookObject.position, null, out miniAnchoredPos);

        bookObject.anchoredPosition = miniAnchoredPos;

        // Ставим в позицию мини-книги и минимальный масштаб
        bookObject.position = miniBookObject.transform.position;
        bookObject.localScale = Vector3.one * 0.2f; // маленькая книга

        bookObject.gameObject.SetActive(true);
        await AnimateRect(bookObject, targetAnchoredPos, targetScale, 1f);

        // После завершения
        await PlayOpenBookAsync();
        ChangeUiActiveStatus(true);
        OnBookOpened?.Invoke();

    }

    public async Task CloseBook()
    {
        if (!_isOpen || _isAnimating) return;

        _isOpen = false;
        ChangeUiActiveStatus(false);
        await PlayBookAnimation(closeBookSprites, true);

        Vector3 targetScale = miniBookObject.localScale;
        Vector2 miniAnchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, miniBookObject.position, null, out miniAnchoredPos);
        await AnimateRect(bookObject, miniAnchoredPos, targetScale, 1f);

        bookObject.gameObject.SetActive(false);
        miniBookObject.gameObject.SetActive(true);
        OnBookClosed.Invoke();
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
