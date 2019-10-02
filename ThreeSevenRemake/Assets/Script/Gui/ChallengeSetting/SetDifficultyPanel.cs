using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDifficultyPanel : SettingPanelBase
{
    public List<Button> DifficultButtons; 
    
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();

        DifficultButtons[(int)Difficulties.EASY].onClick.AddListener(EasyButtonOnClick);
        DifficultButtons[(int)Difficulties.NORMAL].onClick.AddListener(NormalButtonOnClick);
        DifficultButtons[(int)Difficulties.HARD].onClick.AddListener(HardButtonOnClick);
        DifficultButtons[(int)Difficulties.CUSTOMIZE].onClick.AddListener(CostumizeButtonOnClick);

        //DescriptionText.text = "";
        SetDifficulty(Difficulties.EASY);
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_NAME);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.LEAVE_TO_TITLE);
    }

    private void EasyButtonOnClick()
    {
        SetDifficulty(Difficulties.EASY);
    }

    private void NormalButtonOnClick()
    {
        SetDifficulty(Difficulties.NORMAL);
    }

    private void HardButtonOnClick()
    {
        SetDifficulty(Difficulties.HARD);
    }

    private void CostumizeButtonOnClick()
    {
        SetDifficulty(Difficulties.CUSTOMIZE);
    }

    private void SetDifficulty(Difficulties aDifficulty)
    {
        ButtonAppearance(aDifficulty);

        switch (aDifficulty)
        {
            case Difficulties.EASY:
                SwapDifficulties(new bool[4] { true, true, false, false });
                break;
            case Difficulties.NORMAL:
                SwapDifficulties(new bool[4] { false, true, true, false });
                break;
            case Difficulties.HARD:
                SwapDifficulties(new bool[4] { false, false, true, true });
                break;
            case Difficulties.CUSTOMIZE:
                displaySettingPanel?.Invoke(Setting_Index.SET_LINK);
                break;
        }
    }

    private void ButtonAppearance(Difficulties aSelectedDifficulty)
    {
        if(GameRoundManager.Instance.Data != null)
            GameRoundManager.Instance.Data.SelectedDifficulty = aSelectedDifficulty;
        for (int i = 0; i < DifficultButtons.Count; i++)
            DifficultButtons[i].interactable = true;

        DifficultButtons[(int)aSelectedDifficulty].interactable = false;
    }

    private void SwapDifficulties(bool[] someEnables)
    {
        for (int i = 0; i < someEnables.Length; i++)
            GameRoundManager.Instance.Data.EnableScoringMethods[i] = someEnables[i];
    }
}
