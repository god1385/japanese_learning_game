using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class LetterSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private string expectedSymbol;
    [SerializeField] private Image imageToPlaceSymbol;
    public bool IsCorrect { get; private set; }
    public string ExpectedSymbol => expectedSymbol;

    private void Awake()
    {
        imageToPlaceSymbol.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var letter = eventData.pointerDrag.GetComponent<LetterDrag>();
        if (letter == null) return;

        if (letter.Symbol == expectedSymbol)
        {
            letter.transform.SetParent(transform);
            letter.FoundRequiredPlace();
            imageToPlaceSymbol.sprite = letter.ImageOfSymbol.sprite;
            imageToPlaceSymbol.gameObject.SetActive(true);
            letter.gameObject.SetActive(false);
            IsCorrect = true;
        }
        else
            transform.DOShakePosition(0.5f, 10f, 5, 90, false, true);
    }
}
