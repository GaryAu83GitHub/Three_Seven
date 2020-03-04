using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetActiveControllSlot : SettingSlotWithToggles
{
    public Sprite SelectedToggleSprite;

    private ControlType mSelectedControlType = ControlType.XBOX_360;

    public delegate void OnSwitchControlType(ControlType aSelectType);
    public static OnSwitchControlType switchControlType;

    public delegate void OnDisplaySelectControlBinds(ControlType aSelectType);
    public static OnDisplaySelectControlBinds displaySelectControlBinds;

    private List<Image> mToggleBackground = new List<Image>();

    public override void Start()
    {
        base.Start();

        foreach(Toggle toggle in Toggles)
            mToggleBackground.Add(toggle.gameObject.GetComponent<Image>());
    }

    protected override void ChangeGameplaySetting()
    {
        switchControlType?.Invoke(mSelectedControlType);
    }

    protected override void SwitchToggle()
    {
        for(int i = 0; i < Toggles.Count; i++)
        {
            Toggles[i].isOn = false;
            if(i == mCurrentSelectToggleIndex)
            {
                Toggles[i].isOn = true;
                mSelectedControlType = (ControlType)mCurrentSelectToggleIndex;
            }
        }
    }

    protected override void ActiveToggle()
    {
        for (int i = 0; i < mToggleBackground.Count; i++)
            mToggleBackground[i].sprite = (i == mCurrentSelectToggleIndex) ? SelectedToggleSprite : UnSelectedSprite;

        displaySelectControlBinds?.Invoke((ControlType)mCurrentSelectToggleIndex);
    }

    public void SetupSelectiveToggles(ref ControlType anActiveControlType)
    {
        if (!ControlManager.Ins.CheckHaveControlOf(ControlType.XBOX_360))
        {
            Toggles[(int)ControlType.XBOX_360].gameObject.SetActive(false);
            mActiveToggleCount = 1;
        }

        anActiveControlType = ControlManager.Ins.ActiveControlType;

        SetSelectedControlType(anActiveControlType);
    }

    public void SetSelectedControlType(ControlType aControlType)
    {
        mSelectedControlType = aControlType;
        mCurrentSelectToggleIndex = (int)mSelectedControlType;
        ActiveToggle();
        SwitchToggle();
    }
}
