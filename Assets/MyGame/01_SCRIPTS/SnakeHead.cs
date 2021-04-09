using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public GameObject snake;

    private float speed;

    private SnakeAI snakeScript;

    // Start is called before the first frame update
    void Start()
    {
        snakeScript = snake.GetComponent<SnakeAI>();
        speed = snakeScript.speed;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = new Vector3(snake.transform.position.x, transform.position.y, snake.transform.position.z);
        if (!snakeScript.aspirationOn)
        {
            Vector3 targetDirection = GameManager.Instance.player.transform.position - transform.position;
            float singleStep = speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0));
        }
    }
}
