using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Grenade : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float delay;
    public float timeOut;
    public bool detachChildren;
    public Transform explosion;
    public bool explodeAfterBounce;
    private bool hasCollided;
    private float explodeTime;
    public GameObject[] playerThings;
    public virtual void Start()
    {
        explodeTime = Time.time + timeOut;
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

    public Grenade()
    {
        delay = 1f;
        timeOut = 1f;
    }
}