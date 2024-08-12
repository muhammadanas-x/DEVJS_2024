using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private float LookIntervalSeconds = 0.5f;

    [SerializeField] private float UpdateDestinationInterval = .2f;

    [SerializeField] private int FieldOfViewAngle = 90;

    [SerializeField] private float MaxLookDistance = 50;

    [SerializeField] private GameObject PatrolDestination;

    [SerializeField] private float BaseSpeed = 3.5f;

    [SerializeField] private float ArgoMultiplier = 1.3f;

    [SerializeField] private float PowerSpeedAddition = 0.5f;

    private NavMeshAgent agent;

    private GameObject player;

    private bool playerIsInLineOfSight;
    public bool agroPlayer;

    private const float setDestinationInterval = 0.5f;

    private Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


        StartCoroutine(LookForPlayer());
        StartCoroutine(TrackPlayer());

        this.StartCoroutine(SetPatrolDestination());



    }

    private void Update() 
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", agent.desiredVelocity.magnitude > 0);
        }

        //Debug.Log(agroPlayer);
    }

    private IEnumerator SetPatrolDestination()
    {
        while (true)
        {
            if (!agroPlayer)
            {
                agent.SetDestination(PatrolDestination.transform.position);
            }

            yield return new WaitForSeconds(setDestinationInterval);
        }
    }

    private IEnumerator TrackPlayer()
    {
        while (true) {
            if (agroPlayer)
            {
                agent.SetDestination(player.transform.position);
                PatrolDestination.transform.position = player.transform.position;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator LookForPlayer()
    {
        while (true)
        {
            // Calculate the direction from the game object to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // Determine the angle between the forward direction of the game object and the direction to the player
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            // Check if the player is within the field of view
            if (angle < FieldOfViewAngle)
            {
                // Normalize the direction vector
                //directionToPlayer.Normalize();

                // Cast a ray from the game object towards the player
                RaycastHit hit;

                // Check if the ray hit the player
                var castHit = Physics.Raycast(transform.position, directionToPlayer, out hit, MaxLookDistance);
                if (castHit)
                { 
                    agroPlayer = hit.collider.tag == "Player";
                }
            }
            else
            {
                // If the player is behind the monster, they shouldn't be seen
                agroPlayer = false;
            }

            agent.speed = agroPlayer ? this.BaseSpeed * this.ArgoMultiplier : this.BaseSpeed;

            yield return new WaitForSeconds(LookIntervalSeconds);
        }
    }

    public void SetDestination(Vector3 position)
    { 
        PatrolDestination.transform.position = position;
    }
}
