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

        if(Input.GetKeyDown(KeyCode.S))
        {
            if (GridManager.Instance.AvailableMove(Vector2Int.down, mCurrentBlock))
                mCurrentBlock.DropDown();
            else
                Landed();
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
    }

    private void ReplaceTheBlock()
    {
        Destroy(mCurrentBlock.gameObject);
        CreateNewBlock();
    }

    private void Landed()
    {
        mCurrentBlock.Landing();
        mLandedBlock.Add(mCurrentBlock);

        mCurrentBlock.Scoring();
        if (mCurrentBlock.IsScoring)
        {
            mScores += mCurrentBlock.ScoringTimes;
            Rearrangement();
        }
        CreateNewBlock();
    }

    private void Rearrangement()
    {
        List<Block> floatingBlocks = new List<Block>();

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
                // if there's one cube remaing, it'll check if below it is vacant and if does the block will drop until it landed on the ground or on another block
                else
                {
                    while (GridManager.Instance.AvailableMove(Vector2Int.down, b))
                    {
                        b.DropDown();
                    }
                    b.Landing();
                }
            }
        }
    }

    private void NullifyGridFromScoringBlocks()
    {
        foreach (Block b in mLandedBlock)
            GridManager.Instance.NullifyGridWithBlock(b);
    }
}
