using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public float Delay;
    public float Speed;

    private bool start;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CLoseTheDoor());
    }

    IEnumerator CLoseTheDoor()
    {
        yield return new WaitForSeconds(Delay);
        start = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (300 < transform.eulerAngles.y && transform.eulerAngles.y <= 348)
            {
                transform.eulerAngles = new Vector3(0, 348, 0);
                start = false;
            }
            else
            {
                transform.Rotate(0, -Speed * Time.deltaTime, 0);
            }
        }
    }
}
