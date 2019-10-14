using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyboardControl : ControlObject
{
    private readonly Dictionary<CommandIndex, KeyCode> mCommands = new Dictionary<CommandIndex, KeyCode>();

    public KeyboardControl()
    {
        mType = ControlType.KEYBOARD;
    }

    public override void KeySettings(/*Dictionary<CommandIndex, object> someSetting*/)
    {
        Dictionary<CommandIndex, KeyCode> defaultSets = new Dictionary<CommandIndex, KeyCode>
        {
            // navigation
            { CommandIndex.NAVI_LEFT, KeyCode.LeftArrow },
            { CommandIndex.NAVI_RIGHT, KeyCode.RightArrow },
            { CommandIndex.NAVI_DOWN, KeyCode.DownArrow },
            { CommandIndex.NAVI_UP, KeyCode.UpArrow },
            { CommandIndex.SELECT, KeyCode.Space },
            { CommandIndex.CONFIRM, KeyCode.Return },
            { CommandIndex.BACK, KeyCode.Backspace },
            // gameplay
            { CommandIndex.BLOCK_MOVE_LEFT, KeyCode.A },
            { CommandIndex.BLOCK_MOVE_RIGHT, KeyCode.D },
            { CommandIndex.BLOCK_DROP, KeyCode.S },
            { CommandIndex.BLOCK_INSTANT_DROP, KeyCode.X },
            { CommandIndex.BLOCK_ROTATE, KeyCode.W },
            { CommandIndex.BLOCK_INVERT, KeyCode.E },
            { CommandIndex.PREVIEW_SWAP, KeyCode.Space },
            { CommandIndex.PREVIEW_ROTATE, KeyCode.UpArrow },
            { CommandIndex.INGAME_PAUSE, KeyCode.Return }
        };

        foreach (CommandIndex com in defaultSets.Keys)
            mCommands[com] = (KeyCode)defaultSets[com];
    }

    //public override Vector2Int MenuNavigate()
    //{
    //    if (KeyDown(CommandIndex.NAVI_DOWN))
    //        return Vector2Int.down;
    //    if (KeyDown(CommandIndex.NAVI_LEFT))
    //        return Vector2Int.left;
    //    if (KeyDown(CommandIndex.NAVI_RIGHT))
    //        return Vector2Int.right;
    //    if (KeyDown(CommandIndex.NAVI_UP))
    //        return Vector2Int.up;

    //    return base.MenuNavigate();
    //}

    protected override bool KeyDown(CommandIndex aCommand)
    {
        if(!mCommands.ContainsKey(aCommand))
            return base.KeyDown(aCommand);

        return Input.GetKey(mCommands[aCommand]);
    }

    protected override bool KeyPress(CommandIndex aCommand)
    {
        if (!mCommands.ContainsKey(aCommand))
            return base.KeyPress(aCommand);

        return Input.GetKeyDown(mCommands[aCommand]);
    }

    //public override Vector3 GameMoveBlockHorizontal()
    //{
    //    if(!MoveHorizontButtonTimePassed())
    //        return base.GameMoveBlockHorizontal();

    //    Vector3 dir = Vector3.zero;
    //    if (HorizontBottomHit(ref dir))
    //        ResetMoveHorizontTimer();

    //    return dir;
    //}

    protected override bool HorizontBottomHit(ref Vector3 aDir)
    {
        if (KeyDown(CommandIndex.BLOCK_MOVE_LEFT))
        {
            aDir = Vector3.left;
            return true;
        }
        if (KeyDown(CommandIndex.BLOCK_MOVE_RIGHT))
        {
            aDir = Vector3.right;
            return true;
        }

        return base.HorizontBottomHit(ref aDir);
    }
}
