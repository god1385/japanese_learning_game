using System;
using UnityEngine;

public class SymbolPageModel
{
    public SymbolData PageSymbol { get; private set; }
    public bool IsUnlocked {  get; private set; }
    public bool CanBeUnlocked { get; private set; }
    public event Action OnUnlocked;

    public SymbolPageModel(SymbolData data, bool isUnlocked, bool canBeUnlocked)
    {
        PageSymbol = data;
        IsUnlocked = isUnlocked;
        CanBeUnlocked = canBeUnlocked;
    }

    public bool TryUnlock()
    {
        if (!CanBeUnlocked || IsUnlocked)
            return false;

        IsUnlocked = true;
        return true;
    }
}
