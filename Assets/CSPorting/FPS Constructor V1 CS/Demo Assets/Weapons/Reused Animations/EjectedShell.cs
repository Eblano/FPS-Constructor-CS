using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EjectedShell : MonoBehaviour
{
    public Vector3 force;
    public float randomFactorForce;
    //var gravity : float;
    public Vector3 torque;
    public float randomFactorTorque;
    public virtual void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(force * Random.Range(1, randomFactorForce));
        GetComponent<Rigidbody>().AddRelativeTorque(torque * Random.Range(-randomFactorTorque, randomFactorTorque));
    }
}