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
        foreach (var word in _words)
        {
            await word.TryAddSymbol(symbol);
        }

        if (!isActiveAtTheStart)
        {
            isActiveAtTheStart = true;
            _container.gameObject.SetActive(true);
            await FadeRoadMapAsync();
        }

    }

    private async Task FadeRoadMapAsync()
    {
        var seq = DOTween.Sequence();

        // Fade сразу, но пусть длится так же, как движение, чтобы не было рывка
        seq.Append(_canvasGroup.DOFade(1f, 1f));
        foreach(var word in _words)
        {
            seq.Append(word.GetComponent<CanvasGroup>().DOFade(1f, 1f));
        }
        await seq.AsyncWaitForCompletion();

    }

    public void RevealWord(string expectedWord)
    {
        string word = "";

        for (int i = 0; i < _levelDataSet.Words.Count; i++)
        {
            word = "";
            for (int j = 0; j < _levelDataSet.Words[i].symbols.Count; j++)
            {
                word += _levelDataSet.Words[i].symbols[j].japaneseCharacter;
            }
            if (word == expectedWord)
            {
                var CanvasGroup = _words[i].GetComponent<CanvasGroup>();
                CanvasGroup.alpha = 0f;
                _words[i].RevealWord();
                CanvasGroup.DOFade(1f, 1f);
                break;
            }
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
