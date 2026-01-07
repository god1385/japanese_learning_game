using System;
using System.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEngine;

public class BookPresenter
{
    private BookView _bookView;
    private BookModel _bookModel;
    private BookProgressTracker _progressTracker;

    public event Action BookOpened;
    public event Action BookClosed;
    public BookPresenter(BookView view, BookModel model, BookProgressTracker progressTracker)
    {
        _bookView = view;
        _bookModel = model;
        _progressTracker = progressTracker;

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

    public async void TryUnlockSymbol(SymbolData symbol)
    {
        if (_bookModel.TryUnlockSymbol(symbol.id))
        {
            SaveProgress();
            if (!_bookView.IsOpen)
            {
                await _bookView.OpenBook();
                //_bookView.PlayUnlockEffect(symbol);
            }

            RefreshPageData();
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

        AudioSourceHandler.Instance.StopAudio();
        _bookModel.SetPageIndex(_bookModel.CurrentLeftPageIndex + 2);
        RefreshPageData();
        await _bookView.PlayNextPageAsync();

    }
    private async Task OnPreviousPageClicked()
    {
        if (_bookModel.CurrentLeftPageIndex - 2 < 0)
            return;

        AudioSourceHandler.Instance.StopAudio();
        _bookModel.SetPageIndex(_bookModel.CurrentLeftPageIndex - 2);
        RefreshPageData();
        await _bookView.PlayPreviousPageAsync();

    }


    private void SaveProgress()
    {
        var data = _bookModel.CreateSaveData();
        if (data.unlockedElements.Count > 0)
            Debug.Log(data.unlockedElements[0]);
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

        AudioSourceHandler.Instance.PlayAudio(clip);
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
