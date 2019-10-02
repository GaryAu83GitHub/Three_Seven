using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputIndex
{
    BLOCK_MOVE_LEFT,
    BLOCK_MOVE_RIGHT,
    BLOCK_DROP,
    BLOCK_ROTATE,
    BLOCK_INVERT,
    PREVIEW_SWAP,
    PREVIEW_ROTATE,
    CONFIRM,
    BACK,
    MAX_INPUT
}

public class InputManager
{
    public static InputManager Instance
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

    private Dictionary<InputIndex, KeyCode> mKeyBoard = new Dictionary<InputIndex, KeyCode>();
    private Dictionary<InputIndex, KeyCode> mGamePad = new Dictionary<InputIndex, KeyCode>();

    public void DefaultSetting()
    {
        mKeyBoard.Add(InputIndex.BLOCK_MOVE_LEFT, KeyCode.A);
        mKeyBoard.Add(InputIndex.BLOCK_MOVE_RIGHT, KeyCode.D);
        mKeyBoard.Add(InputIndex.BLOCK_DROP, KeyCode.S);
        mKeyBoard.Add(InputIndex.BLOCK_ROTATE, KeyCode.W);
        mKeyBoard.Add(InputIndex.BLOCK_INVERT, KeyCode.LeftArrow);
        mKeyBoard.Add(InputIndex.PREVIEW_SWAP, KeyCode.RightArrow);
    }
}
