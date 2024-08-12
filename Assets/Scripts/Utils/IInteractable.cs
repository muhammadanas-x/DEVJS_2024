using System;

internal interface IInteractable
{
    public event Action OnInteract;

    public void Interact();
}