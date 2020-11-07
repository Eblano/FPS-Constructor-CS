using UnityEngine;
using System.Collections;

public class SecondaryReloadEmptyAnim : MonoBehaviour
{
	public string val;
	private string cache;
	private bool applied = false;

	public void Apply(GunScript gScript)
	{
		cache = gScript.GetComponentInChildren<GunChildAnimation>().secondaryReloadEmpty;
		gScript.GetComponentInChildren<GunChildAnimation>().secondaryReloadEmpty = val;
	}

	public void Remove(GunScript gScript)
	{
		gScript.GetComponentInChildren<GunChildAnimation>().secondaryReloadEmpty = cache;
	}
}