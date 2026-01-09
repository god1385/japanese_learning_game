using DG.Tweening;
using UnityEngine;
using Zenject;

public class MosquitoFly : MonoBehaviour, ISymbolToCollect, IInteractable
{
    [SerializeField] private Transform earAnchor;
    [SerializeField] private SymbolData symbolUnlocked;

    [Header("Fly Settings")]
    [SerializeField] private float radius = 0.15f;
    [SerializeField] private float moveDuration = 0.6f;

    [Header("Visual")]
    [SerializeField] private Transform defaultMosquito;
    [SerializeField] private Transform outlineMosquito;
    [SerializeField] private float scaleJitter = 0.05f;

    private Tween _flyTween;
    private Transform visual;

    [Inject] private TutorialInfo _tutorialInfo;
    [Inject] private SymbolInteractionsConnector _connector;

    public SymbolData SymbolToUnlock => symbolUnlocked;

    public Transform InteractionPoint => this.transform;

    private void Awake()
    {
        visual = defaultMosquito;
    }

    private void OnEnable()
    {
        StartFlying();
    }

    private void OnDisable()
    {
        _flyTween?.Kill();
    }

    private void StartFlying()
    {
        _flyTween?.Kill();

        _flyTween = DOTween.Sequence()
            .AppendCallback(MoveToRandomPoint)
            .AppendInterval(moveDuration)
            .SetLoops(-1)
            .SetEase(Ease.InOutSine);
    }

    private void MoveToRandomPoint()
    {
        if (earAnchor == null) return;

        Vector2 randomOffset = Random.insideUnitCircle * radius;
        Vector3 targetPos = earAnchor.position + (Vector3)randomOffset;

        transform.DOMove(targetPos, moveDuration)
            .SetEase(Ease.InOutSine);
    }

    private void Update()
    {
        if (visual != null)
        {
            float s = 1f + Mathf.Sin(Time.time * 15f) * scaleJitter;
            visual.localScale = Vector3.one * s;
        }
    }

    public void ScareAway()
    {
        _flyTween?.Kill();

        transform.DOMove(transform.position + Vector3.up * 2f, 1f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void Interact()
    {
        Debug.Log("click");
        if (!_tutorialInfo.isMosquitoClicked && !_tutorialInfo.isHandClicked)
        {
            OnUnfocus();
            _tutorialInfo.isMosquitoClicked = true;
        }
        else if (_tutorialInfo.isHandClicked)
        {
            OnUnfocus();
            _connector.CollectSymbol(this);

            gameObject.SetActive(false);
        }
    }

    public void OnFocus()
    {
        if (!_tutorialInfo.isMosquitoClicked || _tutorialInfo.isHandClicked)
        {
            visual = outlineMosquito;
            defaultMosquito.gameObject.SetActive(false);
            outlineMosquito.gameObject.SetActive(true);
        }
    }

    public void OnUnfocus()
    {
        if (!_tutorialInfo.isMosquitoClicked || _tutorialInfo.isHandClicked)
        {
            visual = defaultMosquito;
            defaultMosquito.gameObject.SetActive(true);
            outlineMosquito.gameObject.SetActive(false);
        }
    }
}
