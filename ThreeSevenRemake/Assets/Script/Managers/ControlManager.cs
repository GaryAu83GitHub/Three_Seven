using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlManager
{
    public static ControlManager Ins
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new ControlManager();
            }
            return mInstance;
        }
    }
    private static ControlManager mInstance;

    private List<ControlObject> mControls = new List<ControlObject>();

    private Dictionary<CommandIndex, KeyCode> mDefaultKeyBoard = new Dictionary<CommandIndex, KeyCode>();
    private Dictionary<CommandIndex, KeyCode> mKeyBoard = new Dictionary<CommandIndex, KeyCode>();
    
    public ControlManager()
    {
        
    }

    public void DefaultSetting()
    {
        //// navigation
        //mDefaultKeyBoard.Add(CommandIndex.NAVI_LEFT, KeyCode.LeftArrow);
        //mDefaultKeyBoard.Add(CommandIndex.NAVI_RIGHT, KeyCode.RightArrow);
        //mDefaultKeyBoard.Add(CommandIndex.NAVI_DOWN, KeyCode.DownArrow);
        //mDefaultKeyBoard.Add(CommandIndex.NAVI_UP, KeyCode.UpArrow);
        //mDefaultKeyBoard.Add(CommandIndex.CONFIRM, KeyCode.Return);
        //mDefaultKeyBoard.Add(CommandIndex.BACK, KeyCode.Backspace);
        //// gameplay
        //mDefaultKeyBoard.Add(CommandIndex.BLOCK_MOVE_LEFT, KeyCode.A);
        //mDefaultKeyBoard.Add(CommandIndex.BLOCK_MOVE_RIGHT, KeyCode.D);
        //mDefaultKeyBoard.Add(CommandIndex.BLOCK_DROP, KeyCode.S);
        //mDefaultKeyBoard.Add(CommandIndex.BLOCK_ROTATE, KeyCode.W);
        //mDefaultKeyBoard.Add(CommandIndex.BLOCK_INVERT, KeyCode.E);
        //mDefaultKeyBoard.Add(CommandIndex.PREVIEW_SWAP, KeyCode.Space);
        //mDefaultKeyBoard.Add(CommandIndex.PREVIEW_ROTATE, KeyCode.UpArrow);
        //mDefaultKeyBoard.Add(CommandIndex.INGAME_PAUSE, KeyCode.Return);

        //mKeyBoard = new Dictionary<CommandIndex, KeyCode>(mDefaultKeyBoard);

        mControls.Add(new KeyboardControl());
        List<string> temp = Input.GetJoystickNames().ToList();
        //for (int i = 0; i < temp.Count; i++)
        //{
        //    if (!temp[i].Any())
        //        continue;
        //    //mControls.Add(new KeyboardControl());
        //}
        if (temp.Any() && temp[0].Any())
            mControls[0] = new XBox360Constrol();

        foreach (ControlObject c in mControls)
            c.KeySettings(/*new Dictionary<CommandIndex, KeyCode>()*/);
    }

    public void ResetButtonPressTimer(){ mControls[0].ResetButtonPressTimer(); }

    public bool MenuNavigation(CommandIndex aCommand) { return mControls[0].MenuNavigate(aCommand); }

    public Vector3 MoveBlockHorizontal() { return mControls[0].GameMoveBlockHorizontal(); }

    public bool GamePause() { return mControls[0].GamePause(); }

    public bool DropBlock(float aBlockNextDropTime) { return mControls[0].GameDropBlock(aBlockNextDropTime); }

    public bool DropBlockInstantly() { return mControls[0].GameInstantBlockDrop(); }

    public bool RotateBlock() { return mControls[0].GameRotateBlock(); }

    public bool InvertBlock() { return mControls[0].GameInverteBlock(); }

    public bool SwapPreview() { return mControls[0].GameSwapPreview(); }


    public bool KeyPress(CommandIndex anCommand)
    {
        return mControls[0].KeyPress(anCommand);
        //return Input.GetKey(mKeyBoard[anCommand]);
    }

    public bool KeyDown(CommandIndex anCommand)
    {
        return mControls[0].KeyDown(anCommand);
        //return Input.GetKeyDown(mKeyBoard[anCommand]);
    }
}
