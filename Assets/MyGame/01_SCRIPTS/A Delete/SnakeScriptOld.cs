using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScriptOld : MonoBehaviour
{
    public Transform player;

    public float speed;

    public bool AspirationOn;
    public bool ShootOn;

    private float AspirationTimer;
    public float AspirationDuration;
    public GameObject AspirationTrigger;

    public Transform target;
    private Vector3 SpawnPos;
    public float SpawnDistance;

    public float Cooldown;
    private float CdTimer;
    public Transform Head;

    private float delayTimer;
    public float Delay;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            AI();
            if (AspirationOn)
            {
                Aspiration();
            }
            else
            {
                FollowPlayer();
            }

            if (ShootOn)
            {
                ShootOn = false;
                CdTimer = 0;
            }
        }
    }
    public void AI()
    {
        Debug.DrawLine(Head.position, player.position, Color.white);
        if (!AspirationOn)
        {
            CdTimer += Time.deltaTime;
            Debug.DrawLine(Head.position, player.position + new Vector3(0, 1, 0), Color.white);
            if (CdTimer > Cooldown && Physics.Linecast(Head.position, player.position + new Vector3(0,1,0), out RaycastHit hitinfo))
            {
                if (hitinfo.collider.tag == "Player")
                {
                    delayTimer += Time.deltaTime;
                    if (delayTimer >= Delay)
                    {
                        AspirationOn = true;
                        delayTimer = 0;
                    }
                }
                else
                {
                    delayTimer = 0;
                }
            }
        }
    }

    public void FollowPlayer()
    {
        Vector3 targetDirection = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position;
        float singleStep = speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0));
    }


    public void Aspiration()
    {
        AspirationTrigger.SetActive(true);
        AspirationTimer += Time.deltaTime;
        if (AspirationTimer >= AspirationDuration)
        {
            AspirationOn = false;
            AspirationTrigger.SetActive(false);
            AspirationTimer = 0;
            CdTimer = 0;
        }
    }
}
