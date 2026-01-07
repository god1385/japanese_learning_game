using System.Collections.Generic;
using UnityEngine;

public class BookInitialize : MonoBehaviour
{
    [SerializeField] private BookView bookView;
    [SerializeField] private LessonsData lessonData;
    [SerializeField] private AlphabetData alphabetData;
    [SerializeField] private MonoBehaviour[] inputBlockerBehaviours;

    private List<IInputBlocker> _blockers;

    private BookModel _bookModel;
    private BookPresenter _bookPresenter;

    private void Awake()
    {
        _blockers = new List<IInputBlocker>();

        foreach (var mb in inputBlockerBehaviours)
        {
            if (mb is IInputBlocker blocker)
                _blockers.Add(blocker);
            else
                Debug.LogError($"{mb.name} does not implement IInputBlocker");
        }

        ISaveService saveService = new JsonDataSaveService();

        var progressTracker = new BookProgressTracker(saveService);
        _bookModel = new BookModel(lessonData, alphabetData);
        _bookPresenter = new BookPresenter(bookView, _bookModel, progressTracker);
        _bookPresenter.BookOpened += BlockInput;
        _bookPresenter.BookClosed += UnBlockInput;
        SymbolInteractionsConnector.Instance.Bind(_bookPresenter);
    }

    public void Initialize()
    {
        _bookPresenter.ShowBook();
    }

    private void UnBlockInput()
    {
        foreach (var blocker in _blockers)
        {
            blocker.UnBlock();
        }
    }

    private void BlockInput()
    {
        foreach (var blocker in _blockers)
        {
            blocker.Block();
        }
    }

    private void OnDestroy()
    {
        _bookPresenter.Dispose();
    }
}
