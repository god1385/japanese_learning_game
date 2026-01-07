using System.Threading.Tasks;
using UnityEngine;

public class BookPresenter
{
    private BookView _bookView;
    private BookModel _bookModel;

    private int _leftPageIndex = 0;
    public BookPresenter(BookView view, BookModel model)
    {
        _bookView = view;
        _bookModel = model;
        _bookView.SetButtonActions(OnNextPageClicked, OnPreviousPageClicked);
        _bookView.OnBookOpened += ShowBookPages;
        _bookView.LeftPage.OpenBookDetailsButtonClicked += OnSymbolPageClicked;
        _bookView.RightPage.OpenBookDetailsButtonClicked += OnSymbolPageClicked;
        _bookView.LeftPage.OnPlaySoundClicked += OnPlaySoundClicked;
        _bookView.RightPage.OnPlaySoundClicked += OnPlaySoundClicked;
    }

    public void ShowBook()
    {
        _bookView?.ShowMiniBookButton();
    }

    public void ShowBookPages()
    {
        _leftPageIndex = 0;
        RefreshPageData();
    }
    private void RefreshPageData()
    {
        var pages = _bookModel.Pages;

        SymbolPageModel left = _leftPageIndex < pages.Count ? pages[_leftPageIndex] : null;
        SymbolPageModel right = _leftPageIndex + 1 < pages.Count ? pages[_leftPageIndex + 1] : null;

        _bookView.LinkPage(left, right);

        _bookView.SetButtonsState(canNext: _leftPageIndex + 2 < pages.Count,canPrev: _leftPageIndex > 0);
    }

    public void HideBook()
    {
        _bookView?.HideMiniBookButton();
    }

    private async Task OnNextPageClicked()
    {
        if (_leftPageIndex + 2 >= _bookModel.Pages.Count)
            return;

        AudioSourceHandler.Instance.StopAudio();
        await _bookView.PlayNextPageAsync();

        _leftPageIndex += 2;
        RefreshPageData();
    }

    private async Task OnPreviousPageClicked()
    {
        if (_leftPageIndex - 2 < 0)
            return;

        AudioSourceHandler.Instance.StopAudio();
        await _bookView.PlayPreviousPageAsync();

        _leftPageIndex -= 2;
        RefreshPageData();
    }

    private void OnPlaySoundClicked(SymbolPageView view)
    {
        var page = view.PageModel;

        //if (page == null || !page.IsUnlocked)
        //    return;

        var clip = page.PageSymbol.audioClip;

        if (clip == null)
        {
            Debug.LogWarning($"No audio for {page.PageSymbol.name}");
            return;
        }

        AudioSourceHandler.Instance.PlayAudio(clip);
    }

    private async void OnSymbolPageClicked(SymbolPageView view)
    {
        await view.PageClicked();
    }
}
