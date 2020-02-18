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

    public List<ControlObject> Controls { get { return mControls; } }
    private List<ControlObject> mControls = new List<ControlObject>();

    private ControlObject mActiveControls = new ControlObject();

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
        mActiveControls = mControls[0];

        List<string> temp = Input.GetJoystickNames().ToList();
        if (temp.Any())
        {
            mControls.Add(new XBox360Constrol());
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Any())
                    mActiveControls = mControls[1];
            }
        }
        foreach (ControlObject c in mControls)
            c.KeySettings(/*new Dictionary<CommandIndex, KeyCode>()*/);
    }

    public void ResetButtonPressTimer() { mActiveControls.ResetButtonPressTimer(); }

    public bool MenuNavigationHold(CommandIndex aCommand, float anDelayIntervall = .1f) { return mActiveControls.MenuNavigateHold(aCommand, anDelayIntervall); }

    public bool MenuNavigationPress(CommandIndex aCommand) { return mActiveControls.MenuNavigatePress(aCommand); }

    public bool MenuConfirmButtonPressed() { return mActiveControls.MenuConfirm(); }

    public bool MenuSelectButtonPressed() { return mActiveControls.MenuSelect(); }

    public bool MenuCancelButtonPressed() { return mActiveControls.MenuCancel(); }

    public bool MenuBackButtonPressed() { return mActiveControls.MenuBack(); }

    public Vector3 MoveBlockHorizontal() { return mActiveControls.GameMoveBlockHorizontal(); }

    public int PowerUpSelection() { return mActiveControls.GameMovePowerUpSelection(); }

    public bool GamePause() { return mActiveControls.GamePause(); }

    public bool DropBlockGradually(float aBlockNextDropTime) { return mActiveControls.GameDropBlockGradually(aBlockNextDropTime); }

    public bool DropBlockInstantly() { return mActiveControls.GameInstantBlockDrop(); }

    public bool RotateBlock() { return mActiveControls.GameRotateBlock(); }

    public bool InvertBlock() { return mActiveControls.GameInverteBlock(); }

    public bool PowerUpUse() { return mActiveControls.GameUsePowerUp(); }

    public bool PowerUpSelectLeft() { return mActiveControls.GameMovePowerUpSelectLeft(); }

    public bool PowerUpSelectRight() { return mActiveControls.GameMovePowerUpSelectRight(); }

    public bool SwapPreview() { return mActiveControls.GameSwapPreview(); }

    public bool ChangePreview() { return mActiveControls.GameRotatePreview(); }

    public bool DumpPreview() { return mActiveControls.GameDumpPreview(); }


    public bool KeyPress(CommandIndex anCommand)
    {
        return mActiveControls.KeyPress(anCommand);
        //return Input.GetKey(mKeyBoard[anCommand]);
    }

    public bool KeyDown(CommandIndex anCommand)
    {
        return mActiveControls.KeyDown(anCommand);
        //return Input.GetKeyDown(mKeyBoard[anCommand]);
    }
}