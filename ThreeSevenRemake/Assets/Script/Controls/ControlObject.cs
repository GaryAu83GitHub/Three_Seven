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
    POWER_UP_USE,
    POWER_UP_NAVI_LEFT,
    POWER_UP_NAVI_RIGHT,
    PREVIEW_SWAP,
    PREVIEW_ROTATE,
    PREVIEW_DUMP,
    INGAME_PAUSE,
    // last index
    MAX_INPUT
}

public enum NavigatorType
{
    BLOCK_NAVIGATOR,
    POWER_UP_NAVIGATOR,
    GAME_PAD_TESTING,
    NONE
}

public enum ControlType
{
    KEYBOARD,
    XBOX_360,
    XBOX,
    //PS4_DUALSHOCK
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

    private float mMenuNavigateHoldDelayTimer = 0f;

    public ControlObject()
    { }

    public virtual void UpdateControl()
    {

    }

    public virtual void KeySettings(/*Dictionary<CommandIndex, object> someSetting*/)
    {
        //mKeybindList = new Dictionary<CommandIndex, object>(someSetting);
    }

    public virtual void SetNewCommandoBinding(Dictionary<CommandIndex, KeybindData> someNewBinding)
    { }

    public virtual void SetNewNavgateBinding(Dictionary<NavigatorType, KeybindData> someNewBinding)
    { }

    public virtual bool MenuNavigateHold(CommandIndex aCommand, float anDelayIntervall)
    {
        if(KeyHold(aCommand) && mMenuNavigateHoldDelayTimer <= 0f)
        {
            mMenuNavigateHoldDelayTimer = anDelayIntervall;
            return true;
        }

        if (!KeyHold(aCommand))
            mMenuNavigateHoldDelayTimer = 0f;

        if (mMenuNavigateHoldDelayTimer > 0f)
            mMenuNavigateHoldDelayTimer -= Time.deltaTime;

        return false;
    }

    public virtual bool MenuNavigatePress(CommandIndex aCommand)
    {
        if (KeyPress(aCommand))
            return true;

        return false;
    }

    public virtual bool MenuSelect() { return KeyPress(CommandIndex.SELECT); }

    public virtual bool MenuCancel() { return KeyPress(CommandIndex.CANCEL); }

    public virtual bool MenuConfirm() { return KeyPress(CommandIndex.CONFIRM); }

    public virtual bool MenuBack() { return KeyPress(CommandIndex.BACK); }

    #region public Gameplay methods
    public virtual Vector3 GameMoveBlock()
    {
        if (KeyHold(CommandIndex.BLOCK_MOVE_LEFT))
            return Vector3.left;
        if (KeyHold(CommandIndex.BLOCK_MOVE_RIGHT))
            return Vector3.right;
        return Vector3.zero;
    }

    public virtual int GameMovePowerUpSelection()
    {
        if (KeyPress(CommandIndex.POWER_UP_NAVI_LEFT))
            return -1;
        if (KeyPress(CommandIndex.POWER_UP_NAVI_RIGHT))
            return 1;

        return 0;
    }

    public virtual bool GameSlowDropBlock() { return KeyHold(CommandIndex.BLOCK_DROP); }

    public virtual bool GameInstantBlockDrop() { return KeyPress(CommandIndex.BLOCK_INSTANT_DROP); }

    public virtual bool GameRotateBlock() { return KeyPress(CommandIndex.BLOCK_ROTATE); }

    public virtual bool GameInverteBlock() { return KeyPress(CommandIndex.BLOCK_INVERT); }

    public virtual bool GameUsePowerUp() { return KeyPress(CommandIndex.POWER_UP_USE); }

    public virtual bool GameMovePowerUpSelectLeft() { return KeyPress(CommandIndex.POWER_UP_NAVI_LEFT); }

    public virtual bool GameMovePowerUpSelectRight() { return KeyPress(CommandIndex.POWER_UP_NAVI_RIGHT); }

    public virtual bool GameSwapPreview() { return KeyPress(CommandIndex.PREVIEW_SWAP); }

    public virtual bool GameRotatePreview() { return KeyPress(CommandIndex.PREVIEW_ROTATE); }

    public virtual bool GameDumpPreview() { return KeyPress(CommandIndex.PREVIEW_DUMP); }

    public virtual bool GamePause() { return KeyPress(CommandIndex.INGAME_PAUSE); }

    public virtual void GetInputFor(CommandIndex aCommand, ref KeybindData aBindData) { }
    #endregion

    #region Future removing methods
    // this method was only for temporary test of the control, it'll be removed
    public virtual bool TestGamePadState(XBoxButton aButton) { return false; }

    // this method is to be replaced by GameMoveBlock without the timer checking on this class
    public Vector3 GameMoveBlockHorizontal()
    {
        if (!MoveHorizontButtonTimePassed())
            return Vector3.zero;

        Vector3 dir = Vector3.zero;
        if (HorizontBottomHit(ref dir))
            ResetMoveHorizontTimer();

        return dir;
    }

    // this method will be replaced by GameSlowDropBlock without the timer function on this class
    public virtual bool GameDropBlockGradually(float aBlockNextDropTime)
    {
        if ((KeyHold(CommandIndex.BLOCK_DROP) && DropButtonTimePassed()))
        {
            ResetDropTimer();
            return true;
        }
        return false;
    }
    #endregion

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
    public virtual bool KeyPress(CommandIndex aCommand)
    {
        return false;
    }

    /// <summary>
    /// Keyhold event
    /// </summary>
    /// <param name="aCommand">The requesting command</param>
    /// <returns>true if the command was registrate and is holding down</returns>
    public virtual bool KeyHold(CommandIndex aCommand)
    {
        return false;
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
        //mBlockMoveHorizontButtonDelayTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;
        mBlockMoveHorizontButtonDelayTime = Constants.BUTTON_DOWN_INTERVAL;
    }

    protected bool MoveHorizontButtonTimePassed()
    {
        if(mBlockMoveHorizontButtonDelayTime > 0)
            mBlockMoveHorizontButtonDelayTime -= Time.deltaTime;

        return (mBlockMoveHorizontButtonDelayTime <= 0f);
        //return (Time.time > mBlockMoveHorizontButtonDelayTime);
    }

    protected bool SupressHorizontMove()
    {
        return (mHorizontSurpressTimer > 0f && mHorizontSurpressTimer < .5f);
    }
}