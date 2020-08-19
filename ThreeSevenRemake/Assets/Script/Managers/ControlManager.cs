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
                    mControls.Add(new XBoxControl/*XBox360Constrol*/());
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
    public Vector3 MoveBlockHorizontal() { return mActiveControl.GameMoveBlock();/*GameMoveBlockHorizontal();*/ }

    public int PowerUpSelection() { return mActiveControl.GameMovePowerUpSelection(); }

    public bool GamePause() { return mActiveControl.GamePause(); }

    public bool DropBlockGradually(/*float aBlockNextDropTime*/) { return mActiveControl.GameSlowDropBlock()/*GameDropBlockGradually(aBlockNextDropTime)*/; }

    public bool DropBlockInstantly() { return mActiveControl.GameInstantBlockDrop(); }

    public bool RotateBlock() { return mActiveControl.GameRotateBlock(); }

    public bool InvertBlock() { return mActiveControl.GameInverteBlock(); }

    public bool PowerUpUse() { return mActiveControl.GameUsePowerUp(); }

    public bool PowerUpSelectLeft() { return mActiveControl.GameMovePowerUpSelectLeft(); }

    public bool PowerUpSelectRight() { return mActiveControl.GameMovePowerUpSelectRight(); }

    public bool SwapPreview() { return mActiveControl.GameSwapPreview(); }

    public bool ChangePreview() { return mActiveControl.GameRotatePreview(); }

    public bool DumpPreview() { return mActiveControl.GameDumpPreview(); }

    /// <summary>
    /// Return the boolian of the input to the requesting command is being held down on
    /// the active control
    /// </summary>
    /// <param name="anCommand">Requesting command</param>
    /// <returns>return true if the input is being held</returns>
    public bool KeyHold(CommandIndex anCommand)
    {
        return mActiveControl.KeyHold(anCommand);
    }

    /// <summary>
    /// Return the boolian of the input to the requesting command has been pressed on
    /// the active control
    /// </summary>
    /// <param name="anCommand">Requesting command</param>
    /// <returns>return true if the input is been pressed</returns>

    public bool KeyPress(CommandIndex anCommand)
    {
        return mActiveControl.KeyPress(anCommand);
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

    // this function has not been called anyway, will investigate if it shall be removed
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