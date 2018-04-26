using System.Collections;
using System.Collections.Generic;
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
                BlockLanded(mCurrentBlock);
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

    private void BlockLanded(Block aBlock)
    {
        aBlock.Landing();
        mLandedBlock.Add(aBlock);
        
        aBlock.Scoring();
        if (aBlock.IsScoring)
            mScores += aBlock.ScoringTimes;

        List<Block> scoredBoxes = new List<Block>();

        foreach (Block b in mLandedBlock)
        {
            if (b.IsScoring)
            {
                b.AfterScoreChange();
                scoredBoxes.Add(b);
            }
        }

        for(int i = scoredBoxes.Count -1; i > -1; i--)
        {
            if (scoredBoxes[i].Cubes.Count == 0)
            {
                Destroy(scoredBoxes[i].gameObject);
                mLandedBlock.Remove(scoredBoxes[i]);
                scoredBoxes.Remove(scoredBoxes[i]);
            }
            else
            {
                while(GridManager.Instance.AvailableMove(Vector2Int.down, scoredBoxes[i]))
                {
                    scoredBoxes[i].DropDown();
                }
                scoredBoxes[i].Landing();
            }
        }

        /*for (int i = mLandedBlock.Count - 1; i > 0; i--)
        {
            if (mLandedBlock[i].IsScoring)
            {
                mLandedBlock[i].AfterScoreChange();
                if (mLandedBlock[i].Cubes.Count == 0)
                {
                    Destroy(mLandedBlock[i].gameObject);
                    mLandedBlock.RemoveAt(i);
                }
                else
                    scoredBoxes.Add(mLandedBlock[i]);
            }
        }*/

        CreateNewBlock();
    }
}
