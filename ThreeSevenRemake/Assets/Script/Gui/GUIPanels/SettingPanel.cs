using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Script.Tools;

public class SettingPanel : MenuEnablePanelBase
{
    private enum ButtonIndex
    {
        GAMEPLAY_BUTTON,
        KEYBINDING_BUTTON,
        EXIT_BUTTON,
    }

    private enum SettingPanelIndex
    {
        GAMEPLAY,
        KEYBINDING,
        MAX_COMPONENT
    }

    public PanelButtonsContainer ButtonContainer;
    public List<SettingsContainerBase> SettingContainers;

    private List<bool> mActiveComponents = new List<bool>() { true, false, false };

    private int mPreviousSelectedSettingComponent = 1;
    private int mCurrentSelectedSettingComponent = 0;

    private bool mContainerIsActive = false;

    public override void Start()
    {
        mPanelIndex = MenuPanelIndex.OPTION_PANEL;

        base.Start();
        GameplaySettings.returnToSettingButtonContainer += DeactiveContainer;
        KeybindingSettings.returnToSettingButtonContainer += DeactiveContainer;
    }

    private void OnDestroy()
    {
        GameplaySettings.returnToSettingButtonContainer -= DeactiveContainer;
        KeybindingSettings.returnToSettingButtonContainer -= DeactiveContainer;
    }

    public override void Enter()
    {
        base.Enter();
        ButtonContainer.ActiveContainer(true);
        mPreviousSelectedSettingComponent = 1;
        mCurrentSelectedSettingComponent = 0;
        DisplayInactiveContainer();
        SetSelectedButton(0);
        NavigateSettingComponent();
    }

    protected override void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    {
        if (mContainerIsActive)
            return;

        base.NavigateMenuButtons(CommandIndex.NAVI_DOWN, CommandIndex.NAVI_UP);
        NavigateSettingComponent();
    }

    protected override void SelectButtonPressed()
    {
        switch(mCurrentSelectButtonIndex)
        {
            case (int)ButtonIndex.EXIT_BUTTON:
                ExitButtonOnClick();
                break;
            default:
                ActiveContainer();
                break;
        }
    }

    private void NavigateSettingComponent()
    {
        if (mCurrentSelectedSettingComponent == mCurrentSelectButtonIndex)
            return;

        if(mCurrentSelectedSettingComponent < (int)SettingPanelIndex.MAX_COMPONENT)
            mPreviousSelectedSettingComponent = mCurrentSelectedSettingComponent;

        mCurrentSelectedSettingComponent = mCurrentSelectButtonIndex;
        if(mCurrentSelectedSettingComponent < (int)SettingPanelIndex.MAX_COMPONENT)
        {
            DisplayInactiveContainer();
        }
    }

    private void ActiveDeactivateContainer(bool isActive)
    {
        ButtonContainer.ActiveContainer(!isActive);
        SettingContainers[mCurrentSelectedSettingComponent].ActiveContainer(isActive);
        mContainerIsActive = isActive;
    }

    private void ActiveContainer()
    {
        ButtonContainer.ActiveContainer(false);
        SettingContainers[mCurrentSelectedSettingComponent].Enter();
        mContainerIsActive = true;
        ActivateButtons(false);
    }

    private void DeactiveContainer()
    {
        SettingContainers[mCurrentSelectedSettingComponent].Exit();
        ButtonContainer.ActiveContainer(true);
        mContainerIsActive = false;
        ActivateButtons(true);
    }

    private void DisplayInactiveContainer()
    {
        SettingContainers[mPreviousSelectedSettingComponent].gameObject.SetActive(false);
        SettingContainers[mCurrentSelectedSettingComponent].gameObject.SetActive(true);
    }

    private void DeactiveAllSettingComponent()
    {
        for (int i = 0; i < mActiveComponents.Count; i++)
            mActiveComponents[i] = false;
    }

    private void ExitButtonOnClick()
    {
        MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
    }    
}
