using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyShootProjectile : MonoBehaviour
{
    private float nextAttackTime;
    public Transform pos;
    public Rigidbody projectile;
    public float initialSpeed;
    public float fireRate;
    public float actualSpread;
    public ParticleSystem emitter;
    public float backForce;
    public virtual void Attack()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }
        nextAttackTime = Time.time + fireRate;
        //function Fire (penetration : int, damage : float, force : float, tracer : GameObject, direction : Vector3, firePosition : Vector3) {
        FireProjectile();
    }

    public virtual void FireProjectile()
    {
        Vector3 direction = SprayDirection();
        Quaternion convert = Quaternion.LookRotation(direction + new Vector3(0, 0.04f, 0));
        Rigidbody instantiatedProjectile = null;
        instantiatedProjectile = UnityEngine.Object.Instantiate(projectile, pos.position, convert);
        instantiatedProjectile.velocity = instantiatedProjectile.transform.TransformDirection(new Vector3(0, 0, initialSpeed));
        Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
        emitter.Play();//.Emit();
        transform.root.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, -backForce), ForceMode.Impulse);
    }

    public virtual Vector3 SprayDirection()
    {
        float vx = (1 - (2 * Random.value)) * actualSpread;
        float vy = (1 - (2 * Random.value)) * actualSpread;
        float vz = 1f;
        return transform.TransformDirection(new Vector3(vx, vy, vz));
    }

    public EnemyShootProjectile()
    {
        initialSpeed = 50;
        fireRate = 1;
        actualSpread = 0.2f;
        backForce = 10;
    }
}