using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Axis : MonoBehaviour
{
    public string key;
    public InputItem input;
    public virtual void UpdateInput()
    {
        //Just get the axis value from Unity's input
        input.axis = Input.GetAxisRaw(key);
    }
}