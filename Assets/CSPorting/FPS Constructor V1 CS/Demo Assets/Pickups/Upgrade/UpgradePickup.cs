using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class UpgradePickup : MonoBehaviour
{
    public bool apply;
    public bool own;
    public bool mustBeEquipped;
    public Upgrade upgrade;
    public bool destroys;
    private float nextTime;
    public float delay;
    public bool limited;
    public int limit;
    //Called via message
    //Gives Upgrade
    public virtual void Interact()
    {
        if (((Time.time > nextTime) && ((limit != 0) || !limited)) && (((GunScript)upgrade.transform.parent.GetComponent(typeof(GunScript))).gunActive || !mustBeEquipped)) //if it has been long enough, and we are either not past our limit or not limited
        {
            nextTime = Time.time + delay; //set next time
            if ((own && !upgrade.owned) || (apply && !upgrade.applied)) //if the upgrade isn't already applied
            {
                if (own)
                {
                    upgrade.owned = true;
                }
                if (apply)
                {
                    upgrade.ApplyUpgrade();
                }
                if (GetComponent<AudioSource>())
                {
                    GameObject audioObj = new GameObject("PickupSound");
                    audioObj.transform.position = transform.position;
                    ((TimedObjectDestructorDB)audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = GetComponent<AudioSource>().clip.length + 0.1f;
                    AudioSource aO = (AudioSource)audioObj.AddComponent(typeof(AudioSource)); //play sound
                    aO.clip = GetComponent<AudioSource>().clip;
                    aO.volume = GetComponent<AudioSource>().volume;
                    aO.pitch = GetComponent<AudioSource>().pitch;
                    aO.Play();
                    aO.loop = false;
                    aO.rolloffMode = AudioRolloffMode.Linear;
                }
                limit--; //decrement limit
            }
        }
        if ((limit <= 0) && destroys)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }

    public UpgradePickup()
    {
        apply = true;
        own = true;
        mustBeEquipped = true;
        delay = 1;
    }
}