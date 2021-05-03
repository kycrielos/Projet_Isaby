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
    public Vector3 slideDirection;
    public float slideSpeed;
    public float acceleration;
    public float deceleration;

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
    private PlayerPhysics physics;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        physics = GetComponent<PlayerPhysics>();
        GameManager.Instance.player = gameObject;
        GameManager.Instance.anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PauseButton") /*&& GameManager.Instance.pauseSecurity*/)
        {
            GameManager.Instance.pause = !GameManager.Instance.pause;
            if (GameManager.Instance.pause)
            {
                Time.timeScale = 0f;
                GameManager.Instance.RefreshUIActivation();
            }
            else
            {
                Time.timeScale = 1f;
                GameManager.Instance.RefreshUIActivation();
            }
        }
        if (GameManager.Instance.currentState != GameManager.PlayerState.Die && !GameManager.Instance.pause)
        {
            Jump();
            Move();
        }
        Debug.Log(GameManager.Instance.playerSpeedScale);
    }

    //Deplace Le joueur
    void Move()
    {
        GetInput();
        MovementCalcul();

        moveDirection = new Vector3(movementx, 0, movementy);
        Vector3.Normalize(moveDirection);
        //Debug.Log(GameManager.Instance.currentState);

        //Deplace Le Joueur
        if (GameManager.Instance.currentState == GameManager.PlayerState.Sliding)
        {
            slideDirection.x = ((1f - physics.hitNormal.y) * physics.hitNormal.x) * slideSpeed;
            slideDirection.z = ((1f - physics.hitNormal.y) * physics.hitNormal.z) * slideSpeed;
            controller.Move(new Vector3(slideDirection.x, 0, slideDirection.z));
        }
        else if (moveDirection.magnitude >= 0.1f)
        {
            if (GameManager.Instance.isGrounded && GameManager.Instance.currentState != GameManager.PlayerState.Jumping)
            {
                if (Input.GetButton("Sprint"))
                {
                    if (speed < sprintSpeed)
                    {
                        speed += acceleration * Time.deltaTime;
                    }
                    else
                    {
                        speed = sprintSpeed;
                    }
                    GameManager.Instance.playerSpeedScale = (speed - walkSpeed)/(sprintSpeed - walkSpeed);
                    GameManager.Instance.currentState = GameManager.PlayerState.Running;
                    GameManager.Instance.RefreshAnimation();
                }
                else
                {
                    if (speed > walkSpeed)
                    {
                        speed -= deceleration * Time.deltaTime;
                    }
                    else
                    {
                        speed = walkSpeed;
                    }
                    GameManager.Instance.playerSpeedScale = (speed - walkSpeed) / (sprintSpeed - walkSpeed);
                    GameManager.Instance.currentState = GameManager.PlayerState.Walking;
                    GameManager.Instance.RefreshAnimation();
                }
            }
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move((moveDir * speed  + jumpDirection) * Time.deltaTime);
        }
        else if (GameManager.Instance.isGrounded && GameManager.Instance.currentState != GameManager.PlayerState.Jumping)
        {
            GameManager.Instance.currentState = GameManager.PlayerState.Idle;
            GameManager.Instance.RefreshAnimation();
            speed = walkSpeed;
        }
        else
        {
            controller.Move(jumpDirection * Time.deltaTime);
            speed = walkSpeed;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && GameManager.Instance.currentState != GameManager.PlayerState.Jumping && physics.timeSinceGrounded <= jumpDelay)
        {
            GameManager.Instance.currentState = GameManager.PlayerState.Jumping;
            GameManager.Instance.RefreshAnimation();
            jumpInput = 1;
            physics.timeSinceGrounded = 0;
        }

        if (GameManager.Instance.currentState == GameManager.PlayerState.Jumping)
        {
            if (Input.GetButton("Jump") && physics.timeSinceGrounded <= jumpDuration)
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
            else if (!physics.sliding)
            {
                GameManager.Instance.currentState = GameManager.PlayerState.Falling;
                GameManager.Instance.RefreshAnimation();
                jumpInput = 0;
            }
        }
        jumpMovement = jumpInput * jumpInput * jumpForce + physics.gravityForce;
        jumpDirection = new Vector3(0, jumpMovement, 0);
    }


    void GetInput()
    {
        if (GameManager.Instance.currentState == GameManager.PlayerState.Falling || GameManager.Instance.currentState == GameManager.PlayerState.Jumping)
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
