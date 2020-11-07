using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MouseButton : MonoBehaviour
{
    public int key;
    public InputItem input;
    public virtual void UpdateInput()
    {
        //Just get the values from Unity's input
        input.got = Input.GetMouseButton(key);
        input.down = Input.GetMouseButtonDown(key);
        input.up = Input.GetMouseButtonUp(key);
    }
}