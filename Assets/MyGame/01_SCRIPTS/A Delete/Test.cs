using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Test : MonoBehaviour
{
    public Light L1;
    public Light L2;

    private float timer;

    public CinemachineVirtualCamera cmVCam;

    private float speed = 0.5f;
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(ColorChangeRoutine());
    }

    private IEnumerator ColorChangeRoutine()
    {
        while (true)
        {
            L1.color = new Color32(System.Convert.ToByte(Random.Range(0, 255)), 90, 255, 255);
            L2.color = new Color32(System.Convert.ToByte(Random.Range(0, 255)), 90, 255, 255);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.Rotate(new Vector3(0, speed, 0f));
        if (timer >= 30)
        {
            cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 10;
            speed = 4;
        }
    }
}
