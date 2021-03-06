﻿using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
	public float multiplier = 1.5f;
	private float cache;
	private bool applied = false;

	public void Apply(GunScript gScript)
	{
		cache = gScript.damage * (multiplier - 1);
		gScript.damage += cache;
	}

	public void Remove(GunScript gScript)
	{
		gScript.damage -= cache;
	}
}