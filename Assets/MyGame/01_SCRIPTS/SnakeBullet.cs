using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBullet : MonoBehaviour
{
    public float speed;
    public int damage;

    private Vector3 targetPositionFocus;
    private Vector3 movementVector = Vector3.zero;

    private bool stopMovement;

    private Vector3 predictedPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = targetPosition - shooterPosition;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude <= 0.1f || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            return targetPosition;
        }
        //also Sine Formula
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }

    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(0.9f, 1.1f);
        targetPositionFocus = predictedPosition(GameManager.Instance.player.transform.position + new Vector3(0,0.6f,0), transform.position, GameManager.Instance.player.GetComponent<CharacterController>().velocity, speed * x);
        movementVector = (targetPositionFocus - transform.position).normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (!stopMovement)
        {
            transform.position += movementVector * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !stopMovement)
        {
            GameManager.Instance.player.GetComponent<PlayerDamage>().Damaged(damage);
            stopMovement = true;
            Destroy(this.gameObject, 0.25f);
        }

        if (other.GetComponent<Collider>().isTrigger == false && !stopMovement)
        {
            stopMovement = true;
            Destroy(this.gameObject, 0.25f);
        }
    }
}
