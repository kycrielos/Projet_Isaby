using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public Transform position1;
    public Transform position2;
    public bool goPosition1;
    public float speed = 1;
    public float distanceTo;

    public bool activated;
    public bool autoMovement;

    // Update is called once per frame
    void Update()
    {
        if (goPosition1)
        {
            distanceTo = Vector3.Distance(position1.position, transform.position);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, position1.position, step);
        }
        else
        {
            distanceTo = Vector3.Distance(position2.position, transform.position);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, position2.position, step);
        }

        if (distanceTo <= 0.05f && activated)
        {
            goPosition1 = !goPosition1;
        }
        else if (activated && !autoMovement)
        {
            activated = false;
        }
    }
}
