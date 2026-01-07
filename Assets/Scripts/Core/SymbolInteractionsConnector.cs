using UnityEngine;
using UnityEngine.LightTransport;

public class SymbolInteractionsConnector
{
    public static SymbolInteractionsConnector Instance { get; } = new();

    private BookPresenter _bookPresenter;

    public void Bind(BookPresenter presenter)
    {
        _bookPresenter = presenter;
    }

    public void TryUnlock(ISymbolToCollect source)
    {
        _bookPresenter.TryUnlockSymbol(source.SymbolToUnlock);
    }
}
