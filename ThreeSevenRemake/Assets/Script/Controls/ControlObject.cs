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
    CONFIRM,
    BACK,
    // gameplay
    BLOCK_MOVE_LEFT,
    BLOCK_MOVE_RIGHT,
    BLOCK_DROP,
    BLOCK_ROTATE,
    BLOCK_INVERT,
    PREVIEW_SWAP,
    PREVIEW_ROTATE,
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

    protected Dictionary<CommandIndex, KeyCode> mKeybindList = new Dictionary<CommandIndex, KeyCode>();

    protected float mBlockDropButtonDelayTime = 0f;
    protected float mBlockMoveHorizontButtonDelayTime = 0f;

    public ControlObject()
    { }

    public virtual void KeySettings(Dictionary<CommandIndex, KeyCode> someSetting)
    {
        mKeybindList = new Dictionary<CommandIndex, KeyCode>(someSetting);
    }

    public virtual Vector2Int MenuNavigate() { return Vector2Int.zero; }

    public virtual bool MenuSelect() { return KeyDown(CommandIndex.SELECT); }

    public virtual bool MenuConfirm(){ return KeyDown(CommandIndex.CONFIRM); }

    public virtual bool MenuBack(){ return KeyDown(CommandIndex.BACK); }

    public virtual Vector3 GameMoveBlockHorizontal() { return Vector3.zero; }

    public virtual bool GameDropBlock()
    {
        if (KeyDown(CommandIndex.BLOCK_DROP) && DropButtonTimePassed())
        {
            ResetDropTimer();
            return true;
        }
        return false;
    }

    public bool GameRotateBlock() { return KeyPress(CommandIndex.BLOCK_ROTATE); }

    public bool GameInverteBlock() { return KeyPress(CommandIndex.BLOCK_INVERT); }

    public bool GameSwapPreview() { return KeyPress(CommandIndex.PREVIEW_SWAP); }

    public bool GameRotatePreview() { return KeyPress(CommandIndex.PREVIEW_ROTATE); }

    public bool GamePause() { return KeyPress(CommandIndex.INGAME_PAUSE); }

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
        if (!mKeybindList.ContainsKey(aCommand))
            return false;

        return Input.GetKey(mKeybindList[aCommand]);
    }

    /// <summary>
    /// Keyhold event
    /// </summary>
    /// <param name="aCommand">The requesting command</param>
    /// <returns>true if the command was registrate and is holding down</returns>
    public virtual bool KeyPress(CommandIndex aCommand)
    {
        if (!mKeybindList.ContainsKey(aCommand))
            return false;

        return Input.GetKeyDown(mKeybindList[aCommand]);
    }

    /// <summary>
    /// Reset dropp button's delay intervall timer
    /// </summary>
    protected void ResetDropTimer()
    {
        mBlockDropButtonDelayTime = Time.time + GameManager.Instance.DropRate;
    }

    /// <summary>
    /// Check if the time had for the command of dropping to pass
    /// </summary>
    /// <returns>return true if the time had passed the delaying time</returns>
    protected bool DropButtonTimePassed()
    {
        return (Time.time > mBlockDropButtonDelayTime);
    }

    protected void ResetMoveHorizontTimer()
    {
        mBlockMoveHorizontButtonDelayTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;
    }

    protected bool MoveHorizontButtonTimePassed()
    {
        return (Time.time > mBlockMoveHorizontButtonDelayTime);
    }
}
