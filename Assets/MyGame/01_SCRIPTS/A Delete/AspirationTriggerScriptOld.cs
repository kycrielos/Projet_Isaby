using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspirationTriggerScriptOld : MonoBehaviour
{
    public Transform Head;
    public Transform player;

    public float Force;

    private void Start()
    {
        player = GetComponentInParent<SnakeHeadScriptOld>().player;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Physics.Linecast(Head.position, player.position + new Vector3(0, 1, 0), out RaycastHit hitinfo))
        {
            if (hitinfo.collider.tag == "Player")
            {
                player.GetComponent<CharacterController>().Move(-Head.forward * Force * Time.deltaTime);
            }
        }
    }
}
