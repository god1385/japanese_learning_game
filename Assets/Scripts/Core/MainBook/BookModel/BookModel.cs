using System.Collections.Generic;
using UnityEngine;

public class BookModel
{
    private LessonsData _currentLesson;
    private HashSet<string> _unlockedElements;

    public BookModel(LessonsData data)
    {
        _currentLesson = data;
        _unlockedElements = new HashSet<string>();
    }

    public bool isUnlocked(SymbolData symbol) => _unlockedElements.Contains(symbol.id);
    public IEnumerable<SymbolData> AllElements => _currentLesson.lessonSymbols;

    public void UnlockElements(SymbolData symbol)
    {
        _unlockedElements.Add(symbol.id);
        SaveProgress();
    }

    private void LoadProgress()
    {

    }

    private void SaveProgress()
    {

    }
}
