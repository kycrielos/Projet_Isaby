﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBullet : MonoBehaviour
{
    public float speed;
    public int damage;

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
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.player.GetComponent<PlayerDamage>().Damaged(damage);
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject, 0.2f);
    }
}
