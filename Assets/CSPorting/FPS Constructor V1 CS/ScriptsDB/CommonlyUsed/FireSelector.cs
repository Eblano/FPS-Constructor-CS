using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FireSelector : MonoBehaviour
{
    public GunScript gscript;
    public int state;
    public AudioClip sound;
    public float soundVolume;
    public string anim;
    public virtual void Start()
    {
        gscript.autoFire = state == 0;
        gscript.burstFire = state == 1;
    }

    public virtual void Cycle()
    {
        if (((!gscript.gunActive || AimMode.sprintingPublic) || LockCursor.unPaused) || GunScript.reloading)
        {
            return;
        }
        GetComponent<AudioSource>().PlayOneShot(sound, soundVolume);
        if (anim != "")
        {
            BroadcastMessage("PlayAnim", anim);
        }
        state++;
        if (state == 3)
        {
            state = 0;
        }
        gscript.autoFire = state == 0;
        gscript.burstFire = state == 1;
    }

    public virtual void Update()
    {
        if (InputDB.GetButtonDown("Fire2"))
        {
            Cycle();
        }
    }

    public FireSelector()
    {
        soundVolume = 1;
    }
}