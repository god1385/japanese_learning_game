using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ViewModel : MonoBehaviour
{
    [Header("Curtains")]
    [SerializeField] private RectTransform firstCurtain;
    [SerializeField] private RectTransform secondCurtain;

    private Image firstCurtainImage;
    private Image secondCurtainImage;

    public RectTransform FirstCurtain => firstCurtain;
    public RectTransform SecondCurtain => secondCurtain;
    public Image FirstCurtainImage => firstCurtainImage;
    public Image SecondCurtainImage => secondCurtainImage;

    private void Awake()
    {
        firstCurtainImage = firstCurtain.GetComponent<Image>();
        secondCurtainImage = secondCurtain.GetComponent<Image>();
    }
    public virtual void DisplayTransitionCurtains(RectTransform zoomTarget, float targetFov, float targetCurtainPosition, float fadeTarget, string nextLevelName = null) 
    { 
    }
}
