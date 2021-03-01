using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Input
    private float inputx;
    private float inputy;

    //Movement
    private float movementx;
    private float movementy;
    public float speed = 1;
    public float sprintSpeed;
    public float walkSpeed;
    public float inetie;
    Vector3 moveDirection;

    //Jump
    Vector3 jumpDirection;
    private float jumpInput;
    private float jumpMovement;
    public float jumpForce = 1;
    public float jumpAcceleration = 1;
    public float jumpSmoothAcceleration = 1;
    public float jumpDuration = 1;
    public float jumpDelay = 0.5f;
    public float jumpCD = 0.1f;

    //Rotate
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    //Component
    private CharacterController controller;
    public GameObject cam;

    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Falling,
    }

    public static PlayerState currentState = PlayerState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
    }

    //Deplace Le joueur
    void Move()
    {
        GetInput();
        MovementCalcul();

        moveDirection = new Vector3(movementx, 0, movementy);
        Vector3.Normalize(moveDirection);

        //Deplace Le Joueur
        if (moveDirection.magnitude >= 0.1f)
        {
            if (PlayerPhysics.isGrounded && currentState != PlayerState.Jumping)
            {
                currentState = PlayerState.Walking;
            }
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move((moveDir * speed  + jumpDirection) * Time.deltaTime);
        }
        else if (PlayerPhysics.isGrounded && currentState != PlayerState.Jumping)
        {
            currentState = PlayerState.Idle;
        }
        else
        {
            controller.Move(jumpDirection * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && currentState != PlayerState.Jumping && PlayerPhysics.timeSinceGrounded <= jumpDelay)
        {
            currentState = PlayerState.Jumping;
            jumpInput = 1;
            PlayerPhysics.timeSinceGrounded = 0;
        }

        if (currentState == PlayerState.Jumping)
        {
            if (Input.GetButton("Jump") && PlayerPhysics.timeSinceGrounded <= jumpDuration)
            {
                if (jumpInput > 0)
                {
                    jumpInput -= Time.deltaTime * jumpAcceleration;
                }
                
                if (jumpInput <= 0)
                {
                    jumpInput = 0;
                }

            }
            else if (jumpInput > 0)
            {
                jumpInput -= Time.deltaTime * jumpAcceleration * jumpSmoothAcceleration;
            }
            else
            {
                currentState = PlayerState.Falling;
                jumpInput = 0;
            }
        }
        jumpMovement = jumpInput * jumpInput * jumpForce + PlayerPhysics.gravityForce;
        jumpDirection = new Vector3(0, jumpMovement, 0);
    }


    void GetInput()
    {
        if (currentState == PlayerState.Falling || currentState == PlayerState.Jumping)
        {
            if (inputx != 0)
            {
                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    inputx -= (Time.deltaTime * inetie) * (inputx / Mathf.Abs(inputx));
                }
                else
                {
                    inputx = Input.GetAxis("Horizontal");
                }
            }
            else
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
                {
                    inputx += (Time.deltaTime * inetie) * (inputx / Mathf.Abs(inputx));
                }
                else
                {
                    inputx = 0;
                }
            }

            if (inputy != 0)
            {
                if (Input.GetAxisRaw("Vertical") == 0)
                {
                    inputy -= (Time.deltaTime * inetie) * (inputy / Mathf.Abs(inputy));
                }
                else
                {
                    inputy = Input.GetAxis("Vertical");
                }
            }
            else
            {
                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
                {
                    inputy += (Time.deltaTime * inetie) * (inputy / Mathf.Abs(inputy));
                }
                else
                {
                    inputy = 0;
                }
            }
        }
        else
        {
            inputx = Input.GetAxis("Horizontal");
            inputy = Input.GetAxis("Vertical");
        }

        if (Input.GetButton("Sprint"))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    //Mets en place la courbe de easing
    void MovementCalcul()
    {
        if (inputx != 0)
        {
            movementx = (1 - Mathf.Pow(1 - Mathf.Abs(inputx), 5)) * inputx / Mathf.Abs(inputx);
        }
        else
        {
            movementx = 0;
        }
        if (inputy != 0)
        {
            movementy = (1 - Mathf.Pow(1 - Mathf.Abs(inputy), 5)) * inputy / Mathf.Abs(inputy);
        }
        else
        {
            movementy = 0;
        }
    }
}
