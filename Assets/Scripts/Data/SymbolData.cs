using UnityEngine;

[CreateAssetMenu(fileName = "NewSymbol", menuName = "Game/Symbol", order = 1),]
public class SymbolData : ScriptableObject
{
    public string japaneseCharacter;
    public string pronunciation;
    public Sprite icon;
    public AudioClip audioClip;
}
