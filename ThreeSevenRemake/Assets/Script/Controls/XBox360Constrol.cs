﻿
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
    private event OnGUINavigation guiNavigation;

    private delegate Vector2Int OnGameBlockNavigation(NavigatorType aType = NavigatorType.BLOCK_NAVIGATOR);
    private event OnGameBlockNavigation gameBlockNavigation;

    private delegate Vector2Int OnGamePowerUpNavigation(NavigatorType aType = NavigatorType.POWER_UP_NAVIGATOR);
    private event OnGamePowerUpNavigation gamePowerUpNavigation;

    private delegate Vector2Int OnGameEnableAnalogNavigator();
    private event OnGameEnableAnalogNavigator enableAnalogNavigator;

    private readonly List<string> mButtonNames = new List<string>();
    public static List<string> XboxButtonNames = new List<string>();

    private readonly List<string> mAxisNames;
    public static List<string> XboxAxisNames = new List<string>();
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

        mButtonNames = new List<string>()
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
        for (int i = 0; i < mButtonNames.Count; i++)
            XboxButtonNames.Add(mButtonNames[i]);

        mAxisNames = new List<string>()
        {
            "L_Stick_Hori_Axis",
            "L_Stick_Vert_Axis",
            "Trigger_Axis",
            "R_Stick_Hori_Axis",
            "R_Stick_Vert_Axis",
            "DPad_Hori_Axis",
            "DPad_Vert_Axis",
        };
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

        mEnableNavigationSticks = new Dictionary<AxisInput, bool>()
        {
            { AxisInput.L_STICK, true },
            { AxisInput.R_STICK, false },
            { AxisInput.D_PAD, true }
        };

        guiNavigation += LeftStick;
        //guiNavigation += DPad;

        //System.Delegate[] test = guiNavigation.GetInvocationList();
        //test[0].Method.Name;

        gameBlockNavigation += LeftStickNew;
        gameBlockNavigation += DPadNew;
        //gameBlockNavigation += TriggerNew;


        //gamePowerUpNavigation += RightStickNew;
        gamePowerUpNavigation += TriggerNew;

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


        if(enableAnalogNavigator != null)
        {
            System.Delegate[] clientList = enableAnalogNavigator.GetInvocationList();
            foreach (var d in clientList)
                enableAnalogNavigator -= (d as OnGameEnableAnalogNavigator);
        }

        foreach (CommandIndex com in defaultSets.Keys)
        {
            mCommands[com] = (ControlInput)defaultSets[com];
            if(mCommands[com].AxisType != AxisInput.NONE)
            {
                if (enableAnalogNavigator != null)
                {
                    if (enableAnalogNavigator.GetInvocationList().ToList().Any(x => x.Method.Name == mCommands[com].AxisName))//;.Method.Name == mCommands[com].AxisName)
                        continue;
                }
                switch (mCommands[com].AxisType)
                {
                    case AxisInput.L_STICK:
                        enableAnalogNavigator += LeftStick;
                        break;
                    case AxisInput.TRIGGER:
                        enableAnalogNavigator += Trigger;
                        break;
                    case AxisInput.R_STICK:
                        enableAnalogNavigator += RightStick;
                        break;
                    case AxisInput.D_PAD:
                        enableAnalogNavigator += DPad;
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
                if (gameBlockNavigation.GetInvocationList().Count() > 0)
                {
                    System.Delegate[] clientList = gameBlockNavigation.GetInvocationList();
                    foreach (var d in clientList)
                        gameBlockNavigation -= (d as OnGameBlockNavigation);
                }

                switch(someNewBinding[navi].BindingAxis)
                {
                    case AxisInput.L_STICK:
                        gameBlockNavigation += LeftStickNew;
                        break;
                    case AxisInput.R_STICK:
                        gameBlockNavigation += RightStickNew;
                        break;
                    case AxisInput.D_PAD:
                        gameBlockNavigation += DPadNew;
                        break;
                }

            }
            if(navi == NavigatorType.POWER_UP_NAVIGATOR)
            {
                if (gamePowerUpNavigation.GetInvocationList().Count() > 0)
                {
                    System.Delegate[] clientList = gamePowerUpNavigation.GetInvocationList();
                    foreach (var d in clientList)
                        gamePowerUpNavigation -= (d as OnGamePowerUpNavigation);
                }

                switch (someNewBinding[navi].BindingAxis)
                {
                    case AxisInput.L_STICK:
                        gamePowerUpNavigation += LeftStickNew;
                        break;
                    case AxisInput.R_STICK:
                        gamePowerUpNavigation += RightStickNew;
                        break;
                    case AxisInput.D_PAD:
                        gamePowerUpNavigation += DPadNew;
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
        //float vertical = ((Navigation().x > -1 && Navigation().x < 1) ? Navigation().y : 0f);
        float vertical = ((NavigationNew().x > -1 && NavigationNew().x < 1) ? NavigationNew().y : 0f);
        if ((vertical <= -1f && DropButtonTimePassed()))
        {
            ResetDropTimer();
            return true;
        }
        return false;
    }

    protected override bool HorizontBottomHit(ref Vector3 aDir, float aHorizontValue = 0f)
    {
        //return base.HorizontBottomHit(ref aDir, Navigation().x);
        return base.HorizontBottomHit(ref aDir, NavigationNew().x);
    }

    public override bool KeyDown(CommandIndex aCommand) { return ButtonDown(aCommand); }

    public override bool KeyPress(CommandIndex aCommand) { return ButtonPressed(aCommand); }

    public override int GameMovePowerUpSelection()
    {
        Vector2Int? dir = gamePowerUpNavigation?.Invoke(NavigatorType.POWER_UP_NAVIGATOR);
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

    public void GetButtonFor(CommandIndex aCommand, ref XBoxButton aButton)
    {
        aButton = mCommands[aCommand].Button;
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
    /// Return the vector value after have checked any of the stick is enable
    /// The new Navigation method will be using delegate method to subscribe
    /// the navigate sticks method, so this Navigation method might be replace
    /// in the future
    /// </summary>
    /// <returns></returns>
    private Vector2Int Navigation()
    {
        Vector2Int navi = Vector2Int.zero;

        if (CheckAxisEnableToMove(AxisInput.L_STICK, LeftStick()))
            navi = LeftStick();
        else if (CheckAxisEnableToMove(AxisInput.R_STICK, RightStick()))
            navi = RightStick();
        else if (CheckAxisEnableToMove(AxisInput.D_PAD, DPad()))
            navi = DPad();

        return navi;
    }

    private Vector2Int NavigationNew()
    {
        Vector2Int? navi = gameBlockNavigation?.Invoke();
        return navi.Value;
    }

    private bool ButtonPressed(CommandIndex aCommand)
    {
        if (!mCommands.ContainsKey(aCommand))
            return false;

        if (mCommands[aCommand].Type == GP_InputType.AXIS)
            return GetAxisAsButton(aCommand);

        string buttonName = mButtonIndex[mCommands[aCommand].Button];
        return Input.GetButtonDown(buttonName);
    }

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
        mCurrentMenuNavigateDireciton = guiNavigation.Invoke();
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
        Vector2Int? navi = guiNavigation?.Invoke();
        if (navi.HasValue)
            mCurrentMenuNavigateDireciton = (Vector2Int)navi;
        //mCurrentMenuNavigateDireciton = GetDirectionFromAxis(anInput.AxisType);
        if (CheckThisMovesMoves(mCurrentMenuNavigateDireciton)/*GetAxisInput(anInput.AxisType)*/)
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
        Vector2Int? navi = guiNavigation?.Invoke();
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
            return CheckThisMovesMoves(LeftStick());
        else if (theCheckingAxis == AxisInput.R_STICK)
            return CheckThisMovesMoves(RightStick());
        else if (theCheckingAxis == AxisInput.D_PAD)
            return CheckThisMovesMoves(DPad());
        return false;
    }

    private bool CheckAxisEnableToMove(AxisInput anAxis, Vector2Int theCheckingStick)
    {
        if (mEnableNavigationSticks[anAxis])
            return CheckThisMovesMoves(theCheckingStick);

        return false;
    }

    private bool CheckThisMovesMoves(Vector2Int checkingAxisVector)
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

        //if (aType == NavigatorType.BLOCK_NAVIGATOR)
        //{
        //    if (mLastPulledBlockNavigateAxis == aPullingAxis && aVector == Vector2Int.zero)
        //    {
        //        mCurrentBlockNavigation = Vector2Int.zero;
        //        mLastPulledBlockNavigateAxis = AxisInput.NONE;
        //    }
        //    else if (mCurrentBlockNavigation != Vector2Int.zero)
        //        aVector = mCurrentBlockNavigation;
        //    else
        //    {
        //        mCurrentBlockNavigation = aVector;
        //        mLastPulledBlockNavigateAxis = aPullingAxis;
        //    }
        //}
        //else if (aType == NavigatorType.POWER_UP_NAVIGATOR)
        //{
        //    if (mLastPulledPowerSelectNavigateAxis == aPullingAxis && aVector == Vector2Int.zero)
        //    {
        //        mCurrentPowerUpNavigation = Vector2Int.zero;
        //        mLastPulledPowerSelectNavigateAxis = AxisInput.NONE;
        //    }
        //    else if (mCurrentPowerUpNavigation != Vector2Int.zero)
        //        aVector = mCurrentPowerUpNavigation;
        //    else
        //    {
        //        mCurrentPowerUpNavigation = aVector;
        //        mLastPulledPowerSelectNavigateAxis = aPullingAxis;
        //    }
        //}
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