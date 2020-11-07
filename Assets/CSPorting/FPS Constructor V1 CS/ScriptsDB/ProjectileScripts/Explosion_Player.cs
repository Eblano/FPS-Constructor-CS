using UnityEngine;
using System.Collections;

[System.Serializable]
public class Explosion_Player : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float explosionRadius;
    public float explosionPower;
    public float explosionDamage;
    public float explosionTimeout;
    public float vFactor;
    public float shakeFactor;
    public float minHitShake;
    public float minShake;
    public int highestParent;
    public GameObject[] parentArray;
    public virtual bool AlreadyHit(GameObject GO) //if this function returns true, we have already hit another child of this object's highest parent
    {
        GameObject toCompare = FindTopParent(GO);
        bool toReturn = false;
        int i = 0;
        while (i < highestParent)
        {
            if (parentArray[i] == toCompare)
            {
                toReturn = true;
                break;
            }
            i++;
        }
        if (toReturn == false)
        {
            parentArray[highestParent] = toCompare;
            highestParent++;
        }
        return toReturn;
    }

    //Finds the top parent, *OR* the first parent with EnemyDamageReceiver
    //If the top parent has no EnemyDamageReceiver, it returns the object passed in instead, as if there was no parent
    public virtual GameObject FindTopParent(GameObject GO)
    {
        Transform tempTransform = null;
        GameObject returnObj = null;
        bool keepLooping = true;
        if (GO.transform.parent != null)
        {
            tempTransform = GO.transform;
            while (keepLooping)
            {
                if (tempTransform.parent != null)
                {
                    tempTransform = tempTransform.parent;
                    if ((EnemyDamageReceiver)tempTransform.GetComponent(typeof(EnemyDamageReceiver)))
                    {
                        keepLooping = false;
                    }
                }
                else
                {
                    keepLooping = false;
                }
            }
            if ((EnemyDamageReceiver)tempTransform.GetComponent(typeof(EnemyDamageReceiver)))
            {
                returnObj = tempTransform.gameObject;
            }
            else
            {
                returnObj = GO;
            }
        }
        else
        {
            returnObj = GO;
        }
        return returnObj;
    }

    public virtual IEnumerator Start()
    {
        parentArray = new GameObject[128]; //Arbitrary array size; can be increased
        highestParent = 0;
        object[] sendArray = new object[2];
        bool shook = false;
        Vector3 explosionPosition = transform.position;
        // Apply damage to close by objects first
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (AlreadyHit(hit.gameObject) == false)
            {
                // Calculate distance from the explosion position to the closest point on the collider
                Vector3 closestPoint = hit.ClosestPointOnBounds(explosionPosition);
                float distance = Vector3.Distance(closestPoint, explosionPosition);
                // The hit points we apply fall decrease with distance from the explosion point
                float hitPoints = 1f - Mathf.Clamp01(distance / explosionRadius);
                if ((hit.tag == "Player") && !shook)
                {
                    shook = true;
                    CameraShake.ShakeCam(Mathf.Max(hitPoints * shakeFactor, minShake), 10, Mathf.Max(hitPoints * shakeFactor, 0.3f));
                }
                hitPoints = hitPoints * explosionDamage;
                // Tell the rigidbody or any other script attached to the hit object how much damage is to be applied!
                if (hit.gameObject.layer != 2)
                {
                    sendArray[0] = hitPoints;
                    sendArray[1] = true;
                    hit.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
                    hit.SendMessageUpwards("Direction", transform, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        if (!shook)
        {
            shook = true;
            CameraShake.ShakeCam(minShake, 10, minShake);
        }
        // Apply explosion forces to all rigidbodies
        // This needs to be in two steps for ragdolls to work correctly.
        // (Enemies are first turned into ragdolls with ApplyDamage then we apply forces to all the spawned body parts)
        colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>() && (hit.gameObject.layer != PlayerWeapons.playerLayer))
            {
                hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRadius, vFactor);
            }
        }
        // stop emitting particles
        if (GetComponent<ParticleSystem>())
        {
            GetComponent<ParticleSystem>().enableEmission = true;
            yield return new WaitForSeconds(0.5f);
            GetComponent<ParticleSystem>().enableEmission = false;
        }
        // destroy the explosion after a while
        UnityEngine.Object.Destroy(gameObject, explosionTimeout);
    }

    public Explosion_Player()
    {
        explosionRadius = 5f;
        explosionPower = 10f;
        explosionDamage = 100f;
        explosionTimeout = 2f;
        vFactor = 3;
        shakeFactor = 1.5f;
        minHitShake = 0.07f;
        minShake = 0.01f;
    }
}