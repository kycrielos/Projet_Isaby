using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAI : MonoBehaviour
{
    public bool aspirationOn;
    public GameObject aspirationArea;
    public float aspirationDuration;

    public Transform head;

    public float cooldown;
    private float cdTimer;

    public float delayDuration;

    private int randomNumber;

    public GameObject bullet;
    public float spawnDistance;
    private bool shootOn;

    public float speed;

    // Update is called once per frame
    void Update()
    {
        AI(); 
        if (aspirationOn)
        {
            StartCoroutine(Aspiration());
        }
        else
        {
            FollowPlayer();
        }

        if (shootOn)
        {
            Instantiate(bullet, head.position + transform.forward * spawnDistance, transform.rotation);
            shootOn = false;
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
        if (!aspirationOn)
        {
            cdTimer += Time.deltaTime;
            if (cdTimer > cooldown && Physics.Linecast(head.position, GameManager.Instance.player.transform.position + new Vector3(0, 1, 0), out RaycastHit hitinfo))
            {
                if (hitinfo.collider.tag == "Player")
                {
                    randomNumber = Random.Range(0, 2);
                    cdTimer = 0;
                    StartCoroutine(DelayAttack());
                }
            }
        }
    } 

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayDuration);
        if (randomNumber == 0)
        {
            aspirationOn = true;
        }
        else
        {
            shootOn = true;
        }
    }

    IEnumerator Aspiration()
    {
        aspirationArea.SetActive(true);
        yield return new WaitForSeconds(aspirationDuration);
        aspirationOn = false;
        aspirationArea.SetActive(false);
        cdTimer = 0;
    }
}
