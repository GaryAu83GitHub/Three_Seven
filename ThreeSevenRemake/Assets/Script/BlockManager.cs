using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

    public int BlockCount { get { return mBlocks.Count; } }

    private List<Block> mBlocks = new List<Block>();
    private List<Block> mFloatingBlocks = new List<Block>();
    private List<Cube> mScoringsCubes = new List<Cube>();
    private List<Cube> mNewLandedCubes = new List<Cube>();
    private List<List<Vector2Int>> mScoringPositionGroups = new List<List<Vector2Int>>();

    private int mComboCount = 0;

    /// <summary>
    /// Add in the new landed and registrate it's cubes into the GridData.
    /// At the same time the cubes of the new block will be added into a seperate list 
    /// The order of the block list will be rearranged in their x then y position.
    /// </summary>
    /// <param name="aBlock">The new landed block</param>
    public void AddBlock(Block aBlock)
    {
        mBlocks.Add(aBlock);

        if(!aBlock.CheckIfCellIsVacantBeneath())
            GameManager.Instance.AddSoftScore();

        foreach (Cube c in aBlock.Cubes)
        {
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

    public void ScoreCalculationProgression()
    {

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
        bool gameover = (mBlocks.FirstOrDefault(x => x.MaxGridPos.y > GameSettings.Instance.LimitHigh) ? true : false);
        if (gameover)
        {
            TowerCollapse();
        }

        return gameover;
    }
    
    public bool IsScoringNew()
    {
        return (mScoringPositionGroups = GridData.Instance.GetListOfScoringPositionGroups(mNewLandedCubes)).Any();
    }

    public bool IsScoring()
    {
        // scoring method to list
        List<Vector2Int> scoringPositions = GridData.Instance.CompleteObjectiveScoringMethod(mNewLandedCubes, ref mComboCount);
        

        mScoringsCubes.Clear();
        mNewLandedCubes.Clear();

        if (scoringPositions.Any())
        {
            for(int i = 0; i < mBlocks.Count; i++)
            {
                foreach (Cube c in mBlocks[i].Cubes)
                {
                    if (scoringPositions.Contains(c.GridPos))
                        AddScoringCubes(c);
                }
            }
            return true;
        }

        GameManager.Instance.AddLevelPoint(mComboCount);
        mComboCount = 0;
        return false;
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
            AddBlock(mFloatingBlocks[i]);
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
