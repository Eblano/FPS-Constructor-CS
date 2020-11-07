using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SprayScript : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    [UnityEngine.HideInInspector]
    public GunScript gunScript;
    private float trueDamage;
    private float trueForce;
    [UnityEngine.HideInInspector]
    public bool isActive;
    private ParticleSystem[] emitters;
    private int i;
    public virtual void Awake()
    {
        gunScript = (GunScript)transform.parent.GetComponent(typeof(GunScript));
        emitters = gameObject.GetComponentsInChildren<ParticleSystem>();
        isActive = false;
    }

    public virtual void OnParticleCollision(GameObject hitObj)
    {
        float dist = Vector3.Distance(hitObj.transform.position, transform.position);
        trueDamage = gunScript.damage;
        if (dist > gunScript.maxFalloffDist)
        {
            trueDamage = (gunScript.damage * Mathf.Pow(gunScript.falloffCoefficient, (gunScript.maxFalloffDist - gunScript.minFalloffDist) / gunScript.falloffDistanceScale)) * Time.deltaTime;
        }
        else
        {
            if ((dist < gunScript.maxFalloffDist) && (dist > gunScript.minFalloffDist))
            {
                trueDamage = (gunScript.damage * Mathf.Pow(gunScript.falloffCoefficient, (dist - gunScript.minFalloffDist) / gunScript.falloffDistanceScale)) * Time.deltaTime;
            }
        }
        object[] sendArray = new object[2];
        sendArray[0] = trueDamage;
        sendArray[1] = true;
        hitObj.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
        trueForce = gunScript.force * Mathf.Pow(gunScript.forceFalloffCoefficient, dist);
        if ((Rigidbody)hitObj.GetComponent(typeof(Rigidbody)))
        {
            Rigidbody rigid = (Rigidbody)hitObj.GetComponent(typeof(Rigidbody));
            Vector3 vectorForce = -Vector3.Normalize(transform.position - hitObj.transform.position) * trueForce;
            rigid.AddForce(vectorForce);
        }
    }

    public virtual void ToggleActive(bool activate)
    {
        if (activate == false)
        {
            isActive = false;
            foreach (ParticleSystem emitter in emitters)
            {
                emitter.enableEmission = false;
            }
        }
        else
        {
            isActive = true;
            foreach (ParticleSystem emitter in emitters)
            {
                emitter.enableEmission = true;
            }
        }
    }
}