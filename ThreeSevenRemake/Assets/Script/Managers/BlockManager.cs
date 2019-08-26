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

    public delegate void OnAchieveScoring(TaskRank anObjective, List<Cube> thisGroupsCubes);
    public static OnAchieveScoring achieveScoring;

    public delegate void OnComboOccures(int aComboCount);
    public static OnComboOccures comboOccuring;

    /// <summary>
    /// This was only for the debugging purpose to show how many original block has been landed
    /// </summary>
    public int BlockCount { get { return mBlocks.Count; } }

    public List<Block> Blocks { get { return mBlocks; } }
    private List<Block> mBlocks = new List<Block>();

    public bool GameOver { get { return mGameOver; } }
    private bool mGameOver = false;

    private List<Block> mFloatingBlocks = new List<Block>();
    private List<ScoringGroupAchieveInfo> mScoringInfos = new List<ScoringGroupAchieveInfo>();
    private List<int> mFirstFloatingBlockIndecies = new List<int>();

    private ScoringGroupAchieveInfo mCurrentScoringInfo = null;

    private int mCurrentScoringGroupIndex = 0;
    private float mScoringCalculationTimer = 0f;
    private bool mCurrentGroupScoreCalcInProgress = false;

    private int mComboCount = 0;

    
    /// <summary>
    /// Reset all variabler for a new game
    /// </summary>
    public void Reset()
    {
        ResetBlockList();

        mCurrentScoringGroupIndex = 0;
        mScoringCalculationTimer = 0f;
        mCurrentGroupScoreCalcInProgress = false;

        ResetCombo();
    }

    public void AddRewindBlock(Block aRewindBlock)
    {
        mBlocks.Add(aRewindBlock);
        RegisterBlockCubesToGrid(aRewindBlock);
    }

    public void ResetBlockList()
    {
        mBlocks.Clear();
        mFloatingBlocks.Clear();
    }

    /// <summary>
    /// Collect the new original block that had landed on the table and registrate it's landing position, add the landing score
    /// and collect the scoring info from it position
    /// </summary>
    /// <param name="aBlock">the new landed original block</param>
    public void AddNewOriginalBlock(Block aBlock)
    {        
        if(!mBlocks.Contains(aBlock))
            mBlocks.Add(aBlock);

        RegisterBlockCubesToGrid(aBlock);

        GameManager.Instance.AddScore(ScoreType.ORIGINAL_BLOCK_LANDING);

        mScoringInfos = GridManager.Instance.GetListOfScoringPositionGroups(aBlock);
    }

    public void ResetCombo()
    {
        mComboCount = 0;
    }
    
    // All scoring cube play their scoring animation
    public void PlayScoringAnimation()
    {
        foreach(Block b in mBlocks)
        {
            b.PlayCubeAnimation();
        }
    }

    public bool BlockPassedGameOverLine()
    {
        mGameOver = (mBlocks.FirstOrDefault(block => block.MaxGridPos.y > GameSettings.Instance.LimitHigh) ? true : false);
        if (mGameOver)
            TowerCollapse();

        return mGameOver;
    }
    
    /// <summary>
    /// Boolian method that check if the last landed block/blocks had made any scoring.
    /// If had, then the ScoringProgress method will be called
    /// Else it'll called for the LevelManager to fill up the GUI-bar for the level up GUI
    /// </summary>
    /// <returns>If there was any scoring info collected after the block/blocks landed</returns>
    public bool IsScoring()
    {
        if (mScoringInfos.Any())
            return true;

        LevelManager.Instance.FillUpTheMainBar();
        return false;
    }

    /// <summary>
    /// The progression of the scoring moment
    /// </summary>
    public void ScoreCalculationProgression()
    {
        // if the calculation isn't in progress it'll search through the list of landed block and compare their gridposition
        // with the list of position in the list of scoring group position, add them into the list of scoring cubes and make
        // the cube play it particlar effect
        if (!mCurrentGroupScoreCalcInProgress)
        {
            mCurrentScoringInfo = mScoringInfos[mCurrentScoringGroupIndex];
            TaskRank thisGroupTaskRank = mCurrentScoringInfo.TaskRank;
            List<Cube> thisGroupScoringCubes = new List<Cube>();
            for (int i = 0; i < mBlocks.Count; i++)
            {
                foreach (Cube c in mBlocks[i].Cubes)
                {
                    if (mCurrentScoringInfo.GroupPosition.Contains(c.GridPos))
                    {
                        c.PlayActiveParticlar();
                        thisGroupScoringCubes.Add(c);
                    }
                }
            }

            if (mCurrentScoringInfo.Block != null)
            {
                foreach(Cube c in mCurrentScoringInfo.Block.Cubes)
                    thisGroupScoringCubes.Add(c);
                mCurrentScoringInfo.Block.PlayParticleEffect();
            }
            if (mCurrentScoringInfo.Cube != null)
            {
                thisGroupScoringCubes.Add(mCurrentScoringInfo.Cube);
                mCurrentScoringInfo.Cube.PlayActiveParticlar();
            }

            var sortCubeList = thisGroupScoringCubes.OrderBy(c => c.GridPos.x).ThenByDescending(c => c.GridPos.y).ToList();


            //addLevelScore?.Invoke();
            comboOccuring?.Invoke(mComboCount++);
            achieveScoring?.Invoke(mScoringInfos[mCurrentScoringGroupIndex].TaskRank, sortCubeList);
            LevelManager.Instance.AddLevelScore(1);
            GameManager.Instance.AddScore(ScoreType.LINKING, sortCubeList.Count, thisGroupTaskRank);
            mCurrentGroupScoreCalcInProgress = !mCurrentGroupScoreCalcInProgress;
        }
        else
        {
            mScoringCalculationTimer += Time.deltaTime;
            if (mScoringCalculationTimer >= 1f)
            {
                mCurrentScoringGroupIndex++;
                mScoringCalculationTimer = mScoringCalculationTimer - 1f;
                if (mCurrentScoringGroupIndex >= mScoringInfos.Count)
                {
                    GameManager.Instance.AddScore(ScoreType.COMBO, mScoringInfos.Count - 1);
                    PlayScoringAnimation();
                    mScoringInfos.Clear();
                    mCurrentScoringGroupIndex = 0;
                    mCurrentScoringInfo = null;
                }
                mCurrentGroupScoreCalcInProgress = !mCurrentGroupScoreCalcInProgress;
            }
        }
    }

    /// <summary>
    /// Checking if any block have any of it's cube playing scoring animation
    /// Once the animation is over, the method for collecting floating block indecies will be called
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
        // Check after those blocks 
        StoreTheFirstFloatingBlockIndecies();

        return false;
    }

    /// <summary>
    /// Remove blocks that don't have cube in them
    /// </summary>
    public void ClearCubeLessBlocks()
    {
        if (!mBlocks.Any())
            return;

        //mBlocks.RemoveAll((block) => {
        //    return block.DestroyThisCube();
        //    });
        mBlocks.RemoveAll(b => b.DestroyThisCube());
    }
    
    /// <summary>
    /// This is the second score collecter method during each new original block has beed dropped.
    /// It is combine with the method CheckIfAnyBlocksIsFloating.
    /// Each block will go through a recursive dropping method in to rearrange their position after the scoring,
    /// New scoring position info will be collected with those block that was first detected that was vacant beneath them
    /// And the list that collected the indecies for the first floating block will be cleared.
    /// </summary>
    public void RearrangeBlock()
    {
        DropBlockRecursively(0);

        if(mFirstFloatingBlockIndecies.Any())
        {
            for(int i = 0; i < mFirstFloatingBlockIndecies.Count; i++)
                mScoringInfos = GridManager.Instance.GetListOfScoringPositionGroups(mBlocks[mFirstFloatingBlockIndecies[i]]);
        }

        mFirstFloatingBlockIndecies.Clear();
    }

    /// <summary>
    /// The recrusive method that unregistrate the passive block's cubes from GridManager and reposition the current block if the cell beneath
    /// it is vacant.
    /// Once it had hit the ground or above another block, it'll be registrated to the GridManager with it new position.
    /// </summary>
    /// <param name="aBlockIndex">Index of the previous block from the blocklist</param>
    private void DropBlockRecursively(int aBlockIndex)
    {
        int index = aBlockIndex;
        if (index == mBlocks.Count)
            return;

        if (mBlocks[index].CheckIfCellIsVacantBeneath())
        {
            foreach (Cube c in mBlocks[index].Cubes)
                GridManager.Instance.UnregistrateCell(c.GridPos);
            mBlocks[index].DropDown();
        }

        while (mBlocks[index].CheckIfCellIsVacantBeneath())
            mBlocks[index].DropDown();

        RegisterBlockCubesToGrid(mBlocks[index]);

        index++;
        DropBlockRecursively(index);
    }

    /// <summary>
    /// Return if there was any block that was floating from the scoring
    /// </summary>
    /// <returns>True if any block in the list happened to have a block that have a cell beneath vacant</returns>
    public bool CheckIfAnyBlocksIsFloating()
    {
        return mFirstFloatingBlockIndecies.Any();
    }

    /// <summary>
    /// Registrate a block's cubes into the GridManager and sort the blocks in the blocklist.
    /// </summary>
    /// <param name="aBlock"></param>
    private void RegisterBlockCubesToGrid(Block aBlock)
    {
        foreach (Cube c in aBlock.Cubes)
        {
            GridManager.Instance.RegistrateCell(c);
        }
        SortTheBlocks();
    }
    
    /// <summary>
    /// Trigger every blocks useGravity in their rigidbody component
    /// </summary>
    private void TowerCollapse()
    {
        foreach (Block b in mBlocks)
            b.GetComponent<Rigidbody>().useGravity = true;
    }

    /// <summary>
    /// Sorting the main block list by the block min x position and then by min y position.
    /// </summary>
    private void SortTheBlocks()
    {
        List<Block> SortedList = mBlocks.OrderBy(o => o.MinGridPos.x).ThenBy(o => o.MinGridPos.y).ToList();
        mBlocks = SortedList;
    }

    /// <summary>
    /// Store in the very first blocks that was right above the vanished blocks after the scoring
    /// </summary>
    private void StoreTheFirstFloatingBlockIndecies()
    {
        for(int i = 0; i < mBlocks.Count; i++)
        {
            if (mBlocks[i].CheckIfCellIsVacantBeneath())
                mFirstFloatingBlockIndecies.Add(i);
        }
    }
}
