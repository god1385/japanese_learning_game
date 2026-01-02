using UnityEngine;

public class GameMenuInitialize : MonoBehaviour
{
    [SerializeField] private GameMenuView gameMenuView;
    [SerializeField] private RectTransform zoomTarget;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float targetCurtainMegePoint;
    [SerializeField] private string nextLevelName;

    private void Awake()
    {
        var model = new GameMenuModel(gameMenuView);
        model.SetData(zoomTarget, zoomAmount, targetCurtainMegePoint, 0f, nextLevelName);
        var presenter = new GameMenuPresenter(model, gameMenuView);
    }

    private void Start()
    {
        gameMenuView.DisplayTransitionCurtains(zoomTarget, zoomAmount, targetCurtainMegePoint, 0f);
    }
}
