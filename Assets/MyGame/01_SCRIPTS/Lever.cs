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

    private bool canActive = true;

    private void Start()
    {
        TriggerManager.Activation += ActivateDoor;
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

            AudioManager.Instance.StopSound("Other");
            AudioManager.Instance.PlaySound(AudioManager.SoundName.Levier, "Other", false);
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
