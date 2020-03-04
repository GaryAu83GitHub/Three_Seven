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

    public ControlType ActiveControlType { get { return mActiveControl.Type; } }

    public List<ControlObject> Controls { get { return mControls; } }
    private List<ControlObject> mControls = new List<ControlObject>();

    private ControlObject mActiveControl = new ControlObject();

    private bool mDefaultControlsAreRegistrated = false;

    public ControlManager()
    {

    }

    public void DefaultSetting()
    {
        if (mDefaultControlsAreRegistrated)
            return;
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
        mActiveControl = mControls[0];

        List<string> temp = Input.GetJoystickNames().ToList();
        if (temp.Any())
        {
            //mControls.Add(new XBox360Constrol());
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Any())
                {
                    mControls.Add(new XBox360Constrol());
                    mActiveControl = mControls[1];
                }
            }
        }
        foreach (ControlObject c in mControls)
            c.KeySettings(/*new Dictionary<CommandIndex, KeyCode>()*/);

        mDefaultControlsAreRegistrated = true;
    }

    public void NewBinding(Dictionary<CommandIndex, KeybindData> someCommandoBindings, Dictionary<NavigatorType, KeybindData> someNavigateBindings)
    {
        foreach(CommandIndex com in someCommandoBindings.Keys)
        {

        }

        foreach(NavigatorType navi in someNavigateBindings.Keys)
        {
        }
    }

    public void ResetButtonPressTimer() { mActiveControl.ResetButtonPressTimer(); }

    public bool MenuNavigationHold(CommandIndex aCommand, float aDelayIntervall = .1f)
    {
        return mActiveControl.MenuNavigateHold(aCommand, aDelayIntervall);
    }

    public bool MenuNavigationPress(CommandIndex aCommand)
    {
        return mActiveControl.MenuNavigatePress(aCommand);
    }

    public bool MenuConfirmButtonPressed() { return mActiveControl.MenuConfirm(); }

    public bool MenuSelectButtonPressed() { return mActiveControl.MenuSelect(); }

    public bool MenuCancelButtonPressed() { return mActiveControl.MenuCancel(); }

    public bool MenuBackButtonPressed() { return mActiveControl.MenuBack(); }


    // these are for in game mode
    public Vector3 MoveBlockHorizontal() { return mActiveControl.GameMoveBlockHorizontal(); }

    public int PowerUpSelection() { return mActiveControl.GameMovePowerUpSelection(); }

    public bool GamePause() { return mActiveControl.GamePause(); }

    public bool DropBlockGradually(float aBlockNextDropTime) { return mActiveControl.GameDropBlockGradually(aBlockNextDropTime); }

    public bool DropBlockInstantly() { return mActiveControl.GameInstantBlockDrop(); }

    public bool RotateBlock() { return mActiveControl.GameRotateBlock(); }

    public bool InvertBlock() { return mActiveControl.GameInverteBlock(); }

    public bool PowerUpUse() { return mActiveControl.GameUsePowerUp(); }

    public bool PowerUpSelectLeft() { return mActiveControl.GameMovePowerUpSelectLeft(); }

    public bool PowerUpSelectRight() { return mActiveControl.GameMovePowerUpSelectRight(); }

    public bool SwapPreview() { return mActiveControl.GameSwapPreview(); }

    public bool ChangePreview() { return mActiveControl.GameRotatePreview(); }

    public bool DumpPreview() { return mActiveControl.GameDumpPreview(); }


    public bool KeyPress(CommandIndex anCommand)
    {
        return mActiveControl.KeyPress(anCommand);
        //return Input.GetKey(mKeyBoard[anCommand]);
    }

    public bool KeyDown(CommandIndex anCommand)
    {
        return mActiveControl.KeyDown(anCommand);
        //return Input.GetKeyDown(mKeyBoard[anCommand]);
    }

    public void SetActiveControl(ControlType aControlType)
    {
        mActiveControl = mControls.First(c => c.Type == aControlType);
    }

    public bool CheckHaveControlOf(ControlType aSeekingType)
    {
        return mControls.Any(c => c.Type == aSeekingType);
    }

    private bool MultiControlButton(CommandIndex aCommand)
    {
        for(int i = 0; i < mControls.Count; i++)
        {
            if (aCommand == CommandIndex.CONFIRM)
                return mControls[i].MenuConfirm();
            if (aCommand == CommandIndex.SELECT)
                return mControls[i].MenuSelect();
            if (aCommand == CommandIndex.CANCEL)
                return mControls[i].MenuCancel();
            if (aCommand == CommandIndex.BACK)
                return mControls[i].MenuBack();
        }
        return false;
    }

    private bool MultiControlNavigation(CommandIndex aCommand, float anDelayIntervall = 0f)
    {
        for (int i = 0; i < mControls.Count; i++)
        {
            if(anDelayIntervall != 0f && mControls[i].MenuNavigateHold(aCommand, anDelayIntervall)) 
                return true;
            else if(mControls[i].MenuNavigatePress(aCommand))
                return true;
        }
        return false;
    }
}