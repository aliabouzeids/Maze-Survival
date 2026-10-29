using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera targetCamera;

    void LateUpdate()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        // Make the canvas face the camera
        transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward,
                         targetCamera.transform.rotation * Vector3.up);
    }
}