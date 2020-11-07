using UnityEngine;
using System.Collections;

public class AmmoManager : MonoBehaviour
{
	/*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
	[HideInInspector]
	public string[] tempNamesArray = { "Ammo Set 1", "Ammo Set 2", "Ammo Set 3", "Ammo Set 4", "Ammo Set 5", "Ammo Set 6", "Ammo Set 7", "Ammo Set 8", "Ammo Set 9", "Ammo Set 10" };
	public string[] namesArray = new string[10];
	public int[] clipsArray = new int[10];
	public int[] maxClipsArray = new int[10];
	public bool[] infiniteArray = new bool[10];
}