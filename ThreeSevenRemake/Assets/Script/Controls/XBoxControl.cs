using System.Collections.Generic;
using UnityEngine;

public class XBoxControl
{
    private readonly List<string> mButtonNames = new List<string>()
    {
        "A_Button",
        "B_Button",
        "X_Button",
        "Y_Button",
        "L_Shoulder",
        "R_Shoulder",
        "Back_Button",
        "Start_Button",
        "L_Thumb_Button",
        "R_Thumb_Button",
    };

    private readonly List<string> mAxisNames = new List<string>()
    {
        "L_Stick_Hori_Axis",
        "L_Stick_Vert_Axis",
        "Trigger_Axis",
        "R_Stick_Hori_Axis",
        "R_Stick_Vert_Axis",
        "DPad_Hori_Axis",
        "DPad_Vert_Axis",
    };

    private List<XBoxButton> mButtons = new List<XBoxButton>()
    {
        XBoxButton.A, XBoxButton.B, XBoxButton.X, XBoxButton.Y,
        XBoxButton.L_Shoulder, XBoxButton.R_Shoulder,
        XBoxButton.Back, XBoxButton.Start,
        XBoxButton.L_Thumb, XBoxButton.R_Thumb
    };

    private List<XBoxButton> mLeftStick = new List<XBoxButton>()
    {
        XBoxButton.L_Thumb_Up, XBoxButton.L_Thumb_Right, XBoxButton.L_Thumb_Down, XBoxButton.L_Thumb_Left
    };

    private List<XBoxButton> mTriggers = new List<XBoxButton>()
    {
        XBoxButton.L_Trigger, XBoxButton.R_Trigger
    };

    private List<XBoxButton> mRightStick = new List<XBoxButton>()
    {
        XBoxButton.R_Thumb_Up, XBoxButton.R_Thumb_Right, XBoxButton.R_Thumb_Down, XBoxButton.R_Thumb_Left
    };

    private List<XBoxButton> mDPad = new List<XBoxButton>()
    {
        XBoxButton.DPad_Up, XBoxButton.DPad_Right, XBoxButton.DPad_Down, XBoxButton.DPad_Left
    };

    private Dictionary<XBoxButton, bool> mAnalogueIsPressed = new Dictionary<XBoxButton, bool>()
    {
        { XBoxButton.L_Thumb_Up, false }, { XBoxButton.L_Thumb_Right, false }, { XBoxButton.L_Thumb_Down, false }, { XBoxButton.L_Thumb_Left, false },
        { XBoxButton.L_Trigger, false }, { XBoxButton.R_Trigger, false },
        { XBoxButton.R_Thumb_Up, false }, { XBoxButton.R_Thumb_Right, false }, { XBoxButton.R_Thumb_Down, false }, { XBoxButton.R_Thumb_Left, false },
        { XBoxButton.DPad_Up, false }, { XBoxButton.DPad_Right, false }, { XBoxButton.DPad_Down, false }, { XBoxButton.DPad_Left, false },
    };

    private Dictionary<XBoxButton, int> mInputIsPressedAndCanRelease = new Dictionary<XBoxButton, int>()
    {
        { XBoxButton.A, 0 }, { XBoxButton.B, 0 }, { XBoxButton.X, 0 }, { XBoxButton.Y, 0 },
        { XBoxButton.L_Shoulder, 0 }, { XBoxButton.R_Shoulder, 0 },
        { XBoxButton.Back, 0 }, { XBoxButton.Start, 0 },
        { XBoxButton.L_Thumb, 0 }, { XBoxButton.R_Thumb, 0 },
        { XBoxButton.L_Thumb_Up, 0 }, { XBoxButton.L_Thumb_Right, 0 }, { XBoxButton.L_Thumb_Down, 0 }, { XBoxButton.L_Thumb_Left, 0 },
        { XBoxButton.L_Trigger, 0 }, { XBoxButton.R_Trigger, 0 },
        { XBoxButton.R_Thumb_Up, 0 }, { XBoxButton.R_Thumb_Right, 0 }, { XBoxButton.R_Thumb_Down, 0 }, { XBoxButton.R_Thumb_Left, 0 },
        { XBoxButton.DPad_Up, 0 }, { XBoxButton.DPad_Right, 0 }, { XBoxButton.DPad_Down, 0 }, { XBoxButton.DPad_Left, 0 },
    };

