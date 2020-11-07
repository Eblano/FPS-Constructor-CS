using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class DeathEffects : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public AnimationClip deathAnim;
    private bool dead;
    public Texture deadTexture;
    public float menuDelay;
    private float menuTime;
    public float menuSpeed;
    public virtual void Death()
    {
        UnityEngine.Object.Destroy((GunChildAnimation)GetComponent(typeof(GunChildAnimation)));
        GetComponent<Animation>().clip = deathAnim;
        GetComponent<Animation>().Play();
        dead = true;
        menuTime = Time.time + menuDelay;
    }

    public virtual void OnGUI()
    {
        if (!dead)
        {
            return;
        }
        float temp = Mathf.Clamp(Time.time - menuTime, 0, 1 / menuSpeed);
        GUI.color = new Color(1, 1, 1, temp * menuSpeed);
        if (deadTexture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), deadTexture);
        }
        if (GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 20, 200, 40), "Try Again?") && (temp >= (0.8f / menuSpeed)))
        {
            PlayerWeapons.player.BroadcastMessage("UnFreeze");
            Application.LoadLevel(0);
        }
    }
}