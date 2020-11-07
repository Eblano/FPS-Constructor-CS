using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation
/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule
/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
//[AddComponentMenu("Camera-Control/Mouse Look")]

[System.Serializable]
public class MouseLookDBJS : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseX = 0,
        MouseY = 1
    }

    public RotationAxes axes = RotationAxes.MouseX;

    [HideInInspector] public float sensitivityX = 15F;

    [HideInInspector] public float sensitivityY = 15F;

    [HideInInspector] public float sensitivityStandardX = 15F;

    [HideInInspector] public float sensitivityStandardY = 15F;

    [HideInInspector] public float offsetY = 0;

    [HideInInspector] public float offsetX = 0;

    [HideInInspector] public float totalOffsetY = 0;

    [HideInInspector] public float totalOffsetX = 0;

    [HideInInspector] public float resetSpeed = 1;

    [HideInInspector] public float resetDelay = 0;

    [HideInInspector] public float maxKickback = 0;

    [HideInInspector] public float xDecrease = 0;

    public float minimumX = -360F;

    public float maximumX = 360F;

    public float minimumY = -60F;

    public float maximumY = 60F;

    public bool smooth = true;
    public float smoothFactor = 2;
    //얘는 뭐지...
    public ArrayList smoothIterations = new ArrayList();
    public int iterations = 10;

    private Quaternion tRotation;

    public float idleSway;

    private int minStored;
    private int maxStored;

    //added by dw to pause camera when in store

    [HideInInspector] public static bool freeze = false;

    [HideInInspector] public bool individualFreeze = false;


    [HideInInspector] public float rotationX = 0F;
    [HideInInspector] public float rotationY = 0F;


    [HideInInspector] public Quaternion originalRotation;

    private Quaternion[] temp;
    private Quaternion smoothRotation;

    public void Freeze()
    {
        freeze = true;
    }

    public void UnFreeze()
    {
        freeze = false;
    }

    public void SetRotation(Vector3 target)
    {
        rotationX = target.y;
        //rotationY = target.x;
    }

    void Update()
    {
        if (freeze || !PlayerWeapons.canLook || individualFreeze) return;

        Quaternion xQuaternion;
        Quaternion yQuaternion;
        float offsetVal;

        if (axes == RotationAxes.MouseX)
        {
            rotationX += InputDB.GetAxis("Mouse X") * sensitivityX;

            float xDecrease;

            if (totalOffsetX > 0)
            {
                xDecrease = Mathf.Clamp(resetSpeed * Time.deltaTime, 0, totalOffsetX);
            }
            else
            {
                xDecrease = Mathf.Clamp(resetSpeed * -Time.deltaTime, totalOffsetX, 0);
            }

            if (resetDelay > 0)
            {
                xDecrease = 0;

                resetDelay = Mathf.Clamp(resetDelay - Time.deltaTime, 0, resetDelay);
            }

            if (Random.value < .5)
                offsetX *= -1;

            if ((totalOffsetX < maxKickback && totalOffsetX >= 0) || (totalOffsetX > -maxKickback && totalOffsetX <= 0))
            {
                totalOffsetX += offsetX;
            }
            else
            {
                //offsetX = 0;
                resetDelay *= .5f;
            }

            rotationX = ClampAngle(rotationX, minimumX, maximumX) + offsetX - xDecrease;

            if ((Input.GetAxis("Mouse X") * sensitivityX) < 0)
            {
                totalOffsetX += Input.GetAxis("Mouse X") * sensitivityX;
            }

            rotationX += Mathf.Sin(Time.time) * idleSway;

            totalOffsetX -= xDecrease;

            if (totalOffsetX < 0)

                totalOffsetX = 0;

            xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);

            tRotation = originalRotation * xQuaternion;

            offsetVal = Mathf.Clamp(totalOffsetX * smoothFactor, 1, smoothFactor);

            if (smooth)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, tRotation, Time.deltaTime * 25 / smoothFactor * offsetVal);
            }
            else
            {
                transform.localRotation = tRotation;
            }
        }

        else
        {
            rotationY += InputDB.GetAxis("Mouse Y") * sensitivityY;

            float yDecrease = Mathf.Clamp(resetSpeed * Time.deltaTime, 0, totalOffsetY);

            if (resetDelay > 0)
            {
                yDecrease = 0;

                resetDelay = Mathf.Clamp(resetDelay - Time.deltaTime, 0, resetDelay);
            }

            if (totalOffsetY < maxKickback)
            {
                totalOffsetY += offsetY;
            }
            else
            {
                offsetY = 0;

                resetDelay *= .5f;
            }

            rotationY = ClampAngle(rotationY, minimumY, maximumY) + offsetY - yDecrease;

            if ((Input.GetAxis("Mouse Y") * sensitivityY) < 0)
            {
                totalOffsetY += Input.GetAxis("Mouse Y") * sensitivityY;
            }

            rotationY += Mathf.Sin(Time.time) * idleSway;

            totalOffsetY -= yDecrease;

            if (totalOffsetY < 0)

                totalOffsetY = 0;

            yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            tRotation = originalRotation * yQuaternion;

            offsetVal = Mathf.Clamp(totalOffsetY * smoothFactor, 1, smoothFactor);

            if (smooth)
            {
                transform.localEulerAngles = new Vector3(Quaternion.Slerp(transform.localRotation, tRotation, Time.deltaTime * 25 / smoothFactor * offsetVal).eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            else
            {
                transform.localEulerAngles = new Vector3(tRotation.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
                ;
            }
        }

        offsetY = 0;

        offsetX = 0;
    }


    void Start()
    {
        // Make the rigid body not change rotation

        if (GetComponent<Rigidbody>())

            GetComponent<Rigidbody>().freezeRotation = true;

        originalRotation = transform.localRotation;

        sensitivityX = sensitivityStandardX;

        sensitivityY = sensitivityStandardY;

        if (smoothFactor <= 1)
        {
            smoothFactor = 1;
        }
    }


    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)

            angle += 360F;

        if (angle > 360F)

            angle -= 360F;

        return Mathf.Clamp(angle, min, max);
    }


    public void Aiming(float zoom)
    {
        sensitivityX = sensitivityX / zoom;

        sensitivityY = sensitivityY / zoom;
    }

    public void StopAiming()
    {
        sensitivityX = sensitivityStandardX;

        sensitivityY = sensitivityStandardY;
    }

    public void LockIt(int min, int max)
    {
        if (axes == RotationAxes.MouseX)
        {
            maxStored = (int)maximumX;
            minStored = (int)minimumX;
            maximumX = rotationX + max;
            minimumX = rotationX - min;
        }
        else
        {
            maxStored = (int)maximumY;
            minStored = (int)minimumY;
            maximumY = rotationY + max;
            minimumY = rotationY - min;
        }
    }

    public void LockItSpecific(int min, int max)
    {
        if (axes == RotationAxes.MouseX)
        {
            maxStored = (int)maximumX;
            minStored = (int)minimumX;
            maximumX = max;
            minimumX = min;
        }
        else
        {
            maxStored = (int)maximumY;
            minStored = (int)minimumY;
            maximumY = max;
            minimumY = min;
        }
    }

    public void UnlockIt()
    {
        if (axes == RotationAxes.MouseX)
        {
            maximumX = maxStored;
            minimumX = minStored;
        }
        else
        {
            maximumY = maxStored;
            minimumY = minStored;
        }
    }

    public void UpdateIt()
    {
        rotationX = transform.localEulerAngles.y - originalRotation.eulerAngles.y;
        rotationY = transform.localEulerAngles.x - originalRotation.eulerAngles.x;
        totalOffsetX = 0;
    }
}