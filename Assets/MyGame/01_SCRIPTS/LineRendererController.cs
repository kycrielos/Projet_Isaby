using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private Transform crystalTransform;
    private LineRenderer diagline;
    private LineRendererController nextCrystalRayon;

    private Vector3 spawnPosition;

    public bool IsActive;
    // Start is called before the first frame update
    void Start()
    {
        diagline = GetComponent<LineRenderer>();
        crystalTransform = GetComponentInParent<Transform>();
        spawnPosition = crystalTransform.position + new Vector3(0, 0.2f, 0);
        diagline.SetPosition(0, spawnPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            diagline.enabled = true;
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, crystalTransform.forward * -1 + new Vector3(0, 0.1f, 0), out hit, Mathf.Infinity))
            {
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
