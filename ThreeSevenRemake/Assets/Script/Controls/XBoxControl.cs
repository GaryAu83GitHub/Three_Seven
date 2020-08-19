using System.Collections.Generic;
using UnityEngine;

public enum XBoxButton
{
    A, B, X, Y,
    L_Shoulder, R_Shoulder,
    Back, Start,
    L_Thumb, R_Thumb,
    L_Thumb_Up, L_Thumb_Right, L_Thumb_Down, L_Thumb_Left,
    L_Trigger, R_Trigger,
    R_Thumb_Up, R_Thumb_Right, R_Thumb_Down, R_Thumb_Left,
    DPad_Up, DPad_Right, DPad_Down, DPad_Left,
    NONE,
}

public enum AnalogueSticks
{
    L_STICK,
    TRIGGER,
    R_STICK,
    D_PAD,
    NONE
}

public class XBoxControl : ControlObject
{
    /*private readonly List<string> mButtonNames = new List<string>()
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
    };*/

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

    #region Button list
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
    #endregion

    private readonly Dictionary<XBoxButton, bool> mAnalogueIsPressed = new Dictionary<XBoxButton, bool>()
    {
        { XBoxButton.L_Thumb_Up, false }, { XBoxButton.L_Thumb_Right, false }, { XBoxButton.L_Thumb_Down, false }, { XBoxButton.L_Thumb_Left, false },
        { XBoxButton.L_Trigger, false }, { XBoxButton.R_Trigger, false },
        { XBoxButton.R_Thumb_Up, false }, { XBoxButton.R_Thumb_Right, false }, { XBoxButton.R_Thumb_Down, false }, { XBoxButton.R_Thumb_Left, false },
        { XBoxButton.DPad_Up, false }, { XBoxButton.DPad_Right, false }, { XBoxButton.DPad_Down, false }, { XBoxButton.DPad_Left, false },
    };

    private readonly Dictionary<XBoxButton, int> mInputIsPressedAndCanRelease = new Dictionary<XBoxButton, int>()
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

    private readonly Dictionary<CommandIndex, XBoxButton> mCommands = new Dictionary<CommandIndex, XBoxButton>();

    public XBoxControl()
    {
        mButtonIndex = new Dictionary<XBoxButton, string>()
        {
            { XBoxButton.A, "A_Button" },
            { XBoxButton.B, "B_Button" },
            { XBoxButton.X, "X_Button" },
            { XBoxButton.Y, "Y_Button" },
            { XBoxButton.L_Shoulder, "L_Shoulder" },
            { XBoxButton.R_Shoulder, "R_Shoulder" },
            { XBoxButton.Back, "Back_Button" },
            { XBoxButton.Start, "Start_Button" },
            { XBoxButton.L_Thumb, "L_Thumb_Button" },
            { XBoxButton.R_Thumb, "R_Thumb_Button" },
        };
    }

    public override void KeySettings()
    {
        Dictionary<CommandIndex, XBoxButton> defaultSets = new Dictionary<CommandIndex, XBoxButton>
        {
            // navigation
            { CommandIndex.NAVI_LEFT, XBoxButton.L_Thumb_Left },
            { CommandIndex.NAVI_RIGHT, XBoxButton.L_Thumb_Right },
            { CommandIndex.NAVI_DOWN, XBoxButton.L_Thumb_Down },
            { CommandIndex.NAVI_UP,XBoxButton.L_Thumb_Up },
            { CommandIndex.SELECT,XBoxButton.A },
            { CommandIndex.CANCEL,XBoxButton.B },
            { CommandIndex.CONFIRM,XBoxButton.Start },
            { CommandIndex.BACK,XBoxButton.Back },
            // gameplay
            { CommandIndex.BLOCK_MOVE_LEFT,XBoxButton.L_Thumb_Left },
            { CommandIndex.BLOCK_MOVE_RIGHT,XBoxButton.L_Thumb_Right },
            { CommandIndex.BLOCK_DROP,XBoxButton.L_Thumb_Down },
            { CommandIndex.BLOCK_INSTANT_DROP,XBoxButton.A },
            { CommandIndex.BLOCK_ROTATE,XBoxButton.X },
            { CommandIndex.BLOCK_INVERT,XBoxButton.Y },
            { CommandIndex.POWER_UP_USE,XBoxButton.B },
            { CommandIndex.POWER_UP_NAVI_LEFT,XBoxButton.R_Thumb_Left },
            { CommandIndex.POWER_UP_NAVI_RIGHT,XBoxButton.R_Thumb_Right },
            { CommandIndex.PREVIEW_ROTATE,XBoxButton.R_Shoulder },
            { CommandIndex.INGAME_PAUSE,XBoxButton.Start }
        };

        foreach (CommandIndex com in defaultSets.Keys)
            mCommands[com] = defaultSets[com];
    }

