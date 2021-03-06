﻿using UnityEngine;
using System.Collections;

public class InfiniteAmmo : MonoBehaviour
{
	public bool val;
	private bool cache;
	private bool applied = false;

	public void Apply(GunScript gScript)
	{
		cache = gScript.infiniteAmmo;
		gScript.infiniteAmmo = val;
	}

	public void Remove(GunScript gScript)
	{
		gScript.infiniteAmmo = cache;
	}
}