using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpSlot : GuiSlotBase
{
    public List<PowerUpObject> PowerUpItems;

    public delegate void OnUsePowerUp(PowerUpType aType);
    public static OnUsePowerUp usePowerUp;

    private int mCurrentSelectIndex = 0;

    public override void Awake()
    {
        base.Awake();
        ScoreDisplayBox.powerUpCharging += ObjectCharging;
        MainGamePanel.usePowerUp += UsePowerUp;
        MainGamePanel.navigatePowerUps += NavigateItems;
    }

    public override void Start()
    {
        base.Start();
        mCurrentSelectIndex = 0;
        SelectItem(true);
    }

    private void OnDestroy()
    {
        ScoreDisplayBox.powerUpCharging -= ObjectCharging;
        MainGamePanel.usePowerUp -= UsePowerUp;
        MainGamePanel.navigatePowerUps -= NavigateItems;
    }

    private void ObjectCharging()
    {
        PowerUpItems.ForEach(x => x.Charging());
    }

    private void NavigateItems(int aDirection)
    {
        SelectItem(false);

        if (mCurrentSelectIndex + aDirection < 0)
            mCurrentSelectIndex = PowerUpItems.Count - 1;
        else if (mCurrentSelectIndex + aDirection >= PowerUpItems.Count)
            mCurrentSelectIndex = 0;
        else
            mCurrentSelectIndex += aDirection;

        SelectItem(true);
    }

    private void UsePowerUp()
    {
        if (PowerUpItems[mCurrentSelectIndex].PowerUpUseable)
        {
            PowerUpType powerType = PowerUpItems[mCurrentSelectIndex].Discharge();
            usePowerUp?.Invoke(powerType);
        }
    }

    private void SelectItem(bool isSelected)
    {
        PowerUpItems[mCurrentSelectIndex].Selected(isSelected);
    }
}
