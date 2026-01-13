using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LetterDrag : MonoBehaviour, IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private string symbol;
    [SerializeField] private RectTransform parent;
    [SerializeField] private Image imageOfSymbol;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _startParent;
    private Vector2 startPosition;

    public string Symbol => symbol;
    public bool FoundSlot { get; private set; }
    public Image ImageOfSymbol => imageOfSymbol;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent.transform as RectTransform, transform.position, null, out localPoint);
        startPosition = localPoint;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startParent = transform.parent;
        transform.SetParent(parent.transform); // наверх, поверх всего

        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent.transform as RectTransform,eventData.position, null,out localPoint);

        _rectTransform.localPosition = localPoint;
    }

    public void FoundRequiredPlace()
    {
        FoundSlot = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!FoundSlot)
        {
            if (transform.parent == parent.transform)
                transform.SetParent(_startParent);

            _canvasGroup.blocksRaycasts = true;
            transform.position = startPosition;
        }
    }
}
