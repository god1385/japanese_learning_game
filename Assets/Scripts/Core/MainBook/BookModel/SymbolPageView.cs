using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Threading.Tasks;

[RequireComponent(typeof(Button))]
public class SymbolPageView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI symbolText;
    [SerializeField] private Button playSoundButton;
    [SerializeField] private RectTransform mainContent;
    [SerializeField] private RectTransform detailsPage;
    [SerializeField] private CanvasGroup detailsCanvasGroup;
    [SerializeField] private TextMeshProUGUI detailTextField;
    [SerializeField] private TextMeshProUGUI pronounciationText;
    [SerializeField] private Image iconImage;
    [SerializeField] private float collapseDuration = 0.3f;
    [SerializeField] private float fadeDuration = 0.25f;

    private Button openBookDetailsButton;
    private SymbolPageModel _pageModel;
    private bool isOpened = false;
    private bool isAnimating = false;
    private float defaultHeightOfConent;

    public Action<SymbolPageView> OpenBookDetailsButtonClicked;
    public event Action<SymbolPageView> OnPlaySoundClicked;
    public SymbolPageModel PageModel => _pageModel;

    private void Awake()
    {
        openBookDetailsButton = GetComponent<Button>();
        defaultHeightOfConent = mainContent.rect.height;
        playSoundButton.onClick.AddListener(() => { OnPlaySoundClicked.Invoke(this); });
    }

    public void BindData(SymbolPageModel model)
    {
        _pageModel = model;
        model.OnUnlocked += SetUnlockedState;
        mainContent.sizeDelta = new Vector2(mainContent.sizeDelta.x, defaultHeightOfConent);
        detailsPage.gameObject.SetActive(false);
        detailsCanvasGroup.alpha = 0;
        detailsCanvasGroup.interactable = false;
        detailsCanvasGroup.blocksRaycasts = false;
        isOpened = false;
        Refresh();
    }

    public async Task ShowDetails()
    {
        //if (_pageModel == null || !_pageModel.IsUnlocked) return;
        isOpened = true;
        isAnimating = true;

        float targetHeight = defaultHeightOfConent / 2f;

        await mainContent.DOSizeDelta(new Vector2(mainContent.sizeDelta.x, targetHeight),collapseDuration).SetEase(Ease.OutCubic).AsyncWaitForCompletion();

        detailsPage.gameObject.SetActive(true);
        detailsCanvasGroup.alpha = 0;
        detailsCanvasGroup.interactable = true;
        detailsCanvasGroup.blocksRaycasts = true;

        await detailsCanvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad).AsyncWaitForCompletion();

        isAnimating = false;
    }

    public async Task PageClicked()
    {
        if (isAnimating) return;

        if (isOpened) await HideDetails();
        else await ShowDetails();
    }

    public async Task HideDetails()
    {
        //if (_pageModel == null || !_pageModel.IsUnlocked) return;
        isOpened = false;
        isAnimating = true;

        await detailsCanvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        detailsCanvasGroup.interactable = false;
        detailsCanvasGroup.blocksRaycasts = false;
        detailsPage.gameObject.SetActive(false);


        await mainContent.DOSizeDelta(new Vector2(mainContent.sizeDelta.x, defaultHeightOfConent), collapseDuration).SetEase(Ease.OutCubic).AsyncWaitForCompletion();

        isAnimating = false;
    }
     
    public void Refresh()
    {
        if (_pageModel != null)
        {
            if (_pageModel.IsUnlocked) SetUnlockedState();
            else SetLockedState();

            openBookDetailsButton.onClick.RemoveAllListeners();
            openBookDetailsButton.onClick.AddListener(() => OpenBookDetailsButtonClicked?.Invoke(this));
        }
    }

    public void PlaySound()
    {
        Debug.Log("Play");
    }

    public void SetLockedState()
    {
        //symbolText.text = "?";
        //openBookDetailsButton.interactable = false;
        symbolText.text = _pageModel.PageSymbol.japaneseCharacter;
        pronounciationText.text = _pageModel.PageSymbol.pronunciation;
        detailTextField.text = CreateExamples();
        openBookDetailsButton.interactable = true;
    }
    public void SetUnlockedState()
    {
        symbolText.text = _pageModel.PageSymbol.japaneseCharacter;
        pronounciationText.text = _pageModel.PageSymbol.pronunciation;
        detailTextField.text = CreateExamples();
        openBookDetailsButton.interactable = true;
    }

    private string CreateExamples()
    {
        string examplesString = string.Empty;
        for (int i = 0; i < _pageModel.PageSymbol.examples.Count - 1; i++)
        {
            examplesString += _pageModel.PageSymbol.examples[i] + ";" + " ";
        }

        examplesString += _pageModel.PageSymbol.examples[_pageModel.PageSymbol.examples.Count - 1] + ".";
        return examplesString;
    }
}
