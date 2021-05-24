using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public delegate void ActivateEvent(GameObject triggerObj);

    public static event ActivateEvent Activation;
    public PlayerDamage playerDamage;
    public GameObject Snake;
    public CameraController cam;

    public void SetActive(GameObject triggerObj)
    {
        switch (triggerObj.name)
        {
            case string a when a.Contains("DeathZone"):
                GameManager.Instance.playerHP = 0;
                playerDamage.Damaged(100);
                GameManager.Instance.RefreshUIActivation();
                break;
            case string a when a.Contains("TeddyPart"):
                Destroy(triggerObj);
                GameManager.Instance.teddyPartsNumbers += 1;
                GameManager.Instance.playerHP = 3;
                GameManager.Instance.RefreshUIActivation();
                break;
            case string a when a.Contains("SnakeSpawner"):
                Destroy(triggerObj);
                cam.ShakeCamera();
                Snake.SetActive(true);
                break;
            case string a when a.Contains("AreaChange"):
                break;
            default:
                ActivationEventHandler(triggerObj);
                break;
        }
    }

    protected virtual void ActivationEventHandler(GameObject triggerObj)
    {
        Activation?.Invoke(triggerObj);
    }
}
