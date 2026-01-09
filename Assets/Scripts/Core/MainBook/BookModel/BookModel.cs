using System.Collections.Generic;
using System.Linq;

public class BookModel
{
    private List<SymbolPageModel> _pages;
    public IReadOnlyList<SymbolPageModel> Pages => _pages;
    private AlphabetData _alphabet;
    private LessonsData _currentLesson;
    private HashSet<string> _unlockedElements;
    private Dictionary<string, SymbolPageModel> _pagesById;
    private int _currentLeftPageIndex;
    public int CurrentLeftPageIndex => _currentLeftPageIndex;

    public BookModel(LessonsData data, AlphabetData alphabet)
    {
        _alphabet = alphabet;
        _currentLesson = data;
        _unlockedElements = new HashSet<string>();
        _pagesById = new Dictionary<string, SymbolPageModel>();
        _pages = new List<SymbolPageModel>();

        foreach (var symbol in _alphabet?.alphabetData)
        {
            bool canBeUnlocked = _currentLesson.lessonSymbols.Contains(symbol);
            var page = new SymbolPageModel(symbol, isUnlocked: false, canBeUnlocked);
            _pages.Add(page);
            _pagesById.Add(symbol.id, page);
        }
    }

    public bool TryUnlockSymbol(string symbolId)
    {
        if (!_pagesById.TryGetValue(symbolId, out var page))
            return false;

        if (!page.TryUnlock())
            return false;

        _unlockedElements.Add(symbolId);
        return true;
    }

    public BookSaveModel CreateSaveData()
    {
        return new BookSaveModel
        {
            unlockedElements = _unlockedElements.ToList(),
            pageIndex = _currentLeftPageIndex
        };
    }

    public int GetPageIndexForSymbol(string symbolId)
    {
        for (int i = 0; i < _pages.Count; i++)
        {
            if (_pages[i].PageSymbol.id == symbolId)
                return i;
        }
        return 0;
    }



    public void LoadFromSave(BookSaveModel data)
    {
        _unlockedElements.Clear();
        _unlockedElements = data.unlockedElements.ToHashSet();
        _currentLeftPageIndex = data.pageIndex;

        foreach (var id in data.unlockedElements)
        {
            if (_pagesById.TryGetValue(id, out var page))
            {
                page.TryUnlock();
                _unlockedElements.Add(id);
            }
        }
    }

    public void SetPageIndex(int index)
    {
        _currentLeftPageIndex = index;
    }
}
