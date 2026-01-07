using System;
using UnityEngine;

public class SymbolPageModel
{
    public SymbolData PageSymbol { get; private set; }
    public bool IsUnlocked {  get; private set; }
    public event Action OnUnlocked;

    public SymbolPageModel(SymbolData data, bool isUnlocked)
    {
        PageSymbol = data;
        IsUnlocked = isUnlocked;
    }

    public void Unlock()
    {
        IsUnlocked = true;
        OnUnlocked.Invoke();
    }

}
