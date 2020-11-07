using UnityEngine;
using System.Collections;

public enum dirs
{
    x = 0, //which rotation axis should we use?
    y = 1
}

[System.Serializable]
public partial class Drag : MonoBehaviour
{
    public dirs direction;
    public bool invert; //should it be inverted?
    public InputItem input;
    public float sensitivity;
    public TouchButton[] buttons;
    private int touch;
    private bool inButtons;
    public virtual void FixedUpdate()
    {
        float x = 0.0f;
        float y = 0.0f;
        int t = 0;
        if (Input.touches.Length > 0) //user is touching
        {
            t = 0;
            while (t < Input.touches.Length) //for each touch
            {
                //check if that touch is currently touching a button
                inButtons = false;
                int b = 0;
                while (b < buttons.Length)
                {
                    if (Input.touches[t].fingerId == buttons[b].curTouch)
                    {
                        inButtons = true;
                        break;
                    }
                    b++;
                }
                if (!inButtons) //if it wasn't, then we have found our touch
                {
                    break;
                }
                t++;
            }
            //if no touch was viable
            if (inButtons)
            {
                return;
            }
            if (Input.touches[t].phase == TouchPhase.Moved) //the touch moved
            {
                x = (Input.touches[t].deltaPosition.x * sensitivity) * Time.deltaTime;
                y = (Input.touches[t].deltaPosition.y * sensitivity) * Time.deltaTime;
            }
        }
        else
        {
            // zero out axis
            x = 0;
            y = 0;
        }
        //invert if needed
        if (invert)
        {
            x = x * -1;
            y = y * -1;
        }
        //set proper axis
        if (direction == dirs.x)
        {
            input.axis = x;
        }
        else
        {
            input.axis = y;
        }
    }

    public Drag()
    {
        direction = dirs.x;
    }
}