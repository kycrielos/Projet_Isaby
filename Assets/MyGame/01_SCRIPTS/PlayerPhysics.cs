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
    public float FallingDamage;

    private PlayerDamage playerDamage;
    public TriggerManager triggerManager;
    private GameObject triggerObj;
    private bool isInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerDamage = GetComponent<PlayerDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceGrounded += Time.deltaTime;
        IsGroundedCheck();
        Gravity();
        IsIntriggerCheck();
    }

    public void IsGroundedCheck()
    {
        if (controller.isGrounded)
        {
            isGrounded = true;
            if (timeSinceGrounded > 0.1f)
            {
                playerDamage.Damaged(timeSinceGrounded * FallingDamage);
            }
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
            triggerManager.SetActive(triggerObj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerAuto"))
        {
            triggerObj = other.gameObject;
            triggerManager.SetActive(triggerObj);
        }
        else if (other.CompareTag("PlatformTrigger"))
        {
            transform.parent = other.transform;
        }
        else
        {
            isInTrigger = true;
            triggerObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlatformTrigger"))
        {
            transform.parent = null;
        }
        isInTrigger = false;
    }

}
