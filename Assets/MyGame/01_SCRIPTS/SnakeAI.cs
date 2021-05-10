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

    public GameObject bulletPrefab;
    private GameObject bullet;
    public float spawnDistance;

    public float speed;

    
    // Update is called once per frame
    void Update()
    {
        AI(); 

        if (GameManager.Instance.currentSnakeState == GameManager.SnakeState.Aspiration)
        {
            StartCoroutine(Aspiration());
        }
        else
        {
            FollowPlayer();
        }

        if (GameManager.Instance.currentSnakeState == GameManager.SnakeState.Shoot)
        {
            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab, head.position + transform.forward * spawnDistance, transform.rotation);
                StartCoroutine(DelayStateChange(GameManager.SnakeState.Idle, 0.8f));
                cdTimer = 0;
            }
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
        if (GameManager.Instance.currentSnakeState == GameManager.SnakeState.Idle)
        {
            cdTimer += Time.deltaTime;
            if (cdTimer > cooldown && Physics.Linecast(head.position, GameManager.Instance.player.transform.position + new Vector3(0, 1, 0), out RaycastHit hitinfo))
            {
                if (hitinfo.collider.tag == "Player")
                {
                    if (GameManager.Instance.currentState == GameManager.PlayerState.Jumping || GameManager.Instance.currentState == GameManager.PlayerState.Falling)
                    {
                        GameManager.Instance.currentSnakeState = GameManager.SnakeState.PreAspiration;
                        StartCoroutine(DelayStateChange(GameManager.SnakeState.Aspiration, delayDuration));
                    }
                    else if (PlayerPhysics.playerIsOnMovablePlatform)
                    {
                        GameManager.Instance.currentSnakeState = GameManager.SnakeState.PreShoot;
                        StartCoroutine(DelayStateChange(GameManager.SnakeState.Shoot, delayDuration));
                    }
                    else
                    {
                        randomNumber = Random.Range(0, 2);
                        if (randomNumber == 0)
                        {
                            GameManager.Instance.currentSnakeState = GameManager.SnakeState.PreAspiration;
                            StartCoroutine(DelayStateChange(GameManager.SnakeState.Aspiration, delayDuration));
                        }
                        else
                        {
                            GameManager.Instance.currentSnakeState = GameManager.SnakeState.PreShoot;
                            StartCoroutine(DelayStateChange(GameManager.SnakeState.Shoot, delayDuration));
                        }
                    }
                    cdTimer = 0;
                }
            }
        }
    } 

    IEnumerator DelayStateChange(GameManager.SnakeState state, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.currentSnakeState = state;
    }

    IEnumerator Aspiration()
    {
        aspirationArea.SetActive(true);
         yield return new WaitForSeconds(aspirationDuration);
        aspirationArea.SetActive(false);
        GameManager.Instance.currentSnakeState = GameManager.SnakeState.Idle;
        cdTimer = 0;
    }
}
