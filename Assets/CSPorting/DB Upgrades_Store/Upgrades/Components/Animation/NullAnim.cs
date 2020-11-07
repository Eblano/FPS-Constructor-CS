using UnityEngine;
using System.Collections;

public class NullAnim : MonoBehaviour
{
	public string val;
	private string cache;
	private bool applied = false;

	public void Apply(GunScript gScript)
	{
		cache = gScript.GetComponentInChildren<GunChildAnimation>().nullAnim;
		gScript.GetComponentInChildren<GunChildAnimation>().nullAnim = val;
	}

	public void Remove(GunScript gScript)
	{
		gScript.GetComponentInChildren<GunChildAnimation>().nullAnim = cache;
	}
}