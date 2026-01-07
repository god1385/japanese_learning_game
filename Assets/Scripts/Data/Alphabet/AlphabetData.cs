using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlphabetData", menuName = "Game/Alphabet", order = 3)]
public class AlphabetData : ScriptableObject
{
    public List<SymbolData> alphabetData;
}
