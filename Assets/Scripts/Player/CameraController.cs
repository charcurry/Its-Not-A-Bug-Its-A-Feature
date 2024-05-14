using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody playerRigidbody;  
    public float mouseSensitivity = 1f;

    public Vector3 vecRelativeRotation;
    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = transform.parent;
            playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        }
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        vecRelativeRotation.x -= mouseY;
        vecRelativeRotation.x = Mathf.Clamp(vecRelativeRotation.x, -90f, 90f);

        vecRelativeRotation.y += mouseX;
        vecRelativeRotation.y = vecRelativeRotation.y % 360.0f;

        if (vecRelativeRotation.y > 180.0f)
            vecRelativeRotation.y -= 360.0f;
        else if (vecRelativeRotation.y < -180.0f)
            vecRelativeRotation.y += 360.0f;
        
        transform.localRotation = Quaternion.Euler(vecRelativeRotation.x, 0f, 0f);

        Quaternion newRotation = Quaternion.Euler(0f, vecRelativeRotation.y, 0f);
        playerRigidbody.MoveRotation(newRotation);
    }


}
