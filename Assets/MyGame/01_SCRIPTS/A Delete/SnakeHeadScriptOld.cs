using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadScriptOld : MonoBehaviour
{
    public Transform player;
    public Transform FollowTarget;
    public float speed;
    public SnakeScriptOld Snake;
    // Start is called before the first frame update
    void Start()
    {
        speed = Snake.speed;
        player = Snake.player;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        transform.position = FollowTarget.transform.position + new Vector3(0,20,0);
        if (!Snake.AspirationOn)
        {
            Vector3 targetDirection = player.position - transform.position;
            float singleStep = speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0));

        }
    }
}
