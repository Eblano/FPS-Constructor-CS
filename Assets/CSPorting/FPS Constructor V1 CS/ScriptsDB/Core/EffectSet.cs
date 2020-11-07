using System;
using UnityEngine;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
[System.Serializable]
public class EffectSet : System.Object
{
	public const int maxOfEach = 15; //You can increase this if desired
	public int setID = 0;
	public string setName = "New Set";
	public int localMax = 20;

	public GameObject blankGameObject;

	public GameObject[] bulletDecals = new GameObject[maxOfEach];
	public bool bulletDecalsFolded = false;
	public int lastBulletDecal = 0;

	public GameObject[] dentDecals = new GameObject[maxOfEach];
	public bool dentDecalsFolded = false;
	public int lastDentDecal = 0;

	public GameObject[] hitParticles = new GameObject[maxOfEach];
	public bool hitParticlesFolded = false;
	public int lastHitParticle = 0;

	public AudioClip[] footstepSounds = new AudioClip[maxOfEach];
	public bool footstepSoundsFolded = false;
	public int lastFootstepSound = 0;

	public AudioClip[] crawlSounds = new AudioClip[maxOfEach];
	public bool crawlSoundsFolded = false;
	public int lastCrawlSound = 0;

	public AudioClip[] bulletSounds = new AudioClip[maxOfEach];
	public bool bulletSoundsFolded = false;
	public int lastBulletSound = 0;

	public AudioClip[] collisionSounds = new AudioClip[maxOfEach];
	public bool collisionSoundsFolded = false;
	public int lastCollisionSound = 0;
}