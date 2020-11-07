using UnityEngine;
using System.Collections;

public class HealthDisplay : MonoBehaviour
{
	/*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
	public PlayerHealth playerHealth;

	void Start()
	{
		playerHealth = GetComponent<PlayerHealth>();
	}

	void OnGUI()
	{
		GUI.skin.box.fontSize = 25;
		GUI.Box(new Rect(10, Screen.height - 50, 200, 40), "Health: " + Mathf.Round(playerHealth.health));
	}
}