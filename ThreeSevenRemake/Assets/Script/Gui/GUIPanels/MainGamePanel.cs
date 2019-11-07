using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGamePanel : GUIPanelBase
{
    public override void Start()
    {
        mPanelIndex = MenuPanelIndex.MAIN_GAME_PANEL;
        base.Start();
    }
}
