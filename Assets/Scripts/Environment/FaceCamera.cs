using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FaceCamera : MonoBehaviour
{
    private void Update()
    {
        if (!Application.isPlaying && Camera.current == null) return;
        Transform camTransform = Application.isPlaying ? Camera.main.transform : Camera.current.transform;
        Vector3 camForward = camTransform.forward;
        
        camForward.y = 0;

        transform.rotation = Quaternion.LookRotation(camForward, Vector3.up);
    }
}
