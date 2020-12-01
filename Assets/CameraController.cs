using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 Offset;
    public Transform CameraTarget;
    public Camera PlayerCamera;

    float zoomSpeed = 25f;
    public LayerMask LayerMask;
    List<MazeWall> hidden = new List<MazeWall>();
    [SerializeField] private GameObject Inventory; 

    private void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            PlayerCamera.fieldOfView = Mathf.Clamp(PlayerCamera.fieldOfView - scroll * zoomSpeed, 15f, 100f);
        }

        transform.position = CameraTarget.position + Offset;

        if (Input.GetKeyDown(KeyCode.I))
            Inventory.SetActive(!Inventory.activeSelf);
    }
}