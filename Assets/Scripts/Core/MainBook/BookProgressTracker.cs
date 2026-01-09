using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookProgressTracker
{
    private ISaveService _saveService;
    private const string BOOK_PROGRESS_KEY = "book_progress";

    public BookProgressTracker(ISaveService service)
    {
        _saveService = service;
    }

    public void Save(BookSaveModel saveData)
    {
        _saveService.Save(saveData, BOOK_PROGRESS_KEY);
    }

    public BookSaveModel Load()
    {
        return _saveService.Load<BookSaveModel>(BOOK_PROGRESS_KEY); ;
    }
}

public class BookSaveModel
{
    public List<string> unlockedElements;
    public int pageIndex;
}
