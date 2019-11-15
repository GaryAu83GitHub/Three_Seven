using UnityEngine;
using UnityEngine.UI;


public class SettingSlotBase : GuiSlotBase
{
    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;

    public delegate void OnGameplaySettingHaveChange(ref GameplaySettingData aSettingData);
    public static OnGameplaySettingHaveChange gameplaySettingHaveChange;

    protected bool mLockParentInput = false;
    public bool LockParentInput { get { return mLockParentInput; } }

    protected Image mSelectingImage;

    protected GameplaySettingData mGameplaySettingData = new GameplaySettingData();

    public override void Start()
    {
        base.Start();
        mSelectingImage = GetComponent<Image>();
    }

    public override void Update()
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

    public override void ActivatingSlot(bool isActive)
    {
        base.ActivatingSlot(isActive);//mSlotActivate = isActive;
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
