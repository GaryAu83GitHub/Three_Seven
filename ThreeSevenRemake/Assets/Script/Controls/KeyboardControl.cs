using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyboardControl : ControlObject
{
    private readonly Dictionary<CommandIndex, KeyCode> mCommands = new Dictionary<CommandIndex, KeyCode>();

    private float mMenuNavigationSuppressTimer = 0f;

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
            { CommandIndex.POWER_UP_USE, KeyCode.Space },
            { CommandIndex.PREVIEW_ROTATE, KeyCode.UpArrow },
            { CommandIndex.INGAME_PAUSE, KeyCode.Return }
        };

        foreach (CommandIndex com in defaultSets.Keys)
            mCommands[com] = (KeyCode)defaultSets[com];
    }

    public override bool MenuNavigateHold(CommandIndex aCommand, float anDelayIntervall = .1f)
    {
        //if (KeyDown(CommandIndex.NAVI_DOWN))
        //    return Vector2Int.down;
        //if (KeyDown(CommandIndex.NAVI_LEFT))
        //    return Vector2Int.left;
        //if (KeyDown(CommandIndex.NAVI_RIGHT))
        //    return Vector2Int.right;
        //if (KeyDown(CommandIndex.NAVI_UP))
        //    return Vector2Int.up;

        if(KeyPress(aCommand) && mMenuNavigationSuppressTimer <= 0f)
        {
            mMenuNavigationSuppressTimer = anDelayIntervall;//.1f;
            return true;
        }

        if(!KeyPress(aCommand))
            mMenuNavigationSuppressTimer = 0;

        if (mMenuNavigationSuppressTimer > 0f)
            mMenuNavigationSuppressTimer -= Time.deltaTime/* * anDelayIntervall*/;

        //return KeyPress(aCommand);
        return false;
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

    protected override bool HorizontBottomHit(ref Vector3 aDir, float aHorizontValue = 0f)
    {
        return base.HorizontBottomHit(ref aDir, HorizontNavigation());
    }

    public void GetKeyCodeFor(CommandIndex aCommand, ref KeyCode aKeyCode)
    {
        aKeyCode = mCommands[aCommand];
    }

    public void GetNavigatorCodesFor(NavigatorType aType, ref List<KeyCode> someCodes)
    {
        someCodes.Clear();
        if(aType == NavigatorType.BLOCK_NAVIGATOR)
        {
            someCodes.Add(mCommands[CommandIndex.BLOCK_MOVE_LEFT]);
            someCodes.Add(mCommands[CommandIndex.BLOCK_MOVE_RIGHT]);
            someCodes.Add(mCommands[CommandIndex.BLOCK_DROP]);
        }
        else if(aType == NavigatorType.POWER_UP_NAVIGATOR)
        {
            someCodes.Add(mCommands[CommandIndex.POWER_UP_NAVI_LEFT]);
            someCodes.Add(mCommands[CommandIndex.POWER_UP_NAVI_RIGHT]);
        }
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
