using UnityEngine;

[CreateAssetMenu(fileName = "NewSymbol", menuName = "Game/Symbol", order = 2),]
public class SymbolData : ScriptableObject
{
    public string id;
    public string japaneseCharacter;
    public string pronunciation;
    public Sprite icon;
    public AudioClip audioClip;
}
