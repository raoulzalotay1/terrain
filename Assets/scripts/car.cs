using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{
    public float Motor;

    public float Steer;

    public float Break;

    public WheelCollider Front_Left;

    public WheelCollider Front_Right;

    public WheelCollider Back_Left;

    public WheelCollider Back_Right;

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * this.Steer;

        this.Front_Left.steerAngle = h;
        this.Front_Right.steerAngle = h;

        if (Input.GetKey(KeyCode.S))
        {
            this.Back_Left.brakeTorque = this.Break;
            this.Back_Right.brakeTorque = this.Break;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            this.Back_Left.brakeTorque = 0;
            this.Back_Right.brakeTorque = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.Back_Left.motorTorque = this.Motor;
            this.Back_Right.motorTorque = this.Motor;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            this.Back_Left.motorTorque = 0;
            this.Back_Right.motorTorque = 0;
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            this.Back_Left.brakeTorque = this.Break;
            this.Back_Right.brakeTorque = this.Break;
        }
        else
        {
            this.Back_Left.brakeTorque = 0;
            this.Back_Right.brakeTorque = 0;
        }
    }
}
