using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoadMapPresenter
{
    private readonly LevelDataSet _levelDataSet;
    private readonly Transform _container;
    private readonly RoadMapWordView _wordPrefab;
    private readonly List<RoadMapWordView> _words = new();
    private readonly SymbolInteractionsConnector _connector;
    private int _currentWordIndex = 0;
    private bool isActiveAtTheStart = false;
    private CanvasGroup _canvasGroup;
    public RoadMapPresenter(LevelDataSet levelDataSet, RoadMapWordView wordPrefab, Transform container, SymbolInteractionsConnector connector)
    {
        _levelDataSet = levelDataSet;
        _wordPrefab = wordPrefab;
        _container = container;
        _connector = connector;
        _canvasGroup = container.GetComponent<CanvasGroup>();

        BuildRoadmap();
        _connector.OnSymbolUnlocked += symbol => _ = HandleSymbolUnlocked(symbol);
    }

    private async Task HandleSymbolUnlocked(SymbolData symbol)
    {
        if (!isActiveAtTheStart)
        {
            isActiveAtTheStart = true;
            _container.gameObject.SetActive(true);
            await FadeRoadMapAsync();
        }

        foreach (var word in _words)
        {
            await word.TryAddSymbol(symbol);
        }
    }

    private async Task FadeRoadMapAsync()
    {
        var seq = DOTween.Sequence();

        // Fade сразу, но пусть длится так же, как движение, чтобы не было рывка
        seq.Append(_canvasGroup.DOFade(1f, 2f));
        await seq.AsyncWaitForCompletion();

    }

    private async Task HandleSymbolUnlockedAsync(SymbolData symbol)
    {
        foreach (var word in _words)
        {
            await word.TryAddSymbol(symbol);
        }
    }

    private void BuildRoadmap()
    {
        if (_levelDataSet == null) return;

        _container.gameObject.SetActive(isActiveAtTheStart);
        _canvasGroup.alpha = isActiveAtTheStart ? 1 : 0;

        for (int i = 0; i < _levelDataSet.Words.Count; i++)
        {
            var view = Object.Instantiate(_wordPrefab, _container);
            view.Initialize(_levelDataSet.Words[i], i);
            _words.Add(view);
        }
    }

    public void Dispose()
    {
        _connector.OnSymbolUnlocked -= symbol => _ = HandleSymbolUnlocked(symbol);
    }
}
