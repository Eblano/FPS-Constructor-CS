using UnityEngine;
using System.Collections;

public class ChargeTest : MonoBehaviour
{
    public GunScript gscript;
    public ParticleSystem[] emitters;
    public bool emitting = false;
    public ParticleSystem specialEmitter;
    public float minSpecial;

    public void LateUpdate()
    {
        GetComponentInChildren<Light>().range = gscript.chargeLevel * 10;
        if (gscript.chargeLevel > 0 || emitting)
        {
            gscript.gameObject.GetComponent<AudioSource>().pitch = gscript.chargeLevel;
            if (!emitting)
            {
                emitCharge(true);
            }
            else if (emitting)
            {
                emitCharge(false);
            }
        }
        else
        {
            gscript.gameObject.GetComponent<AudioSource>().pitch = gscript.firePitch;
            specialEmitter.enableEmission = false;
        }
    }

    public void emitCharge(bool s)
    {
        for (int i = 0; i < emitters.Length; i++)
        {
            emitters[i].enableEmission = s;
        }
        if (gscript.chargeLevel > minSpecial)
        {
            specialEmitter.enableEmission = true;
        }
        else
        {
            specialEmitter.enableEmission = false;
        }

        emitting = s;
    }
}