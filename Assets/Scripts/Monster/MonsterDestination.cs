using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
public class MonsterDestination : MonoBehaviour
{
    #region Parameters
    public float Range = 45.0f; // Range within which to find a random point
    public float MinDistance = 5.0f;
    [SerializeField] private GameObject player;
    [SerializeField] private float MaxDistanceFromPlayer;
    [SerializeField] private float GiveUpTimeSeconds = 5;
    [SerializeField] private float MinDistanceFromPlayer = 0.8f;
    #endregion

    #region Fields
    private float GiveUpTimer;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter(Collider other)
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        // If the monster gets inside the trigger box, then reposition
        // Don't reposition if the monster is close to the player
        if (other.tag == "Monster" && dist > MinDistanceFromPlayer)
        {
            Reposition();

        }
    }

    private void Update()
    {
        GiveUpTimer -= Time.deltaTime;
        if (GiveUpTimer <= 0)
        { 
            //Reposition();
        }
    }
    #endregion

    #region Methods
    public Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Vector3.zero;

        NavMeshHit hit;
        bool pointFound = false;

        while (!pointFound)
        {
            randomPoint = transform.position + Random.insideUnitSphere * Range;
            
            if (NavMesh.SamplePosition(randomPoint, out hit, Range, NavMesh.AllAreas))
            {
                var distanceFromPlayer = Vector3.Distance(player.transform.position, hit.position);
                float distance = Vector3.Distance(hit.position, transform.position);
                if (distance >= MinDistance && distanceFromPlayer <= MaxDistanceFromPlayer)
                {
                    pointFound = true;
                    randomPoint = hit.position;
                }
            }
        }

        return randomPoint;
    }

    // Reposition the destination to a random point on the nav mesh
    void Reposition()
    {
        GiveUpTimer = GiveUpTimeSeconds;
        gameObject.transform.position = GetRandomPoint();
    }
    #endregion
}
