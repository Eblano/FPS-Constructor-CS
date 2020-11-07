using UnityEngine;
using System.Collections;

public enum axes
{
    x = 0,
    y = 1, //which rotation axis should we use?
    z = 2
}

public enum directions
{
    positive = 0, //should it be inverted?
    negative = 1
}

[System.Serializable]
public partial class Tilt : MonoBehaviour
{
    public axes axis;
    public directions direction;
    public InputItem input;
    public float sensitivity;
    public float offset;
    public float buffer;
    public virtual void UpdateInput()
    {
        if (axis == axes.x)
        {
            input.axis = Input.acceleration.x;
        }
        else
        {
            if (axis == axes.y)
            {
                input.axis = Input.acceleration.y;
            }
            else
            {
                if (axis == axes.z)
                {
                    input.axis = Input.acceleration.z;
                }
            }
        }
        input.axis = input.axis + offset;
        if (input.axis > 0)
        {
            input.axis = Mathf.Clamp(input.axis - buffer, 0, input.axis);
        }
        else
        {
            input.axis = Mathf.Clamp(input.axis + buffer, input.axis, 0);
        }
        input.axis = input.axis * sensitivity;
        if (direction == directions.negative)
        {
            input.axis = input.axis * -1;
        }
    }

    public Tilt()
    {
        axis = axes.z;
        direction = directions.positive;
    }
}