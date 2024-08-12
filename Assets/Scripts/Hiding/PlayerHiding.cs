using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerHiding : MonoBehaviour
{
    public GameObject door;
    public GameObject player;
    private Transform doorPivot;
    public Transform positionToStand;


    [SerializeField] private float rotationSpeed;

    private bool isHiding = false; // Flag to indicate if the player is hiding
    private bool insidedoor = false; // Flag to indicate if the player is insidedoor
    private bool isDoorLerping = false; //Flag to indicate that if the Door rotation is lerping

    // Start is called before the first frame update
    void Start()
    {
        door = gameObject;
        doorPivot = door.transform.parent;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (CanHide())
        {
            isHiding = true;
            StartCoroutine(HideCoroutine());
            StartCoroutine(HidePlayer());
        }
        else if(CanGetOut())
        {
            StartCoroutine(GetOutCoroutine());
            GetOutPlayer();
        }


    }


    public bool CanGetOut()
    {
        return Input.GetKeyDown(KeyCode.E) && isHiding && insidedoor && !isDoorLerping;
    }


    public bool CanHide()
    {
        return Vector3.Distance(player.transform.position, door.transform.position) < 2f && Input.GetKeyDown(KeyCode.E) && !isHiding && !insidedoor && !isDoorLerping;
    }


    IEnumerator HidePlayer()
    {
        player.GetComponent<PlayerController>().isLerping = true;
        Quaternion startRotation = player.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(positionToStand.transform.right);

        Vector3 startPosition = player.transform.position;
        Vector3 finalPosition = positionToStand.position;

        float elapsedTime = 0f;

        while(elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            player.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            player.transform.position = Vector3.Lerp(startPosition, finalPosition, elapsedTime);
            yield return null;

        }
        player.GetComponent<PlayerController>().LookTowards(positionToStand.transform.right);
        player.GetComponent<PlayerController>().isLerping = false;


    }


    public void GetOutPlayer()
    {

    }

    IEnumerator HideCoroutine()
    {
        Quaternion startRotation = doorPivot.transform.rotation;
        Quaternion targetRotation = new Quaternion(0f, 0f, 0f, 0f);

        Quaternion localRotation = startRotation * targetRotation;


        float elapsedTime = 0f;
        while (elapsedTime < 1f) // 1f represents the duration of rotation
        {
            isDoorLerping = true;

            elapsedTime += Time.deltaTime;
            doorPivot.transform.rotation = Quaternion.Slerp(startRotation, localRotation, elapsedTime);
            yield return null;
        }
        isDoorLerping = false;


        insidedoor = true;

    }


    IEnumerator GetOutCoroutine()
    {

        Quaternion startRotation = doorPivot.transform.rotation;

        Quaternion targetRotation = new Quaternion(0f, 90f, 0f, 0f);
        Quaternion localRotation = Quaternion.Inverse(startRotation) * targetRotation;


        float elapsedTime = 0f;

        while(elapsedTime < 1f)
        {
            isDoorLerping = true;
            elapsedTime += Time.deltaTime;
            doorPivot.transform.rotation = Quaternion.Slerp(startRotation, localRotation, elapsedTime);
            yield return null;
        }
        isDoorLerping = false;
        insidedoor = false;
        isHiding = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
    }

}
