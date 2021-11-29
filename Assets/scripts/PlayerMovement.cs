using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;

    private Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;

    public float groundDistant = .4f;
    public LayerMask GroundMask;

    private bool isOnGround;
    void Update()
    {
        this.isOnGround = Physics.CheckSphere(this.groundCheck.position, this.groundDistant, this.GroundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = this.transform.right * x + this.transform.forward * z;

        CharacterController controller = this.GetComponent<CharacterController>();

        controller.Move(move * this.speed * Time.deltaTime);

        if (this.isOnGround && this.velocity.y < 0)
        {
            this.velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && this.isOnGround)
        {
            this.velocity.y = Mathf.Sqrt(this.jumpHeight * -2f * this.gravity);
        }

        this.velocity.y += this.gravity * Time.deltaTime;

        controller.Move(this.velocity * Time.deltaTime);
    }
}
