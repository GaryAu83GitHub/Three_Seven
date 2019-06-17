﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Script.Tools;

public class Oldmain : MonoBehaviour
{
    /// <summary>
    /// Prefab for the dropping blocks
    /// </summary>
    public GameObject BlockObject;

    public Light DirectionalLight;

    private enum TurningIndex
    {
        COUNTER_CLOCK_WISE = -1,
        CLOCK_WISE = 1,
    }

    public delegate void OnCreateNewBlock(OldBlock aNewBlock);
    public static OnCreateNewBlock createNewBlock;
    private OldBlock mCurrentBlock;

    private Vector3 mBlockStartPosition;

    private List<OldBlock> mLandedBlock = new List<OldBlock>();

    private int mBlockCount = 0;

    public delegate void OnScoreChange(int aNewScore);
    public static OnScoreChange scoreChanging;

    public delegate void OnBlockLandedDebug(Dictionary<int, List<Cube>> aGrid);
    public static OnBlockLandedDebug blockLandedDebug;

    private int mTotalScores = 0;
    //private int mScoreMultiplies = 0;

    public delegate void OnLevelChange(int aLevelUpdate);
    public static OnLevelChange levelUpdate;

    private int mCurrentLevel = 1;
    private int mNextLevelUpScore = 250;

    // the elapse time between each drop of the falling block
    private float mDropRate = 1f;
    private float mNextDropTime = 0;

    private float mButtonDownDropRate = .1f;
    private float mButtonDownNextDropTime = 0f;

    public delegate void GameActive(bool anActive);
    public static GameActive gameIsPlaying;

    public delegate string TimeSpendToString();
    public static TimeSpendToString spendTime;
    public string mTimeSpend = "";

    private bool mIsPause = false;
    private bool mGameOver = false;
    private bool mBlockLanded = false;

    public delegate void InitlizeResult(int aReachedLevel, string aSpendTimeString, int aBlockCount, int aTotalScore);
    public static InitlizeResult finalResult;

    private void Awake()
    {   
        GridManager.Instance.GenerateGrid();
        mBlockStartPosition = GridManager.Instance.StartWorldPosition;
        if (blockLandedDebug != null)
            blockLandedDebug(GridManager.Instance.Grid);
    }

    void Start ()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        CreateNewBlock();

        if (levelUpdate != null)
            levelUpdate(mCurrentLevel);

