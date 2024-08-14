using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColourShift : MonoBehaviour
{
    public Volume vol;
    public float maxDistortion;

    public List<Transform> bunghole;
    public float maxDist;
    public float maxBlueShift;
    ColorAdjustments cap;
    LensDistortion distortion;
    public bool isBH = true;
    // Start is called before the first frame update
    void Start()
    {
        ColorAdjustments ca;
        vol.profile.TryGet<ColorAdjustments>(out ca);
        cap = ca;
        LensDistortion lensDist;
        vol.profile.TryGet<LensDistortion>(out lensDist);
        distortion = lensDist;
        distortion.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void FixedUpdate()
    {
        float biggestD = 0;
        foreach (Transform t in bunghole)
        {
            float dist = Vector3.Distance(transform.position, t.position);
            if (dist > biggestD)
            {
                biggestD = dist;
            }
        }
        float rnge;
        if (biggestD < maxDist)
        {
            rnge = (biggestD / maxDist);
        }
        else
        {
            rnge = 1;
        }

        //float newrnge = Mathf.Clamp(Mathf.Pow(rnge, 2), maxBlueShift, 1);
        //float newrnge = 1 / (1 + Mathf.Exp(-rnge)) + 0.269f;
        distortion.intensity.value = (1-rnge) * maxDistortion;

        if (isBH)
        {
            print(rnge);
            cap.hueShift.value = 177*(1-rnge);
        }

    }
}