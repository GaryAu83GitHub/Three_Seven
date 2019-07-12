using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetPlayerNamePanel : SettingPanelBase
{
    public TMP_InputField PlayerNameInputField;
    
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
    }

    public override void InitBaseValue()
    {
        base.InitBaseValue();

        PlayerNameInputField.ActivateInputField();
        ActivateNextButton();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_LINK);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.LEAVE_TO_TITLE);
    }

    private void PlayerNameInputFieldOnValueChange(TMP_InputField anInput)
    {
        GameRoundManager.Instance.Data.PlayerName = anInput.text;
        //Debug.Log(anInput.text);
        ActivateNextButton();
    }

    private void ActivateNextButton()
    {
        NextButton.interactable = (GameRoundManager.Instance.Data.PlayerName.Length > 0);
    }
}
