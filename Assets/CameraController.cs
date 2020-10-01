using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float PlayerCameraDistance = 12f;
    public Transform CameraTarget;
    public Camera playerCamera;

    float zoomSpeed = 25f;

    private void Start()
    {
        transform.Rotate(45f, 0f, 0f);
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView - scroll * zoomSpeed, 15f, 100f);
        }

        transform.position = new Vector3(CameraTarget.position.x, CameraTarget.position.y +PlayerCameraDistance, CameraTarget.position.z - PlayerCameraDistance);
    }
}
