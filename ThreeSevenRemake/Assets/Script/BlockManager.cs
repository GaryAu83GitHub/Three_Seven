using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ScoringGroupAchieveInfo
{
    private readonly TaskRank mObjectiveRank = TaskRank.X1;
    public TaskRank ObjectiveRank { get { return mObjectiveRank; } }

    private readonly List<Vector2Int> mGroupPositions = new List<Vector2Int>();
    public List<Vector2Int> GroupPosition { get { return mGroupPositions; } }
        
    public ScoringGroupAchieveInfo(TaskRank anObjectiveRank, List<Vector2Int> someGroupPositions)
    {
        mObjectiveRank = anObjectiveRank;
        mGroupPositions = someGroupPositions;
    }
}
/// <summary>
/// This class is use to managing the all blocks that had been created
/// It'll store all blocks that had landed into it collection and remove
/// blocks that don't have any cube in it.
/// 
/// It'll also registrate/unregistrate the blocks cube of the grid when the
/// block has landed or scoring
/// 
/// When cube are scoring, it call the play animation from cube, unregistrate
/// then from the grid and rearrange and sort the blocks according to which
/// row they currently are placed at
/// </summary>
public class BlockManager
{
    // The static instance for this manager class
    public static BlockManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new BlockManager();
            }
            return mInstance;
        }
    }
    private static BlockManager mInstance;

    public delegate void OnAchieveScoring(TaskRank anObjective, List<Cube> thisGroupsCubes);
    public static OnAchieveScoring achieveScoring;

    public delegate void OnComboOccures(int aComboCount);
    public static OnComboOccures comboOccuring;

    public delegate void OnAddLevelScore();
    public static OnAddLevelScore addLevelScore;

    public int BlockCount { get { return mBlocks.Count; } }

    private Block mNewLandedOriginalBlock = null;
    public Block NewLandedOriginalBlock { get { return (mNewLandedOriginalBlock ?? null); } }

    private List<Block> mBlocks = new List<Block>();
    private List<Block> mFloatingBlocks = new List<Block>();
    private List<Cube> mScoringsCubes = new List<Cube>();
    private List<Cube> mNewLandedCubes = new List<Cube>();
    private List<ScoringGroupAchieveInfo> mScoringPositionGroups = new List<ScoringGroupAchieveInfo>();

    private int mCurrentScoringGroupIndex = 0;
    private float mScoringCalculationTimer = 0f;
    private bool mCurrentGroupScoreCalcInProgress = false;

    private int mComboCount = 0;

    

    public void Reset()
    {
        mBlocks.Clear();
        mFloatingBlocks.Clear();
        mScoringsCubes.Clear();
        mNewLandedCubes.Clear();

        mCurrentScoringGroupIndex = 0;
        mScoringCalculationTimer = 0f;
        mCurrentGroupScoreCalcInProgress = false;

        ResetCombo();

        mNewLandedOriginalBlock = null;
    }

    /// <summary>
    /// Add in the new landed and registrate it's cubes into the GridData.
    /// At the same time the cubes of the new block will be added into a seperate list 
    /// The order of the block list will be rearranged in their x then y position.
    /// </summary>
    /// <param name="aBlock">The new landed block</param>
    public void AddBlock(Block aBlock, bool isTheOriginal = true)
    {
        mBlocks.Add(aBlock);

        if (isTheOriginal)
        {
            GameManager.Instance.AddScore(ScoreType.ORIGINAL_BLOCK_LANDING);
            mNewLandedOriginalBlock = aBlock;
        }
        else
        {
            mNewLandedOriginalBlock = null;
        }

        foreach (Cube c in aBlock.Cubes)
        {
            if (isTheOriginal)
                GridData.Instance.AddOriginalBlockPosition(c.GridPos);

            GridData.Instance.RegistrateCell(c);

            mNewLandedCubes.Add(c);
        }
        SortTheBlocks();
    }

    // Add in cubes that had scored
    public void AddScoringCubes(Cube aCube)
    {
        mScoringsCubes.Add(aCube);
    }

    public void ResetCombo()
    {
        mComboCount = 0;
    }
    
    // All scoring cube play their scoring animation
    public void PlayScoringAnimation()
    {
        foreach(Cube c in mScoringsCubes)
        {
            c.PlayAnimation();
            GridData.Instance.UnregistrateCell(c.GridPos);
        }

        mScoringsCubes.Clear();
        mNewLandedCubes.Clear();
    }


    public string BlockOrderInString()
    {
        string blockOrderString = "";
        foreach (Block b in mBlocks)
            blockOrderString += b.name + "\n";

        return blockOrderString;
    }

    public bool BlockPassedGameOverLine()
    {
        bool gameover = (mBlocks.FirstOrDefault(block => block.MaxGridPos.y > GameSettings.Instance.LimitHigh) ? true : false);
        if (gameover)
            TowerCollapse();

        return gameover;
    }
    
    public bool IsScoring()
    {
        if (mScoringPositionGroups.Any())
            return true;

        if ((mScoringPositionGroups = GridData.Instance.GetListOfScoringPositionGroups(mNewLandedCubes)).Any())
            return true;
        //if (mScoringPositionGroups.Any())
        //    return true;

        mScoringsCubes.Clear();
        mNewLandedCubes.Clear();
        return false;
    }

    public bool IsNewOriginalBlockScoring(List<Vector2Int> somePos)
    {
        if (mNewLandedOriginalBlock == null)
            return false;

        return mNewLandedOriginalBlock.IsThisBlockScoringAlone(somePos);
    }

    /// <summary>
    /// The progression of the scoring moment
    /// </summary>
    public void ScoreCalculationProgression()
    {
        // if the calculation isn't in progress it'll search through the list of landed block and compare their gridposition
        // with the list of position in the list of scoring group position, add them into the list of scoring cubes and make
        // the cube play it particlar effect
        if(!mCurrentGroupScoreCalcInProgress)
        {
            TaskRank thisGroupTaskTank = mScoringPositionGroups[mCurrentScoringGroupIndex].ObjectiveRank;
            List<Cube> thisGroupScoringCubes = new List<Cube>();
            for (int i = 0; i < mBlocks.Count; i++)
            {
                foreach (Cube c in mBlocks[i].Cubes)
                {
                    if(mScoringPositionGroups[mCurrentScoringGroupIndex].GroupPosition.Contains(c.GridPos))
                    {
                        AddScoringCubes(c);
                        c.PlayActiveParticlar();
                        thisGroupScoringCubes.Add(c);
                    }
                }
            }
            addLevelScore?.Invoke();
            comboOccuring?.Invoke(mComboCount++);
            achieveScoring?.Invoke(mScoringPositionGroups[mCurrentScoringGroupIndex].ObjectiveRank, thisGroupScoringCubes);
            GameManager.Instance.AddScore(ScoreType.LINKING, thisGroupScoringCubes.Count, thisGroupTaskTank);
            mCurrentGroupScoreCalcInProgress = !mCurrentGroupScoreCalcInProgress;
        }
        else
        {
            mScoringCalculationTimer += Time.deltaTime;
            if(mScoringCalculationTimer >= 1f)
            {
                mCurrentScoringGroupIndex++;
                mScoringCalculationTimer = mScoringCalculationTimer - 1f;
                if(mCurrentScoringGroupIndex >= mScoringPositionGroups.Count)
                {
                    GameManager.Instance.AddScore(ScoreType.COMBO, mScoringPositionGroups.Count - 1);
                    GameManager.Instance.AddLevelPoint(mScoringPositionGroups.Count);
                    PlayScoringAnimation();
                    mScoringPositionGroups.Clear();
                    mCurrentScoringGroupIndex = 0;
                    
                    //mComboCount = 0;
                }
                mCurrentGroupScoreCalcInProgress = !mCurrentGroupScoreCalcInProgress;
            }
        }

    }
    
    /// <summary>
    /// Checking if any block have any of it's cube playing scoring animation
    /// </summary>
    /// <returns></returns>
    public bool AnyBlockPlayingAnimation()
    {
        ClearCubeLessBlocks();
        for(int i = 0; i < mBlocks.Count; i++)
        {
            if (mBlocks[i].IsAnimationPlaying())
                return true;
        }
        return false;
    }

    /// <summary>
    /// Remove blocks that don't have cube in them
    /// </summary>
    public void ClearCubeLessBlocks()
    {
        if (!mBlocks.Any())
            return;
        
        mBlocks.RemoveAll((block) => {
            return block.DestroyThisCube();
            });
    }

    /// <summary>
    /// 
    /// </summary>
    public void RearrangeBlocks()
    {
        for (int i = 0; i < mFloatingBlocks.Count; i++)
        {
            while(mFloatingBlocks[i].CheckIfCellIsVacantBeneath())
                mFloatingBlocks[i].DropDown();
            AddBlock(mFloatingBlocks[i], false);
        }
    }

    /// <summary>
    /// After the scoring, some block might have have several cell vacant beneath their current position on the table.
    /// This is to return if it true or not by checking every blocks that had been storred in the list of block of the
    /// manager
    /// </summary>
    /// <returns>True if any block in the list happened to have a block that have a cell beneath vacant</returns>
    public bool CheckIfAnyBlocksIsFloating()
    {
        mFloatingBlocks.Clear();

        for (int i = mBlocks.Count - 1; i >= 0; i--)
        {
            if (mBlocks[i].CheckIfCellIsVacantBeneath())
            {
                foreach (Cube c in mBlocks[i].Cubes)
                    GridData.Instance.UnregistrateCell(c.GridPos);
                mFloatingBlocks.Add(mBlocks[i]);
                mBlocks.RemoveAt(i);
            }
        }
        return mFloatingBlocks.Any();
    }
    
    private void TowerCollapse()
    {
        foreach (Block b in mBlocks)
            b.GetComponent<Rigidbody>().useGravity = true;
    }

    private void SortTheBlocks()
    {
        List<Block> SortedList = mBlocks.OrderBy(o => o.MinGridPos.x).ThenBy(o => o.MinGridPos.y).ToList();
        mBlocks = SortedList;
    }
}
