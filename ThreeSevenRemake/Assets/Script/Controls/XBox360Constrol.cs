
using System.Collections.Generic;
using System.Linq;
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

public enum GP_InputType
{
    BUTTON,
    AXIS,
    NONE
}

public enum AxisInput
{
    L_STICK,
    TRIGGER,
    R_STICK,
    D_PAD,
    NONE
}

public class XBox360Constrol : ControlObject
{
    //private readonly AxisInput mNavigation = AxisInput.L_STICK;

    private delegate Vector2Int OnGUINavigation();
    private event OnGUINavigation GuiNavigation;

    private delegate Vector2Int OnGameBlockNavigation(NavigatorType aType = NavigatorType.BLOCK_NAVIGATOR);
    private event OnGameBlockNavigation GameBlockNavigation;

    private delegate Vector2Int OnGamePowerUpNavigation(NavigatorType aType = NavigatorType.POWER_UP_NAVIGATOR);
    private event OnGamePowerUpNavigation GamePowerUpNavigation;

    private delegate Vector2Int OnGamePadTesting(NavigatorType aType = NavigatorType.GAME_PAD_TESTING);
    private event OnGamePadTesting GamePadTesting;

    private delegate Vector2Int OnGameEnableAnalogNavigator();
    private event OnGameEnableAnalogNavigator EnableAnalogNavigator;

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
    public static List<string> XboxButtonNames = new List<string>();

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
    public static List<string> XboxAxisNames = new List<string>();

    public static List<XBoxButton> Buttons = new List<XBoxButton>()
    {
        XBoxButton.A, XBoxButton.B, XBoxButton.X, XBoxButton.Y,
        XBoxButton.L_Shoulder, XBoxButton.R_Shoulder,
        XBoxButton.Back, XBoxButton.Start,
        XBoxButton.L_Thumb, XBoxButton.R_Thumb
    };

    public static List<XBoxButton> Analogue = new List<XBoxButton>()
    {
        XBoxButton.L_Thumb_Up, XBoxButton.L_Thumb_Right, XBoxButton.L_Thumb_Down, XBoxButton.L_Thumb_Left,
        XBoxButton.L_Trigger, XBoxButton.R_Trigger,
        XBoxButton.R_Thumb_Up, XBoxButton.R_Thumb_Right, XBoxButton.R_Thumb_Down, XBoxButton.R_Thumb_Left,
        XBoxButton.DPad_Up, XBoxButton.DPad_Right, XBoxButton.DPad_Down, XBoxButton.DPad_Left,
    };

    private readonly Dictionary<XBoxButton, string> mButtonIndex = new Dictionary<XBoxButton, string>();
    private readonly Dictionary<CommandIndex, ControlInput> mCommands = new Dictionary<CommandIndex, ControlInput>();
    private readonly Dictionary<AxisInput, bool> mEnableNavigationSticks = new Dictionary<AxisInput, bool>();

    private Vector2Int mCurrentMenuNavigateDireciton = Vector2Int.zero;
    private Vector2Int mLastNavigateAxisDirection = Vector2Int.zero;


    private readonly Dictionary<NavigatorType, Vector2Int> mCurrentActiveNavigation = new Dictionary<NavigatorType, Vector2Int>
    {
        {NavigatorType.BLOCK_NAVIGATOR, Vector2Int.zero },
        {NavigatorType.POWER_UP_NAVIGATOR, Vector2Int.zero },
    };

    private readonly Dictionary<NavigatorType, AxisInput> mLastPulledAxis = new Dictionary<NavigatorType, AxisInput>
    {
        {NavigatorType.BLOCK_NAVIGATOR, AxisInput.NONE },
        {NavigatorType.POWER_UP_NAVIGATOR, AxisInput.NONE },
    };

    private float mMenuNavigationSuppressTimer = 0f;

    private bool mPowerUpNavigateIsUsed = false;

