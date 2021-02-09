using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Input
    private float Inputx;
    private float Inputy;

    //Movement
    private float Movementx;
    private float Movementy;
    public float Speed = 1;
    public float Inetie;
    Vector3 MoveDirection;

    //Jump
    Vector3 JumpDirection;
    private float JumpInput = 1;
    private float JumpMovement;
    public float JumpForce = 1;
    public float JumpAcceleration = 1;
    public float JumpDuration = 1;
    public float JumpMinimumDuration = 1;
    public float JumpDelay = 0.5f;
    public float JumpCD = 0.1f;
    public float GravityScale = 1;
    private bool CanJump;

    //Rotate
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    //Component
    private CharacterController controller;
    public GameObject Cam;


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

        MoveDirection = new Vector3(Movementx, 0, Movementy);
        Vector3.Normalize(MoveDirection);

        //Deplace Le Joueur
        if (MoveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg + Cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move((moveDir * Speed  + JumpDirection) * Time.deltaTime);
        }
        else
        {
            controller.Move(JumpDirection * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !CanJump && PlayerPhysics.TimeSinceGrounded <= JumpDelay)
        {
            CanJump = true;
            PlayerPhysics.TimeSinceGrounded = 0;
        }

        if (CanJump)
        {
            if (Input.GetButton("Jump") && PlayerPhysics.TimeSinceGrounded <= JumpDuration || PlayerPhysics.TimeSinceGrounded <= JumpMinimumDuration)
            {
                if (JumpInput > 0)
                {
                    JumpInput -= Time.deltaTime * JumpAcceleration;
                }
                
                if (JumpInput <= 0)
                {
                    JumpInput = 0;
                }

                JumpMovement = (1 - Mathf.Pow(Mathf.Abs(1 - JumpInput), 5)) * JumpForce;
            }
            else
            {
                CanJump = false;
                JumpInput = 1;
            }
        }
        else
        {
            if (JumpMovement > -GravityScale)
            {
                JumpMovement -= Time.deltaTime * GravityScale;
            }
            else
            {
                JumpMovement = -GravityScale;
            }
        }

        JumpDirection = new Vector3(0, JumpMovement , 0);
    }


    void GetInput()
    {
        if (PlayerPhysics.IsGrounded)
        {
            Inputx = Input.GetAxis("Horizontal");
            Inputy = Input.GetAxis("Vertical");
        }
        else
        {
            if (Inputx != 0)
            {
                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    Inputx -= (Time.deltaTime * Inetie) * (Inputx/Mathf.Abs(Inputx));
                }
                else
                {
                    Inputx = Input.GetAxis("Horizontal");
                }
            }
            else
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
                {
                    Inputx += (Time.deltaTime * Inetie) * (Inputx / Mathf.Abs(Inputx));
                }
                else
                {
                    Inputx = 0;
                }
            }

            if (Inputy != 0)
            {
                if (Input.GetAxisRaw("Vertical") == 0)
                {
                    Inputy -= (Time.deltaTime * Inetie) * (Inputy / Mathf.Abs(Inputy));
                }
                else
                {
                    Inputy = Input.GetAxis("Vertical");
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") == 1)
                {
                    Inputy += (Time.deltaTime * Inetie) * (Inputy / Mathf.Abs(Inputy));
                }
                else
                {
                    Inputy = 0;
                }
            }
        }
    }

    //Mets en place la courbe de easing
    void MovementCalcul()
    {
        if (Inputx != 0)
        {
            Movementx = (1 - Mathf.Pow(1 - Mathf.Abs(Inputx), 5)) * Inputx / Mathf.Abs(Inputx);
        }
        else
        {
            Movementx = 0;
        }
        if (Inputy != 0)
        {
            Movementy = (1 - Mathf.Pow(1 - Mathf.Abs(Inputy), 5)) * Inputy / Mathf.Abs(Inputy);
        }
        else
        {
            Movementy = 0;
        }
    }
}
