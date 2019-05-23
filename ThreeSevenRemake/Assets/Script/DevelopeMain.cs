using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopeMain : MonoBehaviour
{
    // Object sync with the unity
    public GameObject BlockObject;
    public Light DirectionalLight;

    // delegates
    public delegate void GameActive(bool anActive);
    public static GameActive gameIsPlaying;

    public delegate void OnCreateNewBlock(BlockDeveloping aNewBlock);
    public static OnCreateNewBlock createNewBlock;

    public delegate void OnScoreChange(int aNewScore);
    public static OnScoreChange scoreChanging;

    public delegate void OnLevelChange(int aLevelUpdate);
    public static OnLevelChange levelUpdate;

    public delegate void InitlizeResult(int aReachedLevel, string aSpendTimeString, int aBlockCount, int aTotalScore);
    public static InitlizeResult finalResult;

    public delegate void OnBlockLandedDebug(Dictionary<int, List<Cube>> aGrid);
    public static OnBlockLandedDebug blockLandedDebug;



    // variablers
    // objects
    private BlockDeveloping mCurrentBlock;

        // intergear
    private int mBlockCount = 0;
    private int mCurrentLevel = 1;

        // floats
    private float mNextDropTime = 0f;
    private float mDropRate = 1;
    private float mButtonDownNextDropTime = 0f;
    private float mButtonDownDropRate = .1f;

    // boolean
    private bool mIsPause = false;
    private bool mGameOver = false;
    private bool mBlockLanded = false;

    private void Awake()
    {
        GridData.Instance.GenerateGrid();
        UpdateDebugBoard();
    }

    private void Start()
    {
        // When the game start, begin delay for the first block to be created
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        // If mGameOver is equal to true, don't proceed futher of this 
        if (mGameOver)
        {
            // if mGameover == true, block and cubes will do the following things
                // Call the function to collapse the table
                // Call the function to display the result
            return;
        }

        // If the currentBlock is null or undergoing scoreing progression
        if (mCurrentBlock == null)
        {
            // the block was confirm nullified by the currentblock landed
            if(mBlockLanded)
            {
                if (BlockManager.Instance.AnyBlockPlayingAnimation())
                    return;

                UpdateDebugBoard();
                // if the block manager detect any scoring from the last landing block, the animation will be played
                if (BlockManager.Instance.CheckIfAnyBlocksIsFloating())
                    BlockManager.Instance.RearrangeBlocks();
                else if (BlockManager.Instance.IsScoring())
                    BlockManager.Instance.PlayScoringAnimation();
                else
                    CreateNewBlock();
            }
            
            return; // don't proceed futher of this block
        }
        CheckInput();
    }

    /// <summary>
    /// A checking method in use of call the "Drop" function in Block if the grid cell
    /// below the current block's mininum grid position is vacant.
    /// If it vaccant then the block will keep dropping or else it'll stop and the block's
    /// cube's number will be registrate into the grid's data
    /// </summary>
    /// <returns>if the cell below the block is vaccant it'll return true or else false</returns>
    private bool IsBlockDropping()
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
    private void CheckInput()
    {
        // input for move the block left if the left column is vacant
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            mCurrentBlock.MoveLeft();
        }

        // input for move the block right if the right column is vacant
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            mCurrentBlock.MoveRight();
        }

        // input for move the block downward one row if the row below is vacant
        // and if the time between each keypress has expired
        if((Input.GetKey(KeyCode.DownArrow) && Time.time > mButtonDownNextDropTime) || Time.time > mNextDropTime)
        {
            if (!GridData.Instance.IsCellVacant(mCurrentBlock.MinGridPos + Vector2Int.down) ||
                !GridData.Instance.IsCellVacant(mCurrentBlock.MaxGridPos + Vector2Int.down))
            {
                BlockManager.Instance.AddBlock(mCurrentBlock);

                UpdateDebugBoard();

                mCurrentBlock = null;
                mBlockLanded = true;
            }
            else
                mCurrentBlock.DropDown();

            mButtonDownNextDropTime = Time.time + mButtonDownDropRate;
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
            //if (blockLandedDebug != null)
            //    blockLandedDebug(GridData.Instance.Grid);

            mCurrentBlock.Swap();
        }

        // debug input for removing the current block to a new block.
        // for testing purpose

    }

    /// <summary>
    /// Scoring had been detected from the last block landed and the scoring progress will be active. 
    /// if scored the cube vanish animation plays and the position of the blocks
    /// in the grid will be rearranged
    /// </summary>
    private void ScoringProgress()
    {
        

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
        GameObject newBlock = Instantiate(BlockObject, GridData.Instance.StartWorldPosition, Quaternion.identity);
        newBlock.name = "Block " + mBlockCount.ToString();
        mBlockCount++;

        if (createNewBlock != null)
            createNewBlock(newBlock.GetComponent<BlockDeveloping>());

        if (newBlock.GetComponent<BlockDeveloping>() != null)
        {
            mCurrentBlock = newBlock.GetComponent<BlockDeveloping>();
        }

        // Get the block's droprate of the current level from GameManager
        mDropRate = GameManager.Instance.GetCurrentDroppingRate();

        mButtonDownNextDropTime = Time.time + mButtonDownDropRate;
        mNextDropTime = Time.time + mDropRate;

        mBlockLanded = false;
    }

    /// <summary>
    /// For debug purpose
    /// </summary>
    private void UpdateDebugBoard()
    {
        if (blockLandedDebug != null)
            blockLandedDebug(GridData.Instance.Grid);
    }
}
