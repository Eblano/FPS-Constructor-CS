using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayerWeapons : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
    public GameObject[] weapons;
    public int selectedWeapon; //index of currently selected weapon in array
    public bool reloadWhileSprinting;
    public float displayTime; //How long slot-related message is shown for
    public float sensitivity; //Sensitivity of mouse
    public bool inverted; //Is the mouse y-axis inverted
    public float interactDistance; //How far can an object be to be interacted with
    private Transform lastHit; //the last object we hit
    public LayerMask interactMask; //Mask for raycast
    //Settings
    public static bool autoReload; //Do guns automatically reload when emptied?
    public static float fieldOfView; //Base field of view for cameras
    public static int playerLayer;
    public static int ignorePlayerLayer;
    public static bool canSwapSameWeapon; //Can we pick up multiples of the same weapon? (Replacing it)
    public static bool saveUpgradesToDrops;
    public LayerMask RaycastsIgnore; //Layers that gun raycasts hit
    //Control Variables
    public static bool canMove;
    public static bool canSprint;
    public static bool canLook; //Can the player look around?
    public static bool canFire;
    public static bool canAim;
    public static bool canCrouch;
    public static bool doesIdle;
    public static bool canInteract;
    public static bool canSwitchWeapon;
    //Status
    public static bool hidden;
    public static bool sprinting;
    //Don't change
    public static GameObject player;
    public static CharacterController controller;
    public static CharacterMotorDB CM;
    public static GameObject weaponCam;
    public static GameObject mainCam;
    public static bool autoFire;
    public static bool charge;
    private bool canKickback;
    private float emptyMessageTime;
    private string emptyMessage;
    private bool displayMessage;
    public static bool playerActive;
    public static PlayerWeapons PW; //Singleton object
    public virtual void Start()
    {
        // Select the first weapon
        //playerActive = true;
        ActivateWeapon();
    }

    public virtual void Awake()
    {
        if (PlayerWeapons.PW)
        {
            Debug.LogError("Too many instances of PlayerWeapons! There should only be one per scene");
        }
        PlayerWeapons.PW = null;
        PlayerWeapons.PW = this;
        PlayerWeapons.weaponCam = GameObject.FindWithTag("WeaponCamera");
        PlayerWeapons.mainCam = GameObject.FindWithTag("MainCamera");
        PlayerWeapons.player = GameObject.FindWithTag("Player");
        PlayerWeapons.CM = (CharacterMotorDB)PlayerWeapons.player.GetComponent(typeof(CharacterMotorDB));
        PlayerWeapons.controller = (CharacterController)PlayerWeapons.player.GetComponent(typeof(CharacterController));
        PlayerWeapons.hidden = false;
        SetSensitivity();
    }

    public virtual void SetSensitivity()//sensitivity = Mathf.Abs(sensitivity);
    {
        //transform.parent.GetComponent(MouseLookDBJS).sensitivityStandardX = sensitivity;
        if (inverted)
        {
            sensitivity = sensitivity * -1;
        }
    }

    public virtual void LateUpdate()
    {
        if (!PlayerWeapons.playerActive)
        {
            return;
        }
        if (InputDB.GetButtonDown("Fire1") && canKickback)
        {
            canKickback = false;
        }
        else
        {
            if (InputDB.GetButtonUp("Fire1"))
            {
                canKickback = true;
                gameObject.BroadcastMessage("ReleaseFire", 1, SendMessageOptions.DontRequireReceiver);
            }
        }
        if (weapons[selectedWeapon] != null)
        {
            if (/*!InputDB.GetButton ("Fire1") || */Time.time > ((GunScript)weapons[selectedWeapon].GetComponent(typeof(GunScript))).nextFireTime)
            {
                BroadcastMessage("Cooldown");
            }
        }
        if (InputDB.GetButtonUp("Aim"))
        {
            gameObject.SendMessageUpwards("ReleaseFire", 2, SendMessageOptions.DontRequireReceiver);
        }
    }

    public virtual void Update()
    {
        Ray ray = default(Ray);
        RaycastHit hit = default(RaycastHit);
        Touch touch = default(Touch);
        if (!PlayerWeapons.playerActive)
        {
            return;
        }
        /************************Interact****************************/
        if (interactDistance > 0)
        {
            //Set up ray
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            if (Physics.Raycast(ray, out hit, interactDistance, interactMask))
            {
                //We hit something new
                if (lastHit != hit.transform)
                {
                    //Last object isn't still highlighted
                    if (lastHit)
                    {
                        lastHit.SendMessage("HighlightOff", SendMessageOptions.DontRequireReceiver);
                    }
                    //New one is
                    lastHit = hit.transform;
                    lastHit.SendMessage("HighlightOn", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                if (lastHit != null)
                {
                    //We hit nothing, but still have a object highlighted, so unhighlight it
                    lastHit.SendMessage("HighlightOff", SendMessageOptions.DontRequireReceiver);
                    lastHit = null;
                }
            }
            //Interact
            if (InputDB.GetButtonDown("Interact") && (lastHit != null))
            {
                lastHit.SendMessage("Interact", gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
        /**********************Fire & Reload******************************/
        // Did the user press fire?
        if ((InputDB.GetButton("Fire1") && (PlayerWeapons.autoFire || PlayerWeapons.charge)) && PlayerWeapons.canFire)
        {
            transform.root.BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            if (InputDB.GetButtonDown("Fire1") && PlayerWeapons.canFire)
            {
                transform.root.BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
            }
        }
        if ((InputDB.GetButton("Fire2") && PlayerWeapons.canFire) && (PlayerWeapons.autoFire || PlayerWeapons.charge))
        {
            BroadcastMessage("Fire2", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            if (InputDB.GetButtonDown("Fire2") && PlayerWeapons.canFire)
            {
                BroadcastMessage("Fire2", SendMessageOptions.DontRequireReceiver);
            }
        }
        if (InputDB.GetButtonDown("Reload"))
        {
            BroadcastMessage("Reload", SendMessageOptions.DontRequireReceiver);
        }
        /*************************Weapon Switching***************************/
        if (((!AimMode.canSwitchWeaponAim || PlayerWeapons.hidden) || !PlayerWeapons.canSwitchWeapon) || Avoidance.collided)
        {
            return;
        }
        if (InputDB.GetButtonDown("SelectWeapon"))
        {
            int temp = WeaponSelector.selectedWeapon;
            if (((weapons.Length > temp) && ((selectedWeapon != temp) || (weapons[selectedWeapon] == null))) && (temp >= 0))
            {
                if (weapons[temp] != null)
                {
                    if (!((WeaponInfo)weapons[temp].gameObject.GetComponent(typeof(WeaponInfo))).locked)
                    {
                        SelectWeapon(temp);
                        selectedWeapon = temp;
                        displayMessage = false;
                    }
                    else
                    {
                        SlotEmptyMessage(temp + 1);
                    }
                }
                else
                {
                    SlotEmptyMessage(temp + 1);
                }
            }
        }
    }

    public virtual void SelectWeapon(int index)
    {
        bool allNull = true;
        int i = 0;
        while (i < weapons.Length)
        {
            if ((i != index) && (weapons[i] != null))
            {
                weapons[i].gameObject.BroadcastMessage("DeselectWeapon");
                allNull = false;
            }
            i++;
        }
        if (allNull)
        {
            ActivateWeapon();
        }
    }

    public virtual void ActivateWeapon()
    {
        if (PlayerWeapons.hidden)
        {
            return;
        }
        if (weapons[selectedWeapon] != null)
        {
            weapons[selectedWeapon].BroadcastMessage("SelectWeapon");
        }
    }

    public virtual void FullAuto()
    {
        PlayerWeapons.autoFire = true;
    }

    public virtual void SemiAuto()
    {
        PlayerWeapons.autoFire = false;
    }

    public virtual void Charge()
    {
        PlayerWeapons.charge = true;
    }

    public virtual void NoCharge()
    {
        PlayerWeapons.charge = false;
    }

    public virtual void DeactivateWeapons()
    {
        int i = 0;
        while (i < weapons.Length)
        {
            if (weapons[i] != null)
            {
                weapons[i].gameObject.BroadcastMessage("DeselectWeapon");
            }
            i++;
        }
    }

    public virtual void SetWeapon(GameObject gun, int element)
    {
        weapons[element] = gun;
    }

    public virtual void SlotEmptyMessage(int s)
    {
        //display message
        displayMessage = true;
        emptyMessageTime = Time.time + displayTime;
        emptyMessage = "No Weapon Equipped in Slot " + s;
    }

    public virtual void OnGUI()
    {
        if ((Time.time < emptyMessageTime) && displayMessage)
        {
            GUI.BeginGroup(new Rect((Screen.width / 2) - 120, Screen.height - 60, 240, 60), "");
            GUI.Box(new Rect(0, 0, 240, 60), "");
            GUI.Label(new Rect(20, 20, 200, 20), emptyMessage);
            GUI.EndGroup();
        }
        else
        {
            displayMessage = false;
        }
    }

    //Hides Player's weapon, with put away animation
    public static void HideWeapon()
    {
        PlayerWeapons.hidden = true;
        if (PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon] != null)
        {
            PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].gameObject.BroadcastMessage("DeselectWeapon");
        }
        SmartCrosshair.crosshair = true;
    }

    //Hides Player's weapon instantly
    public static void HideWeaponInstant()
    {
        PlayerWeapons.hidden = true;
        if (PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon] != null)
        {
            PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].gameObject.BroadcastMessage("DeselectInstant");
        }
        SmartCrosshair.crosshair = true;
    }

    //Unhides Player's weapon
    public static void ShowWeapon()
    {
        PlayerWeapons.hidden = false;
        if (PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon] != null)
        {
            PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].gameObject.BroadcastMessage("SelectWeapon");
        }
    }

    public static int HasEquipped()
    {
        int num = 0;
        int i = 0;
        while (i < PlayerWeapons.PW.weapons.Length)
        {
            if (PlayerWeapons.PW.weapons[i] != null)
            {
                num++;
            }
            i++;
        }
        return num;
    }

    public PlayerWeapons()
    {
        displayTime = 1;
        sensitivity = 13;
        interactDistance = 5;
        canKickback = true;
    }

    static PlayerWeapons()
    {
        PlayerWeapons.autoReload = true;
        PlayerWeapons.fieldOfView = 60;
        PlayerWeapons.playerLayer = 8;
        PlayerWeapons.ignorePlayerLayer = 8;
        PlayerWeapons.canSwapSameWeapon = true;
        PlayerWeapons.saveUpgradesToDrops = true;
        PlayerWeapons.canMove = true;
        PlayerWeapons.canSprint = true;
        PlayerWeapons.canLook = true;
        PlayerWeapons.canFire = true;
        PlayerWeapons.canAim = true;
        PlayerWeapons.canCrouch = true;
        PlayerWeapons.doesIdle = true;
        PlayerWeapons.canInteract = true;
        PlayerWeapons.canSwitchWeapon = true;
        PlayerWeapons.playerActive = true;
    }
}