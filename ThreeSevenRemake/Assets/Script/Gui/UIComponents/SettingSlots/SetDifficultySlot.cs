using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDifficultySlot : SettingSlotBase
{
    public Sprite ButtonSelectedSprite;

    public GameObject DifficultyFrame;
    public List<Button> Buttons;
    public GameObject CostumizeFrame;
    public List<Toggle> Costumizes;
    
    private CanvasGroup DifficultyCG;
    private CanvasGroup CostumizeCG;

    private bool mIsCostumizeSelected = false;
    private int mDifficultButtonSelectIndex = 0;
    private int mCostumizeToggleSelectIndex = 0;

    private List<Image> ToggleLable = new List<Image>();

    private Difficulties mSelectedDifficulty = GameRoundManager.Instance.Data.SelectedDifficulty;
    private List<bool> mSelectedCustomize = new List<bool>();
    private List<bool> mChangedCustomize = new List<bool>() { true, false, false, false };

    public override void Start()
    {
        base.Start();
        DifficultyCG = DifficultyFrame.GetComponent<CanvasGroup>();
        CostumizeCG = CostumizeFrame.GetComponent<CanvasGroup>();

        for(int i = 0; i < Costumizes.Count;i++)
        {
            ToggleLable.Add(Costumizes[i].GetComponentInChildren<Image>());
        }

        mSelectedCustomize = new List<bool>(GameRoundManager.Instance.Data.EnableScoringMethods);

        SetDifficulty(mSelectedDifficulty);
    }

    protected override void Navigation()
    {
        if(ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
        {
            SelectButton(-1);
        }
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
        {
            SelectButton(1);
        }
    }

    protected override void MenuButtonPressed()
    {
        if (ControlManager.Ins.MenuSelectButtonPressed())
        {
            if (mDifficultButtonSelectIndex == (int)Difficulties.CUSTOMIZE && !mIsCostumizeSelected)
            {
                mIsCostumizeSelected = true;
                mLockParentInput = true;
                ToggleAppearance();
            }
            else if (mIsCostumizeSelected)
                SwitchCostumizeToggle();

        }
        if (ControlManager.Ins.MenuCancelButtonPressed())
        {
            if (mDifficultButtonSelectIndex == (int)Difficulties.CUSTOMIZE)
            {
                mIsCostumizeSelected = false;
                mLockParentInput = false;
            }
        }
        ActiveCostumizeToggles();
    }

    public override void ChangeSetting(ref GameplaySettingData aSettingData)
    {
        aSettingData.SelectDifficulty = mSelectedDifficulty;
        if (mSelectedDifficulty != Difficulties.CUSTOMIZE)
            aSettingData.EnableDigits = new List<bool>(mSelectedCustomize);
        else
            aSettingData.EnableDigits = new List<bool>(mChangedCustomize);
    }

    private void SelectButton(int anIndex)
    {
        if(!mIsCostumizeSelected)
        {
            if ((mDifficultButtonSelectIndex + anIndex) >= Buttons.Count)
                mDifficultButtonSelectIndex = (int)Difficulties.EASY;
            else if ((mDifficultButtonSelectIndex + anIndex) < 0)
                mDifficultButtonSelectIndex = (int)Difficulties.CUSTOMIZE;
            else
                mDifficultButtonSelectIndex += anIndex;

            SetDifficulty((Difficulties)mDifficultButtonSelectIndex);
        }
        if (mIsCostumizeSelected)
        {
            if ((mCostumizeToggleSelectIndex + anIndex) >= Costumizes.Count)
                mCostumizeToggleSelectIndex = 0;
            else if ((mCostumizeToggleSelectIndex + anIndex) < 0)
                mCostumizeToggleSelectIndex = Costumizes.Count - 1;
            else
                mCostumizeToggleSelectIndex += anIndex;

            ToggleAppearance();
        }
        
    }

    private void ActiveCostumizeToggles()
    {
        DifficultyCG.interactable = !mIsCostumizeSelected;
        CostumizeCG.interactable = mIsCostumizeSelected;

        if(mIsCostumizeSelected)
        {
            DifficultyCG.alpha = .5f;
            CostumizeCG.alpha = 1f;
        }
        else
        {
            DifficultyCG.alpha = 1f;
            CostumizeCG.alpha = .5f;
        }
    }

    private void SwitchCostumizeToggle()
    {
        //mSelectedCustomize[mCostumizeToggleSelectIndex] = !mSelectedCustomize[mCostumizeToggleSelectIndex];
        //bool isThereAnotherOptionEnable = mSelectedCustomize.Contains(true);
        //if (!isThereAnotherOptionEnable)
        //    mSelectedCustomize[mCostumizeToggleSelectIndex] = true;
        mChangedCustomize[mCostumizeToggleSelectIndex] = !mChangedCustomize[mCostumizeToggleSelectIndex];
        bool isThereAnotherOptionEnable = mChangedCustomize.Contains(true);
        if (!isThereAnotherOptionEnable)
            mChangedCustomize[mCostumizeToggleSelectIndex] = true;

        ToggleState();
        //else
        //    Costumizes[mCostumizeToggleSelectIndex].isOn = mSelectedCustomize[mCostumizeToggleSelectIndex];
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
                SwapDifficulties(mChangedCustomize.ToArray());
                break;
        }
    }

    private void ButtonAppearance(Difficulties aSelectedDifficulty)
    {
        //if (GameRoundManager.Instance.Data != null)
        mSelectedDifficulty = aSelectedDifficulty;
        bool isInteractable = false;

        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i == (int)aSelectedDifficulty)
                isInteractable = false;
            else
                isInteractable = true;

            ButtonState(i, isInteractable);
        }
    }

    private void ToggleAppearance()
    {
        for (int i = 0; i < Costumizes.Count; i++)
            Costumizes[i].interactable = false;

        Costumizes[mCostumizeToggleSelectIndex].interactable = true;
    }

    private void SwapDifficulties(bool[] someEnables)
    {
        for (int i = 0; i < someEnables.Length; i++)
            mSelectedCustomize[i] = someEnables[i];

        ToggleState();
    }

    private void ButtonState(int aButtonIndex, bool isButtonInteractable)
    {
        Buttons[aButtonIndex].interactable = isButtonInteractable;
        Color buttonColor = new Color();
        Sprite buttonSprite = Buttons[aButtonIndex].image.sprite;
        if (isButtonInteractable)
        {
            buttonColor = new Color(1, 1, 1, .5f);
            buttonSprite = UnSelectedSprite;
        }
        else
        {
            buttonColor = new Color(1, 1, 1, 1);
            buttonSprite = ButtonSelectedSprite;
        }
        Buttons[aButtonIndex].GetComponentInChildren<TextMeshProUGUI>().color = buttonColor;
        Buttons[aButtonIndex].image.sprite = buttonSprite;
    }

    private void ToggleState()
    {
        for (int i = 0; i < Costumizes.Count; i++)
        {
            if(mDifficultButtonSelectIndex != (int)Difficulties.CUSTOMIZE)
                Costumizes[i].isOn = mSelectedCustomize[i];
            else
                Costumizes[i].isOn = mChangedCustomize[i];
        }
    }
}
