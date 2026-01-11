using UnityEngine;
using Game.Input;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Zenject;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, ITutorial, ISymbolToCollect
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Animator animator;
    [SerializeField] private float spriteChangeDuration = 0.1f;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private SymbolData symbolToCollect;

    private Rigidbody rb;
    private Vector3 direction;
    private bool _enabled = false;
    [Inject] private SymbolInteractionsConnector _connector;

    public SymbolData SymbolToUnlock => symbolToCollect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputHandler.Move += MoveCharacter;
    }

    private void OnDisable()
    {
        InputHandler.Move -= MoveCharacter;
    }

    private void MoveCharacter(Vector2 value)
    {
        if (_enabled)
        {
            direction = new Vector3(value.x, 0, value.y);

            if (direction.sqrMagnitude > 0.01f)
                rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * moveSpeed);
        }
    }

    public async Task PlayAnimationAsync(List<Sprite> frames)
    {

        if (frames == null || frames.Count == 0) return;

        if (frames == null || frames.Count == 0) return;

        _enabled = false;
        animator.enabled = false;

        foreach (var frame in frames)
        {
            playerRenderer.sprite = frame;
            await Task.Delay(TimeSpan.FromSeconds(spriteChangeDuration));
        }

        _enabled = true;
        animator.enabled = true;
    }

    private void Update()
    {
        if (_enabled)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                animator.SetFloat("MoveX", Mathf.Sign(direction.x));
                animator.SetFloat("MoveY", 0);
            }
            else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.z))
            {
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", Mathf.Sign(direction.z));
            }
            else if (direction.x == 0 && direction.z == 0)
            {
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
            }
        }
    }

    public void EnableInteraction(bool enabled)
    {
        _enabled = enabled;

        if (animator != null) animator.enabled = enabled;
    }

    public void CollectSymbol()
    {
        _connector.CollectSymbol(this);
    }
}
