using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChallengeSlot : SettingSlotWithToggles
{
    private List<bool> mChallengeDigitIsOn = new List<bool>();

    public override void Start()
    {
        EnableChallenges(GameSettings.Instance.EnableScoringMethods);
        base.Start();
    }

    public override void SetSlotValue(GameplaySettingData aData)
    {
        EnableChallenges(aData.SelectEnableDigits);
    }

    protected override void ChangeGameplaySetting()
    {
        mGameplaySettingData.SetChallengeDigit(mChallengeDigitIsOn);
        base.ChangeGameplaySetting();
    }

    protected override void SwitchToggle()
    {
        base.SwitchToggle();
        mChallengeDigitIsOn[mCurrentSelectToggleIndex] = !mChallengeDigitIsOn[mCurrentSelectToggleIndex];
        SetToggleCheckbox();
    }

    private void EnableChallenges(List<bool> someDigitEnables)
    {
        for(int i = 0; i < someDigitEnables.Count; i++)
        {
            if (mChallengeDigitIsOn.Count == i)
                mChallengeDigitIsOn.Add(someDigitEnables[i]);
            else
                mChallengeDigitIsOn[i] = someDigitEnables[i];
        }
        SetToggleCheckbox();
    }

    private void SetToggleCheckbox()
    {
        for (int i = 0; i < Toggles.Count; i++)
            Toggles[i].isOn = mChallengeDigitIsOn[i];
    }
}
