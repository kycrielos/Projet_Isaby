using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBulletscriptOld : MonoBehaviour
{
    public Transform target;
    public float Speed;
    public float Damage;
    private PlayerDamage Player;
    bool DamageSecurity;

    private Vector3 playerposition;

    private void Start()
    {
        playerposition = target.position;
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float step = Speed * Time.deltaTime * 2;
        transform.position = Vector3.MoveTowards(transform.position, playerposition, step);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !DamageSecurity)
        {
            Player = other.gameObject.GetComponent<PlayerDamage>();
            DamageSecurity = true;
            Player.Damaged(Damage);
        }
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BulletTrigger")
        {
            Destroy(this.gameObject, 0.5f);
        }
    }
}
