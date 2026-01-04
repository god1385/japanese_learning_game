using Game.Input;
using UnityEngine;

public class PlayerInteractionWithCollectibles : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactableMask;

    private IInteractable currentInteractable;

    private void OnEnable()
    {
        InputHandler.Interact += CheckForInteractables;
    }

    private void OnDisable()
    {
        InputHandler.Interact -= CheckForInteractables;
    }



    private void CheckForInteractables()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,interactionDistance,interactableMask);
        float minDist = float.MaxValue;
        Debug.Log("Kek");
        currentInteractable = null;

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                Debug.Log("Lol");
                var interactable = hit.GetComponent<IInteractable>();

                if (interactable == null) continue;

                float distance = Vector3.Distance(transform.position, interactable.InteractionPoint.position);

                if (distance < minDist)
                {
                    minDist = distance;
                    currentInteractable = interactable;
                }
            }

            currentInteractable?.Interact();
        }
    }
}
