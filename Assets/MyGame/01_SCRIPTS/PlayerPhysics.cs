using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private CharacterController controller;
    public float timeSinceGrounded;
    public bool contactWithGround;
    public float gravityScale;
    public float gravityForce;
    public float FallingDamage;

    public TriggerManager triggerManager;
    private GameObject triggerObj;
    private bool isInTrigger;

    private PlayerDamage playerDamage;
    public float fallingDuration;

    public ParticleSystem dust;

    public LayerMask layerMask;

    public bool sliding;

    public Vector3 hitNormal;

    private Vector3 capsulePos1;
    private Vector3 capsulePos2;

    public static bool playerIsOnMovablePlatform;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerDamage = GetComponent<PlayerDamage>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.PlayerState.Die && GameManager.Instance.currentState != GameManager.PlayerState.Wait)
        {
            if (!sliding)
            {
                timeSinceGrounded += Time.deltaTime;
            }
            else
            {
                timeSinceGrounded += Time.deltaTime * 0.75f;
            }
            GameManager.Instance.timeSinceGrounded = timeSinceGrounded;
            IsGroundedCheck();
            Gravity();
            IsIntriggerCheck();
        }
    }

    public void IsGroundedCheck()
    {
        RaycastHit hit;
        if (controller.isGrounded)
        {
            capsulePos1 = transform.position + controller.center + Vector3.up * -controller.height * 0.3f;
            capsulePos2 = capsulePos1 + Vector3.up * controller.height;


            if (Physics.CapsuleCast(capsulePos1, capsulePos2, controller.radius, Vector3.down, out hit, 0.2f, layerMask))
            {
                GameManager.Instance.isGrounded = true;
                if (fallingDuration > 0.1f)
                {
                    playerDamage.Damaged(Mathf.Exp(fallingDuration * 9) * FallingDamage);
                    dust.Play();
                }
                fallingDuration = 0;
                timeSinceGrounded = 0;
            }
            else
            {
                sliding = true;
                GameManager.Instance.isGrounded = false;
                GameManager.Instance.currentState = GameManager.PlayerState.Sliding;
            }

        }
        else
        {
            sliding = false;
            GameManager.Instance.isGrounded = false;
            if (GameManager.Instance.currentState == GameManager.PlayerState.Falling)
            {
                fallingDuration += Time.deltaTime;
            }
        }
    }

    void Gravity()
    {
        if (GameManager.Instance.currentState != GameManager.PlayerState.Jumping && !GameManager.Instance.isGrounded)
        {
            if (gravityForce > -gravityScale)
            {
                gravityForce -= Time.deltaTime * gravityScale;
            }
            else
            {
                gravityForce = -gravityScale;
            }
            if (timeSinceGrounded >= 0.1f && !sliding)
            {
                GameManager.Instance.currentState = GameManager.PlayerState.Falling;
            }
        }
        else
        {
            gravityForce = 0;
        }
    }

    void IsIntriggerCheck()
    {
        if (MyInputManager.Instance.GetKeyDown("Interaction") && GameManager.Instance.playerIsInActivableObject)
        {
            triggerManager.SetActive(triggerObj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case string a when a.Contains("TriggerAuto"):
                triggerObj = other.gameObject;
                triggerManager.SetActive(triggerObj);
                break;
            case string a when a.Contains("WallPlatform"):
                transform.parent = null;
                playerIsOnMovablePlatform = false;
                break;
            case string a when a.Contains("PlatformTrigger"):
                transform.parent = other.transform;
                playerIsOnMovablePlatform = true;
                break;
            case string a when a.Contains("Stair"):
                controller.slopeLimit = 60;
                break;
            case string a when a.Contains("ActivableObject"):
                GameManager.Instance.playerIsInActivableObject = true;
                GameManager.Instance.RefreshUIActivation();
                isInTrigger = true;
                triggerObj = other.gameObject;
                break;
            default:
                isInTrigger = true;
                triggerObj = other.gameObject;
                break;
        }
        /*if (other.CompareTag("TriggerAuto"))
        {
            triggerObj = other.gameObject;
            triggerManager.SetActive(triggerObj);
        }
        else if (other.CompareTag("WallPlatform"))
        {
            transform.parent = null;
            playerIsOnMovablePlatform = false;
        }
        else if (other.CompareTag("PlatformTrigger"))
        {
            transform.parent = other.transform;
            playerIsOnMovablePlatform = true;
        }
        else
        {
            isInTrigger = true;
            triggerObj = other.gameObject;
        }

        if (other.CompareTag("Stair"))
        {
            controller.slopeLimit = 60;
        }
        if (other.CompareTag("ActivableObject"))
        {
            GameManager.Instance.playerIsInActivableObject = true;
            GameManager.Instance.RefreshUIActivation();
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case string a when a.Contains("TriggerAuto"):
                break;
            case string a when a.Contains("WallPlatform"):
                break;
            case string a when a.Contains("PlatformTrigger"):
                transform.parent = null;
                playerIsOnMovablePlatform = false;
                break;
            case string a when a.Contains("Stair"):
                controller.slopeLimit = 1;
                break;
            case string a when a.Contains("ActivableObject"):
                GameManager.Instance.playerIsInActivableObject = false;
                GameManager.Instance.RefreshUIActivation();
                isInTrigger = false;
                break;
            default:
                isInTrigger = false;
                break;
        }
        /*if (other.CompareTag("PlatformTrigger"))
        {
            transform.parent = null;
            playerIsOnMovablePlatform = false;
        }


        else if (other.CompareTag("Stair"))
        {
            controller.slopeLimit = 1;
        }

        else if (other.CompareTag("ActivableObject"))
        { 
            GameManager.Instance.playerIsInActivableObject = false;
            GameManager.Instance.RefreshUIActivation();
        }

            isInTrigger = false;*/
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
}