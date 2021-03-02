using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] doors;
    public PlatformBehaviour[] platforms;

    private void Start()
    {
        TriggerManager.Activation += ActivateDoor;
    }

    void ActivateDoor(GameObject triggerObj)
    {
        if (triggerObj.name == gameObject.name + "Trigger")
        {
            foreach(GameObject door in doors)
            {
                door.SetActive(!door.activeSelf);
            }
            foreach(PlatformBehaviour platform in platforms)
            {
                platform.activated = !platform.activated;
            }
        }
    }
    ~Lever()
    {
        TriggerManager.Activation -= ActivateDoor;
    }
}
