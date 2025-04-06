using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reset the default velocity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //getting inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Creating the moving vector
        Vector3 move = transform.right * x + transform.forward * z;

        //Actually mooving the character
        controller.Move(move * speed * Time.deltaTime);

        //jump check
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //falling down
        velocity.y += gravity * Time.deltaTime;

        //Actually doing the jumping
        controller.Move(velocity * Time.deltaTime);

        if(lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
            //for later use
        }
        else
        {
            isMoving = false;
            //flor later use
        }

        lastPosition = gameObject.transform.position;
    }
}
