using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspirationArea : MonoBehaviour
{
    public Transform head;

    public float force;


    private void OnTriggerStay(Collider other)
    {
        if (Physics.Linecast(head.position, GameManager.Instance.player.transform.position + new Vector3(0, 1, 0), out RaycastHit hitinfo) && other.CompareTag("Player"))
        {
            if (hitinfo.collider.CompareTag("Player"))
            {
                GameManager.Instance.player.transform.GetComponent<CharacterController>().Move(-head.forward * force * Time.deltaTime);
            }
        }
    }

}
