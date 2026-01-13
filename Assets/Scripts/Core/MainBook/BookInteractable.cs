using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BookInteractable : MonoBehaviour, IInteractable, ITutorial
{
    [SerializeField] private GameObject outline;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private BookInitialize bookInitialize;
    public Transform InteractionPoint => interactionPoint;
    private bool _interacted = false;
    private bool _canInteract = false;

    public event Action OnInteracted;

    public void Interact()
    {
        if (_interacted || !_canInteract) return;

        OnInteracted.Invoke();
        _interacted = true;
        bookInitialize.Initialize();
        gameObject.SetActive(false); 
        OnUnfocus();
    }

    public void OnFocus()
    {
        if (_interacted || !_canInteract) return;

        outline.SetActive(true);
    }

    public void OnUnfocus()
    {
        if (_interacted || !_canInteract) return;

        outline.SetActive(false);
    }

    public void EnableInteraction(bool enabled)
    {
        _canInteract = enabled;
    }

    public Task PlayAnimationAsync(List<Sprite> frames)
    {
        return Task.CompletedTask;
    }
}
