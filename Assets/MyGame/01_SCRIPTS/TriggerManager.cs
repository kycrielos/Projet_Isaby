using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public delegate void ActivateEvent(GameObject triggerObj);

    public static event ActivateEvent Activation;
    public PlayerDamage playerDamage;

    public void SetActive(GameObject triggerObj)
    {
        if (triggerObj.name.Contains("DeathZone"))
        {
            GameManager.Instance.playerHP = 0;
            playerDamage.Damaged(0);
            GameManager.Instance.RefreshUIActivation();
        }
        else if(triggerObj.name.Contains("TeddyPart"))
        {
            Destroy(triggerObj);
            GameManager.Instance.teddyPartsNumbers += 1;
        }
        else if (triggerObj.name.Contains("Coin"))
        {
            Destroy(triggerObj);
            GameManager.Instance.coinNumbers += 1;
        }
        else
        {
            ActivationEventHandler(triggerObj);
        }
    }

    protected virtual void ActivationEventHandler(GameObject triggerObj)
    {
        Activation?.Invoke(triggerObj);
    }
}