    /// <summary>
    /// Set new commando from the list of bindings in the parameter
    /// </summary>
    /// <param name="someNewBinding">new set of commands</param>
    public override void SetNewCommandoBinding(Dictionary<CommandIndex, KeybindData> someNewBinding)
    {
        foreach (CommandIndex com in someNewBinding.Keys)
            mCommands[com] = someNewBinding[com].BindingXBoxButton;
    }

    /// <summary>
    /// Get the button set to the requesting command into the bind data reference
    /// </summary>
    /// <param name="aCommand">requesting command index</param>
    /// <param name="aBindData">reference bind data to set button of the requesting
    /// command index</param>
    public override void GetInputFor(CommandIndex aCommand, ref KeybindData aBindData)
    {
        aBindData.ChangeXBoxButton(mCommands[aCommand]);
    }

    /// <summary>
    /// Return true if the button mapped to the requested command was pressed
    /// </summary>
    /// <param name="aCommand">Requesting mapping command</param>
    /// <returns>Returns true when the mapping button is pressed</returns>
    public override bool KeyPress(CommandIndex aCommand)
    {
        return InputPress(mCommands[aCommand]);
    }

    /// <summary>
    /// Return true so long the button mapped to the requested command is held down
    /// </summary>
    /// <param name="aCommand">Requesting mapping command</param>
    /// <returns>Return true as long the mapping button is held down</returns>
    public override bool KeyHold(CommandIndex aCommand)
    {
        return InputHold(mCommands[aCommand]);
    }

    #region Input Parts

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
            return ButtonPressAndRelease(aButton);

