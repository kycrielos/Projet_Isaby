using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private static CharacterController controller;
    public static float timeSinceGrounded;
    public static bool isGrounded;
    public float gravityScale;
    public static float gravityForce;

    public TriggerManager triggerManager;
    private string triggerName;
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceGrounded += Time.deltaTime;
        IsGroundedCheck();
        Gravity();
        IsIntriggerCheck();
    }

    public static void IsGroundedCheck()
    {
        if (controller.isGrounded)
        {
            isGrounded = true;
            timeSinceGrounded = 0;
        }
        else
        {
            isGrounded = false;
        }
    }

    void Gravity()
    {
        if (PlayerController.currentState != PlayerController.PlayerState.Jumping && !isGrounded)
        {
            if (gravityForce > -gravityScale)
            {
                gravityForce -= Time.deltaTime * gravityScale;
            }
            else
            {
                gravityForce = -gravityScale;
            }
            if (timeSinceGrounded >= 0.1f)
            PlayerController.currentState = PlayerController.PlayerState.Falling;
        }
        else 
        {
            gravityForce = 0;
        }
    }

    void IsIntriggerCheck()
    {
        if (Input.GetButtonDown("Interaction") && isInTrigger)
        {
            triggerManager.SetActive(triggerName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInTrigger = true;
        triggerName = other.name;
    }
    private void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
    }

}
