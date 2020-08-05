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

        mControls.Add(new KeyboardControl());
        mActiveControl = mControls[0];

        List<string> temp = Input.GetJoystickNames().ToList();
        if (temp.Any())
        {
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
            c.KeySettings();

        mDefaultControlsAreRegistrated = true;
    }

    public void NewBinding(Dictionary<CommandIndex, KeybindData> someCommandoBindings, Dictionary<NavigatorType, KeybindData> someNavigateBindings)
    {
        for (int i = 0; i < mControls.Count; i++)
        {
            mControls[i].SetNewCommandoBinding(someCommandoBindings);
            mControls[i].SetNewNavgateBinding(someNavigateBindings);
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

    public bool GetGamePadStateTest(XBoxButton aButton)
    {
        return mActiveControl.TestGamePadState(aButton);
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