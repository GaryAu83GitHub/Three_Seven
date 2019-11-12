using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGamePanel : GUIPanelBase
{
    public delegate void TempOnAddScore(int aNewTotalScore, int anAddOnScore);
    public static TempOnAddScore onAddScore;

    public delegate void TempOnAddLevelFilling(float aFillingValue);
    public static TempOnAddLevelFilling onAddLevelFilling;

    public delegate void TempOnAddCombo(int aComboCount);
    public static TempOnAddCombo onAddCombo;

    private int mCurrentTotalScore = 0;
    private int mCurrentCombo = 0;

    public override void Start()
    {
        mPanelIndex = GUIPanelIndex.MAIN_GAME_PANEL;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if(ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_UP))
        {
            mCurrentTotalScore += 100;
            onAddScore?.Invoke(mCurrentTotalScore, 100);
        }
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_DOWN))
        {
            onAddLevelFilling?.Invoke(.1f);
        }
        if (ControlManager.Ins.MenuSelectButtonPressed())
        {
            onAddCombo?.Invoke(mCurrentCombo);
            mCurrentCombo++;
        }
    }
}
