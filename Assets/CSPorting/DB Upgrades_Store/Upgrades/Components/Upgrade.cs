﻿using UnityEngine;
using System.Collections;

public class Upgrade : MonoBehaviour
{
	[HideInInspector] public bool applied = false;
	public bool owned = false;
	public bool locked = false;
	public string upgradeType;
	public string upgradeName;
	public string description = "Upgrade Locked";
	public string lockedDescription;
	public float buyPrice;
	public float sellPrice;
	public int scriptID = 0;
	public bool showInStore = true;

	private GunScript gScript;

	public void Start()
	{
		Init();
	}

	public void Init()
	{
		GunScript[] gscripts = transform.parent.GetComponents<GunScript>() as GunScript[];
		for (int q = 0; q < gscripts.Length; q++)
		{
			if (q == scriptID)
			{
				gScript = gscripts[q];
			}
		}
	}

	public void ApplyUpgrade()
	{
		Upgrade[] upgrades;
		upgrades = transform.parent.GetComponentsInChildren<Upgrade>();
		for (int i = 0; i < upgrades.Length; i++)
		{
			if (upgrades[i].upgradeType == upgradeType && upgrades[i] != this)
				upgrades[i].RemoveUpgrade();
		}
		if (applied)
			return;
		SendMessage("Apply", gScript);
		applied = true;
		SendMessageUpwards("ApplyUpgrade");
	}

	public void ApplyUpgradeInstant()
	{
		if (applied)
			return;
		BroadcastMessage("TempInstant");
		ApplyUpgrade();
	}

	public void RemoveUpgrade()
	{
		if (!applied)
			return;
		SendMessage("Remove", gScript);
		applied = false;
	}

	public void RemoveUpgradeInstant()
	{
		if (!applied)
			return;
		SendMessage("TempInstant");
		RemoveUpgrade();
	}

	public void DeleteUpgrade()
	{
		RemoveUpgrade();
		Destroy(gameObject);
	}
}