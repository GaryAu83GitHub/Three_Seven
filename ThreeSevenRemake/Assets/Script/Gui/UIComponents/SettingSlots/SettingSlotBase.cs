﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingSlotBase : MonoBehaviour
{
    public TextMeshProUGUI TitleText;

    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;

    public delegate void OnGameplaySettingHaveChange(ref GameplaySettingData aSettingData);
    public static OnGameplaySettingHaveChange gameplaySettingHaveChange;

    protected bool mLockParentInput = false;
    public bool LockParentInput { get { return mLockParentInput; } }

    protected CanvasGroup mCG;
    protected Image mSelectingImage;
    protected bool mSlotActivate = true;

    protected GameplaySettingData mGameplaySettingData = new GameplaySettingData();

    public virtual void Start()
    {
        mCG = GetComponent<CanvasGroup>();
        mSelectingImage = GetComponent<Image>();
    }

    public virtual void Update()
    {
        if (mSlotActivate)
            Input();
    }

    public virtual void SetSlotValue(GameplaySettingData aData)
    { }

    public virtual void Enter()
    {
        ActivatingSlot(true);
    }

    public virtual void Exit()
    {
        ActivatingSlot(false);
    }

    protected virtual void Input()
    {
        Navigation();
        MenuButtonPressed();
    }

    protected virtual void Navigation()
    { }

    protected virtual void MenuButtonPressed()
    { }

    public virtual void ActivatingSlot(bool isActive)
    {
        mSlotActivate = isActive;
        SlotAppearence();
    }

    protected virtual void ChangeGameplaySetting()
    {
        gameplaySettingHaveChange?.Invoke(ref mGameplaySettingData);
    }

    private void SlotAppearence()
    {
        mCG.interactable = mSlotActivate;
        if (mSlotActivate)
        {
            mSelectingImage.sprite = SelectedSprite;
            mCG.alpha = 1f;
        }
        else
        {
            mSelectingImage.sprite = UnSelectedSprite;
            mCG.alpha = .5f;
        }
    }
}
