using System;
using UnityEngine;

public interface IInteractable
{
    void Interact();
    void OnFocus();
    void OnUnfocus();

    public event Action OnInteracted;
    public Transform InteractionPoint {  get; }
}
