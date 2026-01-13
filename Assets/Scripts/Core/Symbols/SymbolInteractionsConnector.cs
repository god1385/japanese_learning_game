using System;
using System.Threading.Tasks;
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

    public async Task CollectSymbol(ISymbolToCollect source)
    {
        foreach (var symbol in source.SymbolsToUnlock)
        {
            await _bookPresenter.EnqueueUnlockSymbol(symbol);
            OnSymbolUnlocked?.Invoke(symbol);
        }
    }
}
