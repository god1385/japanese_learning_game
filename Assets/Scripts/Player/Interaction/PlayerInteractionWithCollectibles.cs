using Game.Input;
using UnityEngine;

public class PlayerInteractionWithCollectibles : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactableMask;

    private IInteractable focusedInteractable;
    private IInteractable mouseCandidate;
    private IInteractable proximityCandidate;
    private bool _enabled = true;

    private void OnEnable()
    {
        //InputHandler.Interact += InteractWithObject;
        InputHandler.InteractWithMouth += InteractWithMouth;
    }

    private void OnDisable()
    {
       // InputHandler.Interact -= InteractWithObject;
        InputHandler.InteractWithMouth -= InteractWithMouth;
    }

    private void InteractWithObject()
    {
        if (_enabled && Vector3.Distance(transform.position, focusedInteractable.InteractionPoint.position) <= interactionDistance)
            focusedInteractable?.Interact();
    }

    private void InteractWithMouth()
    {
        if (_enabled)
            focusedInteractable?.Interact();
    }

    private void Update()
    {
        if (!_enabled) return;

        CheckForMouseInteractable();
        //CheckForInteractables();
        ResolveFocus();
    }

    private void ResolveFocus()
    {
        if (proximityCandidate == focusedInteractable)
            return;

        focusedInteractable?.OnUnfocus();
        focusedInteractable = proximityCandidate;
        focusedInteractable?.OnFocus();
    }

    private void CheckForMouseInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(InputHandler.mousePosition);

        if (Physics.Raycast(ray, out var hit, 100f, interactableMask))
        {
            proximityCandidate = hit.collider.GetComponent<IInteractable>();
        }
        else
        {
            proximityCandidate = null;
        }
    }

    //private void CheckForInteractables()
    //{
    //    Collider[] hits = Physics.OverlapSphere(transform.position, interactionDistance, interactableMask);

    //    float minDist = float.MaxValue;
    //    proximityCandidate = null;

    //    foreach (var hit in hits)
    //    {
    //        var interactable = hit.GetComponent<IInteractable>();
    //        if (interactable == null) continue;

    //        float dist = Vector3.Distance(transform.position, interactable.InteractionPoint.position);

    //        if (dist < minDist)
    //        {
    //            minDist = dist;
    //            proximityCandidate = interactable;
    //        }
    //    }
    //}

    public void SetEnabledValue(bool value)
    {
        _enabled = value;
    }
}
