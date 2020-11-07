using UnityEngine;
using System.Collections;

public class ReloadAnim : MonoBehaviour
{
	public string val;
	private string cache;
	private bool applied = false;

	public void Apply(GunScript gScript)
	{
		cache = gScript.GetComponentInChildren<GunChildAnimation>().reloadAnim;
		gScript.GetComponentInChildren<GunChildAnimation>().reloadAnim = val;
	}

	public void Remove(GunScript gScript)
	{
		gScript.GetComponentInChildren<GunChildAnimation>().reloadAnim = cache;
	}
}