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

    public delegate void OnAddLevelScore();
    public static OnAddLevelScore addLevelScore;

    public int BlockCount { get { return mBlocks.Count; } }

    private Block mNewLandedOriginalBlock = null;
    public Block NewLandedOriginalBlock { get { return (mNewLandedOriginalBlock ?? null); } }

    public List<Block> Blocks { get { return mBlocks; } }
    private List<Block> mBlocks = new List<Block>();

    private List<Block> mFloatingBlocks = new List<Block>();
    private List<Block> mScoringBlocks = new List<Block>();
    private List<Cube> mScoringPassiveCubes = new List<Cube>();
    private List<Cube> mNewLandedCubes = new List<Cube>();
    private List<ScoringGroupAchieveInfo> mScoringPositionGroups = new List<ScoringGroupAchieveInfo>();
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

        mNewLandedOriginalBlock = null;
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
        mScoringBlocks.Clear();
        mScoringPassiveCubes.Clear();
        mNewLandedCubes.Clear();
    }

    /// <summary>
    /// Add in the new landed and registrate it's cubes into the GridData.
    /// At the same time the cubes of the new block will be added into a seperate list 
    /// The order of the block list will be rearranged in their x then y position.
    /// </summary>
    /// <param name="aBlock">The new landed block</param>
    public void AddBlock(Block aBlock, bool isTheOriginal = true)
    {
        //List<List<Vector2Int>> temp = GenerateScoreCombinationPositions.Instance.GetAllScorePositionFor(aBlock);
        
        if (isTheOriginal)
        {
            GameManager.Instance.AddScore(ScoreType.ORIGINAL_BLOCK_LANDING);
            mNewLandedOriginalBlock = aBlock;
        }
        else
        {
            mNewLandedOriginalBlock = null;
        }

        mBlocks.Add(aBlock);

        RegisterBlockCubesToGrid(aBlock);
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

        mScoringPositionGroups = GridData.Instance.GetListOfScoringPositionGroups(aBlock);
    }

    // Add in cubes that had scored
    public void AddScoringCubes(Cube aCube)
    {
        mScoringPassiveCubes.Add(aCube);
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
        //foreach(Cube c in mScoringPassiveCubes)
        //{
        //    c.PlayAnimation();
        //    GridData.Instance.UnregistrateCell(c.GridPos);
        //}

        mScoringPassiveCubes.Clear();
        mNewLandedCubes.Clear();
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

        LevelManager.Instance.FillUpTheMainBar();
        mScoringPassiveCubes.Clear();
        mNewLandedCubes.Clear();
        return false;
    }

    /// <summary>
    /// Boolian method that check if the last landed block/blocks had made any scoring.
    /// If had, then the ScoringProgress method will be called
    /// Else it'll called for the LevelManager to fill up the GUI-bar for the level up GUI
    /// </summary>
    /// <returns>If there was any scoring info collected after the block/blocks landed</returns>
    public bool IsScoringNew()
    {
        if (mScoringPositionGroups.Any())
            return true;

        LevelManager.Instance.FillUpTheMainBar();
        mNewLandedCubes.Clear();
        return false;
    }

    public bool IsNewOriginalBlockScoring(List<Vector2Int> somePos)
    {
        if (mNewLandedOriginalBlock == null)
            return false;

        return mNewLandedOriginalBlock.IsThisBlockScoringAlone(somePos);
    }

    public void ScoreCalculationProgression()
    {
        // if the calculation isn't in progress it'll search through the list of landed block and compare their gridposition
        // with the list of position in the list of scoring group position, add them into the list of scoring cubes and make
        // the cube play it particlar effect
        if(!mCurrentGroupScoreCalcInProgress)
        {
            TaskRank thisGroupTaskTank = mScoringPositionGroups[mCurrentScoringGroupIndex].TaskRank;
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
            //addLevelScore?.Invoke();
            comboOccuring?.Invoke(mComboCount++);
            achieveScoring?.Invoke(mScoringPositionGroups[mCurrentScoringGroupIndex].TaskRank, thisGroupScoringCubes);
            LevelManager.Instance.AddLevelScore(1);
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
                    //GameManager.Instance.AddLevelPoint(mScoringPositionGroups.Count);
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
    /// The progression of the scoring moment
    /// </summary>
    public void ScoreCalculationProgressionNew()
    {
        // if the calculation isn't in progress it'll search through the list of landed block and compare their gridposition
        // with the list of position in the list of scoring group position, add them into the list of scoring cubes and make
        // the cube play it particlar effect
        if (!mCurrentGroupScoreCalcInProgress)
        {
            mCurrentScoringInfo = mScoringPositionGroups[mCurrentScoringGroupIndex];
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
                mCurrentScoringInfo.Block.PlayParticleEffect();
            if (mCurrentScoringInfo.Cube != null)
                mCurrentScoringInfo.Cube.PlayActiveParticlar();

            //addLevelScore?.Invoke();
            comboOccuring?.Invoke(mComboCount++);
            achieveScoring?.Invoke(mScoringPositionGroups[mCurrentScoringGroupIndex].TaskRank, thisGroupScoringCubes);
            LevelManager.Instance.AddLevelScore(1);
            GameManager.Instance.AddScore(ScoreType.LINKING, thisGroupScoringCubes.Count, thisGroupTaskRank);
            mCurrentGroupScoreCalcInProgress = !mCurrentGroupScoreCalcInProgress;
        }
        else
        {
            mScoringCalculationTimer += Time.deltaTime;
            if (mScoringCalculationTimer >= 1f)
            {
                mCurrentScoringGroupIndex++;
                mScoringCalculationTimer = mScoringCalculationTimer - 1f;
                if (mCurrentScoringGroupIndex >= mScoringPositionGroups.Count)
                {
                    GameManager.Instance.AddScore(ScoreType.COMBO, mScoringPositionGroups.Count - 1);
                    PlayScoringAnimation();
                    mScoringPositionGroups.Clear();
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
        
        mBlocks.RemoveAll((block) => {
            return block.DestroyThisCube();
            });
    }
    
    public void RearrangeBlocks()
    {
        for (int i = 0; i < mFloatingBlocks.Count; i++)
        {
            while(mFloatingBlocks[i].CheckIfCellIsVacantBeneath())
                mFloatingBlocks[i].DropDown();
            //AddBlock(mFloatingBlocks[i], false);
            AddBlock(mFloatingBlocks[i], false);
        }
    }

    /// <summary>
    /// This is the second score collecter method during each new original block has beed dropped.
    /// It is combine with the method CheckIfAnyBlocksIsFloating.
    /// Each block will go through a recursive dropping method in to rearrange their position after the scoring,
    /// New scoring position info will be collected with those block that was first detected that was vacant beneath them
    /// And the list that collected the indecies for the first floating block will be cleared.
    /// </summary>
    public void RearrangeBlockNew()
    {
        DropBlockRecursively(0);

        if(mFirstFloatingBlockIndecies.Any())
        {
            for(int i = 0; i < mFirstFloatingBlockIndecies.Count; i++)
                mScoringPositionGroups = GridData.Instance.GetListOfScoringPositionGroups(mBlocks[mFirstFloatingBlockIndecies[i]]);
        }

        mFirstFloatingBlockIndecies.Clear();
    }

    /// <summary>
    /// The recrusive method that unregistrate the passive block's cubes from GridData and reposition the current block if the cell beneath
    /// it is vacant.
    /// Once it had hit the ground or above another block, it'll be registrated to the GridData with it new position.
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
                GridData.Instance.UnregistrateCell(c.GridPos);
            mBlocks[index].DropDown();
        }

        while (mBlocks[index].CheckIfCellIsVacantBeneath())
            mBlocks[index].DropDown();

        RegisterBlockCubesToGrid(mBlocks[index]);

        index++;
        DropBlockRecursively(index);
    }

    
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

    /// <summary>
    /// Return if there was any block that was floating from the scoring
    /// </summary>
    /// <returns>True if any block in the list happened to have a block that have a cell beneath vacant</returns>
    public bool CheckIfAnyBlocksIsFloatingNew()
    {
        return mFirstFloatingBlockIndecies.Any();
    }

    /// <summary>
    /// Registrate a block's cubes into the GridData and sort the blocks in the blocklist.
    /// </summary>
    /// <param name="aBlock"></param>
    private void RegisterBlockCubesToGrid(Block aBlock)
    {
        foreach (Cube c in aBlock.Cubes)
        {
            if (mNewLandedOriginalBlock != null)
                GridData.Instance.AddOriginalBlockPosition(c.GridPos);

            GridData.Instance.RegistrateCell(c);

            mNewLandedCubes.Add(c);
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
