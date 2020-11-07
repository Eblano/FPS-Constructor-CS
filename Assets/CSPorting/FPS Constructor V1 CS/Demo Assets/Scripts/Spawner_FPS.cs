using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Spawner_FPS : MonoBehaviour
{
    public int curWave;
    public Waypoint[] waypoints;
    public Wave[] waves;
    public Transform[] spawners;
    public float spawnDelay;
    public float spawnTime;
    private bool spawning;
    private float nextSpawnTimme;
    public virtual IEnumerator Spawn()
    {
        Wave w = null;
        CubeSet cs = null;
        while (curWave < waves.Length)
        {
            w = waves[curWave];
            int i = 0;
            while (i < w.cubeSets.Length)
            {
                cs = w.cubeSets[i];
                StartCoroutine(cs.SpawnCS(spawners[i], waypoints[i], spawnTime));
                i++;
            }
            while (EnemyMovement.enemies > 0)
            {
                yield return typeof(WaitForFixedUpdate);
            }
            curWave++;
            yield return new WaitForSeconds(spawnDelay + (1 * curWave));
            if (curWave >= waves.Length)
            {
                curWave = 0;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Player")
        if (!spawning)
        {
            spawning = true;
            StartCoroutine(Spawn());
        }
    }

    public Spawner_FPS()
    {
        spawnDelay = 3;
        spawnTime = 0.2f;
    }
}