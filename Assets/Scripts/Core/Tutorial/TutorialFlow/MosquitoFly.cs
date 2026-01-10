using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MosquitoFly : MonoBehaviour, ISymbolToCollect, IInteractable, ITutorial
{
    [SerializeField] private Transform earAnchor;
    [SerializeField] private SymbolData symbolUnlocked;

    [Header("Fly Settings")]
    [SerializeField] private float radius = 0.15f;
    [SerializeField] private float moveDuration = 0.6f;

    [Header("Visual")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material defaultMosquito;
    [SerializeField] private Material outlineMosquito;
    [SerializeField] private float scaleJitter = 0.05f;
    [SerializeField] private float spriteChangeDuration = 0.1f;

    private Tween _flyTween;
    private Transform visual;
    private bool canInteract;
    private TutorialPresenter _tutorial;
    private Vector3 _lastPosition;
    private bool _isClicked = false;

    [Inject] private SymbolInteractionsConnector _connector;

    public event System.Action OnInteracted;

    public SymbolData SymbolToUnlock => symbolUnlocked;

    public Transform InteractionPoint => this.transform;


    private void OnEnable()
    {
        _lastPosition = transform.position;
        StartFlying();
    }

    private void OnDisable()
    {
        _flyTween?.Kill();
    }
    private void LateUpdate()
    {
        Vector3 delta = transform.position - _lastPosition;

        if (delta.sqrMagnitude > 0.0001f)
        {
            delta.Normalize();
            animator.SetFloat("MoveX", delta.x);
            animator.SetFloat("MoveY", delta.y);
        }

        _lastPosition = transform.position;
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

        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;
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

    public void Interact()
    {
        if (canInteract)
        {
            OnUnfocus();
            OnInteracted?.Invoke();
        }
    }

    public void OnFocus()
    {
        if (canInteract)
        {
            spriteRenderer.material = outlineMosquito;
        }
    }

    public void OnUnfocus()
    {
        if (canInteract)
        {
            spriteRenderer.material = defaultMosquito;
        }
    }

    public void EnableInteraction(bool enabled)
    {
        canInteract = enabled;
    }

    public void CollectSymbol()
    {
        if (!_isClicked) { _isClicked = true; return; }

        _connector.CollectSymbol(this);
    }

    public async Task PlayAnimationAsync(List<Sprite> frames)
    {
        if (frames == null || frames.Count == 0) return;

        animator.enabled = false;
        _flyTween?.Kill();
        var moveSeq = DOTween.Sequence();
        moveSeq.Append(transform.DOMove(transform.position + Vector3.up * 2f, spriteChangeDuration * frames.Count)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => gameObject.SetActive(false)));

        var moveTask = moveSeq.AsyncWaitForCompletion();


        var spriteTask = SpriteAnim(frames);

        await Task.WhenAll(moveTask, spriteTask);
    }

    async Task SpriteAnim(List<Sprite> frames)
    {
        foreach (var frame in frames)
        {
            spriteRenderer.sprite = frame;
            await Task.Delay(TimeSpan.FromSeconds(spriteChangeDuration));
        }
    }
}
