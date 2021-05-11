using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyScript : MonoBehaviour
{
    public Transform door;
    public bool isActive;

    public float distanceTo;
    public float speed;
    public Transform position1;
    public Transform position2;
    private void Update()
    {
        if (distanceTo > 0.05f)
        {
            if (isActive)
            {
                distanceTo = Vector3.Distance(position2.position, door.position);
                float step = speed * Time.deltaTime;
                door.position = Vector3.MoveTowards(door.position, position2.position, step);
            }
            else
            {
                distanceTo = Vector3.Distance(position1.position, door.position);
                float step = speed * Time.deltaTime;
                door.position = Vector3.MoveTowards(door.position, position1.position, step);
            }
        }
    }
    public void Activation()
    {
        isActive = !isActive;
        AudioManager.Instance.PlaySound(AudioManager.SoundName.Ouverture_Porte, "Other", false);
        if (isActive)
        {
            distanceTo = Vector3.Distance(position2.position, door.position);
        }
        else
        {
            distanceTo = Vector3.Distance(position1.position, door.position);
        }
    }

}
