using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Key : MonoBehaviour
{
    public string key;
    public InputItem input;
    public virtual void UpdateInput()
    {
        //Just get the values from Unity's input
        input.got = Input.GetButton(key);
        input.down = Input.GetButtonDown(key);
        input.up = Input.GetButtonUp(key);
    }
}