        return AnaloguePressAndRelease(aButton);
    }

    public Vector2 InputAnalogueDirection(AnalogueSticks aStick)
    {
        if (aStick == AnalogueSticks.L_STICK)
            return LeftStickAnalogue();
        else if (aStick == AnalogueSticks.TRIGGER)
            return TriggerAnalogue();
        else if (aStick == AnalogueSticks.R_STICK)
            return RightStickAnalogue();
        else if (aStick == AnalogueSticks.D_PAD)
            return DPadAnalogue();
        return Vector2.zero;
    }

    #region Analogue Press state
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

    private bool LeftStickPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.L_Thumb_Up)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Up, LeftStickButton() == Vector2Int.up);
        if (aButton == XBoxButton.L_Thumb_Right)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Right, LeftStickButton() == Vector2Int.right);
        if (aButton == XBoxButton.L_Thumb_Down)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Down, LeftStickButton() == Vector2Int.down);
        if (aButton == XBoxButton.L_Thumb_Left)
            return GetAnalogueIsPressed(XBoxButton.L_Thumb_Left, LeftStickButton() == Vector2Int.left);
        return false;
    }

    private bool TriggerPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Trigger)
            return GetAnalogueIsPressed(XBoxButton.R_Trigger, TriggerButton() == Vector2Int.right);
        if (aButton == XBoxButton.L_Trigger)
            return GetAnalogueIsPressed(XBoxButton.L_Trigger, TriggerButton() == Vector2Int.left);
        return false;
    }

    private bool RightStickPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Thumb_Up)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Up, RightStickButton() == Vector2Int.up);
        if (aButton == XBoxButton.R_Thumb_Right)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Right, RightStickButton() == Vector2Int.right);
        if (aButton == XBoxButton.R_Thumb_Down)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Down, RightStickButton() == Vector2Int.down);
        if (aButton == XBoxButton.R_Thumb_Left)
            return GetAnalogueIsPressed(XBoxButton.R_Thumb_Left, RightStickButton() == Vector2Int.left);
        return false;
    }

    private bool DPadPress(XBoxButton aButton)
    {
        if (aButton == XBoxButton.DPad_Up)
            return GetAnalogueIsPressed(XBoxButton.DPad_Up, DPadButton() == Vector2Int.up);
        if (aButton == XBoxButton.DPad_Right)
            return GetAnalogueIsPressed(XBoxButton.DPad_Right, DPadButton() == Vector2Int.right);
        if (aButton == XBoxButton.DPad_Down)
            return GetAnalogueIsPressed(XBoxButton.DPad_Down, DPadButton() == Vector2Int.down);
        if (aButton == XBoxButton.DPad_Left)
            return GetAnalogueIsPressed(XBoxButton.DPad_Left, DPadButton() == Vector2Int.left);
        return false;
    }
    #endregion

    #region Analogue Hold stats
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

    private bool LeftStickHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.L_Thumb_Up && LeftStickButton() == Vector2Int.up)
            return true;
        if (aButton == XBoxButton.L_Thumb_Right && LeftStickButton() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.L_Thumb_Down && LeftStickButton() == Vector2Int.down)
            return true;
        if (aButton == XBoxButton.L_Thumb_Left && LeftStickButton() == Vector2Int.left)
            return true;
        return false;
    }

    private bool TriggerHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Trigger && TriggerButton() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.L_Trigger && TriggerButton() == Vector2Int.left)
            return true;
        return false;
    }

    private bool RightStickHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Thumb_Up && RightStickButton() == Vector2Int.up)
            return true;
        if (aButton == XBoxButton.R_Thumb_Right && RightStickButton() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.R_Thumb_Down && RightStickButton() == Vector2Int.down)
            return true;
        if (aButton == XBoxButton.R_Thumb_Left && RightStickButton() == Vector2Int.left)
            return true;
        return false;
    }

    private bool DPadHold(XBoxButton aButton)
    {
        if (aButton == XBoxButton.DPad_Up && DPadButton() == Vector2Int.up)
            return true;
        if (aButton == XBoxButton.DPad_Right && DPadButton() == Vector2Int.right)
            return true;
        if (aButton == XBoxButton.DPad_Down && DPadButton() == Vector2Int.down)
            return true;
        if (aButton == XBoxButton.DPad_Left && DPadButton() == Vector2Int.left)
            return true;
        return false;
    }
    #endregion

    #region Press and Release
    private bool ButtonPressAndRelease(XBoxButton aButton)
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

    private bool LeftStickPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.L_Thumb_Up)
            return GetPressAndRelease(XBoxButton.L_Thumb_Up, LeftStickButton() == Vector2Int.up);
        if (aButton == XBoxButton.L_Thumb_Right)
            return GetPressAndRelease(XBoxButton.L_Thumb_Right, LeftStickButton() == Vector2Int.right);
        if (aButton == XBoxButton.L_Thumb_Down)
            return GetPressAndRelease(XBoxButton.L_Thumb_Down, LeftStickButton() == Vector2Int.down);
        if (aButton == XBoxButton.L_Thumb_Left)
            return GetPressAndRelease(XBoxButton.L_Thumb_Left, LeftStickButton() == Vector2Int.left);
        return false;
    }

    private bool TriggerPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Trigger)
            return GetPressAndRelease(XBoxButton.R_Trigger, TriggerButton() == Vector2Int.right);
        if (aButton == XBoxButton.L_Trigger)
            return GetPressAndRelease(XBoxButton.L_Trigger, TriggerButton() == Vector2Int.left);
        return false;
    }

    private bool RightStickPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.R_Thumb_Up)
            return GetPressAndRelease(XBoxButton.R_Thumb_Up, RightStickButton() == Vector2Int.up);
        if (aButton == XBoxButton.R_Thumb_Right)
            return GetPressAndRelease(XBoxButton.R_Thumb_Right, RightStickButton() == Vector2Int.right);
        if (aButton == XBoxButton.R_Thumb_Down)
            return GetPressAndRelease(XBoxButton.R_Thumb_Down, RightStickButton() == Vector2Int.down);
        if (aButton == XBoxButton.R_Thumb_Left)
            return GetPressAndRelease(XBoxButton.R_Thumb_Left, RightStickButton() == Vector2Int.left);
        return false;
    }

    private bool DPadPressAndRelease(XBoxButton aButton)
    {
        if (aButton == XBoxButton.DPad_Up)
            return GetPressAndRelease(XBoxButton.DPad_Up, DPadButton() == Vector2Int.up);
        if (aButton == XBoxButton.DPad_Right)
            return GetPressAndRelease(XBoxButton.DPad_Right, DPadButton() == Vector2Int.right);
        if (aButton == XBoxButton.DPad_Down)
            return GetPressAndRelease(XBoxButton.DPad_Down, DPadButton() == Vector2Int.down);
        if (aButton == XBoxButton.DPad_Left)
            return GetPressAndRelease(XBoxButton.DPad_Left, DPadButton() == Vector2Int.left);
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

    private Vector2Int LeftStickButton() { return new Vector2Int((int)Input.GetAxis(mAxisNames[0]), (int)Input.GetAxis(mAxisNames[1])); }

    private Vector2 LeftStickAnalogue() { return new Vector2(Input.GetAxis(mAxisNames[0]), Input.GetAxis(mAxisNames[1])); }

    private Vector2Int TriggerButton(){ return new Vector2Int((int)Input.GetAxis(mAxisNames[2]), 0); }

    private Vector2 TriggerAnalogue() { return new Vector2(Input.GetAxis(mAxisNames[2]), 0f); }

    private Vector2Int RightStickButton() { return new Vector2Int((int)Input.GetAxis(mAxisNames[3]), (int)Input.GetAxis(mAxisNames[4])); }

    private Vector2 RightStickAnalogue() { return new Vector2(Input.GetAxis(mAxisNames[3]), Input.GetAxis(mAxisNames[4])); }

    private Vector2Int DPadButton() { return new Vector2Int((int)Input.GetAxis(mAxisNames[5]), (int)Input.GetAxis(mAxisNames[6])); }

    private Vector2 DPadAnalogue() { return new Vector2(Input.GetAxis(mAxisNames[5]), Input.GetAxis(mAxisNames[6])); }

    #endregion
}
