using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyScript : MonoBehaviour
{
    public GameObject door;
    public void Activation()
    {
        GameManager.Instance.PlaySound(GameManager.SoundName.Ouverture_Porte, "Other", false);
        door.SetActive(false);
        gameObject.SetActive(false);
    }

}
