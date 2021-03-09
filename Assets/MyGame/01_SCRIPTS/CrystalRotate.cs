﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRotate : MonoBehaviour
{
    public Transform camTransform;
    public Cinemachine.CinemachineFreeLook camscript;

    public float angularVelocity;

    private bool activated;

    public float delay;
    private bool isReady = true;

    // Start is called before the first frame update
    void Start()
    {
        TriggerManager.Activation += ActivationCrystal;
    }

    private void Update()
    {
        if (activated)
        {
            float step = angularVelocity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, camTransform.position - transform.position, step, 0));
            if (Input.GetButtonDown("Interaction") && isReady)
            {
                GameManager.Instance.player.SetActive(true);
                camscript.Priority = 0;
                isReady = false;
                GameManager.Instance.player.transform.position = new Vector3(camTransform.position.x, GameManager.Instance.player.transform.position.y, camTransform.position.z);
                activated = false;
                StartCoroutine(ActionDelay());
            }
        }
    }

    void ActivationCrystal(GameObject triggerObj)
    {
        if (triggerObj.name == gameObject.name + "Trigger" && isReady)
        {
            camscript.Priority = 11;
            isReady = false;
            GameManager.Instance.player.SetActive(false);
            activated = true;
            StartCoroutine(ActionDelay());
        }
    }

    IEnumerator ActionDelay()
    {
        yield return new WaitForSeconds(delay);
        isReady = true;
    }
}
