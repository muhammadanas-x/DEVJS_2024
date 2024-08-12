using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public TextMeshPro text;
    public int num;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }




    public void SetNumber(int number)
    {
        num = number;
    }


    public int GetNumber()
    {
        return num;
    }
}
