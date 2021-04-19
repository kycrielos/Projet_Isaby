using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAI : MonoBehaviour
{
    public GameObject aspirationArea;
    public float aspirationDuration;

    public Transform head;

    public float cooldown;
    private float cdTimer;

    public float delayDuration;

    private int randomNumber;

    public GameObject bullet;
    public float spawnDistance;

    public float speed;

    public enum SnakeState
    {
        Idle,
        PreAspiration,
        PreShoot,
        Aspiration,
        Shoot,
    }

    public SnakeState currentState = SnakeState.Idle;

    // Update is called once per frame
    void Update()
    {
        AI(); 

        if (currentState == SnakeState.Aspiration)
        {
            StartCoroutine(Aspiration());
        }
        else
        {
            FollowPlayer();
        }

        if (currentState == SnakeState.Shoot)
        {
            Instantiate(bullet, head.position + transform.forward * spawnDistance, transform.rotation);
            currentState = SnakeState.Idle;
            cdTimer = 0;
        }
    }

    public void FollowPlayer()
    {
        Vector3 targetDirection = new Vector3(GameManager.Instance.player.transform.position.x, transform.position.y, GameManager.Instance.player.transform.position.z) - transform.position;
        float singleStep = speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0));
    }

    void AI()
    {
        if (currentState == SnakeState.Idle)
        {
            cdTimer += Time.deltaTime;
            if (cdTimer > cooldown && Physics.Linecast(head.position, GameManager.Instance.player.transform.position + new Vector3(0, 1, 0), out RaycastHit hitinfo))
            {
                if (hitinfo.collider.tag == "Player")
                {
                    if (GameManager.Instance.currentState == GameManager.PlayerState.Jumping || GameManager.Instance.currentState == GameManager.PlayerState.Falling)
                    {
                        currentState = SnakeState.PreAspiration;
                    }
                    else if (PlayerPhysics.playerIsOnMovablePlatform)
                    {
                        currentState = SnakeState.PreShoot;
                    }
                    else
                    {
                        randomNumber = Random.Range(0, 2);
                        if (randomNumber == 0)
                        {
                            currentState = SnakeState.PreAspiration;
                        }
                        else
                        {
                            currentState = SnakeState.PreShoot;
                        }
                    }
                    cdTimer = 0;
                    StartCoroutine(DelayAttack());
                }
            }
        }
    } 

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayDuration);
        if (currentState == SnakeState.PreAspiration)
        {
            currentState = SnakeState.Aspiration;
        }
        else if (currentState == SnakeState.PreShoot)
        {
            currentState = SnakeState.Shoot;
        }
    }

    IEnumerator Aspiration()
    {
        aspirationArea.SetActive(true);
         yield return new WaitForSeconds(aspirationDuration);
        aspirationArea.SetActive(false);
        currentState = SnakeState.Idle;
        cdTimer = 0;
    }
}
