using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BookInitialize : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] inputBlockerBehaviours;
    private List<IInputBlocker> _blockers;
    private BookPresenter _bookPresenter;

    private void Awake()
    {
        _blockers = new List<IInputBlocker>();
    }

    [Inject]
    public void Construct(BookPresenter bookPresenter)
    {
        _bookPresenter = bookPresenter;
    }

    public void Initialize()
    {
        foreach (var mb in inputBlockerBehaviours)
        {
            if (mb is IInputBlocker blocker)
                _blockers.Add(blocker);
            else
                Debug.LogError($"{mb.name} does not implement IInputBlocker");
        }

        _bookPresenter.BookOpened += BlockInput;
        _bookPresenter.BookClosed += UnBlockInput;
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
        _bookPresenter.BookOpened -= BlockInput;
        _bookPresenter.BookClosed -= UnBlockInput;
        _bookPresenter.Dispose();
    }
}
