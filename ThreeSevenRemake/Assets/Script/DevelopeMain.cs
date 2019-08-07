﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopeMain : MonoBehaviour
{
    // Object sync with the unity
    public GameObject BlockObject;
    public GameObject GuideBlockObject;
    public GameObject LimitLine;
    public GameObject TableCover;
    public Light DirectionalLight;

    // delegates
    public delegate void GameActive(bool anActive);
    public static GameActive gameIsPlaying;

    public delegate void OnCreateNewBlock(Block aNewBlock);
    public static OnCreateNewBlock createNewBlock;
    
    public delegate void InitlizeResult();
    public static InitlizeResult finalResult;

    public delegate void OnBlockLandedDebug(Dictionary<int, List<Cube>> aGrid);
    public static OnBlockLandedDebug blockLandedDebug;

    // variablers
    // objects
    private Block mCurrentBlock;
    private GuideBlock mGuideBlock;

        // intergear
    private int mBlockCount = 0;

        // floats
    private float mNextDropTime = 0f;
    private float mDropRate = 1;
    private float mNextVerticalButtonDownTime = 0f;
    private float mNextHorizontalButtonDownTime = 0f;

    // boolean
    private bool mGameInProgress = false;
    private bool mBlockLanded = false;

    private void Awake()
    {
        GridData.Instance.GenerateGrid();
        UpdateDebugBoard();
    }

    private void Start()
    {
        GameOverMenu.leaveTheGame += ResetGame;
        PauseMenu.leaveTheGame += ResetGame;

        LimitLine.transform.position += new Vector3(0f, .25f + (.5f * GameSettings.Instance.LimitHigh), 0f);
        // When the game start, begin delay for the first block to be created
        StartCoroutine(GameStart());
    }

    private void OnDestroy()
    {
        GameOverMenu.leaveTheGame -= ResetGame;
        PauseMenu.leaveTheGame -= ResetGame;
    }

    private void Update()
    {
        TableCover.SetActive(PauseMenu.GameIsPause);
        // If mGameOver is equal to true, don't proceed futher of this 
        if (BlockManager.Instance.BlockPassedGameOverLine())
        {
            // Call the function to display the result
            finalResult?.Invoke();
            return;
        }
        
        // If the currentBlock is null or undergoing scoreing progression
        if (mCurrentBlock == null)
        {
            // the block was confirm nullified by the currentblock landed
            if (mBlockLanded)
            {
                //BlockManager.Instance.BlockPassedGameOverLine();
                if (BlockManager.Instance.AnyBlockPlayingAnimation())
                    return;

                UpdateDebugBoard();
                // if the block manager detect any scoring from the last landing block, the animation will be played
                //if (BlockManager.Instance.CheckIfAnyBlocksIsFloating())
                //    BlockManager.Instance.RearrangeBlocks();
                if (BlockManager.Instance.CheckIfAnyBlocksIsFloatingNew())
                    BlockManager.Instance.RearrangeBlockNew();
                //else if (BlockManager.Instance.IsScoring())
                //    BlockManager.Instance.ScoreCalculationProgression();
                else if (BlockManager.Instance.IsScoringNew())
                    BlockManager.Instance.ScoreCalculationProgressionNew();
                else
                {
                    TaskManager.Instance.ChangeObjective();
                    CreateNewBlock();
                }
            }
            
            return; // don't proceed futher of this block
        }
        CheckInput();
    }

    /// <summary>
    /// All navigation input to the block include suspend the game is done from 
    /// this block
    /// </summary>
    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            RewindTurn();
        }

        if (PauseMenu.GameIsPause)
            return;

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && Time.time > mNextHorizontalButtonDownTime)
        {
            // input for move the block left if the left column is vacant
            if (Input.GetKey(KeyCode.LeftArrow))
                mCurrentBlock.MoveLeft();

            // input for move the block right if the right column is vacant
            if (Input.GetKey(KeyCode.RightArrow))
                mCurrentBlock.MoveRight();

            mNextHorizontalButtonDownTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;
        }

        // input for move the block downward one row if the row below is vacant
        // and if the time between each keypress has expired
        if((Input.GetKey(KeyCode.DownArrow) && Time.time > mNextVerticalButtonDownTime) || Time.time > mNextDropTime)
        {
            if(!mCurrentBlock.CheckIfCellIsVacantBeneath())
            {
                //BlockManager.Instance.AddBlock(mCurrentBlock);
                BlockManager.Instance.AddNewOriginalBlock(mCurrentBlock);
                GameManager.Instance.LandedBlockCount++;
                UpdateDebugBoard();

                mCurrentBlock = null;
                mBlockLanded = true;
                GuideBlockObject.SetActive(GameSettings.Instance.GetGuideBlockVisible(false));
            }
            else
                mCurrentBlock.DropDown();

            mNextVerticalButtonDownTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;//mButtonDownDropRate;
            mNextDropTime = Time.time + mDropRate;
        }

        // input for rotate the block clockwise if the column or row of where the block
        // rotate to is vacant
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            mCurrentBlock.Rotate();
        }

        // input for swaping the cubes value inside the block
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mCurrentBlock.Swap();
        }

        if(Input.GetKeyDown(KeyCode.RightControl))
        {
            mCurrentBlock.SwapWithPreviewBlock();
        }

        if (mCurrentBlock != null)
        {
            mGuideBlock.SetupGuideBlock(mCurrentBlock);
            mGuideBlock.SetPosition(mCurrentBlock);
        }
        
    }
    
    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);


        TaskManager.Instance.StartFirstSetOfObjective();
        //GameManager.Instance.SetupGameset();
        CreateNewBlock();
        
        mGameInProgress = true;
        gameIsPlaying?.Invoke(mGameInProgress);
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, GridData.Instance.StartWorldPosition, Quaternion.identity, transform);
        newBlock.name = "Block " + mBlockCount.ToString();
        mBlockCount++;

        createNewBlock?.Invoke(newBlock.GetComponent<Block>());

        if (newBlock.GetComponent<Block>() != null)
        {
            mCurrentBlock = newBlock.GetComponent<Block>();
        }


        mGuideBlock = GuideBlockObject.GetComponent<GuideBlock>();
        //mGuideBlock.SetupGuideBlock(mCurrentBlock);
        GuideBlockObject.SetActive(GameSettings.Instance.GetGuideBlockVisible(true));

        // Get the block's droprate of the current level from GameManager
        mDropRate = GameManager.Instance.GetCurrentDroppingRate();

        mNextVerticalButtonDownTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;//mButtonDownDropRate;
        mNextHorizontalButtonDownTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;

        mNextDropTime = Time.time + mDropRate;

        mBlockLanded = false;

        BlockManager.Instance.ResetCombo();

        RecordingManager.Instance.Record(new TurnData(mCurrentBlock));
    }

    /// <summary>
    /// For debug purpose
    /// </summary>
    private void UpdateDebugBoard()
    {
        blockLandedDebug?.Invoke(GridData.Instance.Grid);
    }

    private void ResetGame()
    {
        BlockManager.Instance.Reset();
        GameManager.Instance.Reset();
        GameSettings.Instance.Reset();
    }

    private void RewindTurn()
    {
        if (RecordingManager.Instance.RecordCount < 2)
            return;

        int childs = transform.childCount;
        for (int i = childs - 1; i > -1; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        GridData.Instance.GenerateGrid();
        BlockManager.Instance.ResetBlockList();

        StartCoroutine(Rewind());
        ////GameObject.Destroy(mCurrentBlock.gameObject);
        ////mBlockCount = 0;

        //TurnData data = RecordingManager.Instance.Rewind();

        //foreach(Block b in data.Blocks)
        //{
        //    GameObject rewindBlock = Instantiate(b.gameObject, new Vector3(b.RootCube.GridPos.x * Constants.CUBE_GAP_DISTANCE, b.RootCube.GridPos.y * Constants.CUBE_GAP_DISTANCE, 0f), Quaternion.identity, transform);
        //    rewindBlock.name = b.gameObject.name;
        //    //mBlockCount++;
        //    BlockManager.Instance.AddRewindBlock(rewindBlock.GetComponent<Block>());
        //}

        //mCurrentBlock = Instantiate(data.ThisTurnFallingBlock.gameObject, GridData.Instance.StartWorldPosition, Quaternion.identity, transform).GetComponent<Block>();
        ////GridData.Instance.SetGridAfterData(data.Blocks);
        //GameManager.Instance.RewindNextNumber(data.ThisTurnNextBlock);
    }

    private IEnumerator Rewind()
    {
        yield return new WaitForSeconds(.5f);

        TurnData data = RecordingManager.Instance.Rewind();

        foreach (Block b in data.Blocks)
        {
            GameObject rewindBlock = Instantiate(BlockObject, new Vector3(b.RootCube.GridPos.x * Constants.CUBE_GAP_DISTANCE, b.RootCube.GridPos.y * Constants.CUBE_GAP_DISTANCE, 0f), Quaternion.identity, transform);
            rewindBlock.name = b.BlockName;
            //mBlockCount++;
            BlockManager.Instance.AddRewindBlock(rewindBlock.GetComponent<Block>());
        }

        //mCurrentBlock = Instantiate(data.ThisTurnFallingBlock.gameObject, GridData.Instance.StartWorldPosition, Quaternion.identity, transform).GetComponent<Block>();
        //GridData.Instance.SetGridAfterData(data.Blocks);
        GameManager.Instance.RewindNextNumber(data.ThisTurnNextBlock);

    }
}
