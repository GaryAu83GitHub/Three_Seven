using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGamePanel : GUIPanelBase
{
    public GameObject LeftContainer;
    public GameObject RightContainer;

    public delegate void TempOnAddScore(int aNewTotalScore, int anAddOnScore);
    public static TempOnAddScore onAddScore;

    public delegate void TempOnAddLevelFilling(float aFillingValue);
    public static TempOnAddLevelFilling onAddLevelFilling;

    public delegate void TempOnAddCombo(int aComboCount);
    public static TempOnAddCombo onAddCombo;

    public delegate void OnRoundStart();
    public static OnRoundStart roundStart;

    public delegate void OnSaveResult(ref ResultData aResultData);
    public static OnSaveResult saveResult;

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

    public delegate void OnBlockSwaping();
    public static OnBlockSwaping blockSwaping;

    public delegate void OnChangePreviewOrder();
    public static OnChangePreviewOrder changePreviewOrder;

    public delegate void OnDumpPreviewBlock();
    public static OnDumpPreviewBlock dumpPreviewBlock;

    public delegate void OnGatherResultData(ref ResultData aData);
    public static OnGatherResultData gatherResultData;

    public delegate void OnSendingResultData(ResultData aData);
    public static OnSendingResultData sendingResultData;

    private bool mGameRoundOver = false;
    public bool GameRoundOver { get { return mGameRoundOver; } }

    private Animator mAnimator;

    private float mCurrentBlockNextDropTime = 0;
    private bool mBlockLanded = false;
    private bool mPreviewFunctionEnable = true;

    public override void Start()
    {
        mPanelIndex = GUIPanelIndex.MAIN_GAME_PANEL;
        base.Start();

        LeftContainer.SetActive(false);
        RightContainer.SetActive(false);

        mAnimator = GetComponent<Animator>();

        GameSceneMain.roundStart += GuiEnter;
        GameSceneMain.getNextDropTime += GetNextDropTime;
        GameSceneMain.passingTheTop += GameOver;
        GameSceneMain.gameTimeOver += GameOver;

        PreviewNormalSlot.enabblePreviewInput += OnPreviewInputEnable;
        TaskManagerNew.enablePreviewFunction += OnPreviewInputEnable;

    }

    private void OnDestroy()
    {
        GameSceneMain.roundStart -= GuiEnter;
        GameSceneMain.getNextDropTime -= GetNextDropTime;
        GameSceneMain.passingTheTop -= GameOver;
        GameSceneMain.gameTimeOver -= GameOver;

        PreviewNormalSlot.enabblePreviewInput -= OnPreviewInputEnable;
        TaskManagerNew.enablePreviewFunction -= OnPreviewInputEnable;
    }

    public override void Update()
    {
        base.Update();
        if (!mBlockLanded && !mGameRoundOver)
            InputHandle();
    }

    private void InputHandle()
    {
        blockMoveHorizontal?.Invoke(ControlManager.Ins.MoveBlockHorizontal());

        if (!mPreviewFunctionEnable)
            return;

        if (ControlManager.Ins.DropBlockGradually(0) || mCurrentBlockNextDropTime <= 0f)
            blockDropGradually?.Invoke();

        if (ControlManager.Ins.DropBlockInstantly())
            blockDropInstantly?.Invoke();

        if (ControlManager.Ins.RotateBlock())
            blockRotate?.Invoke();

        if (ControlManager.Ins.InvertBlock())
            blockInvert?.Invoke();

        if (ControlManager.Ins.SwapPreview())
            blockSwaping?.Invoke();

        if (ControlManager.Ins.ChangePreview())
            changePreviewOrder?.Invoke();

        //if (ControlManager.Ins.DumpPreview())
        //    dumpPreviewBlock?.Invoke();

        if (ControlManager.Ins.GamePause())
        {
            //GuiExit();
        }
    }

    public void OnRoundStartAnimationEvent()
    {
        mAnimator.SetBool("RoundInProgress", true);
        roundStart?.Invoke();
    }

    public void OnRoundOverAnimationEvent()
    {
        ChangePanel(GUIPanelIndex.RESULT_PANEL);
    }

    public void OnGuiEnterAnimationEvent()
    {
        LeftContainer.SetActive(true);
        RightContainer.SetActive(true);
    }

    private void OnPreviewInputEnable(bool anEnable)
    {
        mPreviewFunctionEnable = anEnable;
    }

    private void GuiEnter()
    {
        mAnimator.SetTrigger("RoundBegin");
    }

    private void GuiExit()
    {
        ResultData tempData = new ResultData();
        gatherResultData?.Invoke(ref tempData);
        mAnimator.SetBool("RoundIsOver", true);
        sendingResultData?.Invoke(tempData);
    }

    private void GetNextDropTime(float aNextDropTime)
    {
        mCurrentBlockNextDropTime = aNextDropTime;
    }

    private void GetBlockHasLanded(bool hasBlockLanded)
    {
        mBlockLanded = hasBlockLanded;
    }

    private void GameOver()
    {
        mGameRoundOver = true;
        GuiExit();
    }
}