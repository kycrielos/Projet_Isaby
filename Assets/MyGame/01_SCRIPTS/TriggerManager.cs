using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public delegate void ActivateEvent(GameObject triggerObj);

    public static event ActivateEvent Activation;

    public void SetActive(GameObject triggerObj)
    {
        if (triggerObj.name.Contains("DeathZone"))
        {
            GameManager.Instance.playerHP = 0;
            PlayerDamage.Damaged(0);
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
