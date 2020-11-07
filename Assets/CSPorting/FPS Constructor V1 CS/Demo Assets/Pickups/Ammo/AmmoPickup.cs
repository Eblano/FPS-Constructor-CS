using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AmmoPickup : MonoBehaviour
{
    public int amount;
    public GunScript target;
    private GunScript wp;
    public float delay;
    private float nextTime;
    public bool limited;
    public int limit;
    private bool removed;
    public bool destroyAtLimit;
    //Called via message
    //Adds ammo to player
    public virtual void Interact()
    {
        if ((Time.time > nextTime) && ((limit != 0) || !limited)) //if it has been long enough, and we are either not past our limit or not limited
        {
            nextTime = Time.time + delay; //set next use time
            if (target == null) //if there isn't a target, use currently equipped weapon
            {
                wp = (GunScript)PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].GetComponent(typeof(GunScript)); //currently equipped weapon	
            }
            else
            {
                //otherwise use target
                wp = target;
            }
            if (wp.clips < wp.maxClips) //if ammo isn't already full
            {
                wp.clips = Mathf.Clamp(wp.clips + amount, wp.clips, wp.maxClips); //add up to max
                if (GetComponent<AudioSource>())
                {
                    GetComponent<AudioSource>().Play(); //play sound
                }
                removed = true; //decrement limit
            }
            wp.ApplyToSharedAmmo();
            if (wp.secondaryWeapon != null)
            {
                wp = wp.secondaryWeapon;
            }
            if (wp.clips < wp.maxClips) //if ammo isn't already full
            {
                wp.clips = Mathf.Clamp(wp.clips + amount, wp.clips, wp.maxClips); //add up to max
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
                removed = true;
            }
            wp.ApplyToSharedAmmo();
            if (removed)
            {
                limit--;
                removed = false;
            }
            if ((limit <= 0) && destroyAtLimit)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}