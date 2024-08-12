using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolDestination : MonoBehaviour
{
    // Make sure this value is always bigger then the map
    [SerializeField] private int MapSize = 200;
    [SerializeField] private float MinumumPatrolDistance = 10;
    [SerializeField] private float MaximumDistanceFromPlayer = 10;

    private GameObject player;
    private MonsterController monster;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the monster gets inside the trigger box, then reposition
        if (other.tag == "Monster" && !monster.agroPlayer)
        {
            gameObject.transform.position = GetRandomPoint();
        }
    }

    public Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Vector3.zero;

        NavMeshHit hit;
        bool pointFound = false;

        while (!pointFound)
        {
            randomPoint = transform.position + Random.insideUnitSphere * MapSize;

            if (NavMesh.SamplePosition(randomPoint, out hit, MapSize, NavMesh.AllAreas))
            {
                var distanceFromPlayer = Vector3.Distance(player.transform.position, hit.position);
                float distance = Vector3.Distance(hit.position, transform.position);
                if (distance >= MinumumPatrolDistance && distanceFromPlayer <= MaximumDistanceFromPlayer)
                {
                    pointFound = true;
                    randomPoint = hit.position;
                }
            }
        }

        return randomPoint;
    }
}
