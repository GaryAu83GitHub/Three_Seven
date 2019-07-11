﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetPlayerNamePanel : SettingPanelBase
{
    public TMP_InputField PlayerNameInputField;

    private string mPlayerName = "";
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();
        PlayerNameInputField.onValueChanged.AddListener(delegate { PlayerNameInputFieldOnValueChange(PlayerNameInputField); });

        ActivateNextButton();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Issue.SET_NAME + 1);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Issue.SET_NAME - 1);
    }

    private void PlayerNameInputFieldOnValueChange(TMP_InputField anInput)
    {
        mPlayerName = anInput.text;
        //Debug.Log(anInput.text);
        ActivateNextButton();
    }

    private void ActivateNextButton()
    {
        NextButton.interactable = (mPlayerName.Length > 0);
    }
}