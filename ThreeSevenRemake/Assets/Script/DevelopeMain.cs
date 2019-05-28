using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopeMain : MonoBehaviour
{
    // Object sync with the unity
    public GameObject BlockObject;
    public GameObject LimitLine;
    public Light DirectionalLight;

    // delegates
    public delegate void GameActive(bool anActive);
    public static GameActive gameIsPlaying;

    public delegate void OnCreateNewBlock(BlockDeveloping aNewBlock);
    public static OnCreateNewBlock createNewBlock;
    
    public delegate void InitlizeResult();
    public static InitlizeResult finalResult;

    public delegate void OnBlockLandedDebug(Dictionary<int, List<Cube>> aGrid);
    public static OnBlockLandedDebug blockLandedDebug;

    // variablers
    // objects
    private BlockDeveloping mCurrentBlock;

        // intergear
    private int mBlockCount = 0;

        // floats
    private float mNextDropTime = 0f;
    private float mDropRate = 1;
    private float mButtonDownNextDropTime = 0f;
    private readonly float mButtonDownDropRate = .1f;

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
        // When the game start, begin delay for the first block to be created
        StartCoroutine(GameStart());
    }

    private void Update()
    {
        
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
            if(mBlockLanded)
            {
                BlockManager.Instance.BlockPassedGameOverLine();
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
            if(!mCurrentBlock.CheckIfCellIsVacantBeneath())
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
            mCurrentBlock.Swap();
        }
        
    }
    
    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        
        LimitLine.transform.position += new Vector3(0f, .25f + (.5f * GameManager.Instance.SetLimitLineLevel(9)), 0f); 

        CreateNewBlock();
        mGameInProgress = true;
        gameIsPlaying?.Invoke(mGameInProgress);
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, GridData.Instance.StartWorldPosition, Quaternion.identity);
        newBlock.name = "Block " + mBlockCount.ToString();
        mBlockCount++;

        createNewBlock?.Invoke(newBlock.GetComponent<BlockDeveloping>());

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
        blockLandedDebug?.Invoke(GridData.Instance.Grid);
    }
}
