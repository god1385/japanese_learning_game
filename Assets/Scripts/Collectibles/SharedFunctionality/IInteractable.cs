using UnityEngine;

public interface IInteractable
{
    void Interact();
    void OnFocus();
    void OnUnfocus();
    public Transform InteractionPoint {  get; }
}
