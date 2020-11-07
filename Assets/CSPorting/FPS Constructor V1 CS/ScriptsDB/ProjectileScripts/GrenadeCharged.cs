using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class GrenadeCharged : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float delay;
    public float timeOut;
    public bool detachChildren;
    public Transform explosion;
    public Transform explosionCharged;
    public bool explodeAfterBounce;
    private bool hasCollided;
    private float explodeTime;
    public GameObject[] playerThings;
    public float chargeVal;
    public virtual void Start()
    {
        explodeTime = Time.time + timeOut;
        playerThings = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        while (i < playerThings.Length)
        {
            if (playerThings[i].GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), playerThings[i].GetComponent<Collider>());
            }
            i++;
        }
    }

    public virtual IEnumerator OnCollisionEnter(Collision collision)
    {
        if (hasCollided || !explodeAfterBounce)
        {
            DestroyNow();
        }
        yield return new WaitForSeconds(delay);
        hasCollided = true;
    }

    public virtual void ChargeLevel(float charge)
    {
        if (charge >= chargeVal)
        {
            ((Rigidbody)GetComponent(typeof(Rigidbody))).useGravity = false;
            explosion = explosionCharged;
        }
    }

    public virtual void DestroyNow()
    {
        if (detachChildren)
        {
            transform.DetachChildren();
        }
        UnityEngine.Object.DestroyObject(gameObject);
        if (explosion)
        {
            UnityEngine.Object.Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    public virtual void Update()
    {
        RaycastHit hit = default(RaycastHit);
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        if (Time.time > explodeTime)
        {
            DestroyNow();
        }
    }

    public GrenadeCharged()
    {
        delay = 1f;
        timeOut = 1f;
    }
}