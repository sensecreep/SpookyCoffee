using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float defaultSpeed = 7;

    public float groundDrag;

    public float jumpForce;
    public float JumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    private string currentDirection;

    public float runSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode speedKey = KeyCode.LeftShift;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        //Handle Drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), JumpCooldown);
        }

        if(Input.GetKey(speedKey) && grounded)
        {
            moveSpeed = runSpeed;
        }    
       
        else
        {
            moveSpeed = defaultSpeed; 
        }
    }

    private void MovePlayer()
    {
        // Calculate Movement Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On ground
        if (grounded)
        {
            if (Input.GetKey(speedKey))
            {
                // Set run speed
                rb.velocity = new Vector3(moveDirection.normalized.x * runSpeed, rb.velocity.y, moveDirection.normalized.z * runSpeed);
            }
            else
            {
                // Set normal speed
                rb.velocity = new Vector3(moveDirection.normalized.x * moveSpeed, rb.velocity.y, moveDirection.normalized.z * moveSpeed);
            }
        }
        else
        {
            // In air, apply movement with air multiplier
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }



    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 

        //limit velocity if needed 
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        if(Input.GetKey(speedKey))
        {
            Vector3 limitedRunVel = flatVel.normalized * runSpeed;
            rb.velocity = new Vector3(limitedRunVel.x, rb.velocity.y, limitedRunVel.z);
        }

    }

    private void Jump()
    {
        float currentXVelocity = rb.velocity.x;
        float currentZVelocity = rb.velocity.z;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        //reset y velocity 
        rb.velocity = new Vector3(currentXVelocity, rb.velocity.y, rb.velocity.z);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
