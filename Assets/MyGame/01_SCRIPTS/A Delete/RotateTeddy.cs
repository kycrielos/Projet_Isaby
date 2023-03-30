using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTeddy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0.5f, 0));
    }
}
