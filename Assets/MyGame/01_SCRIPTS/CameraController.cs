using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    //Rotation
    private float Mousex = 180;
    private float Mousey = 10;
    public float AngularVelocity = 4f;
    public GameObject Player;
    public CinemachineVirtualCamera cmVCam;

    public float shakeIntensity;
    public float shakeDuration;

    // Start is called before the first frame update
    void Start()
    {
        //Retire le curseur
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.pause)
        {
            transform.position = Player.transform.position;
            Rotate();
        }
    }

    void GetInput()
    {
        Mousex = (Mousex + AngularVelocity * Input.GetAxis("Mouse X")) % 360f;
        Mousey = (Mousey + AngularVelocity * Input.GetAxis("Mouse Y")) % 360f;
    }

    void Rotate()
    {
        GetInput();
        //Limite de la rotation
        if (!(Mousey < 60))
        {
            Mousey = 60;
        }
        if (!(Mousey > -60))
        {
            Mousey = -60;
        }

        //Gere la rotation
        transform.eulerAngles = new Vector3(Mousey, Mousex, 0);
    }
    public void ShakeCamera()
    {
        StartCoroutine(CameraShakeCoroutine());
    }

    IEnumerator CameraShakeCoroutine()
    {
        cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeIntensity;
        yield return new WaitForSeconds(shakeDuration);
        cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
}
