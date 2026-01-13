using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ISymbolToCollect
{
    IReadOnlyList<SymbolData> SymbolsToUnlock { get; }
    Task CollectSymbol();
}
