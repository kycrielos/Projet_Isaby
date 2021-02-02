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
    Vector3 MoveDirection;

    //Jump
    Vector3 JumpDirection;

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
            controller.Move(moveDir * Speed * Time.deltaTime + JumpDirection);
        }
        else
        {
            controller.Move(JumpDirection);
        }
    }

    void GetInput()
    {
        //Recupere les Input
        Inputx = Input.GetAxis("Horizontal");
        Inputy = Input.GetAxis("Vertical");
    }

    //Mets en place la courbe de easing
    void MovementCalcul()
    {
        if (Inputx != 0)
        {
            Movementx = (1 - Mathf.Pow(Mathf.Abs(Inputx ) - 1, 5)) * Inputx / Mathf.Abs(Inputx);
        }
        else
        {
            Movementx = 0;
        }
        if (Inputy != 0)
        {
            Movementy = (1 - Mathf.Pow(Mathf.Abs(Inputy) - 1, 5)) * Inputy / Mathf.Abs(Inputy);
        }
        else
        {
            Movementy = 0;
        }
        Debug.Log(Movementx);
    }
}
