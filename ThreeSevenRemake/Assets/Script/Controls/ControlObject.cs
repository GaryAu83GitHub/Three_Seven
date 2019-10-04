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

    public ControlObject()
    { }

    public virtual void DefaultSetting(Dictionary<CommandIndex, KeyCode> someSetting)
    {
        mKeybindList = new Dictionary<CommandIndex, KeyCode>(someSetting);
    }

    public virtual bool KeyPress(CommandIndex aCommand)
    {
        if (!mKeybindList.ContainsKey(aCommand))
            return false;

        return Input.GetKey(mKeybindList[aCommand]);
    }

    public virtual bool KeyDown(CommandIndex aCommand)
    {
        if (!mKeybindList.ContainsKey(aCommand))
            return false;

        return Input.GetKeyDown(mKeybindList[aCommand]);
    }
}
