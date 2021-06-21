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
            if (MyInputManager.Instance.GetKeyDown("Interaction") && isReady)
            {
                camscript.m_XAxis.m_MaxSpeed = 0;
                camscript.m_YAxis.m_MaxSpeed = 0;
                GameManager.Instance.currentState = GameManager.PlayerState.Idle;
                GameManager.Instance.player.SetActive(true);
                GameManager.Instance.player.transform.position = new Vector3(camTransform.position.x, GameManager.Instance.player.transform.position.y, camTransform.position.z);
                GameManager.Instance.player.transform.eulerAngles = new Vector3(0,transform.rotation.eulerAngles.y + 180f, 0);
                GameManager.Instance.followPlayer.GetComponent<CameraController>().Mousex = GameManager.Instance.player.transform.rotation.eulerAngles.y;
                camscript.Priority = 0;
                isReady = false;
                activated = false;
                StartCoroutine(ActionDelay());
            }
        }
    }

    void ActivationCrystal(GameObject triggerObj)
    {
        if (triggerObj.name == gameObject.name + "Trigger" && isReady && GameManager.Instance.timeSinceGrounded < 0.75f)
        {
            camscript.m_XAxis.Value = Camera.main.transform.rotation.eulerAngles.y;
            camscript.m_XAxis.m_MaxSpeed = 0;
            camscript.m_YAxis.m_MaxSpeed = 0;
            camscript.Priority = 11;
            isReady = false;
            GameManager.Instance.currentState = GameManager.PlayerState.None;
            GameManager.Instance.player.GetComponent<PlayerController>().Jump();
            GameManager.Instance.player.SetActive(false);
            activated = true;
            StartCoroutine(ActionDelay());
        }
    }

    IEnumerator ActionDelay()
    {
        yield return new WaitForSeconds(delay);
        camscript.m_XAxis.m_MaxSpeed = 180;
        camscript.m_YAxis.m_MaxSpeed = 6;
        isReady = true;
    }
}
