using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    private Transform frontPart;
    private Transform backPart;
    private Vector3 offsetPosition;
    private bool isLerping;
    public int wireID;
    public int[] adjacentWires; // Adjacency array for wireID

    private void Awake()
    {

        

    }

    private void Update()
    {
        for(int i  = 0; i < transform.childCount; i++)
        {
            //getting child cuz wires only got child .
            Transform child = transform.GetChild(i);
            ConnectionWire connectedWire = child.GetComponent<ConnectionWire>();
            //connection wire basically connects with other wires.

            if(connectedWire != null )
            {
                int connectedID = connectedWire.connectedWireID; // Check ConnectionWire Script it gets the connected id of the parent of the connection wire on trigger method
                
                 if (!IsInArray(adjacentWires, connectedID))
                {
                    // Resize array and add the new connected wire ID
                    int newArrayLength = adjacentWires.Length + 1;
                    int[] newArray = new int[newArrayLength];
                    adjacentWires.CopyTo(newArray, 0);
                    newArray[newArrayLength - 1] = connectedID;
                    adjacentWires = newArray;
                }



            }
        }
    }


    //method to check if element in array
    bool IsInArray(int[] array, int value)
    {
        foreach (int item in array)
        {
            if (item == value)
                return true;
        }
        return false;
    }


    public Vector3 GetOffsetPosition()
    {
        return offsetPosition;
    }
    public void setRotating(bool rotating)
    {
        isLerping = rotating;
    }

    public bool getRotating()
    {
        return isLerping;
    }
}