using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuView : ViewModel
{
    public override void DisplayTransitionCurtains(RectTransform zoomTarget, float targetFov, float targetCurtainPosition, float fadeTarget, string nextLevelName = null)
    {
        Sequence seq = DOTween.Sequence();
        Image firstCurtainImage = base.FirstCurtain.GetComponent<Image>();
        Image secondCurtainImage = base.SecondCurtain.GetComponent<Image>();

        // Шторки
        seq.Append(base.FirstCurtain.DOAnchorPosX(targetCurtainPosition, 0.7f).SetEase(Ease.InOutSine));
        seq.Join(base.SecondCurtain.DOAnchorPosX(-targetCurtainPosition, 0.7f).SetEase(Ease.InOutSine));
        seq.Join(firstCurtainImage.DOFade(fadeTarget, 0.7f));
        seq.Join(secondCurtainImage.DOFade(fadeTarget, 0.7f));
    }
}
