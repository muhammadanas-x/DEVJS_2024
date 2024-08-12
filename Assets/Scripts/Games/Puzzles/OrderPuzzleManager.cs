using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderPuzzleManager : BasePuzzle
{
    [SerializeField]
    private BaseSwitch[] switches; 

    private int iterator = 1;
    private int numOfCorrectClicks = 0;

    public override bool Solved => numOfCorrectClicks == NbSwitches;
    public override BaseSwitch[] Switches => switches;

    protected override void Start()
    {
        base.Start();
        GenerateUniqueNumbers();
    }

    protected override void HandleInteraction(BaseSwitch baseSwitchUsed)
    {
        if (solvedAlready) return;

        if (baseSwitchUsed.isActive) return;

        if (IsCorrectClicked(baseSwitchUsed.Id))
        {
            ActivateSwitch(baseSwitchUsed);

            CarryOn();
        }
        else
        {
            ResetGame();
        }
    }

    public override void ResetGame()
    {
        iterator = 1;
        numOfCorrectClicks = 0;
        
        base.ResetGame();
    }

    private void CarryOn()
    {
        iterator++;
        numOfCorrectClicks++;
    }

    //gives the order of solving
    private void GenerateUniqueNumbers()
    {
        List<int> numbers = new List<int>();

        for (int i = 1; i <= Switches.Length; i++)
        {
            numbers.Add(i);
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            int temp = numbers[i];
            int randomIndex = UnityEngine.Random.Range(i, numbers.Count);
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        for (int i = 0; i < Switches.Length; i++)
        {
            Switches[i].SetId(numbers[i]);
        }
    }

    private bool IsCorrectClicked(int id) => id == iterator;
}
