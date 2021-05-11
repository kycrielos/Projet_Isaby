using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyScript : MonoBehaviour
{
    public GameObject door;
    public void Activation()
    {
        AudioManager.Instance.PlaySound(AudioManager.SoundName.Ouverture_Porte, "Other", false);
        door.SetActive(false);
        gameObject.SetActive(false);
    }

}
