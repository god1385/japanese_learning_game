using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class RoadMapSlotView : MonoBehaviour
{
    [SerializeField] private Image icon;

    private SymbolData _expectedSymbol;
    private bool _filled = false;

    public void Initialize(SymbolData expected)
    {
        _expectedSymbol = expected;
        icon.enabled = false;
    }

    public async Task<bool> PlayFill(SymbolData symbol)
    {
        if (symbol != _expectedSymbol || _filled)
            return false;

        _filled = true;
        icon.sprite = symbol.icon;
        icon.enabled = true;

        // DOTween bounce / scale / glow
        await Task.CompletedTask;
        return true;
    }
}