        if (gameIsPlaying != null)
            gameIsPlaying(true);
    }
	
	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.P) && !mGameOver)
        //    PauseGame();

        if (mGameOver)
        {
            TowerCollapse();
            GameResult();
            return;
        }

        if (mCurrentBlock == null)
            return;

        if(mBlockLanded)
        {
            ScoringPart();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && GridManager.Instance.AvailableMove(Vector2Int.left, mCurrentBlock))
        {
            mCurrentBlock.MoveLeft();
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) && GridManager.Instance.AvailableMove(Vector2Int.right, mCurrentBlock))
        {
            mCurrentBlock.MoveRight();
        }

        else if ((Input.GetKey(KeyCode.DownArrow) && Time.time > mButtonDownNextDropTime) || Time.time > mNextDropTime)
        {
            if (GridManager.Instance.AvailableMove(Vector2Int.down, mCurrentBlock))
                mCurrentBlock.DropDown();
            else
                Landed();

            mButtonDownNextDropTime = Time.time + mButtonDownDropRate;
            mNextDropTime = Time.time + mDropRate;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && GridManager.Instance.AvailableRot((int)TurningIndex.CLOCK_WISE, mCurrentBlock))
            mCurrentBlock.TurnClockWise();

        else if (Input.GetKeyDown(KeyCode.Space))
            mCurrentBlock.Swap();
        else if (Input.GetKeyDown(KeyCode.End))
            ReplaceTheBlock();

    }

    private void Reset()
    {
        mBlockCount = 0;
        mCurrentLevel = 1;
        mTotalScores = 0;
        mNextLevelUpScore = 250;
        mDropRate = 1f;

        mLandedBlock.Clear();
    }

    private void PauseGame()
    {
        mIsPause = !mIsPause;

        gameIsPlaying(mIsPause);

        if (mIsPause)
        {
            Time.timeScale = 0;
            DirectionalLight.intensity = 0;
        }
        else
        {
            Time.timeScale = 1;
            DirectionalLight.intensity = 1.2f;
        }
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, GridManager.Instance.StartWorldPosition, Quaternion.identity);
        newBlock.name = "Block " + mBlockCount.ToString();
        mBlockCount++;

        if(createNewBlock != null)
            createNewBlock(newBlock.GetComponent<OldBlock>());

        if (newBlock.GetComponent<OldBlock>() != null)
            mCurrentBlock = newBlock.GetComponent<OldBlock>();

        mNextDropTime = Time.time + mDropRate;

        mBlockLanded = false;
    }

    private void ReplaceTheBlock()
    {
        Destroy(mCurrentBlock.gameObject);
        CreateNewBlock();
    }

    /// <summary>
    /// When the current dropping block had landed on ground or on another previous landed block
    /// its cubes will be registrated into the grid for scoring purpose and in the same time be store
    /// into the list of landed blocks.
    /// It'll go through if the current block had manage any scores and if does it'll rearrange all blocks
    /// that had their cubes involved to change, and lastly create a new block at the top of the starting 
    /// position.
    /// </summary>
    private void Landed()
    {
        // set the current block to landing
        mCurrentBlock.Landing();
        // these delegate is use for debug and developing purpose
        if (blockLandedDebug != null)
            blockLandedDebug(GridManager.Instance.Grid);

        // store the landing block into the list of remaining blocks in the grid
        mLandedBlock.Add(mCurrentBlock);

        if(GridManager.Instance.PassingLimit())
        {
            mGameOver = true;
            gameIsPlaying(!mGameOver);
            return;
        }

        // sort the list of landings block base on the minimum y position (this is more than a matter of "just in case" to prevent
        // blocks landing miss placing)
        var sortBlockList = mLandedBlock.OrderBy(b => b.MinGridPos.y).ThenBy(b => b.MinGridPos.x);
        mLandedBlock = sortBlockList.ToList();

        mBlockLanded = true;
    }

    private void ScoringPart()
    {
        // the current block check if it score
        mCurrentBlock.Scoring();
        if (mCurrentBlock.IsScoring)
        {
            // when this block is scoring, it'll involved other blocks around it so all the block that
            // was involved will have their cubes position in the grid to be nullify.
            NullifyGridFromScoringBlocks();

            // how many times the block scored will be added into the score interger
            ScoreCalclulation(mCurrentBlock.ScoringTimes);
            //mScoreMultiplies += mCurrentBlock.ScoringTimes;


            // All blocks that was involve have too rearrange their position or been removed.
            Rearrangement();
            if (blockLandedDebug != null)
                blockLandedDebug(GridManager.Instance.Grid);
        }
        else
        {
            ScoreCalclulation(0);
        }

        // Calculate the scoring for either lading the block or fulfill the multiply scoring condition
        

        // called for create the a new falling block.
        CreateNewBlock();
    }

    private void Rearrangement()
    {
        // Check for any block is scoring
        foreach (var b in mLandedBlock.ToList())
        {
            // if it scoring the block will changed and depending on how it change it'll act different 
            if (b.IsScoring)
            {
                b.AfterScoreChange();
                
                // if both cube in the clock scores, it'll be remove from the storage of landing blocks
                if(b.Cubes.Count == 0)
                {
                    Destroy(b.gameObject);
                    mLandedBlock.Remove(b);
                }                
            }

            // if below the block is empty, it'll keep droping until it lands on the ground or on another block
            while (GridManager.Instance.AvailableMove(Vector2Int.down, b))
            {
                b.DropDown();
            }
            b.Landing();
        }
    }

    private void ScoreCalclulation(int aScoreMultiply)
    {
        mTotalScores += mCurrentLevel + (mCurrentLevel * aScoreMultiply);
        if(scoreChanging != null)
            scoreChanging(mTotalScores);

        if(mTotalScores >= mNextLevelUpScore)
        {
            mCurrentLevel++;
            if(levelUpdate != null)
                levelUpdate(mCurrentLevel);

            mNextLevelUpScore += mNextLevelUpScore + (mNextLevelUpScore / 2);
            if (mCurrentLevel % 3 == 0)
                mDropRate = (mDropRate * .95f);
        }

    }

    /// <summary>
    /// Nullify all landed block that was involved in scoring in the grid.
    /// </summary>
    private void NullifyGridFromScoringBlocks()
    {
        foreach (OldBlock b in mLandedBlock)
        {
            if(b.IsScoring)
                GridManager.Instance.NullifyGridWithBlock(b);
        }
    }

    private void TowerCollapse()
    {
        foreach (OldBlock b in mLandedBlock)
            b.GetComponent<Rigidbody>().useGravity = true;
    }

    private void GameResult()
    {
        mTimeSpend = spendTime();
        if(finalResult != null)
            finalResult(mCurrentLevel, mTimeSpend, mBlockCount, mTotalScores);
    }
}