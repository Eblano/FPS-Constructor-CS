﻿using UnityEngine;
using System.Collections;

public class BurstFire : MonoBehaviour
{
	public bool val;
	private bool cache;
	private bool applied = false;

	public void Apply(GunScript gScript)
	{
		cache = gScript.burstFire;
		gScript.burstFire = val;
	}

	public void Remove(GunScript gScript)
	{
		gScript.burstFire = cache;
	}
}