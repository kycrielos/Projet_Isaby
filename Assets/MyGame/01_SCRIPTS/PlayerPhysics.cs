using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private static CharacterController controller;
    public static float TimeSinceGrounded;
    public static bool IsGrounded;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeSinceGrounded += Time.deltaTime;
        IsGroundedCheck();
    }

    public static void IsGroundedCheck()
    {
        if (controller.isGrounded)
        {
            IsGrounded = true;
            TimeSinceGrounded = 0;
        }
        else
        {
            IsGrounded = false;
        }
    }

}
