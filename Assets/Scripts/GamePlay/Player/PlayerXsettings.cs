using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerXsettings
{
    public KeyCode Up { get; set; }
    public KeyCode Down { get; set; }
    public KeyCode Right { get; set; }
    public KeyCode Left { get; set; }
    public KeyCode Shoot { get; set; }

    /// <summary>
    /// Basic constructor for PlayerXsettings
    /// </summary>
    /// <param name="up">The Up KeyCode</param>
    /// <param name="down">The Down KeyCode</param>
    /// <param name="right">The Right KeyCode</param>
    /// <param name="left">The Left KeyCode</param>
    /// <param name="shoot">The Shoot KeyCode</param>

    public PlayerXsettings (KeyCode up, KeyCode down, KeyCode right, KeyCode left, KeyCode shoot)
    {
        Up = up;
        Down = down;
        Right = right;
        Left = left;
        Shoot = shoot;
    }

    /// <summary>
    /// Change the KeyCode of an input depending on the tag of the input's button
    /// </summary>
    /// <param name="tag">The tag of the button that we change the keycode</param>
    /// <param name="NewKeyCode">The new KeyCode that will be applied</param>
    public void ChangeKeyCode (string tag, KeyCode NewKeyCode)
    {
        if (tag == "Up")
        {
            Up = NewKeyCode;
        }
        else if (tag == "Down")
        {
            Down = NewKeyCode;
        }
        else if (tag == "Right")
        {
            Right = NewKeyCode;
        }
        else if (tag == "Left")
        {
            Left = NewKeyCode;
        }
        else if (tag == "Shoot")
        {
            Shoot = NewKeyCode;
        }
    }

    public override string ToString()
    {
        return "Settings : " + Up.ToString() + " | " + Down.ToString() + " | " + Right.ToString() + " | " + Left.ToString();
    }
}
