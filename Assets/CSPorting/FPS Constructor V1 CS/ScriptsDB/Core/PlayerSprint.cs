using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayerSprint : MonoBehaviour
{
    public static bool sprinting;
    private bool exhausted;
    private float sprintEndTime;
    private CharacterMotorDB CM;
    [UnityEngine.HideInInspector]
    public bool weaponsInactive;
    [UnityEngine.HideInInspector]
    public MovementValues values;
    public virtual void Start()
    {
        CM = PlayerWeapons.CM;
        values = MovementValues.singleton;
    }

    public virtual void Update()
    {
        float tempSprintTime = 0.0f;
        weaponsInactive = PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon] == null;
        if (!weaponsInactive)
        {
            weaponsInactive = ((GunScript)PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].GetComponent(typeof(GunScript))).gunActive == false;
        }
        if (!weaponsInactive)
        {
            return;
        }
        //Replenish Sprint time
        if (PlayerWeapons.controller.velocity.magnitude == 0)
        {
            tempSprintTime = sprintEndTime;
        }
        if (((AimMode.sprintNum < values.sprintDuration) && !AimMode.sprintingPublic) && (Time.time > tempSprintTime))
        {
            if (PlayerWeapons.controller.velocity.magnitude == 0)
            {
                AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum + (values.sprintAddStand * Time.deltaTime), 0, values.sprintDuration);
            }
            else
            {
                AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum + (values.sprintAddWalk * Time.deltaTime), 0, values.sprintDuration);
            }
        }
        if (AimMode.sprintNum > values.sprintMin)
        {
            exhausted = false;
        }
        //Handle sprint
        if (((((InputDB.GetButton("Sprint") && !InputDB.GetButton("Aim")) && PlayerWeapons.canSprint) && CM.grounded) && !exhausted) && ((PlayerWeapons.controller.velocity.magnitude/*CM.prone || */ > CM.movement.minSprintSpeed) || CharacterMotorDB.crouching))
        {
            AimMode.sprintNum = Mathf.Clamp(AimMode.sprintNum - Time.deltaTime, 0, values.sprintDuration);
            if (!AimMode.sprintingPublic)
            {
                AimMode.sprintingPublic = true;
                BroadcastMessage("Sprinting", SendMessageOptions.DontRequireReceiver);
                AimMode.canSwitchWeaponAim = false;
            }
            //Check if we're out of sprint
            if (AimMode.sprintNum <= 0)
            {
                exhausted = true;
                sprintEndTime = Time.time + values.recoverDelay;
            }
        }
        else
        {
            if (AimMode.sprintingPublic)
            {
                AimMode.sprintingPublic = false;
                BroadcastMessage("StopSprinting", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage("NormalSpeed");
                AimMode.canSwitchWeaponAim = true;
            }
        }
    }
}