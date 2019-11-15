﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMain : MonoBehaviour
{
    public GameObject BlockObject;
    public GameObject GuideBlockObject;
    public GameObject LimitLine;
    public GameObject TableCover;

    public delegate void OnCreateNewBlock(Block aNewBlock);
    public static OnCreateNewBlock createNewBlock;

    public delegate void OnGetNextDropTime(float aNextDropTime);
    public static OnGetNextDropTime getNextDropTime;

    private Block mCurrentBlock;
    private GuideBlock mGuideBlock;

    private int mBlockCount = 0;
    private float mNextDropTime = 0f;

    private bool mBlockLanded = false;

    private void Awake()
    {
        GridManager.Instance.GenerateGrid();

        // only for debug purpose
        GenerateScoreCombinationPositions.Instance.GenerateCombinationPositions();
        TaskManagerNew.Instance.PrepareNewTaskSubjects();
    }

    void Start()
    {
        GUIPanelManager.Instance.StartWithPanel(GUIPanelIndex.MAIN_GAME_PANEL);

        MainGamePanel.blockMoveHorizontal += BlockMoveHorizontal;
        MainGamePanel.blockDropGradually += BlockDropGradually;
        MainGamePanel.blockDropInstantly += BlockDropInstantly;
        MainGamePanel.blockRotate += BlockRotate;
        MainGamePanel.blockInvert += BlockInvert;
        MainGamePanel.blockSwapWithPreview += BlockSwapWithPreview;

        LimitLine.transform.position += new Vector3(0f, .625f + (Constants.CUBE_GAP_DISTANCE * GameSettings.Instance.LimitHigh), 0f);

        StartCoroutine(GameStart());
    }

    private void OnDestroy()
    {
        MainGamePanel.blockMoveHorizontal -= BlockMoveHorizontal;
        MainGamePanel.blockDropGradually -= BlockDropGradually;
        MainGamePanel.blockDropInstantly -= BlockDropInstantly;
        MainGamePanel.blockRotate -= BlockRotate;
        MainGamePanel.blockInvert -= BlockInvert;
        MainGamePanel.blockSwapWithPreview -= BlockSwapWithPreview;
    }

    void Update()
    {
        TableCover.SetActive(PauseMenu.GameIsPause);

        if (mCurrentBlock == null && !BlockManager.Instance.GameOver)
        {
            CurrentBlockStatus();
            return;
        }

        if (BlockManager.Instance.BlockPassedGameOverLine())
            return;

        //InputHandle()
        if (mCurrentBlock != null)
        {
            mGuideBlock.SetupGuideBlock(mCurrentBlock);
            mGuideBlock.SetPosition(mCurrentBlock);
        }
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);

        TaskManagerNew.Instance.StartFirstSetOfTask();
        CreateNewBlock();
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, GridManager.Instance.StartWorldPosition, Quaternion.identity);
        newBlock.name = "FallingBlock";
        mBlockCount++;

        createNewBlock?.Invoke(newBlock.GetComponent<Block>());

        if (newBlock.GetComponent<Block>() != null)
            mCurrentBlock = newBlock.GetComponent<Block>();

        mGuideBlock = GuideBlockObject.GetComponent<GuideBlock>();
        GuideBlockObject.SetActive(true);

        ControlManager.Ins.ResetButtonPressTimer();
        mNextDropTime = GameManager.Instance.BlockNextDropTime;//Time.time + GameManager.Instance.DropRate;//mDropRate;

        mBlockLanded = false;
    }

    private void CurrentBlockStatus()
    {
        if(mBlockLanded)
        {
            { // these skall be encaps into the blackmanager
                if (BlockManager.Instance.AnyBlockPlayingAnimation())
                    return;

                if (BlockManager.Instance.CheckIfAnyBlocksIsFloating())
                    BlockManager.Instance.RearrangeBlock();
                else if (BlockManager.Instance.IsScoring())
                    BlockManager.Instance.ScoreCalculationProgression();
                else
                {
                    TaskManagerNew.Instance.ChangeTask();
                    CreateNewBlock();
                }
            }
        }
    }

    // These are the method subscribed to MainGamePanels delegate for navigate the block
    private void BlockMoveHorizontal(Vector3 aDir)
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.Move(aDir);
    }

    private void BlockDropGradually()
    {
        if (mCurrentBlock == null)
            return;

        if (!mCurrentBlock.CheckIfCellIsVacantBeneath())
            RegistrateNewLandedBlock();
        else
            mCurrentBlock.DropDown();

        getNextDropTime?.Invoke(GameManager.Instance.BlockNextDropTime);
    }

    private void BlockDropInstantly()
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.InstantDrop();
        RegistrateNewLandedBlock();
        getNextDropTime?.Invoke(GameManager.Instance.BlockNextDropTime);
    }

    private void BlockRotate()
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.RotateBlockUpgrade();
    }

    private void BlockInvert()
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.InvertBlock();
    }

    private void BlockSwapWithPreview(List<int> givenNumbers, ref List<int> retrieveNumbers)
    {
        if (mCurrentBlock == null)
            return;

        retrieveNumbers = new List<int>(mCurrentBlock.CubeNumbers);
        mCurrentBlock.SetCubeNumbers(givenNumbers);
    }


    private void RegistrateNewLandedBlock()
    {
        mCurrentBlock.name = "Block " + mBlockCount.ToString();
        BlockManager.Instance.AddNewOriginalBlock(mCurrentBlock);
        GameManager.Instance.LandedBlockCount++;
        //UpdateDebugBoard();

        mCurrentBlock = null;
        mBlockLanded = true;
        GuideBlockObject.SetActive(GameSettings.Instance.GetGuideBlockVisible(false));
    }
}
