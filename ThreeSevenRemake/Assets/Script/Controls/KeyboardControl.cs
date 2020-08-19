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
            { CommandIndex.CANCEL, KeyCode.Backspace },
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
            { CommandIndex.POWER_UP_NAVI_LEFT, KeyCode.LeftArrow },
            { CommandIndex.POWER_UP_NAVI_RIGHT, KeyCode.RightArrow },
            { CommandIndex.PREVIEW_ROTATE, KeyCode.UpArrow },
            { CommandIndex.INGAME_PAUSE, KeyCode.Return }
        };

        foreach (CommandIndex com in defaultSets.Keys)
            mCommands[com] = (KeyCode)defaultSets[com];
    }

    public override void SetNewCommandoBinding(Dictionary<CommandIndex, KeybindData> someNewBinding)
    {
        foreach(CommandIndex com in someNewBinding.Keys)
            mCommands[com] = someNewBinding[com].BindingKeyCode;
    }

    public override void SetNewNavgateBinding(Dictionary<NavigatorType, KeybindData> someNewBinding)
    {
        foreach(NavigatorType navi in someNewBinding.Keys)
        {
            if(navi == NavigatorType.BLOCK_NAVIGATOR)
            {
                mCommands[CommandIndex.BLOCK_MOVE_LEFT] = someNewBinding[navi].BindingKeyCodes[0];
                mCommands[CommandIndex.BLOCK_MOVE_RIGHT] = someNewBinding[navi].BindingKeyCodes[1];
                mCommands[CommandIndex.BLOCK_DROP] = someNewBinding[navi].BindingKeyCodes[2];
            }
            if(navi == NavigatorType.POWER_UP_NAVIGATOR)
            {
                mCommands[CommandIndex.POWER_UP_NAVI_LEFT] = someNewBinding[navi].BindingKeyCodes[0];
                mCommands[CommandIndex.POWER_UP_NAVI_RIGHT] = someNewBinding[navi].BindingKeyCodes[1];
            }
        }
    }

    

    // this method is outcommented is due to it's function has been transfered to its
    // parents class virutal method, it is possible this method will be removed in he
    // future
    //public override bool MenuNavigateHold(CommandIndex aCommand, float anDelayIntervall = .1f)
    //{
    //    if(KeyHold(aCommand) && mMenuNavigationSuppressTimer <= 0f)
    //    {
    //        mMenuNavigationSuppressTimer = anDelayIntervall;
    //        return true;
    //    }

    //    if(!KeyHold(aCommand))
    //        mMenuNavigationSuppressTimer = 0;

    //    if (mMenuNavigationSuppressTimer > 0f)
    //        mMenuNavigationSuppressTimer -= Time.deltaTime;

    //    //return KeyPress(aCommand);
    //    return false;
    //}

    public override bool MenuNavigatePress(CommandIndex aCommand)
    {
        if (KeyPress(aCommand))
            return true;
        return base.MenuNavigatePress(aCommand);
    }

    public override bool KeyPress(CommandIndex aCommand)
    {
        if(!mCommands.ContainsKey(aCommand))
            return base.KeyPress(aCommand);

        return Input.GetKeyDown(mCommands[aCommand]);
    }

    public override bool KeyHold(CommandIndex aCommand)
    {
        if (!mCommands.ContainsKey(aCommand))
            return base.KeyHold(aCommand);

        return Input.GetKey(mCommands[aCommand]);
    }

    protected override bool HorizontBottomHit(ref Vector3 aDir, float aHorizontValue = 0f)
    {
        return base.HorizontBottomHit(ref aDir, HorizontNavigation());
    }

    /// <summary>
    /// Get the key set to the requesting command into the bind data reference
    /// </summary>
    /// <param name="aCommand">requesting command index</param>
    /// <param name="aBindData">reference bind data to set button of the requesting
    /// command index</param>
    public override void GetInputFor(CommandIndex aCommand, ref KeybindData aBindData)
    {
        aBindData.ChangeBindingKeyCode(mCommands[aCommand]);
    }

    public void GetKeyCodeFor(CommandIndex aCommand, ref KeyCode aKeyCode)
    {
        aKeyCode = mCommands[aCommand];
    }

    public void GetCodeFor(CommandIndex aCommand, ref KeybindData aBindData)
    {
        aBindData.ChangeBindingKeyCode(mCommands[aCommand]);
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
        if (KeyHold(CommandIndex.BLOCK_MOVE_LEFT))
            navi = -1f;
        if (KeyHold(CommandIndex.BLOCK_MOVE_RIGHT))
            navi = 1f;
        return navi;
    }
}
