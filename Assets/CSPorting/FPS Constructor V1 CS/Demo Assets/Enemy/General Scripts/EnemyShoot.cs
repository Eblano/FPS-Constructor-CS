using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyShoot : MonoBehaviour
{
    private Transform target;
    private float nextAttackTime;
    public float damage;
    public float force;
    public float fireRate;
    public Fire fire;
    public GameObject tracer;
    public Transform shootPos;
    public float actualSpread;
    public virtual void Start()
    {
        target = PlayerWeapons.weaponCam.transform;
    }

    public virtual void Attack()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }
        nextAttackTime = Time.time + fireRate;
        //function Fire (penetration : int, damage : float, force : float, tracer : GameObject, direction : Vector3, firePosition : Vector3) {
        fire.Fire_(0, damage, force, tracer, SprayDirection(), shootPos.position);
    }

    public virtual Vector3 SprayDirection()
    {
        float vx = (1 - (2 * Random.value)) * actualSpread;
        float vy = (1 - (2 * Random.value)) * actualSpread;
        float vz = 1f;
        return transform.TransformDirection(new Vector3(vx, vy, vz));
    }
}