    private ControlInput mLastInput = null;
    //private int mCurrentHorizontDirection = 0;
    public XBox360Constrol()
    {
        mType = ControlType.XBOX_360;

        for (int i = 0; i < mButtonNames.Count; i++)
            XboxButtonNames.Add(mButtonNames[i]);

       
        for (int i = 0; i < mAxisNames.Count; i++)
            XboxAxisNames.Add(mAxisNames[i]);

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

        GuiNavigation += LeftStick;

        GameBlockNavigation += LeftStickNew;
        GameBlockNavigation += DPadNew;

        GamePowerUpNavigation += TriggerNew;

        GamePadTesting += LeftStickNew;
        GamePadTesting += RightStickNew;
        GamePadTesting += DPadNew;
        GamePadTesting += TriggerNew;

    }

    public override void KeySettings(/*Dictionary<CommandIndex, object> someSetting*/)
    {
        Dictionary<CommandIndex, ControlInput> defaultSets = new Dictionary<CommandIndex, ControlInput>
        {
            // navigation
            { CommandIndex.NAVI_LEFT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_Thumb_Left, Vector2Int.left) },
            { CommandIndex.NAVI_RIGHT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_Thumb_Right, Vector2Int.right) },
            { CommandIndex.NAVI_DOWN, new ControlInput(AxisInput.L_STICK, XBoxButton.L_Thumb_Down, Vector2Int.down) },
            { CommandIndex.NAVI_UP, new ControlInput(AxisInput.L_STICK, XBoxButton.L_Thumb_Up, Vector2Int.up) },
            { CommandIndex.SELECT, new ControlInput(XBoxButton.A) },
            { CommandIndex.CANCEL, new ControlInput(XBoxButton.B) },
            { CommandIndex.CONFIRM, new ControlInput(XBoxButton.Start) },
            { CommandIndex.BACK, new ControlInput(XBoxButton.Back) },
            // gameplay
            { CommandIndex.BLOCK_MOVE_LEFT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_Thumb_Left, Vector2Int.left) },
            { CommandIndex.BLOCK_MOVE_RIGHT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_Thumb_Right, Vector2Int.right) },
            { CommandIndex.BLOCK_DROP, new ControlInput(AxisInput.L_STICK, XBoxButton.L_Thumb_Down, Vector2Int.down) },
            { CommandIndex.BLOCK_INSTANT_DROP, new ControlInput(XBoxButton.A) },
            { CommandIndex.BLOCK_ROTATE, new ControlInput(XBoxButton.X) },
            { CommandIndex.BLOCK_INVERT, new ControlInput(XBoxButton.Y) },
            { CommandIndex.POWER_UP_USE, new ControlInput(XBoxButton.B) },
            { CommandIndex.POWER_UP_NAVI_LEFT, new ControlInput(AxisInput.R_STICK, XBoxButton.R_Thumb_Left, Vector2Int.left) },
            { CommandIndex.POWER_UP_NAVI_RIGHT, new ControlInput(AxisInput.R_STICK, XBoxButton.R_Thumb_Right, Vector2Int.right) },
            { CommandIndex.PREVIEW_ROTATE, new ControlInput(XBoxButton.R_Shoulder) },
            { CommandIndex.INGAME_PAUSE, new ControlInput(XBoxButton.Start) }
        };


        if(EnableAnalogNavigator != null)
        {
            System.Delegate[] clientList = EnableAnalogNavigator.GetInvocationList();
            foreach (var d in clientList)
                EnableAnalogNavigator -= (d as OnGameEnableAnalogNavigator);
        }

        foreach (CommandIndex com in defaultSets.Keys)
        {
            mCommands[com] = (ControlInput)defaultSets[com];
            if(mCommands[com].AxisType != AxisInput.NONE)
            {
                if (EnableAnalogNavigator != null)
                {
                    if (EnableAnalogNavigator.GetInvocationList().ToList().Any(x => x.Method.Name == mCommands[com].AxisName))//;.Method.Name == mCommands[com].AxisName)
                        continue;
                }
                switch (mCommands[com].AxisType)
                {
                    case AxisInput.L_STICK:
                        EnableAnalogNavigator += LeftStick;
                        break;
                    case AxisInput.TRIGGER:
                        EnableAnalogNavigator += Trigger;
                        break;
                    case AxisInput.R_STICK:
                        EnableAnalogNavigator += RightStick;
                        break;
                    case AxisInput.D_PAD:
                        EnableAnalogNavigator += DPad;
                        break;
                }
            }
        }
    }

    public override void SetNewCommandoBinding(Dictionary<CommandIndex, KeybindData> someNewBinding)
    {
        foreach (CommandIndex com in someNewBinding.Keys)
            mCommands[com] = new ControlInput(someNewBinding[com].BindingXBoxButton);
    }

    public override void SetNewNavgateBinding(Dictionary<NavigatorType, KeybindData> someNewBinding)
    {
        foreach(NavigatorType navi in someNewBinding.Keys)
        {
            if (navi == NavigatorType.BLOCK_NAVIGATOR)
            {
                if (GameBlockNavigation.GetInvocationList().Count() > 0)
                {
                    System.Delegate[] clientList = GameBlockNavigation.GetInvocationList();
                    foreach (var d in clientList)
                        GameBlockNavigation -= (d as OnGameBlockNavigation);
                }

                switch(someNewBinding[navi].BindingAxis)
                {
                    case AxisInput.L_STICK:
                        GameBlockNavigation += LeftStickNew;
                        break;
                    case AxisInput.R_STICK:
                        GameBlockNavigation += RightStickNew;
                        break;
                    case AxisInput.D_PAD:
                        GameBlockNavigation += DPadNew;
                        break;
                }

            }
            if(navi == NavigatorType.POWER_UP_NAVIGATOR)
            {
                if (GamePowerUpNavigation.GetInvocationList().Count() > 0)
                {
                    System.Delegate[] clientList = GamePowerUpNavigation.GetInvocationList();
                    foreach (var d in clientList)
                        GamePowerUpNavigation -= (d as OnGamePowerUpNavigation);
                }

                switch (someNewBinding[navi].BindingAxis)
                {
                    case AxisInput.L_STICK:
                        GamePowerUpNavigation += LeftStickNew;
                        break;
                    case AxisInput.R_STICK:
                        GamePowerUpNavigation += RightStickNew;
                        break;
                    case AxisInput.D_PAD:
                        GamePowerUpNavigation += DPadNew;
                        break;
                }
            }
        }
    }

    public override bool MenuNavigateHold(CommandIndex aCommand, float anDelayIntervall = .1f)
    {
        if (CheckNaviCommandsWithTimer(mCommands[aCommand], anDelayIntervall))
            return true;

        return base.MenuNavigateHold(aCommand, anDelayIntervall);
    }

    public override bool MenuNavigatePress(CommandIndex aCommand)
    {
        if (CheckNaviCommandsWithPreviousDirection(mCommands[aCommand]))
            return true;

        return base.MenuNavigatePress(aCommand);
    }

    public override bool GameDropBlockGradually(float aBlockNextDropTime)
    {
        float vertical = ((BlockNavigationWithAnalogue().x > -1 && BlockNavigationWithAnalogue().x < 1) ? BlockNavigationWithAnalogue().y : 0f);
        if ((vertical <= -1f && DropButtonTimePassed()))
        {
            ResetDropTimer();
            return true;
        }
        return false;
    }

    protected override bool HorizontBottomHit(ref Vector3 aDir, float aHorizontValue = 0f)
    {
        return base.HorizontBottomHit(ref aDir, BlockNavigationWithAnalogue().x);
    }

    /// <summary>
    /// Return true so long the button mapped to the requested command is held down
    /// </summary>
    /// <param name="aCommand">Requesting mapping command</param>
    /// <returns>Return true as long the mapping button is held down</returns>
    public override bool KeyDown(CommandIndex aCommand) { return ButtonDown(aCommand); }

    /// <summary>
    /// Return true if the button mapped to the requested command was pressed
    /// </summary>
    /// <param name="aCommand">Requesting mapping command</param>
    /// <returns></returns>
    public override bool KeyPress(CommandIndex aCommand) { return ButtonPressed(aCommand); }

    public override int GameMovePowerUpSelection()
    {
        Vector2Int? dir = GamePowerUpNavigation?.Invoke(NavigatorType.POWER_UP_NAVIGATOR);
        if (mPowerUpNavigateIsUsed)
        {
            if (dir.Value.x != 1 && dir.Value.x != -1)
                mPowerUpNavigateIsUsed = false;
            return 0;
        }

        if (dir.Value.x == 1 || dir.Value.x == -1)
            mPowerUpNavigateIsUsed = true;

        return dir.Value.x;
    }

    /// <summary>
    /// This is mearly for testing purpose, might be put into use with futhure
    /// development
    /// </summary>
    /// <returns></returns>
    public override bool TestGamePadState(XBoxButton aButton)
    {   
        if (XBox360Constrol.Buttons.Contains(aButton))
            return Input.GetButton(mButtonIndex[aButton]);
        else if(XBox360Constrol.Analogue.Contains(aButton))
        {
            if ((aButton == XBoxButton.L_Thumb_Up && LeftStick() == Vector2Int.up) ||
                (aButton == XBoxButton.L_Thumb_Right && LeftStick() == Vector2Int.right) ||
                (aButton == XBoxButton.L_Thumb_Down && LeftStick() == Vector2Int.down) ||
                (aButton == XBoxButton.L_Thumb_Left && LeftStick() == Vector2Int.left))
                return true;
            else if ((aButton == XBoxButton.R_Thumb_Up && RightStick() == Vector2Int.up) ||
                (aButton == XBoxButton.R_Thumb_Right && RightStick() == Vector2Int.right) ||
                (aButton == XBoxButton.R_Thumb_Down && RightStick() == Vector2Int.down) ||
                (aButton == XBoxButton.R_Thumb_Left && RightStick() == Vector2Int.left))
                return true;
            else if ((aButton == XBoxButton.DPad_Up && DPad() == Vector2Int.up) ||
                (aButton == XBoxButton.DPad_Right && DPad() == Vector2Int.right) ||
                (aButton == XBoxButton.DPad_Down && DPad() == Vector2Int.down) ||
                (aButton == XBoxButton.DPad_Left && DPad() == Vector2Int.left))
                return true;
            else if((aButton == XBoxButton.R_Trigger && DPad() == Vector2Int.right) ||
                    (aButton == XBoxButton.L_Trigger && DPad() == Vector2Int.left))
                return true;
        }
        
       
        return base.TestGamePadState(aButton);
    }

    public void GetButtonFor(CommandIndex aCommand, ref XBoxButton aButton)
    {
        aButton = mCommands[aCommand].Button;
    }

    public void GetButtonFor(CommandIndex aCommand, ref KeybindData aBindData)
    {
        XBoxButton button = mCommands[aCommand].Button;
        if (XBox360Constrol.Analogue.Contains(button))
            aBindData.ChangeXBoxAnaloge(button);
        else if (XBox360Constrol.Buttons.Contains(button))
            aBindData.ChangeXBoxButton(button);
    }

    public void GetNavigationSticksFor(NavigatorType aType, ref AxisInput anAxis)
    {
        if (aType == NavigatorType.BLOCK_NAVIGATOR)
            anAxis = mCommands[CommandIndex.BLOCK_DROP].AxisType;
        else if (aType == NavigatorType.POWER_UP_NAVIGATOR)
            anAxis = mCommands[CommandIndex.POWER_UP_NAVI_LEFT].AxisType;
    }

    public XBoxButton GetButtonInput()
    {
        XBoxButton button = XBoxButton.NONE;
        if (Input.GetButtonDown(mButtonNames[0]))
            button = XBoxButton.A;
        if (Input.GetButtonDown(mButtonNames[1]))
            button = XBoxButton.B;
        if (Input.GetButtonDown(mButtonNames[2]))
            button = XBoxButton.X;
        if (Input.GetButtonDown(mButtonNames[3]))
            button = XBoxButton.Y;
        if (Input.GetButtonDown(mButtonNames[4]))
            button = XBoxButton.L_Shoulder;
        if (Input.GetButtonDown(mButtonNames[5]))
            button = XBoxButton.R_Shoulder;
        if (Input.GetButtonDown(mButtonNames[8]))
            button = XBoxButton.L_Thumb;
        if (Input.GetButtonDown(mButtonNames[9]))
            button = XBoxButton.R_Thumb;

        return button;
    }

    /// <summary>
    /// The checking function for analogue control to the dropping blocks 
    /// navigation.
    /// It return the vector value of any subscribed analogue control has the
    /// value
    /// </summary>
    /// <returns>Return of the vector value</returns>
    private Vector2Int BlockNavigationWithAnalogue()
    {
        Vector2Int? navi = GameBlockNavigation?.Invoke();
        return navi.Value;
    }

    /// <summary>
    /// Returns true if the command index dictionary contain the request the command
    /// and during the frame the user pressed down the button
    /// </summary>
    /// <param name="aCommand"></param>
    /// <returns></returns>
    private bool ButtonPressed(CommandIndex aCommand)
    {
        if (!mCommands.ContainsKey(aCommand))
            return false;

        if (mCommands[aCommand].Type == GP_InputType.AXIS)
            return GetAxisAsButton(aCommand);

        string buttonName = mButtonIndex[mCommands[aCommand].Button];
        return Input.GetButtonDown(buttonName);
    }

    /// <summary>
    /// Returns true if the command index dictionary contain the request the command
    /// and while the button is held down.
    /// </summary>
    /// <param name="aCommand"></param>
    /// <returns>True when the butten to the requested command has been pressed and not released.</returns>
    private bool ButtonDown(CommandIndex aCommand)
    {
        if (!mCommands.ContainsKey(aCommand))
            return false;

        if (mCommands[aCommand].Type == GP_InputType.AXIS)
            return GetAxisAsButton(aCommand);

        string buttonName = mButtonIndex[mCommands[aCommand].Button];
        return Input.GetButton(buttonName);
    }

    // This will be updateed with AxisInput
    private bool GetAxisAsButton(CommandIndex aCommand)
    {
        if (AxisSwitch(aCommand) == mCommands[aCommand].Direction)
            return true;

        return false;
    }

    private bool AxisButtonHit(CommandIndex aCommand)
    {
        //Vector2Int? dir = enableAnalogNavigator?.Invoke();
        //enableAnalogNavigator.GetInvocationList()
        //if(dir.HasValue)

        //enableAnalogNavigator.GetInvocationList()[0].Method.Name
        return false;
    }

    private bool CheckNaviCommandsWithTimer(ControlInput anInput, float anDelayIntervall)
    {
        mCurrentMenuNavigateDireciton = GuiNavigation.Invoke();
        if ((mCurrentMenuNavigateDireciton == anInput.Direction) && 
            mMenuNavigationSuppressTimer <= 0f)
        {
            mMenuNavigationSuppressTimer = anDelayIntervall;
            return true;
        }

        if(mCurrentMenuNavigateDireciton == Vector2Int.zero)
            mMenuNavigationSuppressTimer = 0;

        if (mMenuNavigationSuppressTimer > 0f)
            mMenuNavigationSuppressTimer -= Time.deltaTime;

        return false;
    }

    private bool CheckNaviCommandsWithPreviousDirection(ControlInput anInput)
    {
        Vector2Int? navi = GuiNavigation?.Invoke();
        if (navi.HasValue)
            mCurrentMenuNavigateDireciton = (Vector2Int)navi;


        if (CheckThisMoves(mCurrentMenuNavigateDireciton))
        {
            if ((mLastInput != null) && (mCurrentMenuNavigateDireciton == mLastInput.Direction))
                return false;
            else if ((mLastInput == null) && (mCurrentMenuNavigateDireciton == anInput.Direction))
            {
                mLastInput = anInput;
                return true;
            }
        }
       
        mLastInput = null;

        return false;
    }

    private Vector2Int AxisSwitch(CommandIndex aCommand)
    {
        Vector2Int dir = Vector2Int.zero;

        switch (mCommands[aCommand].AxisType)
        {
            case AxisInput.L_STICK:
                dir = LeftStick();
                break;
            case AxisInput.TRIGGER:
                dir = Trigger();
                break;
            case AxisInput.R_STICK:
                dir = RightStick();
                break;
            case AxisInput.D_PAD:
                dir = DPad();
                break;
        }
        return Vector2Int.zero;
    }

    /// <summary>
    /// Currently is for test if the guiNavigation delegate works
    /// </summary>
    /// <returns>If any of the subscribed delegate had moved</returns>
    private bool GuiNavigatorMoved(ref Vector2Int aDir)
    {
        Vector2Int? navi = GuiNavigation?.Invoke();
        if (navi.HasValue)
            aDir = (Vector2Int)navi;
        if (aDir.x <= -1 || aDir.y <= -1 || aDir.x >= 1 || aDir.y >= 1)
            return true;
        return false;
    }

    // this might be removed later and replace by each type of navigat moved method
    // It'll be used for a moment
    private bool GetAxisInput(AxisInput theCheckingAxis)
    {
        if (theCheckingAxis == AxisInput.L_STICK)
            return CheckThisMoves(LeftStick());
        else if (theCheckingAxis == AxisInput.R_STICK)
            return CheckThisMoves(RightStick());
        else if (theCheckingAxis == AxisInput.D_PAD)
            return CheckThisMoves(DPad());
        return false;
    }

    private bool CheckThisMoves(Vector2Int checkingAxisVector)
    {
        if (checkingAxisVector.x <= -1 || checkingAxisVector.y <= -1 || checkingAxisVector.x >= 1 || checkingAxisVector.y >= 1)
            return true;
        return false;
    }

    private Vector2Int GetDirectionFromAxis(AxisInput aSelectedAxis)
    {
        Vector2Int returnDirection = Vector2Int.zero;
        switch (aSelectedAxis)
        {
            case AxisInput.TRIGGER:
                returnDirection = Trigger();//new Vector2Int(Trigger(), 0);
                break;
            case AxisInput.R_STICK:
                returnDirection = RightStick();
                break;
            case AxisInput.D_PAD:
                returnDirection = DPad();
                break;
            default:
                returnDirection = LeftStick();
                break;
        }
        return returnDirection;
    }

    private Vector2Int LeftStick() { return new Vector2Int((int)Input.GetAxis(mAxisNames[0]), (int)Input.GetAxis(mAxisNames[1])); }

    private Vector2Int Trigger() { return new Vector2Int((int)Input.GetAxis(mAxisNames[2]), 0); }

    private Vector2Int RightStick() { return new Vector2Int((int)Input.GetAxis(mAxisNames[3]), (int)Input.GetAxis(mAxisNames[4])); }

    private Vector2Int DPad() { return new Vector2Int(Mathf.RoundToInt(Input.GetAxis(mAxisNames[5])), Mathf.RoundToInt(Input.GetAxis(mAxisNames[6]))); }

    /// <summary>
    /// Test for the subscribtion of the analogue controls of left stick
    /// Might replace the four function from above
    /// </summary>
    /// <param name="aType">Requesting of navigatetype</param>
    /// <returns>return the navigation value</returns>
    private Vector2Int LeftStickNew(NavigatorType aType)
    {
        Vector2Int returnVector = new Vector2Int((int)Input.GetAxis(mAxisNames[0]), (int)Input.GetAxis(mAxisNames[1]));
        SetLastNavigationTo(aType, AxisInput.L_STICK, ref returnVector);
        return returnVector;
    }

    /// <summary>
    /// Test for the subscribtion of the analogue controls of triggers
    /// Might replace the four function from above
    /// </summary>
    /// <param name="aType">Requesting of navigatetype</param>
    /// <returns>return the navigation value</returns>
    private Vector2Int TriggerNew(NavigatorType aType)
    {
        Vector2Int returnVector = new Vector2Int((int)Input.GetAxis(mAxisNames[2]), 0);
        SetLastNavigationTo(aType, AxisInput.TRIGGER, ref returnVector);
        return returnVector;
    }

    /// <summary>
    /// Test for the subscribtion of the analogue controls of right stick
    /// Might replace the four function from above
    /// </summary>
    /// <param name="aType">Requesting of navigatetype</param>
    /// <returns>return the navigation value</returns>
    private Vector2Int RightStickNew(NavigatorType aType)
    {
        Vector2Int returnVector = new Vector2Int((int)Input.GetAxis(mAxisNames[3]), (int)Input.GetAxis(mAxisNames[4]));
        SetLastNavigationTo(aType, AxisInput.R_STICK, ref returnVector);
        return returnVector;
    }

    /// <summary>
    /// Test for the subscribtion of the analogue controls of dpad
    /// Might replace the four function from above
    /// </summary>
    /// <param name="aType">Requesting of navigatetype</param>
    /// <returns>return the navigation value</returns>
    private Vector2Int DPadNew(NavigatorType aType)
    {
        Vector2Int returnVector = new Vector2Int(Mathf.RoundToInt(Input.GetAxis(mAxisNames[5])), Mathf.RoundToInt(Input.GetAxis(mAxisNames[6])));
        SetLastNavigationTo(aType, AxisInput.D_PAD, ref returnVector);
        return returnVector;
    }

    /// <summary>
    /// Store the last navigation stick been pulled and reset when condition of the stick is zero
    /// </summary>
    /// <param name="aType">Reqeusting checking navigation type from the dictionary with stored the last pulled axisInput</param>
    /// <param name="aPullingAxis">The current pulled axisInput</param>
    /// <param name="aVector">returning vector value of the pulling axisInput that been pulled</param>
    private void SetLastNavigationTo(NavigatorType aType, AxisInput aPullingAxis,  ref Vector2Int aVector)
    {
        if (mLastPulledAxis[aType] == aPullingAxis && aVector == Vector2Int.zero)
        {
            mCurrentActiveNavigation[aType] = Vector2Int.zero;
            mLastPulledAxis[aType] = AxisInput.NONE;
        }
        else if (mCurrentActiveNavigation[aType] != Vector2Int.zero)
            aVector = mCurrentActiveNavigation[aType];
        else
        {
            mCurrentActiveNavigation[aType] = aVector;
            mLastPulledAxis[aType] = aPullingAxis;
        }
    }
}

