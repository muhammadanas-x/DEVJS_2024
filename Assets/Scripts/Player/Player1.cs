using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player1 : MonoBehaviour
{
    #region Parameters
    #endregion

    #region Fields
    private PlayerController controller;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        Cursor.visible = false;
    }

    private void Update()
    {
        var movementVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var lookVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var isSprinting = Input.GetAxis("Sprint") > 0;

        // controller.Move(movementVector, isSprinting);
        controller.Look(lookVector);
        controller.CheckForInteraction();
    }
    #endregion  
}
