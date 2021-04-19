using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBullet : MonoBehaviour
{
    public float speed;
    public int damage;

    bool security;
    private Vector3 targetPositionFocus;
    private Vector3 predictedPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = targetPosition - shooterPosition;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            Debug.Log("Position prediction is not feasible.");
            return targetPosition;
        }
        //also Sine Formula
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }

    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(1f, 1.5f);
        targetPositionFocus = predictedPosition(GameManager.Instance.player.transform.position, transform.position, GameManager.Instance.player.GetComponent<CharacterController>().velocity, speed * x);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPositionFocus, step);
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
