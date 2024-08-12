using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Puzzle
{
    public BaseSwitch[] Switches { get; }
    public bool Solved { get; }
}
public abstract class BasePuzzle : MonoBehaviour, Puzzle
{
    [SerializeField]
    protected GameObject player;
    [SerializeField]
    protected float distanceDetectionThreshold = 3f;

    public abstract bool Solved { get; }
    public bool solvedAlready = false;
    public int NbSwitches => Switches.Length;

    public abstract BaseSwitch[] Switches { get; }

    protected virtual void Start()
    {
        BindActionToSwitch();
    }

    protected virtual void OnDestroy()
    {
        UnBindActionToSwitch();
    }

    protected virtual void Update()
    {
        if (solvedAlready) return;
        EnableAndDisableSwitchsPrompt();

        if (Solved) { GameManager.Instance.PuzzleComplete(); solvedAlready = true; }
    }

    protected virtual void BindActionToSwitch()
    {
        foreach (BaseSwitch baseSwitch in Switches)
        {
            baseSwitch.OnInteract += () => HandleInteraction(baseSwitch);
        }
    }

    protected virtual void UnBindActionToSwitch()
    {
        foreach (BaseSwitch baseSwitch in Switches)
        {
            baseSwitch.OnInteract -= () => HandleInteraction(baseSwitch);
        }
    }

    protected virtual void EnableAndDisableSwitchsPrompt()
    {
        for (int i = 0; i < NbSwitches; i++)
        {
            if(Switches[i].Prompt != null)
            {
                Switches[i].Prompt.enabled = Vector3.Distance(player.transform.position, Switches[i].transform.position) <= distanceDetectionThreshold;
            }
        }
    }

    //todo: remove to do animation of flicking instead
    protected virtual void ActivateSwitch(BaseSwitch switchComponent)
    {
        switchComponent.GetComponent<Renderer>().material.color = Color.green;
    }

    //todo: remove to do animation of flicking instead
    protected virtual void DesactivateAllSwitch()
    {
        for (int i = 0; i < NbSwitches; i++)
        {
            Switches[i].GetComponent<BaseSwitch>().isActive = true;
            StartCoroutine(TurnRedThenBlack(Switches[i].gameObject));
        }
    }

    public virtual void ResetGame()
    {
        DesactivateAllSwitch();
    }


    IEnumerator TurnRedThenBlack(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        renderer.material.color = Color.red;
        yield return new WaitForSeconds(1f); // Change the duration as needed
        renderer.material.color = Color.black;
        obj.GetComponent<BaseSwitch>().isActive = false;
    }

    protected abstract void HandleInteraction(BaseSwitch baseSwitchUsed);
}