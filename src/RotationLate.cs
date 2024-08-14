using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Valve.VR.InteractionSystem;
using static Unity.Burst.Intrinsics.X86.Avx;

public class RotationLate : MonoBehaviour
{
    // Public Vars
    public float rotate_angle = 0.5f;
    public GameObject centre_object = null;
    public List<Vector3> points;
    public bool rotating = true;
    public CircularDrive circ;
    public float speedMult = 5.0f;
    public float rotateSpeed = 0.1f;
    public Transform BlackHole;
    public Transform WhiteHole;
    public Animator wormhole_anim;
    public Transform wormhole_cam_pos;
    public float animation_len;
    public Volume vol;
    public Volume vol_fallback;
    public Transform innerHole;
    public Material wormholeMat;

    // Private vars
    private int count = 0;
    private Vector3 look = new Vector3(-90, 0, 180);
    private bool already_at_wormhole = false;
    private bool animation_finished = false;
    private Vector3 startPos;
    ColorAdjustments cap;
    ColorAdjustments cap_fb;
    LensDistortion distort;
    LensDistortion distort_fb;
    public float maxDistortion = 0.2f;
    public float maxDist = 11000;
    private bool animRunning = false;



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        ColorAdjustments ca;
        vol.profile.TryGet<ColorAdjustments>(out ca);
        cap = ca;

        ColorAdjustments ca_fb;
        vol_fallback.profile.TryGet<ColorAdjustments>(out ca_fb);
        cap_fb = ca_fb;

        LensDistortion distortion;
        vol.profile.TryGet<LensDistortion>(out distortion);
        distort = distortion;

        LensDistortion distortion_fb;
        vol_fallback.profile.TryGet<LensDistortion>(out distortion_fb);
        distort_fb = distortion_fb;

    }

    private void FixedUpdate()
    { 
        if (rotating)
        {
            // Check if the object to rotate and angle are assigned
            if (centre_object != null)
            {
                // Get the center of the object to rotate
                Vector3 center = centre_object.transform.position;

                // Rotate the object around the center by the given angle
                transform.RotateAround(center, Vector3.up, rotate_angle);
            }
            else
            {
                Debug.LogWarning("Object to rotate is not assigned.");
            }
        }
        else if(!already_at_wormhole && Vector3.Distance(transform.position, BlackHole.position) > 120.0f)
        {

            if (Vector3.Distance(transform.position, points[count]) < 0.05f)
            {
                count++;
                count %= points.Count;
            }
            // Create a rotation towards the target
            Quaternion targetRotation = Quaternion.LookRotation(points[count] - transform.position) * Quaternion.Euler(look);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);
            transform.position = Vector3.MoveTowards(transform.position, points[count], (circ.outAngle / circ.maxAngle) * speedMult);
            if (count == 1)
            {
                float rnge = 0;
                // Heading towards BH
                float dist = Vector3.Distance(transform.position, BlackHole.position);
                if (dist < 30f)
                {
                    // Make inner hole bigger
                    innerHole.localScale += new Vector3(0.05f, 0f, 0.05f);
                }
                if (dist < maxDist)
                {
                    rnge = dist / maxDist;
                }
                else
                {
                    rnge = 1;
                }
                // Set shift values - BH
                distort.intensity.value = (1 - rnge) * maxDistortion;
                float newrnge = Mathf.Clamp(Mathf.Sqrt(rnge + 0.5f), 0.7f, 1);
                cap.colorFilter.value = new Color(newrnge, newrnge, 1f, 0.2f);
                distort_fb.intensity.value = (1 - rnge) * maxDistortion;
                cap_fb.colorFilter.value = new Color(newrnge, newrnge, 1f, 0.2f);
            }
            
        }
        else if(!animation_finished)
        {
            distort.intensity.value = 0.1f;
            distort_fb.intensity.value = 0.1f;
            if (!animRunning)
            {
                animRunning = true;
                StartCoroutine(FadeToBlack());
            }
            // Near black hole centre
        }
        else if (Vector3.Distance(transform.position, startPos) > 5.0f)
        {
            // Now at white hole
            float rnge = 0;
            float dist = Vector3.Distance(transform.position, WhiteHole.position);
            if (dist < maxDist)
            {
                rnge = dist/maxDist;
            }
            else
            {
                rnge = 1;
            }
            // Set shift values - BH
            distort.intensity.value = (1 - rnge) * maxDistortion;
            distort_fb.intensity.value = (1 - rnge) * maxDistortion;
            // Create a rotation towards the target
            Quaternion targetRotation = Quaternion.LookRotation(startPos - transform.position) * Quaternion.Euler(look);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);
            transform.position = Vector3.MoveTowards(transform.position, startPos, (circ.outAngle / circ.maxAngle) * speedMult);
            
        }
        else
        {
            // we are back at start
            rotating = true;
            already_at_wormhole = false;
            animation_finished = false;

        }
    }
    IEnumerator waitForAnimation()
    {
        transform.position = wormhole_cam_pos.position;
        cap.hueShift.value = -40f;
        cap_fb.hueShift.value = -40f;
        // Teleport to wormhole position
        already_at_wormhole = true;
        // Play animation
        wormhole_anim.enabled = false;
        wormhole_anim.enabled = true;

        float elapsedTime = 0f;

        wormholeMat.mainTextureScale = new Vector2(1f, 1f);

        while (elapsedTime < animation_len)
        {
            // MAKE MATERIAL MOVE ------------------------
            wormholeMat.mainTextureScale += new Vector2(0f, 0.004f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        wormholeMat.mainTextureScale = new Vector2(1f, 1f);
        //yield return new WaitForSeconds(animation_len);
        StartCoroutine(FadeToWhite());
    }
    IEnumerator FadeToBlack()
    {

        float elapsedTime = 0f;
        Color initialColor = cap.colorFilter.value;
        Color targetColor = Color.black;

        while (elapsedTime < 0.5f)
        {
            float t = elapsedTime / 0.5f;
            cap.colorFilter.value = Color.Lerp(initialColor, targetColor, t);
            cap_fb.colorFilter.value = Color.Lerp(initialColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cap.colorFilter.value = targetColor;
        cap_fb.colorFilter.value = targetColor;

        StartCoroutine(FadeFromBlack());
        StartCoroutine(waitForAnimation());
    }

    IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        Color initialColor = cap.colorFilter.value;
        Color targetColor = Color.white;

        while (elapsedTime < 1f)
        {
            float t = elapsedTime / 1f;
            cap.colorFilter.value = Color.Lerp(initialColor, targetColor, t);
            cap_fb.colorFilter.value = Color.Lerp(initialColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cap.colorFilter.value = targetColor;
        cap_fb.colorFilter.value = targetColor;
    }

    IEnumerator FadeToWhite()
    {
        float elapsedTime = 0f;
        Color initialColor = cap.colorFilter.value;
        Color targetColor = new Color(1, 0.5f, 1, 1);

        while (elapsedTime < 0.7f)
        {
            float t = elapsedTime / 0.7f;
            cap.colorFilter.value = Color.Lerp(initialColor, targetColor, t);
            cap_fb.colorFilter.value = Color.Lerp(initialColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cap.colorFilter.value = targetColor;
        cap_fb.colorFilter.value = targetColor;
        // Move to exit of white hole
        transform.position = WhiteHole.position;
        animation_finished = true;
        StartCoroutine(FadeFromBlack());
    }
}
