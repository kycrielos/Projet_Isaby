using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private CharacterController controller;
    public float timeSinceGrounded;
    public bool isGrounded;
    public bool contactWithGround;
    public float gravityScale;
    public float gravityForce;
    public float FallingDamage;

    public TriggerManager triggerManager;
    private GameObject triggerObj;
    private bool isInTrigger;

    private PlayerDamage playerDamage;
    private float fallingDuration;

    public ParticleSystem dust;

    private int layerMask = 1 << 9;

    public bool sliding;

    public Vector3 hitNormal;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerDamage = GetComponent<PlayerDamage>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.PlayerState.Die)
        {
            if (!sliding)
            {
                timeSinceGrounded += Time.deltaTime;
            }
            else
            {
                timeSinceGrounded += Time.deltaTime/2;
            }
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
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f, layerMask))
            {
                isGrounded = true;
                if (fallingDuration > 0.1f)
                {
                    playerDamage.Damaged(Mathf.Exp(fallingDuration * 10) * FallingDamage);
                    dust.Play();
                }
                fallingDuration = 0;
                timeSinceGrounded = 0;
            }
            else
            {
                sliding = true;
                isGrounded = false;
                GameManager.Instance.currentState = GameManager.PlayerState.Sliding;
            }

        }
        else
        {
            sliding = false;
            isGrounded = false;
            if (GameManager.Instance.currentState == GameManager.PlayerState.Falling)
            {
                fallingDuration += Time.deltaTime;
            }
        }
    }

    void Gravity()
    {
        if (GameManager.Instance.currentState != GameManager.PlayerState.Jumping && !isGrounded)
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
}
