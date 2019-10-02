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
        //ActivateNextButton();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.FINISH_SETTING);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_DIFFICULTY);
        //if (OnlyTwoDigitLinkIsEnable())
        //    displaySettingPanel?.Invoke(Setting_Index.SET_LINK);
        //else
        //    displaySettingPanel?.Invoke(Setting_Index.SET_START_TASK_VALUE);
    }

    private void PlayerNameInputFieldOnValueChange(TMP_InputField anInput)
    {
        GameSettings.Instance.SetPlayerName(anInput.text);
        //GameRoundManager.Instance.Data.PlayerName = anInput.text;
        //Debug.Log(anInput.text);
        //ActivateNextButton();
    }

    private void ActivateNextButton()
    {
        NextButton.interactable = (GameRoundManager.Instance.Data.PlayerName.Length > 0);
    }
}
