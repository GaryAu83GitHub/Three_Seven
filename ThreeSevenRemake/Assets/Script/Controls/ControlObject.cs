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
    BLOCK_INSTANT_DROP,
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

    //protected Dictionary<CommandIndex, object> mKeybindList = new Dictionary<CommandIndex, object>();

    protected float mBlockDropButtonDelayTime = 0f;
    protected float mBlockMoveHorizontButtonDelayTime = 0f;

    public ControlObject()
    { }

    public virtual void UpdateControl()
    {

    }

    public virtual void KeySettings(/*Dictionary<CommandIndex, object> someSetting*/)
    {
        //mKeybindList = new Dictionary<CommandIndex, object>(someSetting);
    }

    public virtual Vector2Int MenuNavigate()
    {
        if (KeyDown(CommandIndex.NAVI_DOWN))
            return Vector2Int.down;
        if (KeyDown(CommandIndex.NAVI_LEFT))
            return Vector2Int.left;
        if (KeyDown(CommandIndex.NAVI_RIGHT))
            return Vector2Int.right;
        if (KeyDown(CommandIndex.NAVI_UP))
            return Vector2Int.up;

        return Vector2Int.zero;
    }

    public virtual bool MenuSelect() { return KeyPress(CommandIndex.SELECT); }

    public virtual bool MenuConfirm(){ return KeyPress(CommandIndex.CONFIRM); }

    public virtual bool MenuBack(){ return KeyPress(CommandIndex.BACK); }

    public Vector3 GameMoveBlockHorizontal()
    {
        if(!MoveHorizontButtonTimePassed())
            return Vector3.zero;

        Vector3 dir = Vector3.zero;
        if (HorizontBottomHit(ref dir))
            ResetMoveHorizontTimer();

        return dir;
    }

    public virtual bool GameDropBlock(float aBlockNextDropTime)
    {
        if ((KeyDown(CommandIndex.BLOCK_DROP) && DropButtonTimePassed()) || Time.time > aBlockNextDropTime)
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

    public virtual bool GamePause() { return KeyPress(CommandIndex.INGAME_PAUSE); }

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
    protected virtual bool KeyDown(CommandIndex aCommand)
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
    protected virtual bool KeyPress(CommandIndex aCommand)
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
        mBlockDropButtonDelayTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;
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

    protected virtual bool HorizontBottomHit(ref Vector3 aDir) { return false; }
    
}
