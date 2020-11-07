using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MuzzleFlash : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public bool isPrimary;
    public void MuzzleFlash_(bool temp)
    {
        BroadcastMessage("Activate", SendMessageOptions.DontRequireReceiver);
        if (temp != isPrimary)
        {
            return;
        }
        object[] emitters = new object[0];
        emitters = GetComponentsInChildren(typeof(ParticleSystem));
        int i = 0;
        while (i < emitters.Length)
        {
            ParticleSystem p = emitters[i] as ParticleSystem;
            if (p != null)
            {
                p.Play();
            }
            i++;
        }
    }

    public MuzzleFlash()
    {
        isPrimary = true;
    }
}