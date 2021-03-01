using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    // Start is called before the first frame update
    private void Start()
    {
        spawnPoint = transform.position;
        TriggerManager.Activation += UpdateSpawnPoint;
        PlayerDamage.PlayerDie += RespawnThePlayer;
    }

    void UpdateSpawnPoint(GameObject triggerObj)
    {
        if (triggerObj.name == "SpawnPointTrigger")
        {
            spawnPoint = triggerObj.GetComponentInChildren<Transform>().transform.position;
        }
    }
    void RespawnThePlayer()
    {
        transform.position = spawnPoint;
    }

    ~PlayerRespawn()
    {
        TriggerManager.Activation -= UpdateSpawnPoint;
        PlayerDamage.PlayerDie -= RespawnThePlayer;
    }
}
