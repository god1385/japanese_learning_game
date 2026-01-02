using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ViewModel : MonoBehaviour
{
    [Header("Curtains")]
    [SerializeField] private RectTransform firstCurtain;
    [SerializeField] private RectTransform secondCurtain;

    public RectTransform FirstCurtain => firstCurtain;
    public RectTransform SecondCurtain => secondCurtain;
    public virtual void DisplayTransitionCurtains(RectTransform zoomTarget, float targetFov, float targetCurtainPosition, float fadeTarget, string nextLevelName = null) 
    { 
    }
}
