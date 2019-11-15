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

    public delegate void OnBlockMoveHorizontal(Vector3 aDir);
    public static OnBlockMoveHorizontal blockMoveHorizontal;

    public delegate void OnBlockDropGradually();
    public static OnBlockDropGradually blockDropGradually;

    public delegate void OnBlockDropInstantly();
    public static OnBlockDropInstantly blockDropInstantly;

    public delegate void OnBlockRotate();
    public static OnBlockRotate blockRotate;

    public delegate void OnBlockInvert();
    public static OnBlockInvert blockInvert;

    public delegate void OnBlockSwapWithPreview(List<int> givingNumbers, ref List<int> retrieveNumbers);
    public static OnBlockSwapWithPreview blockSwapWithPreview;

    public delegate void OnChangePreviewOrder();
    public static OnChangePreviewOrder changePreviewOrder;

    private float mCurrentBlockNextDropTime = 0;

    public override void Start()
    {
        mPanelIndex = GUIPanelIndex.MAIN_GAME_PANEL;
        base.Start();

        GameSceneMain.getNextDropTime += GetNextDropTime;
    }

    private void OnDestroy()
    {
        GameSceneMain.getNextDropTime -= GetNextDropTime;
    }

    public override void Update()
    {
        base.Update();
        InputHandle();
        //if(ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_UP))
        //{
        //    mCurrentTotalScore += 100;
        //    onAddScore?.Invoke(mCurrentTotalScore, 100);
        //}
        //if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_DOWN))
        //{
        //    onAddLevelFilling?.Invoke(.1f);
        //}
        //if (ControlManager.Ins.MenuSelectButtonPressed())
        //{
        //    onAddCombo?.Invoke(mCurrentCombo);
        //    mCurrentCombo++;
        //}
    }

    private void InputHandle()
    {
        blockMoveHorizontal?.Invoke(ControlManager.Ins.MoveBlockHorizontal());

        if (ControlManager.Ins.DropBlockGradually(mCurrentBlockNextDropTime))
            blockDropGradually?.Invoke();

        if (ControlManager.Ins.DropBlockInstantly())
            blockDropInstantly?.Invoke();

        if (ControlManager.Ins.RotateBlock())
            blockRotate?.Invoke();

        if (ControlManager.Ins.InvertBlock())
            blockInvert?.Invoke();

        if(ControlManager.Ins.SwapPreview())
        { }

        if (ControlManager.Ins.ChangePreview())
            changePreviewOrder?.Invoke();

        if (ControlManager.Ins.GamePause())
        { }
    }

    private void GetNextDropTime(float aNextDropTime)
    {
        mCurrentBlockNextDropTime = aNextDropTime;
    }
}
