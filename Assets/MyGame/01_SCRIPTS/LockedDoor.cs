using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        TriggerManager.Activation += ActivateDoor;
    }

    void ActivateDoor(GameObject triggerObj)
    {
        if (triggerObj.name == gameObject.name + "Trigger")
        {
            Debug.Log("Let's go");
        }
    }
}