public class ControlInput
{
    private readonly XBoxButton mButtonIndex = XBoxButton.NONE;
    public XBoxButton Button { get { return mButtonIndex; } }

    private readonly GP_InputType mInputType = GP_InputType.NONE;
    public GP_InputType Type { get { return mInputType; } }

    private readonly AxisInput mAxisType = AxisInput.NONE;
    public AxisInput AxisType { get { return mAxisType; } }

    private readonly Vector2Int mAxisDirection = Vector2Int.zero;
    public Vector2Int Direction { get { return mAxisDirection; } }

    private readonly string mAxisName = "";
    public string AxisName { get { return mAxisName; } }

    public ControlInput(XBoxButton anIndex)
    {
        mInputType = GP_InputType.BUTTON;
        mButtonIndex = anIndex;
    }

    public ControlInput(AxisInput anAxis, XBoxButton anIndex, Vector2Int aDirection)
    {
        mInputType = GP_InputType.AXIS;
        mAxisType = anAxis;
        mButtonIndex = anIndex;
        mAxisDirection = aDirection;

        switch(anAxis)
        {
            case AxisInput.L_STICK:
                mAxisName = "LeftStick";
                break;
            case AxisInput.R_STICK:
                mAxisName = "RightStick";
                break;
            case AxisInput.TRIGGER:
                mAxisName = "Trigger";
                break;
            case AxisInput.D_PAD:
                mAxisName = "DPad";
                break;
        }
    }

