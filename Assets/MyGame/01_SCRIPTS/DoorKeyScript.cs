using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyScript : MonoBehaviour
{
    public GameObject door;
    public void Activation()
    {
        door.SetActive(false);
        gameObject.SetActive(false);
    }
}
