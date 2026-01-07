using Game.Input;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;

public class PlayerInteractionWithCollectibles : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactableMask;

    private IInteractable currentInteractable;

    private void OnEnable()
    {
        InputHandler.Interact += InteractWithObject;
    }

    private void OnDisable()
    {
        InputHandler.Interact -= InteractWithObject;
    }

    private void InteractWithObject()
    {
        currentInteractable?.Interact();
    }

    private void Update()
    {
        CheckForInteractables();
    }



    private void CheckForInteractables()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,interactionDistance,interactableMask);
        float minDist = float.MaxValue;
        IInteractable closest = null;

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                var interactable = hit.GetComponent<IInteractable>();

                if (interactable == null) continue;

                float distance = Vector3.Distance(transform.position, interactable.InteractionPoint.position);

                if (distance < minDist)
                {
                    minDist = distance;
                    closest = interactable;
                }
            }

            if (closest != currentInteractable)
            {
                currentInteractable?.OnUnfocus();
                currentInteractable = closest;
                currentInteractable?.OnFocus();
            }
        }

        if (closest == null && currentInteractable != null)
        {
            currentInteractable.OnUnfocus();
            currentInteractable = null;
        }
    }
}