    public bool AxisMatched(AxisInput theAxis, Vector2Int aDirection)
    {
        if (theAxis == mAxisType && mAxisDirection == aDirection)
            return true;
        return false;
    }
}

public class GamePadStat
{
    private readonly XBoxButton mButton;
    public XBoxButton Button { get { return mButton; } }

    private readonly bool mButtonIsPressed;
    public bool ButtonIsPressed { get { return mButtonIsPressed; } }
    public Vector2Int Direction { get; }

    public GamePadStat(XBoxButton aButton, bool isPressed)
    {
        mButton = aButton;

        if (isPressed)
            mButtonIsPressed = isPressed;
        else
            mButtonIsPressed = false;

        if (aButton == XBoxButton.L_Thumb_Up || aButton == XBoxButton.R_Thumb_Up || aButton == XBoxButton.DPad_Up)
            Direction = Vector2Int.up;
        else if (aButton == XBoxButton.L_Thumb_Right || aButton == XBoxButton.R_Thumb_Right || aButton == XBoxButton.DPad_Right)
            Direction = Vector2Int.right;
        else if (aButton == XBoxButton.L_Thumb_Down || aButton == XBoxButton.R_Thumb_Down || aButton == XBoxButton.DPad_Down)
            Direction = Vector2Int.down;
        else if (aButton == XBoxButton.L_Thumb_Left || aButton == XBoxButton.R_Thumb_Left || aButton == XBoxButton.DPad_Left)
            Direction = Vector2Int.left;
        else
            Direction = Vector2Int.zero;
    }
    
}