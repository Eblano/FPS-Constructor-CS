using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SmartCrosshair : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÃ¯Â¿Â½ Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float length1;
    public float width1;
    public bool scale;
    private Texture textu;
    private GUIStyle lineStyle;
    public bool debug;
    public static bool displayWhenAiming;
    public bool useTexture;
    public static bool ownTexture;
    public GameObject crosshairObj;
    public static GameObject cObj;
    public static bool scl;
    public static float cSize;
    public static float sclRef;
    public static bool draw;
    public float crosshairSize;
    public float minimumSize;
    public float maximumSize;
    public Texture2D crosshairTexture;
    public Texture2D friendTexture;
    public Texture2D foeTexture;
    public Texture2D otherTexture;
    public bool colorFoldout;
    public float colorDist;
    private bool hitEffectOn;
    public Texture2D hitEffectTexture;
    private float hitEffectTime;
    public float hitLength;
    public float hitWidth;
    public Vector2 hitEffectOffset;
    public AudioClip hitSound;
    public bool hitEffectFoldout;
    public bool displayHealth;
    public EnemyDamageReceiver enemyDamageReceiver;
    public int crosshairRange;
    private Shader currentShader;// = ((Renderer)GetComponent.<Renderer>()).sh;
    public float outlineSize;//0.01f;
    public Color outlineColor;
    public Color meshColor;
    //public var outlineColor:Color = Color(1f,1f,0f,1f);
    public static bool crosshair;
    public LayerMask RaycastsIgnore; //Layers that gun raycasts hit
    public virtual void Awake()
    {
        DefaultCrosshair();
        SmartCrosshair.sclRef = 1;
        SmartCrosshair.crosshair = true;
        lineStyle = new GUIStyle();
        lineStyle.normal.background = crosshairTexture;
    }

    //Right now this script fires a raycast every frame
    //This might impact performance, and is an area to consider when optimizing
    public virtual void Update()
    {
        float temp = 0.0f;
        float temp2 = 0.0f;
        RaycastHit hit = default(RaycastHit);
        if (!PlayerWeapons.playerActive)
        {
            if (SmartCrosshair.cObj)
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
            }
            return;
        }
        else
        {
            if (SmartCrosshair.cObj)
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = true;
            }
        }
        if (SmartCrosshair.cObj != null)
        {
            if (SmartCrosshair.crosshair && SmartCrosshair.ownTexture)
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
            }
        }
        if (!SmartCrosshair.scl)
        {
            temp = 1;
            temp2 = 1 / Screen.width;
        }
        else
        {
            temp = GunScript.crosshairSpread;
            temp = temp / 180;
            temp = temp * GunScript.weaponCam.GetComponent<Camera>().fieldOfView;
            temp = temp / Screen.height;
            temp = temp / SmartCrosshair.sclRef;
            temp2 = SmartCrosshair.cSize * temp;
        }
        if (SmartCrosshair.cObj != null)
        {
            if (SmartCrosshair.scl)
            {
                SmartCrosshair.cObj.transform.localScale = new Vector3(Mathf.Clamp(temp2, minimumSize, maximumSize), 1, Mathf.Clamp(temp2, minimumSize, maximumSize));
            }
            else
            {
                SmartCrosshair.cObj.transform.localScale = new Vector3(SmartCrosshair.cSize, 1, SmartCrosshair.cSize);
            }
        }
        int layerMask = 1 << PlayerWeapons.playerLayer;
        //layerMask |= (1 << 2);
        layerMask = layerMask | Physics.IgnoreRaycastLayer;
        layerMask = ~layerMask;
        Vector3 direction = transform.TransformDirection(new Vector3(0, 0, 1));
        if (Physics.Raycast(transform.position, direction, out hit, crosshairRange, layerMask))
        {
            if ((hit.collider && (((CrosshairColor)hit.transform.gameObject.GetComponent(typeof(CrosshairColor))) != null)) && ((hit.distance <= colorDist) || (colorDist < 0)))
            {
                CrosshairColor colorScript = (CrosshairColor)hit.transform.gameObject.GetComponent(typeof(CrosshairColor));
                if (colorScript.crosshairType == crosshairTypes.Friend)
                {
                    ChangeColor("Friend");
                    //display the current health of "Friend"
                    enemyDamageReceiver = (EnemyDamageReceiver)hit.transform.gameObject.GetComponent(typeof(EnemyDamageReceiver));
                    displayHealth = true;
                }
                else
                {
                    //HighlightGameObject(hit.transform.gameObject);
                    if (colorScript.crosshairType == crosshairTypes.Foe)
                    {
                        ChangeColor("Foe");
                        displayHealth = false;
                    }
                    else
                    {
                        if (colorScript.crosshairType == crosshairTypes.Other)
                        {
                            ChangeColor("Other");
                            displayHealth = false;
                        }
                    }
                }
            }
            else
            {
                ChangeColor(""); //Any string not recognized by ChangeColor is the default color
                displayHealth = false;
            }
        }
        else
        {
            ChangeColor("");
            displayHealth = false;
        }
        if (hitEffectTime <= 0)
        {
            hitEffectOn = false;
        }
    }

    public virtual void OnGUI()
    {
        if (!PlayerWeapons.playerActive)
        {
            return;
        }
        GUI.color = Color.white;
        if (!SmartCrosshair.ownTexture)
        {
            float distance1 = GunScript.crosshairSpread;
            if (!(distance1 > (Screen.height / 2)) && ((SmartCrosshair.crosshair || debug) || SmartCrosshair.displayWhenAiming))
            {
                GUI.Box(new Rect(((Screen.width - distance1) / 2) - length1, (Screen.height - width1) / 2, length1, width1), textu, lineStyle);
                GUI.Box(new Rect((Screen.width + distance1) / 2, (Screen.height - width1) / 2, length1, width1), textu, lineStyle);
                GUI.Box(new Rect((Screen.width - width1) / 2, ((Screen.height - distance1) / 2) - length1, width1, length1), textu, lineStyle);
                GUI.Box(new Rect((Screen.width - width1) / 2, (Screen.height + distance1) / 2, width1, length1), textu, lineStyle);
                if (displayHealth && (enemyDamageReceiver != null))
                {
                    GUI.Box(new Rect((Screen.width - 100) / 2, ((Screen.height + width1) / 2) - 75, 100, 50), (enemyDamageReceiver.transform.name + " Health: ") + Mathf.Round(enemyDamageReceiver.hitPoints));
                }
            }
        }
        if (hitEffectOn)
        {
            hitEffectTime = hitEffectTime - (Time.deltaTime * 0.5f);
            GUI.color = new Color(1, 1, 1, hitEffectTime);
            GUI.DrawTexture(new Rect(((Screen.width - hitEffectOffset.x) / 2) - (hitLength / 2), ((Screen.height - hitEffectOffset.y) / 2) - (hitWidth / 2), hitLength, hitWidth), hitEffectTexture);
        }
    }

    public virtual void HighlightGameObject(GameObject objectToHighlight)//Invoke("HighlightGameObjectRemove",3);
    {
        // Set the transparent material to this object
        MeshRenderer meshRenderer = ((MeshRenderer)GameObject.FindObjectOfType(typeof(MeshRenderer))) as MeshRenderer;
        Material[] materials = meshRenderer.materials;
        int materialsNum = materials.Length;
        int i = 0;
        while (i < materialsNum)
        {
            materials[i].shader = Shader.Find("Outline/Transparent");
            materials[i].SetColor("_color", meshColor);
            i++;
        }
        // Create copy of this object, this will have the shader that makes the real outline
        GameObject outlineObj = new GameObject();
        outlineObj.transform.position = objectToHighlight.transform.position;
        outlineObj.transform.rotation = objectToHighlight.transform.rotation;
        outlineObj.AddComponent(typeof(MeshFilter));
        outlineObj.AddComponent(typeof(MeshRenderer));
        Mesh mesh = null;
        mesh = UnityEngine.Object.Instantiate((((MeshFilter)GameObject.FindObjectOfType(typeof(MeshFilter))) as MeshFilter).mesh);
        ((MeshFilter)outlineObj.GetComponent(typeof(MeshFilter))).mesh = mesh;
        outlineObj.transform.parent = objectToHighlight.transform;
        //materials = Material[materialsNum];//create empty materials array
        i = 0;
        while (i < materialsNum)
        {
            materials[i] = new Material(Shader.Find("Self-Illumin/Outlined Diffuse"));
            materials[i].SetColor("_OutlineColor", outlineColor);
            i++;
        }
        ((MeshRenderer)outlineObj.GetComponent(typeof(MeshRenderer))).materials = materials;
    }

    public virtual void HighlightGameObjectRemove()
    {
        GetComponent<Renderer>().material.shader = currentShader;
    }

    public virtual void ChangeColor(string targetStatus)
    {
        if (targetStatus == "Friend")
        {
            lineStyle.normal.background = friendTexture;
        }
        else
        {
            if (targetStatus == "Foe")
            {
                lineStyle.normal.background = foeTexture;
            }
            else
            {
                if (targetStatus == "Other")
                {
                    lineStyle.normal.background = otherTexture;
                }
                else
                {
                    lineStyle.normal.background = crosshairTexture;
                }
            }
        }
    }

    public virtual void Aiming()
    {
        SmartCrosshair.crosshair = false;
    }

    public virtual void NormalSpeed()
    {
        SmartCrosshair.crosshair = true;
    }

    public virtual void Sprinting()
    {
        SmartCrosshair.crosshair = false;
    }

    public virtual void SetCrosshair()
    {
        if (SmartCrosshair.cObj != null)
        {
            SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
        }
    }

    public virtual void DefaultCrosshair()
    {
        if (SmartCrosshair.cObj != null)
        {
            SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
        }
        SmartCrosshair.ownTexture = useTexture;
        if (crosshairObj != null)
        {
            SmartCrosshair.cObj = crosshairObj;
        }
        if (scale)
        {
            SmartCrosshair.cSize = maximumSize;
        }
        else
        {
            SmartCrosshair.cSize = crosshairSize;
        }
        SmartCrosshair.scl = scale;
    }

    public virtual void HitEffect()
    {
        hitEffectOn = true;
        hitEffectTime = 1;
        if (GetComponent<AudioSource>() && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = hitSound;
            GetComponent<AudioSource>().Play();
        }
    }

    public SmartCrosshair()
    {
        colorDist = 40;
        hitEffectOffset = new Vector2(0, 0);
        crosshairRange = 200;
        outlineSize = 2;
        outlineColor = Color.red;
        meshColor = new Color(1f, 1f, 1f, 0.5f);
    }

    static SmartCrosshair()
    {
        SmartCrosshair.displayWhenAiming = true;
        SmartCrosshair.crosshair = true;
    }
}