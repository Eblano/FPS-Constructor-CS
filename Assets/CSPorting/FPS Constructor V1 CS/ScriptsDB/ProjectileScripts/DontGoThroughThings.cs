using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class DontGoThroughThings : MonoBehaviour
{
    //From the awesome unifycommunity wiki - http://www.unifycommunity.com/wiki/index.php?title=DontGoThroughThings
    public LayerMask layerMask; //make sure we aren't in this layer 
    public float skinWidth; //probably doesn't need to be changed 
    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    //initialize values 
    public virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(GetComponent<Collider>().bounds.extents.x, GetComponent<Collider>().bounds.extents.y), GetComponent<Collider>().bounds.extents.z);
        partialExtent = minimumExtent * (1f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    public virtual void FixedUpdate()
    {
        RaycastHit hitInfo = default(RaycastHit);
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;
        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            //check for obstructions we might have missed 
            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
            {
                myRigidbody.position = hitInfo.point - ((movementThisStep / movementMagnitude) * partialExtent);
            }
        }
        previousPosition = myRigidbody.position;
    }

    public DontGoThroughThings()
    {
        layerMask = ~(1 << PlayerWeapons.playerLayer);
        skinWidth = 0.1f;
    }
}