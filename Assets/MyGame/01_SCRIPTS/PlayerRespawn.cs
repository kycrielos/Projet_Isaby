using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    public float TimeBeforeRespawn;
    public float DelayAfterRespawn;

    private CharacterController controller;
    private PlayerPhysics physics;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoint = transform.position;
        TriggerManager.Activation += UpdateSpawnPoint;
        PlayerDamage.PlayerDie += RespawnThePlayer;
        controller = GetComponent<CharacterController>();
        physics = GetComponent<PlayerPhysics>();
    }

    void UpdateSpawnPoint(GameObject triggerObj)
    {
        if (triggerObj.name.Contains("SpawnPointTrigger") && GameManager.Instance.currentState != GameManager.PlayerState.Falling && GameManager.Instance.currentState != GameManager.PlayerState.Die)
        {
            spawnPoint = triggerObj.transform.position;
        }
    }

    void RespawnThePlayer()
    {
        StartCoroutine(PlayerRespawner());
    }

    IEnumerator PlayerRespawner()
    {
        controller.enabled = false;
        yield return new WaitForSeconds(TimeBeforeRespawn);
        transform.position = spawnPoint;
        GameManager.Instance.playerHP = GameManager.Instance.maxPlayerHp;
        GameManager.Instance.RefreshUIActivation();
        yield return new WaitForSeconds(DelayAfterRespawn);
        controller.enabled = true;
        physics.timeSinceGrounded = 0;
        physics.fallingDuration = 0;
        GameManager.Instance.currentState = GameManager.PlayerState.Idle;
        GameManager.Instance.RefreshAnimation();
    }

    ~PlayerRespawn()
    {
        TriggerManager.Activation -= UpdateSpawnPoint;
        PlayerDamage.PlayerDie -= RespawnThePlayer;
    }
}
