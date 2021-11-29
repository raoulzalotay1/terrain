using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRot = 0f;

    private bool IsMouseLooked;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsMouseLooked = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * this.mouseSensitivity * Time.fixedDeltaTime;

        this.xRot -= mouseY;
        this.xRot = Mathf.Clamp(this.xRot, -90, 90);

        this.transform.localRotation = Quaternion.Euler(this.xRot, 0, 0);
        this.playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsMouseLooked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                IsMouseLooked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                IsMouseLooked = true;
            }
        }
    }
}
