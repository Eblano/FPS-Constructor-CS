using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MovementValues : MonoBehaviour
{
    //This script stores values to be taken by CharacterMotorDB, to allow a simpler customEditor
    public float defaultForwardSpeed;
    public float maxCrouchSpeed;
    public float maxSprintSpeed;
    public float minSprintSpeed;
    public float maxAimSpeed;
    public float maxProneSpeed;
    public float defaultSidewaysSpeed;
    public float sprintSidewaysSpeed;
    public float crouchSidewaysSpeed;
    public float aimSidewaysSpeed;
    public float proneSidewaysSpeed;
    public float defaultBackwardsSpeed;
    public float crouchBackwardsSpeed;
    public float aimBackwardsSpeed;
    public float proneBackwardsSpeed;
    public bool sprintFoldout;
    public bool crouchFoldout;
    public bool defaultFoldout;
    public bool proneFoldout;
    public bool aimFoldout;
    public bool jumpFoldout;
    public CharacterMotorDB CM;
    public int sprintDuration; //how long can we sprint for?
    public float sprintAddStand; //how quickly does sprint replenish when idle?
    public float sprintAddWalk; //how quickly does sprint replenish when moving?
    public float sprintMin; //What is the minimum value ofsprint at which we can begin sprinting?
    public float recoverDelay; //how much time after sprinting does it take to start recovering sprint?
    public float exhaustedDelay; //how much time after sprinting to exhaustion does it take to start recovering sprint?
    public static MovementValues singleton;
    public virtual void Awake()
    {
        MovementValues.singleton = this;
    }

    public MovementValues()
    {
        defaultForwardSpeed = 10;
        maxCrouchSpeed = 6;
        maxSprintSpeed = 13;
        minSprintSpeed = 10;
        maxAimSpeed = 4;
        maxProneSpeed = 4;
        defaultSidewaysSpeed = 10;
        sprintSidewaysSpeed = 15;
        crouchSidewaysSpeed = 6;
        aimSidewaysSpeed = 4;
        proneSidewaysSpeed = 2;
        defaultBackwardsSpeed = 10;
        crouchBackwardsSpeed = 6;
        aimBackwardsSpeed = 4;
        proneBackwardsSpeed = 2;
        sprintDuration = 5;
        sprintAddStand = 1;
        sprintAddWalk = 0.3f;
        sprintMin = 1;
        recoverDelay = 0.7f;
        exhaustedDelay = 1;
    }
}