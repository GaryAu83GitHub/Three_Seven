using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetKeybindConfirmSlot : SetSettingsConfirmSlot
{
    public Button BackButton;

    public GameObject ApplyButtonContainer;
    public GameObject BackButtonContainer;

    public delegate void OnBackButtonPressed();
    public static OnBackButtonPressed backButtonPressed;

    private bool mkeybindsHasChanged = false;

    public override void Start()
    {
        base.Start();
    }

    public void BackAndApplyButtonsSwap(bool keyBindsHasChanged)
    {
        mkeybindsHasChanged = keyBindsHasChanged;
        ApplyButtonContainer.SetActive(!mkeybindsHasChanged);
        BackButtonContainer.SetActive(mkeybindsHasChanged);
    }

    protected override void MenuButtonPressed()
    {
        if (mkeybindsHasChanged)
        {
            if (ControlManager.Ins.MenuSelectButtonPressed())
                backButtonPressed?.Invoke();
        }
        else
            base.MenuButtonPressed();
            
    }
}
