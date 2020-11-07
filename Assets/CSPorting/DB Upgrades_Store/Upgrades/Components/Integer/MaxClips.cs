using UnityEngine;
using System.Collections;

public class MaxClips : MonoBehaviour
{
	public int val;
	private int cache;
	private bool applied = false;

	public void Apply(GunScript gscript)
	{
		cache = val - gscript.maxClips;
		gscript.maxClips += cache;
	}

	public void Remove(GunScript gscript)
	{
		gscript.maxClips -= cache;
	}
}