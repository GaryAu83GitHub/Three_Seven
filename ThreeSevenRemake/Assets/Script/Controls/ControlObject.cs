using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum CommandIndex
{
    // navigation
    NAVI_LEFT,
    NAVI_RIGHT,
    NAVI_DOWN,
    NAVI_UP,
    SELECT,
    CANCEL,
    CONFIRM,
    BACK,
    // gameplay
    BLOCK_MOVE_LEFT,
    BLOCK_MOVE_RIGHT,
    BLOCK_DROP,
    BLOCK_INSTANT_DROP,
    BLOCK_ROTATE,
    BLOCK_INVERT,
    PREVIEW_SWAP,
    PREVIEW_ROTATE,
    PREVIEW_DUMP,
    INGAME_PAUSE,
    // last index
    MAX_INPUT
}

public enum ControlType
{
    KEYBOARD,
    XBOX_360,
    PS4_DUALSHOCK
}

public class ControlObject
{
    protected ControlType mType = ControlType.KEYBOARD;
    public ControlType Type { get { return mType; } }

    //protected Dictionary<CommandIndex, object> mKeybindList = new Dictionary<CommandIndex, object>();

    protected float mBlockDropButtonDelayTime = 0f;
    protected float mBlockMoveHorizontButtonDelayTime = 0f;

    protected float mHorizontSurpressTimer = 0f;
    protected int mCurrentHorizontDirection = 0;

    protected const int mIncreaseDeltaTimeConst = 10;

    public ControlObject()
    { }

    public virtual void UpdateControl()
    {

    }

    public virtual void KeySettings(/*Dictionary<CommandIndex, object> someSetting*/)
    {
        //mKeybindList = new Dictionary<CommandIndex, object>(someSetting);
    }

    public virtual bool MenuNavigateHold(CommandIndex aCommand, float anDelayIntervall)
    {
        return false;
    }

    public virtual bool MenuNavigatePress(CommandIndex aCommand)
    {
        return false;
    }

    public virtual bool MenuSelect() { return KeyPress(CommandIndex.SELECT); }

    public virtual bool MenuCancel() { return KeyPress(CommandIndex.CANCEL); }

    public virtual bool MenuConfirm() { return KeyPress(CommandIndex.CONFIRM); }

    public virtual bool MenuBack() { return KeyPress(CommandIndex.BACK); }

    public Vector3 GameMoveBlockHorizontal()
    {
        if (!MoveHorizontButtonTimePassed())
            return Vector3.zero;

        Vector3 dir = Vector3.zero;
        if (HorizontBottomHit(ref dir))
            ResetMoveHorizontTimer();

        return dir;
    }

    public virtual bool GameDropBlockGradually(float aBlockNextDropTime)
    {
        if ((KeyDown(CommandIndex.BLOCK_DROP) && DropButtonTimePassed())/* || Time.time > aBlockNextDropTime*/)
        {
            ResetDropTimer();
            return true;
        }
        return false;
    }

    public virtual bool GameInstantBlockDrop() { return KeyPress(CommandIndex.BLOCK_INSTANT_DROP); }

    public virtual bool GameRotateBlock() { return KeyPress(CommandIndex.BLOCK_ROTATE); }

    public virtual bool GameInverteBlock() { return KeyPress(CommandIndex.BLOCK_INVERT); }

    public virtual bool GameSwapPreview() { return KeyPress(CommandIndex.PREVIEW_SWAP); }

    public virtual bool GameRotatePreview() { return KeyPress(CommandIndex.PREVIEW_ROTATE); }

    public virtual bool GameDumpPreview() { return KeyPress(CommandIndex.PREVIEW_DUMP); }

    public virtual bool GamePause() {
        return KeyPress(CommandIndex.INGAME_PAUSE);
    }

    protected virtual bool HorizontBottomHit(ref Vector3 aDir, float aHorizontValue = 0f)
    {
        return MoveHorizontal(ref aDir, aHorizontValue);
    }

    protected virtual bool MoveHorizontal(ref Vector3 aDir, float aHorizontValue)
    {
        if ((aHorizontValue <= -1 || aHorizontValue >= 1))
        {
            if (!SupressHorizontMove())
                aDir = new Vector3(aHorizontValue, 0, 0);
            else
                aDir = Vector3.zero;

            if (mCurrentHorizontDirection != (int)aHorizontValue)
            {
                mCurrentHorizontDirection = (int)aHorizontValue;
                mHorizontSurpressTimer = 0f;
            }

            mHorizontSurpressTimer += Time.deltaTime * mIncreaseDeltaTimeConst;
            return true;
        }
        mHorizontSurpressTimer = 0f;
        return false;
    }

    public void ResetButtonPressTimer()
    {
        ResetDropTimer();
        ResetMoveHorizontTimer();
    }

    /// <summary>
    /// Clicking event
    /// </summary>
    /// <param name="aCommand">The requesting command</param>
    /// <returns>true if the command was registrate and was pressed</returns>
    public virtual bool KeyDown(CommandIndex aCommand)
    {
        return false;
        //if (!mKeybindList.ContainsKey(aCommand))
        //    return false;

        //return Input.GetKey(mKeybindList[aCommand]);
    }

    /// <summary>
    /// Keyhold event
    /// </summary>
    /// <param name="aCommand">The requesting command</param>
    /// <returns>true if the command was registrate and is holding down</returns>
    public virtual bool KeyPress(CommandIndex aCommand)
    {
        return false;
        //if (!mKeybindList.ContainsKey(aCommand))
        //    return false;

        //return Input.GetKeyDown(mKeybindList[aCommand]);
    }


    /// <summary>
    /// Reset dropp button's delay intervall timer
    /// </summary>
    protected void ResetDropTimer()
    {
        //mBlockDropButtonDelayTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;
        mBlockDropButtonDelayTime = Constants.BUTTON_DOWN_INTERVAL;
    }

    /// <summary>
    /// Check if the time had for the command of dropping to pass
    /// </summary>
    /// <returns>return true if the time had passed the delaying time</returns>
    protected bool DropButtonTimePassed()
    {
        //return (Time.time > mBlockDropButtonDelayTime);
        if (mBlockDropButtonDelayTime > 0)
            mBlockDropButtonDelayTime -= Time.deltaTime;

        return (mBlockDropButtonDelayTime <= 0);
    }

    protected void ResetMoveHorizontTimer()
    {
        mBlockMoveHorizontButtonDelayTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;
    }

    protected bool MoveHorizontButtonTimePassed()
    {
        return (Time.time > mBlockMoveHorizontButtonDelayTime);
    }

    protected bool SupressHorizontMove()
    {
        return (mHorizontSurpressTimer > 0f && mHorizontSurpressTimer < .5f);
    }
}