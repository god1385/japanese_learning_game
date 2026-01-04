using UnityEngine;

public interface IInteractable
{
    void Interact();
    public Transform InteractionPoint {  get; }
}
