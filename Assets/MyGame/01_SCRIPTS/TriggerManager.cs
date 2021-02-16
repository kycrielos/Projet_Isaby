using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public delegate void ActivateEvent(string triggerName);

    public static event ActivateEvent Activation;

    public void SetActive(string triggerName)
    {
        ActivationEventHandler(triggerName);
    }

    protected virtual void ActivationEventHandler(string triggerName)
    {
        Activation?.Invoke(triggerName);
    }
}
