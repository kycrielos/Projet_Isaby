using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private LineRenderer diagline;
    private LineRendererController nextCrystalRayon;

    private Vector3 spawnPosition;

    public bool IsActive;
    public bool isFirst;

    // Start is called before the first frame update
    void Start()
    {
        diagline = GetComponent<LineRenderer>();
        diagline.SetPosition(0, GetComponentInParent<Transform>().position + new Vector3(0, 0.1f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive || isFirst)
        {
            diagline.enabled = true;
            RaycastHit hit;
            if (Physics.Raycast(GetComponentInParent<Transform>().position + new Vector3(0, 0.1f, 0), GetComponentInParent<Transform>().forward * -1 + new Vector3(0, 0.1f, 0), out hit, Mathf.Infinity))
            {
                diagline.SetPosition(0, GetComponentInParent<Transform>().position + new Vector3(0, 0.1f, 0));
                diagline.SetPosition(1, hit.point);
                if (hit.collider.tag == "Crystal")
                {
                    nextCrystalRayon = hit.collider.gameObject.GetComponentInChildren<LineRendererController>();
                    nextCrystalRayon.IsActive = true;
                }
                else if (nextCrystalRayon != null)
                {
                    nextCrystalRayon.IsActive = false;
                    nextCrystalRayon.diagline.enabled = false;
                    nextCrystalRayon = null;
                }

                if (hit.collider.tag == "DoorKey")
                {
                    hit.collider.gameObject.GetComponent<DoorKeyScript>().Activation();
                }
            }
        }
        else if (nextCrystalRayon != null)
        {
            nextCrystalRayon.IsActive = false;
            nextCrystalRayon.diagline.enabled = false;
            nextCrystalRayon = null;
        }
    }
}
