using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    #region Parameters
    [SerializeField] private float LookSensitivity = 1.0f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float RotationLimitY = 88f;
    [Range(0, .3f)][SerializeField] private float MovementSmoothing = .05f;
    [SerializeField] private GameObject CameraObject;
    [SerializeField] private float smoothTime = 0.2f;
    [Header("Interactor")]
    [SerializeField] private float DistanceOfInteration;
    [SerializeField] private KeyCode interactionKey; 
    public bool isLerping = false;
    #endregion

    #region Fields
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector2 rotation = Vector2.zero;

    private Vector2 currentLookingPos;



    private Vector2 smoothV;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    #endregion

    #region Methods
    // public void Move(Vector2 movementVector, bool isSprinting)
    // {
    //     var targetVelocity = new Vector3(movementVector.x, 0 ,movementVector.y) * Speed;
        
    //     if (isSprinting)
    //     {
    //         targetVelocity *= SprintMultiplier;
    //     }

    //     targetVelocity.y = rb.velocity.y;

    //     var localTargetVelocity = transform.TransformDirection(targetVelocity);

    //     rb.velocity = Vector3.SmoothDamp(rb.velocity, localTargetVelocity, ref velocity, MovementSmoothing);
    // }

    public void Look(Vector2 lookVector)
    {
        if (isLerping) return;
        rotation.x += lookVector.x * LookSensitivity;
        rotation.y += lookVector.y * LookSensitivity;

        rotation.y = Mathf.Clamp(rotation.y, -RotationLimitY, RotationLimitY);

        // Smoothly interpolate the rotation values
        currentLookingPos.x = Mathf.SmoothDamp(currentLookingPos.x, rotation.x, ref smoothV.x, smoothTime);
        currentLookingPos.y = Mathf.SmoothDamp(currentLookingPos.y, rotation.y, ref smoothV.y, smoothTime);


        // Apply the smoothed rotation to the camera's transform
        var xQuat = Quaternion.AngleAxis(currentLookingPos.x, Vector3.up);
        transform.localRotation = xQuat;


        var yQuat = Quaternion.AngleAxis(currentLookingPos.y, Vector3.left);
        CameraObject.transform.localRotation = yQuat;

    }

    public void LookTowards(Vector3 direction)
    {
      
        // Calculate the target rotation from the direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Extract the y rotation angle
        float targetYRotation = targetRotation.eulerAngles.y;

        // Extract the x rotation angle
        float targetXRotation = targetRotation.eulerAngles.x;

        currentLookingPos.x = targetYRotation;
        rotation.x = targetYRotation;
        currentLookingPos.y = targetXRotation;
        rotation.y = targetXRotation;
    }

    public void CheckForInteraction()
    {
        if(Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out RaycastHit hitInfo, 5f, 3, QueryTriggerInteraction.UseGlobal))
        {

            if (hitInfo.collider != null && Input.GetKeyDown(interactionKey) && hitInfo.collider.gameObject.CompareTag("Switch"))
            {
                

                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable component))
                {
                    component.Interact();
                }
            }
        }

       
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("LightSwitch"))
        {
            if (collision.gameObject.GetComponent<Renderer>().material.color == Color.red) collision.gameObject.GetComponent<Renderer>().material.color = Color.black;
            else collision.gameObject.GetComponent<Renderer>().material.color = Color.red;

            BaseSwitch baseSwitch = collision.gameObject.GetComponent<BaseSwitch>();
            baseSwitch.Interact();

        }
    }




    #endregion
}
