using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBulletTriggerOld : MonoBehaviour
{
    public Transform Target;
    public SnakeScriptOld Snake;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Target.position = other.transform.position;
            Snake.target = Target;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Snake.target = null;
        }

    }
}
