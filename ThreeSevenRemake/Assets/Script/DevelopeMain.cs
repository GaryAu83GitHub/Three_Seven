using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Script.Tools;

public class DevelopeMain : MonoBehaviour
{
    public GameObject BlockObject;

    public Light DirectionalLight;

    // delegates
    public delegate void GameActive(bool anActive);
    public static GameActive gameIsPlaying;

    public delegate void OnCreateNewBlock(Block aNewBlock);
    public static OnCreateNewBlock createNewBlock;

    public delegate void OnScoreChange(int aNewScore);
    public static OnScoreChange scoreChanging;

    public delegate void OnLevelChange(int aLevelUpdate);
    public static OnLevelChange levelUpdate;

    public delegate void InitlizeResult(int aReachedLevel, string aSpendTimeString, int aBlockCount, int aTotalScore);
    public static InitlizeResult finalResult;

    public delegate void OnBlockLandedDebug(Dictionary<int, List<int>> aGrid);
    public static OnBlockLandedDebug blockLandedDebug;

    // variablers
    // objects
    private Block mCurrentBlock;

        // vectors
    private Vector3 mBlockStartPosition;

        // intergear
    private int mBlockCount = 0;
    private int mCurrentLevel = 1;
    private int mNextLevelUpScore = 250;
    private int mTotalScores = 0;

        // floats
    private float mNextDropTime = 0f;
    private float mDropRate = 1f;
    private float mButtonDownNextDropTime = 0f;
    private float mButtonDownDropRate = .1f;

    // boolean
    private bool mIsPause = false;
    private bool mGameOver = false;
    private bool mBlockLanded = false;

    private void Awake()
    {
        GridData.Instance.GenerateGrid();
        if (blockLandedDebug != null)
            blockLandedDebug(GridData.Instance.Grid);
    }

    private void Start()
    {
        // When the game start, begin delay for the first block to be created
        // StartCoroutine(StartGame())
    }

    private void Update()
    {
        // If mGameOver is equal to true, don't proceed futher of this 
        if (mGameOver)
        {
            // block and do the following things
                // Call the function to collapse the table
                // Call the function to display the result
            return;
        }
        NavigationInput();

        // If the currentBlock is null or undergoing scoreing progression
        if (mCurrentBlock == null)
        {
            // don't proceed futher of this block
            return;
        }

        

        // If the currenBlock has landed
            // Call the function for Scoring calcultating
            // If not proceed further


    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool BlockDropping()
    {
        // navigate the block as long the lower row still is vacant

        // check if the current block can drop to the row below it
        // if the row below is not vacant
            // put in the block into the collection of landed block
            // set the boolean for check for scoring to true
            // return the block has stopped dropping

        return true;
    }

    /// <summary>
    /// All navigation input to the block include suspend the game is done from 
    /// this block
    /// </summary>
    private void NavigationInput()
    {
        // input for move the block left if the left column is vacant

        // input for move the block right if the right column is vacant

        // input for move the block downward one row if the row below is vacant
        // and if the time between each keypress has expired

        // input for rotate the block clockwise if the column or row of where the block
        // rotate to is vacant

        // input for swaping the cubes value inside the block

        // debug input for removing the current block to a new block.
        // for testing purpose

        if(Input.GetKey(KeyCode.Space))
        {
            if (blockLandedDebug != null)
                blockLandedDebug(GridData.Instance.Grid);
        }
    }

    /// <summary>
    /// Checking for if the just landed block and those that have rows belows 
    /// vacant and dropped scored
    /// if scored the cube vanish animation plays and the position of the blocks
    /// in the grid will be rearranged
    /// </summary>
    /// <returns>return if the scoring is under progress</returns>
    private bool UndergoingScoringProgression()
    {

        return true;
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

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, GridManager.Instance.StartWorldPosition, Quaternion.identity);
        newBlock.name = "Block " + mBlockCount.ToString();
        mBlockCount++;

        if (createNewBlock != null)
            createNewBlock(newBlock.GetComponent<Block>());

        if (newBlock.GetComponent<Block>() != null)
            mCurrentBlock = newBlock.GetComponent<Block>();

        mNextDropTime = Time.time + mDropRate;

        //mBlockLanded = false;
    }
}
