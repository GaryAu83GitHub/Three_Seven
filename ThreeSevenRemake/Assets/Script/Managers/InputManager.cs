using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputIndex
{
    // navigation
    NAVI_LEFT,
    NAVI_RIGHT,
    NAVI_DOWN,
    NAVI_UP,
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

    private Dictionary<InputIndex, KeyCode> mDefaultKeyBoard = new Dictionary<InputIndex, KeyCode>();
    private Dictionary<InputIndex, KeyCode> mKeyBoard = new Dictionary<InputIndex, KeyCode>();
    //private Dictionary<InputIndex, KeyCode> mGamePad = new Dictionary<InputIndex, KeyCode>();

    public void DefaultSetting()
    {
        // navigation
        mDefaultKeyBoard.Add(InputIndex.NAVI_LEFT, KeyCode.LeftArrow);
        mDefaultKeyBoard.Add(InputIndex.NAVI_RIGHT, KeyCode.RightArrow);
        mDefaultKeyBoard.Add(InputIndex.NAVI_DOWN, KeyCode.DownArrow);
        mDefaultKeyBoard.Add(InputIndex.NAVI_UP, KeyCode.UpArrow);
        mDefaultKeyBoard.Add(InputIndex.CONFIRM, KeyCode.Return);
        mDefaultKeyBoard.Add(InputIndex.BACK, KeyCode.Backspace);
        // gameplay
        mDefaultKeyBoard.Add(InputIndex.BLOCK_MOVE_LEFT, KeyCode.A);
        mDefaultKeyBoard.Add(InputIndex.BLOCK_MOVE_RIGHT, KeyCode.D);
        mDefaultKeyBoard.Add(InputIndex.BLOCK_DROP, KeyCode.S);
        mDefaultKeyBoard.Add(InputIndex.BLOCK_ROTATE, KeyCode.W);
        mDefaultKeyBoard.Add(InputIndex.BLOCK_INVERT, KeyCode.E);
        mDefaultKeyBoard.Add(InputIndex.PREVIEW_SWAP, KeyCode.Space);
        mDefaultKeyBoard.Add(InputIndex.PREVIEW_ROTATE, KeyCode.UpArrow);
        mDefaultKeyBoard.Add(InputIndex.INGAME_PAUSE, KeyCode.Return);

        mKeyBoard = new Dictionary<InputIndex, KeyCode>(mDefaultKeyBoard);
    }

    public bool KeyPress(InputIndex anCommand)
    {
        return Input.GetKey(mKeyBoard[anCommand]);
    }

    public bool KeyDown(InputIndex anCommand)
    {
        return Input.GetKeyDown(mKeyBoard[anCommand]);
    }
}
