
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGameManager : MonoBehaviour
{
    public GameObject wirePrefab; //this has wire script
    public GameObject rotatedWirePrefab; // this has rotatewire script
    public GameObject rotatedWirePrefab2; // this has rotateWire script
    private GameObject[] wires;


    public bool solvedAlready = false;

    private Dictionary<int, List<int>> adjacencyList;


    private bool isRotating = false;

    public Transform puzzleParent;
    private void Start()
    {



       
        adjacencyList = new Dictionary<int, List<int>>();

        wires = GameObject.FindGameObjectsWithTag("Wire");

        MakeAdjacencyListForBFS();

    }


    private void Update()
    {
        if (solvedAlready) return;

        MakeAdjacencyListForBFS();


        if (!isRotating && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider is attached to this GameObject
                if (hit.collider != null)
                {
                    RotatedWire rotatedWire = hit.collider.gameObject.GetComponent<RotatedWire>();
                    Wire wire = hit.collider.gameObject.GetComponent<Wire>();

                    

                    if (rotatedWire != null  && !rotatedWire.getRotating())
                    {
                        //animating
                        rotatedWire.setRotating(true);
                        StartCoroutine(RotateObject(hit.collider.gameObject));
                    }
                    else if(wire != null && !wire.getRotating())
                    {
                        //animating
                        wire.setRotating(true);
                        StartCoroutine(RotateObject(hit.collider.gameObject));
                    }



                  
                }
            }
        }




      




        List<int> ans = GetConnectedWires(adjacencyList);
        HashSet<int> connectedWireIDs = new HashSet<int>(ans);

        for (int i = 0; i < 16; i++)
        {
            Wire tempWire = wires[i].GetComponent<Wire>();
            RotatedWire tempRotatedWire = wires[i].GetComponent<RotatedWire>();

            if (tempWire != null)
            {
                if (connectedWireIDs.Contains(tempWire.wireID))
                {
                    tempWire.GetComponent<Renderer>().material.color = Color.cyan;
                }
                else
                {
                    tempWire.GetComponent<Renderer>().material.color = Color.white; // Revert color to white
                }
            }
            else if (tempRotatedWire != null)
            {
                if (connectedWireIDs.Contains(tempRotatedWire.wireID))
                {
                    for (int j = 0; j < tempRotatedWire.transform.childCount; j++)
                    {
                        tempRotatedWire.transform.GetChild(j).gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                    }
                }
                else
                {
                    for (int j = 0; j < tempRotatedWire.transform.childCount; j++)
                    {
                        tempRotatedWire.transform.GetChild(j).gameObject.GetComponent<Renderer>().material.color = Color.white; // Revert color to white
                    }
                }
            }


        }


    }


    List<int> GetConnectedWires(Dictionary<int, List<int>> adjList)
    {
        // Implement BFS to find all connected wires starting from wire ID 1
        Queue<int> queue = new Queue<int>();
        HashSet<int> visited = new HashSet<int>();
        Dictionary<int, int> parent = new Dictionary<int, int>(); // To store parent wire ID

        // Start BFS from wire ID 1
        queue.Enqueue(1);
        visited.Add(1);

        List<int> connectedWires = new List<int>();

        while (queue.Count > 0)
        {
            int currentWireID = queue.Dequeue();

            if (currentWireID == 16)
            {
                Debug.Log("Reached destination: " + 16);
                GameManager.Instance.PuzzleComplete();
                solvedAlready = true;
                break; 
            }

            connectedWires.Add(currentWireID);

            // Explore adjacent wires
            if (adjList.ContainsKey(currentWireID))
            {
                List<int> adjacentWires = adjList[currentWireID];
                foreach (int adjacentWireID in adjacentWires)
                {
                    if (!visited.Contains(adjacentWireID))
                    {
                        queue.Enqueue(adjacentWireID);
                        visited.Add(adjacentWireID);
                        parent[adjacentWireID] = currentWireID; // Set parent of adjacent wire
                    }
                }
            }
        }

        return connectedWires;
    }




    void MakeAdjacencyListForBFS()
    {
        foreach (GameObject wireObject in wires)
        {
            Wire wire = wireObject.GetComponent<Wire>();
            RotatedWire rotatedWire = wireObject.GetComponent<RotatedWire>();

            int wireID = 0;
            List<int> adjacentWiresList = new List<int>(); // Initialize the list for adjacent wires

            if (wire != null)
            {
                wireID = wire.wireID;

                foreach (Transform child in wire.transform)
                {
                    ConnectionWire connectionWire = child.GetComponent<ConnectionWire>();

                    if (connectionWire != null)
                    {
                        adjacentWiresList.Add(connectionWire.connectedWireID);
                    }
                }
            }
            else if (rotatedWire != null)
            {
                wireID = rotatedWire.wireID;

                foreach (Transform child in rotatedWire.transform)
                {
                    foreach (Transform subChild in child)
                    {
                        ConnectionWire connectionWire = subChild.GetComponent<ConnectionWire>();

                        if (connectionWire != null)
                        {
                            adjacentWiresList.Add(connectionWire.connectedWireID);
                        }
                    }
                }
            }

            // Add the adjacent wires list to the adjacency list

            if (adjacencyList.ContainsKey(wireID))
            {
                // Update the existing inner list with the new adjacent wires
                adjacencyList[wireID] = adjacentWiresList;
            }
            else
            {
                // Add the wire ID and its adjacent wires list to the dictionary
                adjacencyList.Add(wireID, adjacentWiresList);
            }
        }


        foreach (var entry in adjacencyList)
        {
            int wireID = entry.Key;
            List<int> adjacentWires = entry.Value;

            string adjacentWiresStr = string.Join(", ", adjacentWires);
        }
    }


    IEnumerator RotateObject(GameObject Object)
    {
        isRotating = true;
        Quaternion startRotation = Object.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f) * startRotation;

        float duration = 1.0f; // Adjust as needed

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Object.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);

            yield return null;
        }


        //from here inefficient
        RotatedWire test = Object.GetComponent<RotatedWire>();
        Wire testWire = Object.GetComponent<Wire>();

        //inefficent code need to fix.
        if(test != null)
        {
            test.setRotating(false);
        }
        else if(testWire != null)
        {
            testWire.setRotating(false);
        }
       
        //To here
        Object.transform.rotation = targetRotation;
       
        isRotating = false;
    }

}