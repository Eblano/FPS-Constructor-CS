using UnityEngine;
using System.Collections;

[System.Serializable]
public class FadeLight : MonoBehaviour
{
    public float delay;
    public float fadeTime;
    private float fadeSpeed;
    private float intensity;
    private Color color;
    public virtual void Start()//alpha = 1.0;
    {
        if (GetComponent<Light>() == null)
        {
            UnityEngine.Object.Destroy(this);
            return;
        }
        intensity = GetComponent<Light>().intensity;
        fadeTime = Mathf.Abs(fadeTime);
        if (fadeTime > 0f)
        {
            fadeSpeed = intensity / fadeTime;
        }
        else
        {
            fadeSpeed = intensity;
        }
    }

    public virtual void Update()
    {
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
}