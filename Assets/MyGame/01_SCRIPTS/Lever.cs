using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] doors;
    public PlatformBehaviour[] platforms;

    private bool isActive;
    public GameObject activeOnObj;
    public GameObject activeOffObj;

    public AudioSource LevierSFX;

    private void Start()
    {
        TriggerManager.Activation += ActivateDoor;
    }

    void ActivateDoor(GameObject triggerObj)
    {
        if (triggerObj.name == gameObject.name + "Trigger")
        {
            isActive = !isActive;
            foreach (GameObject door in doors)
            {
                door.SetActive(!door.activeSelf);
            }
            foreach(PlatformBehaviour platform in platforms)
            {
                platform.activated = !platform.activated;
            }

            if (isActive)
            {
                activeOnObj.SetActive(true);
                activeOffObj.SetActive(false);
            }
            else
            {
                activeOnObj.SetActive(false);
                activeOffObj.SetActive(true);
            }

            LevierSFX.Play();
        }

    }
    ~Lever()
    {
        TriggerManager.Activation -= ActivateDoor;
    }
}
