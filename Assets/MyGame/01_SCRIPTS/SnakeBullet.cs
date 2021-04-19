using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBullet : MonoBehaviour
{
    public float speed;
    public int damage;

    bool security;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GameManager.Instance.player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !security)
        {
            GameManager.Instance.player.GetComponent<PlayerDamage>().Damaged(damage);
            security = true;
            Destroy(this.gameObject);
        }

        if (other.tag != "NotTargetable")
        {
            Debug.Log(other.name);
            Destroy(this.gameObject, 0.2f);
        }
    }
}
