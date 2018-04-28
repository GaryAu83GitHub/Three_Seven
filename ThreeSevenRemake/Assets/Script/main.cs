using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class main : MonoBehaviour
{

    public GameObject BlockObject;

    private enum TurningIndex
    {
        COUNTER_CLOCK_WISE = -1,
        CLOCK_WISE = 1,
    }

    private Block mCurrentBlock;

    private Vector3 mBlockStartPosition;

    private List<Block> mLandedBlock = new List<Block>();

    private int mBlockCount = 0;

    [SerializeField]
    private int mScores = 0;

    private float mDropRate = 1f;
    private float mNextDropTime = 0;
    
    private void Awake()
    {
        GridManager.Instance.GenerateGrid();
        mBlockStartPosition = GridManager.Instance.StartWorldPosition;
    }

    void Start ()
    {
        CreateNewBlock();
	}
	
	void Update ()
    {
        // this input is use for developing purpose for discarding the current block and replace it with the next block.
        // In the same time a new next block will be generate
        // It can helped to get new block without filling up the grid
        if (Input.GetKeyDown(KeyCode.Space))
            ReplaceTheBlock();

        if (Input.GetKeyDown(KeyCode.A) && GridManager.Instance.AvailableMove(Vector2Int.left, mCurrentBlock))
        {
            mCurrentBlock.MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.D) && GridManager.Instance.AvailableMove(Vector2Int.right, mCurrentBlock))
        {
            mCurrentBlock.MoveRight();
        }

        if(Input.GetKeyDown(KeyCode.S) || Time.time > mNextDropTime)
        {
            if (GridManager.Instance.AvailableMove(Vector2Int.down, mCurrentBlock))
                mCurrentBlock.DropDown();
            else
                Landed();
            mNextDropTime = Time.time + mDropRate;
        }

        if (Input.GetKeyDown(KeyCode.Q) && GridManager.Instance.AvailableRot((int)TurningIndex.COUNTER_CLOCK_WISE, mCurrentBlock))
            mCurrentBlock.TurnCounterClockWise();

        if (Input.GetKeyDown(KeyCode.E) && GridManager.Instance.AvailableRot((int)TurningIndex.CLOCK_WISE, mCurrentBlock))
            mCurrentBlock.TurnClockWise();
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, GridManager.Instance.StartWorldPosition, Quaternion.identity);
        newBlock.name = "Block " + mBlockCount.ToString();
        mBlockCount++;

        if (newBlock.GetComponent<Block>() != null)
            mCurrentBlock = newBlock.GetComponent<Block>();

        mNextDropTime = Time.time + mDropRate;
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

        // store the landing block into the list of remaining blocks in the grid
        mLandedBlock.Add(mCurrentBlock);

        // sort the list of landings block base on the minimum y position (this is more than a matter of "just in case" to prevent
        // blocks landing miss placing)
        var sortBlockList = mLandedBlock.OrderBy(b => b.MinGridPos.y).ThenBy(b => b.MinGridPos.x);
        mLandedBlock = sortBlockList.ToList();

        // the current block check if it score
        mCurrentBlock.Scoring();
        if (mCurrentBlock.IsScoring)
        {
            // when this block is scoring, it'll involved other blocks around it so all the block that
            // was involved will have their cubes position in the grid to be nullify.
            NullifyGridFromScoringBlocks();

            // how many times the block scored will be added into the score interger
            mScores += mCurrentBlock.ScoringTimes;

            // All blocks that was involve have too rearrange their position or been removed.
            Rearrangement();
        }

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

    /// <summary>
    /// Nullify all landed block that was involved in scoring in the grid.
    /// </summary>
    private void NullifyGridFromScoringBlocks()
    {
        foreach (Block b in mLandedBlock)
        {
            if(b.IsScoring)
                GridManager.Instance.NullifyGridWithBlock(b);
        }
    }
}
