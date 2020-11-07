using UnityEngine;
using System.Collections;

public class AimPosition : MonoBehaviour
{
	[HideInInspector] public AimMode gscript;
	public Vector3 val;
	private Vector3 cache;
	private bool applied = false;

	public void Start()
	{
		gscript = transform.parent.GetComponent<GunScript>().GetComponentInChildren<AimMode>();
	}

	public void Apply()
	{
		cache = gscript.aimPosition1;
		gscript.aimPosition1 = val;
		gscript.aimPosition = val;
	}

	public void Remove()
	{
		gscript.aimPosition1 = cache;
		gscript.aimPosition = cache;
	}
}