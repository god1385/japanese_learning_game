using UnityEngine;
using UnityEngine.LightTransport;

public class SymbolInteractionsConnector
{
    private readonly BookPresenter _bookPresenter;

    public SymbolInteractionsConnector(BookPresenter bookPresenter)
    {
        _bookPresenter = bookPresenter;
    }

    public async void CollectSymbol(ISymbolToCollect source)
    {
        await _bookPresenter.TryUnlockSymbol(source.SymbolToUnlock);
    }
}
