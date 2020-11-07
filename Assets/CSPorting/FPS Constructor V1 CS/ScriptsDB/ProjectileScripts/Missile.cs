using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Missile : MonoBehaviour
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
    public bool explodeAfterBounce;
    private bool hasCollided;
    private float explodeTime;
    private float initiateTime;
    public GameObject[] playerThings;
    public Transform t;
    public float turnSpeed;
    public float flySpeed;
    public float initiatedSpeed;
    public ParticleSystem em;
    public bool soundPlaying;
    public GameObject lockObj;
    private Camera cam;
    //private var hasExploded : boolean = false;
    public virtual void Start()
    {
        explodeTime = Time.time + timeOut;
        initiateTime = Time.time + delay;
        cam = GameObject.FindWithTag("WeaponCamera").GetComponent<Camera>();
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
        LockOnMissile temp = null;
        temp = (LockOnMissile)GameObject.FindWithTag("Missile").GetComponent(typeof(LockOnMissile));
        t = temp.Target();
        if (t != null)
        {
            lockObj.transform.position = t.position;
            lockObj.transform.parent = null;
        }
    }

    public virtual void DestroyNow()
    {
        if (detachChildren)
        {
            transform.DetachChildren();
        }
        if (lockObj != null)
        {
            UnityEngine.Object.Destroy(lockObj);
        }
        UnityEngine.Object.DestroyObject(gameObject);
        if (explosion)
        {
            UnityEngine.Object.Instantiate(explosion, transform.position, new Quaternion(0, 0, 0, 0));
        }
    }

    public virtual void LateUpdate()
    {
        Quaternion temp = default(Quaternion);
        if (lockObj != null)
        {
            if (t != null)
            {
                ((Renderer)lockObj.GetComponentInChildren(typeof(Renderer))).enabled = true;
                lockObj.transform.position = t.position;
            }
            else
            {
                ((Renderer)lockObj.GetComponentInChildren(typeof(Renderer))).enabled = false;
            }
            lockObj.transform.LookAt(cam.transform);
        }
        if (Time.time > initiateTime)
        {
            if (!soundPlaying)
            {
                GetComponent<AudioSource>().Play();
                soundPlaying = true;
            }
            if (t != null)
            {
                temp = Quaternion.LookRotation(t.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, temp, Time.deltaTime * turnSpeed);
            }
            else
            {
                UnityEngine.Object.Destroy(lockObj);
            }
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward) * initiatedSpeed;
            em.enableEmission = true;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward) * flySpeed;
            em.enableEmission = false;
        }
        if (Time.time > explodeTime)
        {
            DestroyNow();
        }
    }

    public Missile()
    {
        delay = 1f;
        timeOut = 1f;
    }
}