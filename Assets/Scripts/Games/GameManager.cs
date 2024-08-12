using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Static instance

    public int maxLevel = 3; // Maximum power level
    public int TargetPowerLevel = 0; // The power level the monster is trying to reach

    // Reference to the player game object
    public GameObject Player; 

    // Reference to the monster game object
    private GameObject Monster;

    public Slider progressBar; // Reference to the progress bar UI element

    private void Awake()
    {
        // Ensure there's only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
            Player = GameObject.FindWithTag("Player");
            Monster = GameObject.FindWithTag("Monster");
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateProgressBar();
    }

    // Called when a puzzle is complete
    public void PuzzleComplete()
    {
        if (TargetPowerLevel < maxLevel)
        {
            TargetPowerLevel++;
            UpdateProgressBar();
            PowerUpMonster();
        }
    }

    public void PowerUpMonster()
    { 
        Monster.GetComponent<Monster>().PowerLevel = TargetPowerLevel;
    }

    // Updates the progress bar UI element
    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            // Update the progress bar value based on the power level
            progressBar.value = (float)TargetPowerLevel / maxLevel;
        }
    }
}
