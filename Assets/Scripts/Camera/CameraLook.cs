using System;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private GameObject player = null;

    [SerializeField] private float xSens = 200f, ySens = 200f;
    
    private float xRotation = 0f;

    private bool escape = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float xAngle = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime;
        float yAngle = Input.GetAxis("Mouse X") * xSens * Time.deltaTime;

        xRotation -= xAngle;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        
        transform.localRotation = Quaternion.Euler(xRotation,0,0);
        player.transform.Rotate(0f,yAngle,0f);

        bool cursor = Input.GetKeyDown(KeyCode.Escape);

        if (cursor)
        {
            escape = !escape;
        }

        if (escape)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
