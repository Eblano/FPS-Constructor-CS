using UnityEngine;
using System.Collections;

public class WeaponDisplay : MonoBehaviour
{
	/*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
	// Sample script to display weapon info when a weapon is selected.
	private bool display = false;
	private float endTime;
	private GameObject weapon;
	private WeaponInfo WeaponInfo;

	public float displayTime = 2;

	void Start()
	{
		WeaponInfo = GetComponent<WeaponInfo>();
		display = false;
	}

	public void Select()
	{
		display = true;
		endTime = Time.time + displayTime;
	}

	void OnGUI()
	{
		if (display && Time.time != 0.0f)
		{
			GUI.skin.box.fontSize = 15;
			if (Time.time > endTime) display = false;
			GUI.Box(new Rect(Screen.width - 490, Screen.height - 160, 270, 150), WeaponInfo.gunName + "\n" + WeaponInfo.gunDescription + "\n");
		}
	}
}