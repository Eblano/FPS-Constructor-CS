using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class CubeSet : MonoBehaviour
{
    public Transform[] cubes;
    public int[] amts;
    public virtual IEnumerator SpawnCS(Transform pos, Waypoint w, float t)
    {
        Transform spawned = null;
        int j = 0;
        while (j < cubes.Length)
        {
            int q = 0;
            while (q < amts[j])
            {
                spawned = UnityEngine.Object.Instantiate(cubes[j], pos.position + (new Vector3(0, 4, 0) * q), pos.rotation);
                EnemyMovement.enemies++;
                ((EnemyMovement)spawned.GetComponent(typeof(EnemyMovement))).waypoint = w.transform;
                yield return new WaitForSeconds(t);
                q++;
            }
            j++;
        }
    }
}