    private readonly Dictionary<XBoxButton, string> mButtonIndex = new Dictionary<XBoxButton, string>();

    public XBoxControl()
    {
        mButtonIndex = new Dictionary<XBoxButton, string>()
        {
            { XBoxButton.A, mButtonNames[(int)XBoxButton.A] },
            { XBoxButton.B, mButtonNames[(int)XBoxButton.B] },
            { XBoxButton.X, mButtonNames[(int)XBoxButton.X] },
            { XBoxButton.Y, mButtonNames[(int)XBoxButton.Y] },
            { XBoxButton.L_Shoulder, mButtonNames[(int)XBoxButton.L_Shoulder] },
            { XBoxButton.R_Shoulder, mButtonNames[(int)XBoxButton.R_Shoulder] },
            { XBoxButton.Back, mButtonNames[(int)XBoxButton.Back] },
            { XBoxButton.Start, mButtonNames[(int)XBoxButton.Start] },
            { XBoxButton.L_Thumb, mButtonNames[(int)XBoxButton.L_Thumb] },
            { XBoxButton.R_Thumb, mButtonNames[(int)XBoxButton.R_Thumb] },
        };
    }

    public bool InputPress(XBoxButton aButton)
    {
        if (mButtons.Contains(aButton))
            return ButtonPress(aButton);
        
        return AnaloguePress(aButton);
    }

    public bool InputHold(XBoxButton aButton)
    {
        if (mButtons.Contains(aButton))
            return ButtonHold(aButton);

        return AnalogueHold(aButton);
    } 
    
    public bool InputPressAndHold(XBoxButton aButton)
    {
        if (mButtons.Contains(aButton))
            return ButtonPressAndHold(aButton);

        return AnaloguePressAndRelease(aButton);
    }

    private bool ButtonPress(XBoxButton aButton)
    {
        return Input.GetButtonDown(mButtonIndex[aButton]);
    }

    private bool AnaloguePress(XBoxButton aButton)
    {
        if (mLeftStick.Contains(aButton))
            return LeftStickPress(aButton);
        else if (mTriggers.Contains(aButton))
            return TriggerPress(aButton);
        else if (mRightStick.Contains(aButton))
            return RightStickPress(aButton);
        else if (mDPad.Contains(aButton))
            return DPadPress(aButton);
        return false;
    }

    private bool ButtonHold(XBoxButton aButton)
    {
        return Input.GetButton(mButtonIndex[aButton]);
    }

    private bool AnalogueHold(XBoxButton aButton)
    {
        if (mLeftStick.Contains(aButton))
            return LeftStickHold(aButton);
        else if (mTriggers.Contains(aButton))
            return TriggerHold(aButton);
        else if (mRightStick.Contains(aButton))
            return RightStickHold(aButton);
        else if (mDPad.Contains(aButton))
            return DPadHold(aButton);
        return false;
    }

    private bool ButtonPressAndHold(XBoxButton aButton)
    {
        return GetPressAndRelease(aButton, ButtonHold(aButton));
    }

    private bool AnaloguePressAndRelease(XBoxButton aButton)
    {
        if (mLeftStick.Contains(aButton))
            return LeftStickPressAndRelease(aButton);
        else if (mTriggers.Contains(aButton))
            return TriggerPressAndRelease(aButton);
        else if (mRightStick.Contains(aButton))
            return RightStickPressAndRelease(aButton);
        else if (mDPad.Contains(aButton))
            return DPadPressAndRelease(aButton);
        return false;
    }

