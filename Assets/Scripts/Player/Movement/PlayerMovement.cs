using UnityEngine;
using Game.Input;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isTutorial;

    private Rigidbody rb;
    private Vector3 direction;
    private bool _enabled = false;
    
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

    public void SetEnabledValue(bool value)
    {
        _enabled = value;
        animator.enabled = value;
    }
}
