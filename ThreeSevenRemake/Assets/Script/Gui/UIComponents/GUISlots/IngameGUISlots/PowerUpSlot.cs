using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSlot : GuiSlotBase
{
    public List<PowerUpObject> PowerUpItems;

    private int mCurrentHighlightItemIndex = 0;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    private void NavigateItems(int aDirection)
    {
        if (mCurrentHighlightItemIndex + aDirection < 0)
        { }
        else if (mCurrentHighlightItemIndex + aDirection > 0)
        { }
        else
            mCurrentHighlightItemIndex += aDirection;
    }
}
