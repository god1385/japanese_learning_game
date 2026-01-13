using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordData", menuName = "LevelData/WordData")]
public class WordData : ScriptableObject
{
    public string wordId;
    public List<SymbolData> symbols;
    public Sprite resultImage;

    [Header("Display Options")]
    public bool hideUntilTutorialStep = false;
    public Sprite placeholderSprite;
}
