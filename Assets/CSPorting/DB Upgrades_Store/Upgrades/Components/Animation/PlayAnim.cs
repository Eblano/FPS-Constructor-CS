using UnityEngine;
using System.Collections;

public class PlayAnim : MonoBehaviour
{
	public string removeAnim;
	public string applyAnim;

	public void Apply(GunScript g)
	{
		if (applyAnim != "" && g.gunActive)
			transform.parent.BroadcastMessage("PlayAnim", applyAnim);
	}

	public void Remove(GunScript g)
	{
		if (removeAnim != "" && g.gunActive)
			transform.parent.BroadcastMessage("PlayAnim", applyAnim);
	}
}