    #region Analogue Press state
    private bool LeftStickPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.L_Thumb_Up)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Up, LeftStick() == Vector2Int.up);
        if (aButton == XBoxButton.L_Thumb_Right)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Right, LeftStick() == Vector2Int.right);
        if (aButton == XBoxButton.L_Thumb_Down)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Down, LeftStick() == Vector2Int.down);
        if (aButton == XBoxButton.L_Thumb_Left)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Left, LeftStick() == Vector2Int.left);
        return false;
    }

    private bool TriggerPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Trigger)
            return GetAnalogueIsPressed(XBoxButton.R_Trigger, Trigger() == Vector2Int.right);
        if (aButton == XBoxButton.L_Trigger)
            return GetAnalogueIsPressed(XBoxButton.L_Trigger, Trigger() == Vector2Int.left);
        return false;
    }

    private bool RightStickPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Thumb_Up)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Up, RightStick() == Vector2Int.up);
        if (aButton == XBoxButton.R_Thumb_Right)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Right, RightStick() == Vector2Int.right);
        if (aButton == XBoxButton.R_Thumb_Down)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Down, RightStick() == Vector2Int.down);
        if (aButton == XBoxButton.R_Thumb_Left)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Left, RightStick() == Vector2Int.left);
        return false;
    }

    private bool DPadPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.DPad_Up)
            return GetAnalogueIsPressed(XBoxButton.DPad_Up, DPad() == Vector2Int.up);
        if (aButton == XBoxButton.DPad_Right)
            return GetAnalogueIsPressed(XBoxButton.DPad_Right, DPad() == Vector2Int.right);
        if (aButton == XBoxButton.DPad_Down)
            return GetAnalogueIsPressed(XBoxButton.DPad_Down, DPad() == Vector2Int.down);
        if (aButton == XBoxButton.DPad_Left)
            return GetAnalogueIsPressed(XBoxButton.DPad_Left, DPad() == Vector2Int.left);
        return false;
    }    
    #endregion

    #region Analogue Hold stats
    private bool LeftStickHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.L_Thumb_Up && LeftStick() == Vector2Int.up)
            return true;
        if (aButton == XBoxButton.L_Thumb_Right && LeftStick() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.L_Thumb_Down && LeftStick() == Vector2Int.down)
            return true;
        if (aButton == XBoxButton.L_Thumb_Left && LeftStick() == Vector2Int.left)
            return true;
        return false;
    }

    private bool TriggerHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Trigger && Trigger() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.L_Trigger && Trigger() == Vector2Int.left)
            return true;
        return false;
    }

    private bool RightStickHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Thumb_Up && RightStick() == Vector2Int.up)
            return true;
        if (aButton == XBoxButton.R_Thumb_Right && RightStick() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.R_Thumb_Down && RightStick() == Vector2Int.down)
            return true;
        if (aButton == XBoxButton.R_Thumb_Left && RightStick() == Vector2Int.left)
            return true;
        return false;
    }

    private bool DPadHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.DPad_Up && DPad() == Vector2Int.up)
            return true;
        if (aButton == XBoxButton.DPad_Right && DPad() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.DPad_Down && DPad() == Vector2Int.down)
            return true;
        if (aButton == XBoxButton.DPad_Left && DPad() == Vector2Int.left)
            return true;
        return false;
    }
    #endregion

    #region Press and Release
    private bool LeftStickPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.L_Thumb_Up)
            return GetPressAndRelease(XBoxButton.L_Thumb_Up, LeftStick() == Vector2Int.up);
        if (aButton == XBoxButton.L_Thumb_Right)
            return GetPressAndRelease(XBoxButton.L_Thumb_Right, LeftStick() == Vector2Int.right);
        if (aButton == XBoxButton.L_Thumb_Down)
            return GetPressAndRelease(XBoxButton.L_Thumb_Down, LeftStick() == Vector2Int.down);
        if (aButton == XBoxButton.L_Thumb_Left)
            return GetPressAndRelease(XBoxButton.L_Thumb_Left, LeftStick() == Vector2Int.left);
        return false;
    }

    private bool TriggerPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Trigger)
            return GetPressAndRelease(XBoxButton.R_Trigger, Trigger() == Vector2Int.right);
        if (aButton == XBoxButton.L_Trigger)
            return GetPressAndRelease(XBoxButton.L_Trigger, Trigger() == Vector2Int.left);
        return false;
    }

    private bool RightStickPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Thumb_Up)
            return GetPressAndRelease(XBoxButton.R_Thumb_Up, RightStick() == Vector2Int.up);
        if (aButton == XBoxButton.R_Thumb_Right)
            return GetPressAndRelease(XBoxButton.R_Thumb_Right, RightStick() == Vector2Int.right);
        if (aButton == XBoxButton.R_Thumb_Down)
            return GetPressAndRelease(XBoxButton.R_Thumb_Down, RightStick() == Vector2Int.down);
        if (aButton == XBoxButton.R_Thumb_Left)
            return GetPressAndRelease(XBoxButton.R_Thumb_Left, RightStick() == Vector2Int.left);
        return false;
    }

    private bool DPadPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.DPad_Up)
            return GetPressAndRelease(XBoxButton.DPad_Up, DPad() == Vector2Int.up);
        if (aButton == XBoxButton.DPad_Right)
            return GetPressAndRelease(XBoxButton.DPad_Right, DPad() == Vector2Int.right);
        if (aButton == XBoxButton.DPad_Down)
            return GetPressAndRelease(XBoxButton.DPad_Down, DPad() == Vector2Int.down);
        if (aButton == XBoxButton.DPad_Left)
            return GetPressAndRelease(XBoxButton.DPad_Left, DPad() == Vector2Int.left);
        return false;
    }
    #endregion

    private bool GetAnalogueIsPressed(XBoxButton anAnalogueButton, bool directionIsMatched)
    {
        if (!mAnalogueIsPressed[anAnalogueButton] && directionIsMatched)
        {
            mAnalogueIsPressed[anAnalogueButton] = true;
            return true;
        }
        else if (mAnalogueIsPressed[anAnalogueButton] && directionIsMatched)
            return false;

        mAnalogueIsPressed[anAnalogueButton] = false;

        return false;
    }

    private bool GetPressAndRelease(XBoxButton aButton, bool theButtonIsPressed)
    {
        if(mInputIsPressedAndCanRelease[aButton] == 0 && theButtonIsPressed)
        {
            Debug.Log("Step 1: 0 and the buttonIsPressed = true");
            mInputIsPressedAndCanRelease[aButton] = 1;
            return false;
        }
        else if(mInputIsPressedAndCanRelease[aButton] == 1 && theButtonIsPressed)
        {
            Debug.Log("Step 2: 1 and the buttonIsPressed = true");
            return false;
        }
        else if(mInputIsPressedAndCanRelease[aButton] == 1 && !theButtonIsPressed)
        {
            Debug.Log("Step 3: 2 and the buttonIsPressed = false");
            mInputIsPressedAndCanRelease[aButton] = 2;
            return true;
        }
        mInputIsPressedAndCanRelease[aButton] = 0;
        return false;
    }

    private Vector2Int LeftStick() { return new Vector2Int((int)Input.GetAxis(mAxisNames[0]), (int)Input.GetAxis(mAxisNames[1])); }

    private Vector2Int Trigger(){ return new Vector2Int((int)Input.GetAxis(mAxisNames[2]), 0); }

    private Vector2Int RightStick() { return new Vector2Int((int)Input.GetAxis(mAxisNames[3]), (int)Input.GetAxis(mAxisNames[4])); }

    private Vector2Int DPad() { return new Vector2Int(Mathf.RoundToInt(Input.GetAxis(mAxisNames[5])), Mathf.RoundToInt(Input.GetAxis(mAxisNames[6]))); }
}
