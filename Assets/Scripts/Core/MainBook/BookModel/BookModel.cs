using System.Collections.Generic;
using UnityEngine;

public class BookModel
{
    private List<SymbolPageModel> _pages;
    public IReadOnlyList<SymbolPageModel> Pages => _pages;
    private AlphabetData _alphabet;
    private LessonsData _currentLesson;
    private HashSet<string> _unlockedElements;
    private Dictionary<string, SymbolPageModel> _pagesById;

    public BookModel(LessonsData data, AlphabetData alphabet)
    {
        _alphabet = alphabet;
        _currentLesson = data;
        _unlockedElements = new HashSet<string>();
        _pagesById = new Dictionary<string, SymbolPageModel>();
        _pages = new List<SymbolPageModel>();
        LoadProgress();

        foreach (var symbol in _alphabet?.alphabetData)
        {
            var page = new SymbolPageModel(symbol, IsUnlocked(symbol));
            _pages.Add(page);
            _pagesById.Add(symbol.id, page);
        }
    }

    public bool IsUnlocked(SymbolData symbol) => _unlockedElements.Contains(symbol.id);
    public IEnumerable<SymbolData> AllElements => _currentLesson.lessonSymbols;

    public void UnlockElements(SymbolData symbol)
    {
        if (!_currentLesson.lessonSymbols.Contains(symbol)) return;

        var page = _pagesById[symbol.id];

        if (page != null)
        {
            page.Unlock();
            _unlockedElements.Add(symbol.id);
            SaveProgress();
        }
    }

    private void LoadProgress()
    {

    }

    private void SaveProgress()
    {

    }
}
