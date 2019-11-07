using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsContainerBase : MonoBehaviour
{
    public List<SettingSlotBase> SettingSlots;

    public Sprite ActiveFrameSprite;
    public Sprite DeactiveFrameSprite;

    private Image BackgroundFrame;
    private CanvasGroup CanvasGroup;

    protected SettingSlotBase mCurrentSelectedSlot = null;
    protected int mCurrentSelectingSlotIndex = 0;
    protected int mCurrentDisplaySlotsCount = 0;

    protected bool mActiveContainer = false;

    protected virtual void Start()
    {
        BackgroundFrame = GetComponent<Image>();
        CanvasGroup = GetComponent<CanvasGroup>();
        DeselectAllSlots();
        mCurrentDisplaySlotsCount = SettingSlots.Count;
    }

    protected virtual void Update()
    {
        ContainerDisplay();
        if (!mActiveContainer)
            return;
        if (mCurrentSelectedSlot != null && mCurrentSelectedSlot.LockParentInput)
            return;

        Input();
    }

    protected virtual void Input()
    { }

    public virtual void Enter()
    {
        ActiveContainer(true);
        DeselectAllSlots();
        mCurrentSelectedSlot = SettingSlots[mCurrentSelectingSlotIndex];
        mCurrentSelectedSlot.ActivatingSlot(true);
    }

    public virtual void Exit()
    {
        ActiveContainer(false);
        DeselectAllSlots();
    }

    public void ActiveContainer(bool isActive)
    {
        mActiveContainer = isActive;
    }

    protected void ContainerDisplay()
    {
        if (mActiveContainer)
        {
            BackgroundFrame.sprite = ActiveFrameSprite;
            CanvasGroup.alpha = 1f;
            CanvasGroup.interactable = true;
        }
        else
        {
            BackgroundFrame.sprite = DeactiveFrameSprite;
            CanvasGroup.alpha = .5f;
            CanvasGroup.interactable = false;
        }
    }

    protected void CheckNumberOfActiveSlots()
    {
        mCurrentDisplaySlotsCount = 0;
        for (int i = 0; i < SettingSlots.Count; i++)
        {
            if (SettingSlots[i].gameObject.activeInHierarchy)
                mCurrentDisplaySlotsCount++;
        }

        //Debug.Log(mCurrentDisplaySlotsCount);
    }

    private void DeselectAllSlots()
    {
        for (int i = 0; i < SettingSlots.Count; i++)
            SettingSlots[i].ActivatingSlot(false);
    }
}
