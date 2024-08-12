using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    const float MinFallingSpeed = -.5f;
    #region Parameters
    [SerializeField] private float Speed = 8f;
    [SerializeField] private float SprintMultiplier = 1.2f;
    [SerializeField] private float downwardsAcceleration = -9.8f;
    public bool InputOverridden;
    #endregion

    #region Fields
    private PlayerController controller;
    private CharacterController characterController;
    private float fallingSpeed = MinFallingSpeed;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (characterController.isGrounded) 
            fallingSpeed = MinFallingSpeed;
        else 
            fallingSpeed += downwardsAcceleration * Time.deltaTime;

        var movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movementVector.magnitude > 1)
            movementVector = movementVector.normalized;

        float mouseX = Mouse.current.delta.x.ReadValue() * 0.3f;
        float mouseY = Mouse.current.delta.y.ReadValue() * 0.3f;

        //var lookVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var lookVector = new Vector2(mouseX, mouseY);
        var isSprinting = Input.GetAxis("Sprint") > 0;

        controller.Look(lookVector);
        controller.CheckForInteraction();
        movementVector = transform.forward * movementVector.z + transform.right * movementVector.x;
        characterController.Move(Time.deltaTime * Speed * (isSprinting ? SprintMultiplier : 1) * movementVector);
        characterController.Move(fallingSpeed * Time.deltaTime * Vector3.up);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster")
        {
            Die();
        }
    }
    #endregion

    #region Public Methods
    public void Die()
    {
        // Load the death menu
        SceneManager.LoadScene(2);
    }
    #endregion
}
