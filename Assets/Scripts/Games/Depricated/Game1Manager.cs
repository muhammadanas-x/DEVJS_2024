using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game1Manager : MonoBehaviour
{

    public GameObject[] switches;
    public int numOfCorrectClicks;
    public GameObject player;
    private int iterator;
    private int totalSwitches;
    private bool isGameDisabled;
   



    void Start()
    {
        numOfCorrectClicks = 0;
        iterator = 1;
        totalSwitches = switches.Length;
        GenerateUniqueNumbers(switches);

    }




    void Update()
    {


        if (isPuzzleDisabled()) return;
      

        if (Input.GetKeyDown(KeyCode.E))
        {
           
            Switch switchComponent = FindNearestSwitch();

           
            if(switchComponent != null)
            {
                int numberOfSwitch = switchComponent.GetNumber();
               
                    if (isCorrectClicked(numberOfSwitch))
                    {
                        CarryOn(switchComponent);
                    }
                    else
                    {
                        resetGame();
                        ChangeColorBack();


                }



            }

        }
        EnableAndDisableText();


        if (numOfCorrectClicks == totalSwitches)
        {

            DisableText();

            resetGame();
            GameManager.Instance.PuzzleComplete();

            DisableGame();
        }



    }

    
    void CarryOn(Switch switchComponent)
    {
        switchComponent.gameObject.GetComponent<Renderer>().material.color = Color.red;
        iterator++;
        numOfCorrectClicks++;
    }
    bool isCorrectClicked(int numberOfSwitch)
    {
        return numberOfSwitch == iterator;
    }


    void resetGame()
    {
        iterator = 1;
        numOfCorrectClicks = 0;


    }

    void ChangeColorBack()
    {
        for (int i = 0; i < totalSwitches; i++)
        {
            switches[i].GetComponent<Renderer>().material.color = Color.white;


        }
    }

    void EnableAndDisableText()
    {


        for (int i = 0; i < totalSwitches; i++)
        {

            if (Vector3.Distance(player.transform.position, switches[i].transform.position) > 2f)
            {
                switches[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                switches[i].transform.GetChild(0).gameObject.SetActive(true);

            }
        }
    }


    void DisableText()
    {
        for(int i = 0; i < totalSwitches; i++)
        {
            switches[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }


    Switch FindNearestSwitch()
    {
        float minDistance = Mathf.Infinity;
        Switch nearest = null;

        foreach (Switch switchObj in FindObjectsOfType<Switch>())
        {
            float distance = Vector3.Distance(player.transform.position, switchObj.transform.position);
            if (distance < minDistance && distance < 3f)
            {
                
                minDistance = distance;
                nearest = switchObj;
               

            }

        }

        return nearest;
    }







    void GenerateUniqueNumbers(GameObject[] switches)
    {
        List<int> numbers = new List<int>();

        for (int i = 1; i <= switches.Length; i++)
        {
            numbers.Add(i);
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            int temp = numbers[i];
            int randomIndex = Random.Range(i, numbers.Count);
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        for (int i = 0; i < switches.Length; i++)
        {
            switches[i].GetComponent<Switch>().SetNumber(numbers[i]);
        }
    }




    void DisableGame()
    {
        isGameDisabled = true;

    }


    bool isPuzzleDisabled()
    {
        return isGameDisabled;
    }
}
