using System;
using System.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEngine;

public class BookPresenter
{
    private BookView _bookView;
    private BookModel _bookModel;
    private BookProgressTracker _progressTracker;
    private AudioSourceHandler _audioSourceHandler;
    private bool _bookOpened = false;

    public event Action BookOpened;
    public event Action BookClosed;
    public BookPresenter(BookView view, BookModel model, BookProgressTracker progressTracker, AudioSourceHandler audioSourceHandler)
    {
        _bookView = view;
        _bookModel = model;
        _progressTracker = progressTracker;
        _audioSourceHandler = audioSourceHandler;

        BookOpened += OnBookOpened;
        BookClosed += OnBookClosed;
        _bookView.OnBookOpened += HandleBookOpen;
        _bookView.OnBookClosed += HandleBookClose;

        _bookView.SetButtonActions(OnNextPageClicked, OnPreviousPageClicked);
        _bookView.LeftPage.OpenBookDetailsButtonClicked += OnSymbolPageClicked;
        _bookView.RightPage.OpenBookDetailsButtonClicked += OnSymbolPageClicked;
        _bookView.LeftPage.OnPlaySoundClicked += OnPlaySoundClicked;
        _bookView.RightPage.OnPlaySoundClicked += OnPlaySoundClicked;
    }

    public async Task TryUnlockSymbol(SymbolData symbol)
    {
        if (!_bookModel.TryUnlockSymbol(symbol.id)) return;

        int symbolPageIndex = _bookModel.GetPageIndexForSymbol(symbol.id);
        int leftPageIndex = symbolPageIndex % 2 == 0 ? symbolPageIndex : symbolPageIndex - 1;
        _bookModel.SetPageIndex(leftPageIndex);

        SaveProgress();
        await _bookView.PlayUnlockSymbolAnimation(symbol);

        if (_bookOpened)
        {

            if (!_bookView.IsOpen)
            {
                await _bookView.OpenBook();
            }

            var page = symbolPageIndex % 2 == 0 ? _bookView.LeftPage : _bookView.RightPage;
            RefreshPageData();
            await _bookView.HighlightUnlockedSymbol(page);
        }
    }

    private void HandleBookOpen()
    {
        BookOpened.Invoke();
    }

    private void HandleBookClose()
    {
        BookClosed.Invoke();
    }

    public void ShowBook()
    {
        _bookView?.ShowMiniBookButton();
    }
    public void HideBook()
    {
        _bookView?.HideMiniBookButton();
    }

    public void OnBookOpened()
    {
        _bookOpened = true;
        LoadProgress();
        RefreshPageData();
    }
    private void OnBookClosed()
    {
        SaveProgress();
    }

    private void RefreshPageData()
    {
        var pages = _bookModel.Pages;

        SymbolPageModel left = _bookModel.CurrentLeftPageIndex < pages.Count ? pages[_bookModel.CurrentLeftPageIndex] : null;
        SymbolPageModel right = _bookModel.CurrentLeftPageIndex + 1 < pages.Count ? pages[_bookModel.CurrentLeftPageIndex + 1] : null;

        _bookView.LinkPage(left, right);

        _bookView.SetButtonsState(canNext: _bookModel.CurrentLeftPageIndex + 2 < pages.Count,canPrev: _bookModel.CurrentLeftPageIndex > 0);
    }


    private async Task OnNextPageClicked()
    {
        if (_bookModel.CurrentLeftPageIndex + 2 >= _bookModel.Pages.Count)
            return;

        _audioSourceHandler.StopAudio();
        _bookModel.SetPageIndex(_bookModel.CurrentLeftPageIndex + 2);
        RefreshPageData();
        await _bookView.PlayNextPageAsync();

    }
    private async Task OnPreviousPageClicked()
    {
        if (_bookModel.CurrentLeftPageIndex - 2 < 0)
            return;

        _audioSourceHandler.StopAudio();
        _bookModel.SetPageIndex(_bookModel.CurrentLeftPageIndex - 2);
        RefreshPageData();
        await _bookView.PlayPreviousPageAsync();

    }


    private void SaveProgress()
    {
        var data = _bookModel.CreateSaveData();

        _progressTracker.Save(data);
    }

    private void LoadProgress()
    {
        var data = _progressTracker.Load();

        if (data != default(BookSaveModel))
        {
            _bookModel.LoadFromSave(data);
        }
        else
            _bookModel.SetPageIndex(0);
    }
    private void OnPlaySoundClicked(SymbolPageView view)
    {
        var page = view.PageModel;

        if (page == null || !page.IsUnlocked)
            return;

        var clip = page.PageSymbol.audioClip;

        if (clip == null)
        {
            Debug.LogWarning($"No audio for {page.PageSymbol.name}");
            return;
        }

        _audioSourceHandler.PlayAudio(clip);
    }

    private async void OnSymbolPageClicked(SymbolPageView view)
    {
        await view.PageClicked();
    }

    public void Dispose()
    {
        BookOpened -= OnBookOpened;
        BookClosed -= OnBookClosed;
        _bookView.OnBookOpened -= HandleBookOpen;
        _bookView.OnBookClosed -= HandleBookClose;
        _bookView.LeftPage.OpenBookDetailsButtonClicked -= OnSymbolPageClicked;
        _bookView.RightPage.OpenBookDetailsButtonClicked -= OnSymbolPageClicked;
        _bookView.LeftPage.OnPlaySoundClicked -= OnPlaySoundClicked;
        _bookView.RightPage.OnPlaySoundClicked -= OnPlaySoundClicked;
        _bookView.ActionDispose();
    }
}
