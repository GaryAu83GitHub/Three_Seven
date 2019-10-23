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
            { CommandIndex.CANCEL, KeyCode.LeftAlt },
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

    public override bool MenuNavigate(CommandIndex aCommand)
    {
        //if (KeyDown(CommandIndex.NAVI_DOWN))
        //    return Vector2Int.down;
        //if (KeyDown(CommandIndex.NAVI_LEFT))
        //    return Vector2Int.left;
        //if (KeyDown(CommandIndex.NAVI_RIGHT))
        //    return Vector2Int.right;
        //if (KeyDown(CommandIndex.NAVI_UP))
        //    return Vector2Int.up;

        return KeyPress(aCommand);
    }

    public override bool KeyDown(CommandIndex aCommand)
    {
        if(!mCommands.ContainsKey(aCommand))
            return base.KeyDown(aCommand);

        return Input.GetKey(mCommands[aCommand]);
    }

    public override bool KeyPress(CommandIndex aCommand)
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

    protected override bool HorizontBottomHit(ref Vector3 aDir, float aHorizontValue = 0f)
    {
        //float horizontal = HorizontNavigation();
        //if ((horizontal <= -1 || horizontal >= 1))
        //{
        //    if (!SupressHorizontMove())
        //        aDir = new Vector3(horizontal, 0, 0);
        //    else
        //        aDir = Vector3.zero;

        //    if(mCurrentHorizontDirection != (int)horizontal)
        //    {
        //        mCurrentHorizontDirection = (int)horizontal;
        //        mHorizontSurpressTimer = 0f;
        //    }

        //    mHorizontSurpressTimer += Time.deltaTime * mIncreaseDeltaTimeConst;
        //    return true;
        //}

        return base.HorizontBottomHit(ref aDir, HorizontNavigation());
    }

    private float HorizontNavigation()
    {
        float navi = 0;
        if (KeyDown(CommandIndex.BLOCK_MOVE_LEFT))
            navi = -1f;
        if (KeyDown(CommandIndex.BLOCK_MOVE_RIGHT))
            navi = 1f;
        return navi;
    }
}
