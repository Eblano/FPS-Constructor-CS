using UnityEngine;
using System.Collections;

public class PenetrateVal : MonoBehaviour
{
	public int val;
	private int cache;
	private bool applied = false;

	public void Apply(GunScript gscript)
	{
		cache = val - gscript.penetrateVal;
		gscript.penetrateVal += cache;
	}

	public void Remove(GunScript gscript)
	{
		gscript.penetrateVal -= cache;
	}
}