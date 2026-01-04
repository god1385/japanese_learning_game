using UnityEngine;

public class BookView : MonoBehaviour
{
    [SerializeField] GameObject bookObject;
    [SerializeField] Transform gridWithSymbols;
    [SerializeField] SymbolCardPreview cardPrefab;

    public void ShowBook() => bookObject.SetActive(true);
    public void HideBook() => bookObject.SetActive(false);

    public SymbolCardPreview CreateElementOnUi(SymbolData element)
    {
        var card = Instantiate(cardPrefab);
        card.transform.SetParent(gridWithSymbols, false);
        card.ActivateCard(element);
        return card;
    }
}
