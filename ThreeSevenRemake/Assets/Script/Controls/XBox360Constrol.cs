
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum XBoxButton
{
    A, B, X, Y,
    L_SHOULDER, R_SHOULDER,
    BACK, START,
    L_THUMB_PRESSED, R_THUMB_PRESSED,
    L_THUMB_UP, L_THUMB_RIGHT, L_THUMB_DOWN, L_THUMB_LEFT,
    L_TRIGGER, R_TRIGGER,
    R_THUMB_UP, R_THUMB_RIGHT, R_THUMB_DOWN, R_THUMB_LEFT,
    DPAD_UP, DPAD_RIGHT, DPAD_DOWN, DPAD_LEFT,
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

    private readonly List<string> mButtonNames;
    private readonly List<string> mAxisNames;
    private readonly Dictionary<XBoxButton, string> mButtonIndex = new Dictionary<XBoxButton, string>();
    private readonly Dictionary<CommandIndex, ControlInput> mCommands = new Dictionary<CommandIndex, ControlInput>();
    private readonly Dictionary<AxisInput, bool> mEnableNavigationSticks = new Dictionary<AxisInput, bool>();

    private Vector2Int mCurrentMenuNavigateDireciton = Vector2Int.zero;
    private Vector2Int mLastNavigateAxisDirection = Vector2Int.zero;
    private float mMenuNavigationSuppressTimer = 0f;

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

        mButtonIndex = new Dictionary<XBoxButton, string>()
        {
            { XBoxButton.A, mButtonNames[(int)XBoxButton.A] },
            { XBoxButton.B, mButtonNames[(int)XBoxButton.B] },
            { XBoxButton.X, mButtonNames[(int)XBoxButton.X] },
            { XBoxButton.Y, mButtonNames[(int)XBoxButton.Y] },
            { XBoxButton.L_SHOULDER, mButtonNames[(int)XBoxButton.L_SHOULDER] },
            { XBoxButton.R_SHOULDER, mButtonNames[(int)XBoxButton.R_SHOULDER] },
            { XBoxButton.BACK, mButtonNames[(int)XBoxButton.BACK] },
            { XBoxButton.START, mButtonNames[(int)XBoxButton.START] },
            { XBoxButton.L_THUMB_PRESSED, mButtonNames[(int)XBoxButton.L_THUMB_PRESSED] },
            { XBoxButton.R_THUMB_PRESSED, mButtonNames[(int)XBoxButton.R_THUMB_PRESSED] },
        };

        mEnableNavigationSticks = new Dictionary<AxisInput, bool>()
        {
            { AxisInput.L_STICK, true },
            { AxisInput.R_STICK, false },
            { AxisInput.D_PAD, true }
        };
    }

    public override void KeySettings(/*Dictionary<CommandIndex, object> someSetting*/)
    {
        Dictionary<CommandIndex, ControlInput> defaultSets = new Dictionary<CommandIndex, ControlInput>
        {
            // navigation
            { CommandIndex.NAVI_LEFT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_THUMB_LEFT, Vector2Int.left) },
            { CommandIndex.NAVI_RIGHT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_THUMB_RIGHT, Vector2Int.right) },
            { CommandIndex.NAVI_DOWN, new ControlInput(AxisInput.L_STICK, XBoxButton.L_THUMB_DOWN, Vector2Int.down) },
            { CommandIndex.NAVI_UP, new ControlInput(AxisInput.L_STICK, XBoxButton.L_THUMB_UP, Vector2Int.up) },
            { CommandIndex.SELECT, new ControlInput(XBoxButton.A) },
            { CommandIndex.CANCEL, new ControlInput(XBoxButton.B) },
            { CommandIndex.CONFIRM, new ControlInput(XBoxButton.START) },
            { CommandIndex.BACK, new ControlInput(XBoxButton.BACK) },
            // gameplay
            { CommandIndex.BLOCK_MOVE_LEFT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_THUMB_LEFT, Vector2Int.left) },
            { CommandIndex.BLOCK_MOVE_RIGHT, new ControlInput(AxisInput.L_STICK, XBoxButton.L_THUMB_RIGHT, Vector2Int.right) },
            { CommandIndex.BLOCK_DROP, new ControlInput(AxisInput.L_STICK, XBoxButton.L_THUMB_DOWN, Vector2Int.down) },
            { CommandIndex.BLOCK_INSTANT_DROP, new ControlInput(XBoxButton.A) },
            { CommandIndex.BLOCK_ROTATE, new ControlInput(XBoxButton.X) },
            { CommandIndex.BLOCK_INVERT, new ControlInput(XBoxButton.Y) },
            { CommandIndex.PREVIEW_SWAP, new ControlInput(XBoxButton.B) },
            { CommandIndex.PREVIEW_ROTATE, new ControlInput(XBoxButton.R_SHOULDER) },
            { CommandIndex.PREVIEW_DUMP, new ControlInput(XBoxButton.L_SHOULDER) },//new ControlInput(AxisInput.TRIGGER, XBoxButton.R_TRIGGER, Vector2Int.right) },
            { CommandIndex.INGAME_PAUSE, new ControlInput(XBoxButton.START) }
        };

        foreach (CommandIndex com in defaultSets.Keys)
            mCommands[com] = (ControlInput)defaultSets[com];
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
        float vertical = ((Navigation().x > -1 && Navigation().x < 1) ? Navigation().y : 0f);
        if ((vertical <= -1f && DropButtonTimePassed())/* || Time.time > aBlockNextDropTime*/)
        {
            ResetDropTimer();
            return true;
        }
        return false;
    }

    //public override bool GameDumpPreview()
    //{
    //    if (Trigger() == mCommands[CommandIndex.PREVIEW_DUMP].Direction.x)
    //        return true;
    //    return false;
    //}

    protected override bool HorizontBottomHit(ref Vector3 aDir, float aHorizontValue = 0f)
    {
        //Vector2Int navi = Navigation();
        //if ((navi.x <= -1 || navi.x >= 1))
        //{
        //    if (!SupressHorizontMove())
        //        aDir = new Vector3(navi.x, 0, 0);
        //    else
        //        aDir = Vector3.zero;

        //    if(mCurrentHorizontDirection != (int)navi.x)
        //    {
        //        mCurrentHorizontDirection = (int)navi.x;
        //        mHorizontSurpressTimer = 0f;
        //    }


        //    mHorizontSurpressTimer += Time.deltaTime * mIncreaseDeltaTimeConst;
        //    return true;
        //}


        return base.HorizontBottomHit(ref aDir, Navigation().x);
    }

    public override bool KeyDown(CommandIndex aCommand) { return ButtonDown(aCommand); }

    public override bool KeyPress(CommandIndex aCommand) { return ButtonPressed(aCommand); }

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

    private bool GetAxisAsButton(CommandIndex aCommand)
    {
        if (AxisSwitch(aCommand) == mCommands[aCommand].Direction)
            return true;

        return false;
    }

    private bool CheckNaviCommandsWithTimer(ControlInput anInput, float anDelayIntervall)
    {
        if (GetAxisInput(anInput.AxisType) &&
            (GetDirectionFromAxis(anInput.AxisType) == anInput.Direction) &&
            mMenuNavigationSuppressTimer <= 0f)
        {
            mCurrentMenuNavigateDireciton = anInput.Direction;
            mMenuNavigationSuppressTimer = anDelayIntervall;//.1f;//Constants.BUTTON_DOWN_INTERVAL;
            return true;
        }

        if (GetDirectionFromAxis(anInput.AxisType) == Vector2Int.zero)
            mMenuNavigationSuppressTimer = 0;

        if (mMenuNavigationSuppressTimer > 0f)
            mMenuNavigationSuppressTimer -= Time.deltaTime;  // anDelayIntervall;

        return false;
    }

    private bool CheckNaviCommandsWithPreviousDirection(ControlInput anInput)
    {
        mCurrentMenuNavigateDireciton = GetDirectionFromAxis(anInput.AxisType);
        //Debug.Log(anInput.Button);
        if (GetAxisInput(anInput.AxisType))
        {
            if ((mLastInput != null) && (mCurrentMenuNavigateDireciton == mLastInput.Direction))
                return false;
            else if ((mLastInput == null) && (mCurrentMenuNavigateDireciton == anInput.Direction))
            {
                mLastInput = anInput;
                return true;
            }
        }
        //if (GetAxisInput(anInput.AxisType) && (mCurrentMenuNavigateDireciton == mLastNavigateAxisDirection))
        //{
        //    return false;
        //}
        //else if (GetAxisInput(anInput.AxisType) && (mCurrentMenuNavigateDireciton == anInput.Direction))
        //{
        //    mLastNavigateAxisDirection = mCurrentMenuNavigateDireciton;
        //    return true;
        //}

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
                dir.x = Trigger();
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

    private bool GetAxisInput(AxisInput theCheckingAxis)
    {
        if (theCheckingAxis == AxisInput.L_STICK)
            return CheckThisStickMoves(LeftStick());
        else if (theCheckingAxis == AxisInput.R_STICK)
            return CheckThisStickMoves(RightStick());
        else if (theCheckingAxis == AxisInput.D_PAD)
            return CheckThisStickMoves(DPad());
        return false;
    }

    private bool CheckAxisEnableToMove(AxisInput anAxis, Vector2Int theCheckingStick)
    {
        if (mEnableNavigationSticks[anAxis])
        {
            //if (theCheckingStick.x <= -1 || theCheckingStick.y <= -1 || theCheckingStick.x >= 1 || theCheckingStick.y >= 1)
            //    return true;
            return CheckThisStickMoves(theCheckingStick);
        }

        return false;
    }

    private bool CheckThisStickMoves(Vector2Int theCheckStick)
    {
        if (theCheckStick.x <= -1 || theCheckStick.y <= -1 || theCheckStick.x >= 1 || theCheckStick.y >= 1)
            return true;
        return false;
    }

    private Vector2Int GetDirectionFromAxis(AxisInput aSelectedAxis)
    {
        Vector2Int returnDirection = Vector2Int.zero;
        switch (aSelectedAxis)
        {
            case AxisInput.TRIGGER:
                returnDirection = new Vector2Int(Trigger(), 0);
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

    private int Trigger() { return (int)Input.GetAxis(mAxisNames[2]); }

    private Vector2Int RightStick() { return new Vector2Int((int)Input.GetAxis(mAxisNames[3]), (int)Input.GetAxis(mAxisNames[4])); }

    private Vector2Int DPad() { return new Vector2Int(Mathf.RoundToInt(Input.GetAxis(mAxisNames[5])), Mathf.RoundToInt(Input.GetAxis(mAxisNames[6]))); }
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
    }

    public bool AxisMatched(AxisInput theAxis, Vector2Int aDirection)
    {
        if (theAxis == mAxisType && mAxisDirection == aDirection)
            return true;
        return false;
    }
}