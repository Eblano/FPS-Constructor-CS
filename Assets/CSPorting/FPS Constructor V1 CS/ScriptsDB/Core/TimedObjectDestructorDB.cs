using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class TimedObjectDestructorDB : MonoBehaviour
{
    public float timeOut;
    public bool detachChildren;
    public virtual void Awake()
    {
        Invoke("DestroyNow", timeOut);
    }

    public virtual void DestroyNow()
    {
        if (detachChildren)
        {
            transform.DetachChildren();
        }
        UnityEngine.Object.DestroyObject(gameObject);
    }

    public TimedObjectDestructorDB()
    {
        timeOut = 1f;
    }
}