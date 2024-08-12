using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedWire : MonoBehaviour
{
    private bool isLerping;
    public int wireID;
    public int[] adjacentWires; // Adjacency array for wireID

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            for(int j  = 0; j < transform.GetChild(i).childCount; j++)
            {          
                ConnectionWire connectionWire = transform.GetChild(i).GetChild(j).GetComponent<ConnectionWire>();

                if(connectionWire != null)
                {
                    int connectedWireId = connectionWire.connectedWireID;
                    
                    if (!IsInArray(adjacentWires, connectedWireId))
                    {
                        // Resize array and add the new connected wire ID
                        int newArrayLength = adjacentWires.Length + 1;
                        int[] newArray = new int[newArrayLength];
                        adjacentWires.CopyTo(newArray, 0);
                        newArray[newArrayLength - 1] = connectedWireId;
                        adjacentWires = newArray;
                    }

                }
            }
        }


    }

    public void setRotating(bool rotating)
    {
        isLerping = rotating;
    }

    public bool getRotating()
    {
        return isLerping;
    }





    bool IsInArray(int[] array, int value)
    {
        foreach (int item in array)
        {
            if (item == value)
                return true;
        }
        return false;
    }

}
