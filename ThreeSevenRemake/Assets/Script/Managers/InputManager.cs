using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static InputManager Ins
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new InputManager();
            }
            return mInstance;
        }
    }
    private static InputManager mInstance;

    private Dictionary<CommandIndex, KeyCode> mDefaultKeyBoard = new Dictionary<CommandIndex, KeyCode>();
    private Dictionary<CommandIndex, KeyCode> mKeyBoard = new Dictionary<CommandIndex, KeyCode>();
    //private Dictionary<InputIndex, KeyCode> mGamePad = new Dictionary<InputIndex, KeyCode>();

    public void DefaultSetting()
    {
        // navigation
        mDefaultKeyBoard.Add(CommandIndex.NAVI_LEFT, KeyCode.LeftArrow);
        mDefaultKeyBoard.Add(CommandIndex.NAVI_RIGHT, KeyCode.RightArrow);
        mDefaultKeyBoard.Add(CommandIndex.NAVI_DOWN, KeyCode.DownArrow);
        mDefaultKeyBoard.Add(CommandIndex.NAVI_UP, KeyCode.UpArrow);
        mDefaultKeyBoard.Add(CommandIndex.CONFIRM, KeyCode.Return);
        mDefaultKeyBoard.Add(CommandIndex.BACK, KeyCode.Backspace);
        // gameplay
        mDefaultKeyBoard.Add(CommandIndex.BLOCK_MOVE_LEFT, KeyCode.A);
        mDefaultKeyBoard.Add(CommandIndex.BLOCK_MOVE_RIGHT, KeyCode.D);
        mDefaultKeyBoard.Add(CommandIndex.BLOCK_DROP, KeyCode.S);
        mDefaultKeyBoard.Add(CommandIndex.BLOCK_ROTATE, KeyCode.W);
        mDefaultKeyBoard.Add(CommandIndex.BLOCK_INVERT, KeyCode.E);
        mDefaultKeyBoard.Add(CommandIndex.PREVIEW_SWAP, KeyCode.Space);
        mDefaultKeyBoard.Add(CommandIndex.PREVIEW_ROTATE, KeyCode.UpArrow);
        mDefaultKeyBoard.Add(CommandIndex.INGAME_PAUSE, KeyCode.Return);

        mKeyBoard = new Dictionary<CommandIndex, KeyCode>(mDefaultKeyBoard);
    }

    public bool KeyPress(CommandIndex anCommand)
    {
        return Input.GetKey(mKeyBoard[anCommand]);
    }

    public bool KeyDown(CommandIndex anCommand)
    {
        return Input.GetKeyDown(mKeyBoard[anCommand]);
    }
}
