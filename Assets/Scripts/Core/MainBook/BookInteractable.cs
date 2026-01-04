using UnityEngine;

public class BookInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private BookInitialize bookInitialize;
    public Transform InteractionPoint => interactionPoint;

    public void Interact()
    {
        bookInitialize.Initialize();
    }
}
