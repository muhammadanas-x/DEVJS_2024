using System;
using TMPro;
using UnityEngine;

public class BaseSwitch : MonoBehaviour, IInteractable
{
    public event Action OnInteract;

    [SerializeField]
    private TextMeshPro prompt;

    private int id;
    private bool status = false;

    public bool Status => status;
    public TextMeshPro Prompt => prompt;
    public int Id => id;
    public bool isActive;
   

    public void SetId(int number)
    {
        id = number;
    }

    private void Awake()
    {
        OnInteract += () => status = !status;
    }

    private void OnDestroy()
    {
        OnInteract -= () => status = !status;
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }

}
