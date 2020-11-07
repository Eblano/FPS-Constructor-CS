using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FootStep : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float footstepInterval;
    public float footstepVolume;
    private float distanceMoved;
    private Vector3 lastStep;
    private bool landing;
    [UnityEngine.HideInInspector]
    public EffectsManager effectsManager;
    [UnityEngine.HideInInspector]
    public CharacterMotorDB characterMotor;
    [UnityEngine.HideInInspector]
    public AudioSource source;
    [UnityEngine.HideInInspector]
    public AudioClip soundClip;
    [UnityEngine.HideInInspector]
    public int playDex;
    [UnityEngine.HideInInspector]
    public GameObject surface;
    public virtual void Awake()
    {
        effectsManager = (EffectsManager)GameObject.FindObjectOfType(typeof(EffectsManager));
        characterMotor = (CharacterMotorDB)gameObject.GetComponent(typeof(CharacterMotorDB));
        source = (AudioSource)gameObject.GetComponent(typeof(AudioSource));
    }

    public virtual void Update()
    {
        if (!PlayerWeapons.playerActive)
        {
            return;
        }
        if (!characterMotor.grounded)
        {
            distanceMoved = footstepInterval;
            landing = true;
        }
        distanceMoved = distanceMoved + Vector3.Distance(transform.position, lastStep);
        lastStep = transform.position;
        if (CharacterMotorDB.walking)//|| (landing && characterMotor.grounded))){
        {
            if (CharacterMotorDB.prone)
            {
                Crawl();
                landing = false;
            }
            else
            {
                Footstep();
                landing = false;
            }
        }
    }

    public virtual void Airborne()
    {
        if (CharacterMotorDB.prone)
        {
            Crawl();
            landing = false;
        }
        else
        {
            Footstep();
            landing = false;
        }
    }

    public virtual void Landed()
    {
        if (CharacterMotorDB.prone)
        {
            Crawl();
            landing = false;
        }
        else
        {
            Footstep();
            landing = false;
        }
    }

    public virtual void Footstep()
    {
        if (distanceMoved >= footstepInterval)
        {
            GetClip();
            /*source.clip = soundClip;
		source.volume = footstepVolume;
		source.Play();*/
            if (soundClip != null)
            {
                GameObject audioObj = new GameObject("Footstep");
                audioObj.transform.position = transform.position;
                audioObj.transform.parent = transform;
                ((TimedObjectDestructorDB)audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = soundClip.length + 0.1f;
                AudioSource aO = (AudioSource)audioObj.AddComponent(typeof(AudioSource));
                aO.clip = soundClip;
                aO.volume = footstepVolume;
                aO.Play();
                aO.loop = false;
                aO.rolloffMode = AudioRolloffMode.Linear;
            }
            distanceMoved = 0;
        }
    }

    public virtual void Crawl()
    {
        if (distanceMoved >= footstepInterval)
        {
            GetCrawlClip();
            /*source.clip = soundClip;
		source.volume = footstepVolume;
		source.Play();*/
            if (soundClip != null)
            {
                GameObject audioObj = new GameObject("Footstep");
                audioObj.transform.position = transform.position;
                audioObj.transform.parent = transform;
                ((TimedObjectDestructorDB)audioObj.AddComponent(typeof(TimedObjectDestructorDB))).timeOut = soundClip.length + 0.1f;
                AudioSource aO = (AudioSource)audioObj.AddComponent(typeof(AudioSource));
                aO.clip = soundClip;
                aO.volume = footstepVolume;
                aO.Play();
                aO.loop = false;
                aO.rolloffMode = AudioRolloffMode.Linear;
            }
            distanceMoved = 0;
        }
    }

    //This function, called by Crawl, gets a random clip and sets soundClip to equal that clip
    public virtual void GetCrawlClip()
    {
        RaycastHit hit = default(RaycastHit);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100, ~(1 << PlayerWeapons.playerLayer)))
        {
            if ((UseEffects)hit.transform.GetComponent(typeof(UseEffects)))
            {
                UseEffects effectScript = (UseEffects)hit.transform.GetComponent(typeof(UseEffects));
                int dex = effectScript.setIndex;
                if (!(effectsManager.setArray[0] == null))
                {
                    if (!(effectsManager.setArray[dex].crawlSounds == null))
                    {
                        soundClip = effectsManager.setArray[dex].crawlSounds[playDex];
                        if (playDex >= (effectsManager.setArray[dex].lastCrawlSound - 1))
                        {
                            playDex = 0;
                        }
                        else
                        {
                            playDex++;
                        }
                    }
                }
            }
        }
    }

    //This function, called by Footstep, gets a random clip and sets soundClip to equal that clip
    public virtual void GetClip()
    {
        RaycastHit hit = default(RaycastHit);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100, ~(1 << PlayerWeapons.playerLayer)))
        {
            if ((UseEffects)hit.transform.GetComponent(typeof(UseEffects)))
            {
                UseEffects effectScript = (UseEffects)hit.transform.GetComponent(typeof(UseEffects));
                int dex = effectScript.setIndex;
                if (!(effectsManager.setArray[0] == null))
                {
                    if (!(effectsManager.setArray[dex].footstepSounds == null))
                    {
                        soundClip = effectsManager.setArray[dex].footstepSounds[playDex];
                        if (playDex >= (effectsManager.setArray[dex].lastFootstepSound - 1))
                        {
                            playDex = 0;
                        }
                        else
                        {
                            playDex++;
                        }
                    }
                }
            }
        }
    }

    public FootStep()
    {
        footstepInterval = 0.5f;
        footstepVolume = 0.5f;
    }
}