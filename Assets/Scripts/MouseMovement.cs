using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;
    void Start()
    {
        //Lock the cursor beacuse it's a fps
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Getting mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotate arround the x axis (Looking up and down)
        xRotation -= mouseY;

        //limmiting the rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Rotate arround the x axis (Loking left and right)
        yRotation += mouseX;

        //Apply rotations to transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
