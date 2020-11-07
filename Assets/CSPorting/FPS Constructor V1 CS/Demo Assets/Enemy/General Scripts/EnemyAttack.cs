using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyAttack : MonoBehaviour
{
    private Transform target;
    private float nextAttackTime;
    public float damage;
    public float attackTime;
    public virtual void Start()//    target = PlayerWeapons.weaponCam.transform;
    {
    }

    public virtual void Attack()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }
        object[] sendArray = new object[2];
        sendArray[0] = damage;
        sendArray[1] = false;
        target.SendMessageUpwards("ApplyDamage", sendArray, SendMessageOptions.DontRequireReceiver);
        target.SendMessageUpwards("Direction", transform, SendMessageOptions.DontRequireReceiver);
        nextAttackTime = Time.time + attackTime;
        GetComponent<Animation>().Play();
    }
}