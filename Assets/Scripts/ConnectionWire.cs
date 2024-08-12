using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionWire : MonoBehaviour
{    // Start is called before the first frame update
    public int connectedWireID; // the id we need to make the adjacency list



    void OnTriggerStay(Collider other)
    {
        ConnectionWire connectedConnectionPoint = other.gameObject.GetComponent<ConnectionWire>(); //gets the other component connection wire and then 
        //gets his parent

        if(connectedConnectionPoint != null)
        {

            Wire wire = connectedConnectionPoint.gameObject.GetComponentInParent<Wire>(); //getting his parent if it wire

            //since rotatedWire has multiple childs so it has different getting procedure
            RotatedWire rotatedWire = connectedConnectionPoint.gameObject.transform.parent.GetComponentInParent<RotatedWire>(); 

            if(wire != null)
            {
                connectedWireID = wire.wireID;
            }
            else if (rotatedWire != null)
            {
                connectedWireID = rotatedWire.wireID;
            }


            //assigning above
        }
    }


    private void OnTriggerExit(Collider other)
    {

        //reseting values of array and initializing it again each time a wire rotates
        ConnectionWire connectedConnectionPoint = other.gameObject.GetComponent<ConnectionWire>();

        if (connectedConnectionPoint != null)
        {
            Wire wire = connectedConnectionPoint.gameObject.GetComponentInParent<Wire>();
            RotatedWire rotatedwire = other.gameObject.transform.transform.parent.GetComponentInParent<RotatedWire>();

            if (wire != null)

            {
                for (int i = 0; i < wire.adjacentWires.Length; i++)
                {
                    connectedWireID = 0;
                    wire.adjacentWires[i] = 0;
                    wire.adjacentWires = new int[1];
                }



            }
            else if (rotatedwire != null)
            {
                for(int i = 0; i < rotatedwire.adjacentWires.Length; i++ )
                {
                    connectedWireID = 0;
                    rotatedwire.adjacentWires[i] = 0;
                    rotatedwire.adjacentWires = new int[1];
                }

            }
        }


    }
}
