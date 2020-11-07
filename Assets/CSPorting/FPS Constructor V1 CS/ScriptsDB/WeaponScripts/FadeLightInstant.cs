using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FadeLightInstant : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float delay;
    public float fadeTime;
    private float fadeSpeed;
    private float intensity;
    public float startIntensity;
    private Color color;
    private bool active1;
    public virtual void Start()
    {
        if (GetComponent<Light>() == null)
        {
            UnityEngine.Object.Destroy(this);
            return;
        }
    }

    public virtual void Update()
    {
        if (!active1)
        {
            return;
        }
        if (delay > 0f)
        {
            delay = delay - Time.deltaTime;
        }
        else
        {
            if (intensity > 0f)
            {
                intensity = intensity - (fadeSpeed * Time.deltaTime);
                GetComponent<Light>().intensity = intensity;
            }
        }
    }

    public virtual void Activate()
    {
        GetComponent<Light>().intensity = startIntensity;
        intensity = GetComponent<Light>().intensity;
        active1 = true;
        if (fadeTime > 0f)
        {
            fadeSpeed = intensity / fadeTime;
        }
        else
        {
            fadeSpeed = intensity;
        }
    }

    public FadeLightInstant()
    {
        startIntensity = 6;
    }
}