using UnityEngine;
using System.Collections;

public class ReloadOutAnim : MonoBehaviour
{
	public string val;
	private string cache;
	private bool applied = false;

	public void Apply(GunScript gScript)
	{
		cache = gScript.GetComponentInChildren<GunChildAnimation>().reloadOut;
		gScript.GetComponentInChildren<GunChildAnimation>().reloadOut = val;
	}

	public void Remove(GunScript gScript)
	{
		gScript.GetComponentInChildren<GunChildAnimation>().reloadOut = cache;
	}
}