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

    private bool canActive = true;

    public bool autoLever;

    private float timer;
    public float delay;

    private void Start()
    {
        TriggerManager.Activation += ActivateDoor;
    }

    private void Update()
    {
        if (autoLever)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                isActive = !isActive;
                foreach (GameObject door in doors)
                {
                    door.GetComponent<DoorKeyScript>().Activation(false);
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
                timer = 0;
            }
        }
    }

    void ActivateDoor(GameObject triggerObj)
    {
        if (triggerObj.name == gameObject.name + "Trigger" && canActive)
        {
            isActive = !isActive;
            foreach (GameObject door in doors)
            {
                door.GetComponent<DoorKeyScript>().Activation(false);
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
            canActive = false;
            StartCoroutine(Timer());
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        canActive = true;
    }

    ~Lever()
    {
        TriggerManager.Activation -= ActivateDoor;
    }
}
