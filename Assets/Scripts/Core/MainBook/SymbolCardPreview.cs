using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent (typeof(Button))]
public class SymbolCardPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI symbolText;
    private Button _cardButton;
    private SymbolData _symbolData;
    public Button CardButton => _cardButton;

    private void Awake()
    {
        _cardButton = GetComponent<Button>();
    }

    public void ActivateCard(SymbolData data)
    {
        _symbolData = data;
        symbolText.text = _symbolData.japaneseCharacter;
    }

    public void SetInLockedState()
    {
        
    }

    private void OnDisable()
    {
        _cardButton.onClick.RemoveAllListeners();
    }
}
