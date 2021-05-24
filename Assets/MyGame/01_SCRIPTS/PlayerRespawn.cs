using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform spawnPoint;
    public float TimeBeforeRespawn;
    public float DelayAfterRespawn;

    private CharacterController controller;
    private PlayerPhysics physics;

    public Transform followPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        TriggerManager.Activation += UpdateSpawnPoint;
        PlayerDamage.PlayerDie += RespawnThePlayer;
        controller = GetComponent<CharacterController>();
        physics = GetComponent<PlayerPhysics>();
    }

    void UpdateSpawnPoint(GameObject triggerObj)
    {
        if (triggerObj.name.Contains("SpawnPointTrigger") && GameManager.Instance.timeSinceGrounded <= 0.75f && GameManager.Instance.currentState != GameManager.PlayerState.Die)
        {
            spawnPoint = triggerObj.transform.GetChild(0);
        }
    }

    void RespawnThePlayer()
    {
        StartCoroutine(PlayerRespawner());
    }

    IEnumerator PlayerRespawner()
    {
        controller.enabled = false;
        transform.position += new Vector3(0, -0.5f, 0);
        yield return new WaitForSeconds(TimeBeforeRespawn);
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        followPlayer.GetComponent<CameraController>().Mousex = spawnPoint.rotation.eulerAngles.y;
        GameManager.Instance.playerHP = GameManager.Instance.maxPlayerHp;
        GameManager.Instance.RefreshUIActivation();
        yield return new WaitForSeconds(DelayAfterRespawn);
        controller.enabled = true;
        physics.timeSinceGrounded = 0;
        physics.fallingDuration = 0;
        GameManager.Instance.currentState = GameManager.PlayerState.Idle;
    }

    ~PlayerRespawn()
    {
        TriggerManager.Activation -= UpdateSpawnPoint;
        PlayerDamage.PlayerDie -= RespawnThePlayer;
    }
}
