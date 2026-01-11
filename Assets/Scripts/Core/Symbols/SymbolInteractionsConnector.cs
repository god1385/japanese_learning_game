using System;
using UnityEngine;
using UnityEngine.LightTransport;

public class SymbolInteractionsConnector
{
    private readonly BookPresenter _bookPresenter;
    public event Action<SymbolData> OnSymbolUnlocked;

    public SymbolInteractionsConnector(BookPresenter bookPresenter)
    {
        _bookPresenter = bookPresenter;
    }

    public void CollectSymbol(ISymbolToCollect source)
    {
        _bookPresenter.EnqueueUnlockSymbol(source.SymbolToUnlock);
        OnSymbolUnlocked?.Invoke(source.SymbolToUnlock);
    }
}
