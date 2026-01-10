using System;
using UnityEngine;
using UnityEngine.UI;

public class BookInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject outline;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private BookInitialize bookInitialize;
    public Transform InteractionPoint => interactionPoint;
    private bool _interacted = false;

    public event Action OnInteracted;

    public void Interact()
    {
        if (_interacted) return;

        _interacted = true;
        bookInitialize.Initialize();
        OnUnfocus();
    }

    public void OnFocus()
    {
        if (!_interacted)
            outline.SetActive(true);
    }

    public void OnUnfocus()
    {
        outline.SetActive(false);
    }
}
