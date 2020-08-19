using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class has been deactivated from the main game scene and is replaced by GameSceneMain
/// </summary>
public class DevelopeMain : MonoBehaviour
{
    // Object sync with the unity
    public GameObject BlockObject;
    public GameObject GuideBlockObject;
    public GameObject LimitLine;
    public GameObject TableCover;
    public Light DirectionalLight;

    // delegates
    public delegate void GameActive(bool anActive);
    public static GameActive gameIsPlaying;

    public delegate void OnCreateNewBlock(Block aNewBlock);
    public static OnCreateNewBlock createNewBlock;
    
    public delegate void OnBlockLandedDebug(Dictionary<int, List<Cube>> aGrid);
    public static OnBlockLandedDebug blockLandedDebug;

    // variablers
    // objects
    private Block mCurrentBlock;
    private GuideBlock mGuideBlock;

        // intergear
    private int mBlockCount = 0;

        // floats
    private float mNextDropTime = 0f;
    //private float mDropRate = 1;
    //private float mNextVerticalButtonDownTime = 0f;
    //private float mNextHorizontalButtonDownTime = 0f;

    // boolean
    private bool mGameInProgress = false;
    private bool mBlockLanded = false;

    private List<Block> mRevindBlocks = new List<Block>();
    private bool mIsRewinding = false;

    private void Awake()
    {
        GridManager.Instance.GenerateGrid();
        UpdateDebugBoard();
    }

    private void Start()
    {
        GameOverMenu.leaveTheGame += ResetGame;
        PauseMenu.leaveTheGame += ResetGame;

        LimitLine.transform.position += new Vector3(0f, .625f + (Constants.CUBE_GAP_DISTANCE * GameSettings.Instance.LimitHigh), 0f);
        // When the game start, begin delay for the first block to be created
        StartCoroutine(GameStart());
    }

    private void OnDestroy()
    {
        GameOverMenu.leaveTheGame -= ResetGame;
        PauseMenu.leaveTheGame -= ResetGame;
    }

    private void Update()
    {
        if(mIsRewinding)
        {
            foreach(Block b in mRevindBlocks)
            {
                BlockManager.Instance.AddRewindBlock(b);
            }
            mIsRewinding = !mIsRewinding;
            UpdateDebugBoard();
            return;
        }

        TableCover.SetActive(PauseMenu.GameIsPause);
        
        
        // If the currentBlock is null or undergoing scoreing progression
        if (mCurrentBlock == null && !BlockManager.Instance.GameOver)
        {
            // the block was confirm nullified by the currentblock landed
            if (mBlockLanded)
            {
                //BlockManager.Instance.BlockPassedGameOverLine();
                if (BlockManager.Instance.AnyBlockPlayingAnimation())
                    return;

                UpdateDebugBoard();
                // if the block manager detect any scoring from the last landing block, the animation will be played
                if (BlockManager.Instance.CheckIfAnyBlocksIsFloating())
                    BlockManager.Instance.RearrangeBlock();
                else if (BlockManager.Instance.IsScoring())
                    BlockManager.Instance.ScoreCalculationProgression();
                else
                {
                    TaskManagerNew.Instance.CheckForCompletedTasks();
                    CreateNewBlock();                    
                }
            }
            
            return; // don't proceed futher of this block
        }

        // If mGameOver is equal to true, don't proceed futher of this 
        if (BlockManager.Instance.BlockPassedGameOverLine())
        {
            //Call the function to display the result
            return;
        }
        InputHandle();
    }

    /// <summary>
    /// All navigation input to the block include suspend the game is done from 
    /// this block
    /// </summary>
    private void InputHandle()
    {
        if (PauseMenu.GameIsPause)
            return;

        mCurrentBlock.Move(ControlManager.Ins.MoveBlockHorizontal());

        // input for move the block downward one row if the row below is vacant
        // and if the time between each keypress has expired
        if (ControlManager.Ins.DropBlockGradually(/*mNextDropTime*/)/* || Time.time > mNextDropTime*/)
        {
            if (!mCurrentBlock.CheckIfCellIsVacantBeneath())
                RegistrateNewLandedBlock();
            else
                mCurrentBlock.DropDown();

            mNextDropTime = GamingManager.Instance.BlockNextDropTime;//Time.time + GamingManager.Instance.DropRate; //mDropRate;
        }

        if(ControlManager.Ins.DropBlockInstantly())
        {
            mCurrentBlock.InstantDrop();
            RegistrateNewLandedBlock();
            mNextDropTime = GamingManager.Instance.BlockNextDropTime;
        }

        // input for rotate the block clockwise if the column or row of where the block
        // rotate to is vacant
        if (ControlManager.Ins.RotateBlock())
            mCurrentBlock.RotateBlockUpgrade();

        // input for swaping the cubes value inside the block
        if (ControlManager.Ins.InvertBlock())
            mCurrentBlock.InvertBlock();

        if (ControlManager.Ins.SwapPreview())
            mCurrentBlock.SwapWithPreviewBlock();

        if (Input.GetKeyDown(KeyCode.F9))
        {
            Destroy(mCurrentBlock.gameObject);
            CreateNewBlock();
            return;
        }

        if (mCurrentBlock != null)
        {
            mGuideBlock.SetupGuideBlock(mCurrentBlock);
            mGuideBlock.SetPosition(mCurrentBlock);
        }
    }
        
    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);

        TaskManagerNew.Instance.StartFirstSetOfTask();
        CreateNewBlock();
        
        mGameInProgress = true;
        gameIsPlaying?.Invoke(mGameInProgress);
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, GridManager.Instance.StartWorldPosition, Quaternion.identity/*, transform*/);
        newBlock.name = "FallingBlock";
        mBlockCount++;

        createNewBlock?.Invoke(newBlock.GetComponent<Block>());

        if (newBlock.GetComponent<Block>() != null)
        {
            mCurrentBlock = newBlock.GetComponent<Block>();

            //RecordingManager.Instance.Record(new TurnData(mCurrentBlock));
        }


        mGuideBlock = GuideBlockObject.GetComponent<GuideBlock>();
        //mGuideBlock.SetupGuideBlock(mCurrentBlock);
        GuideBlockObject.SetActive(GameSettings.Instance.GetGuideBlockVisible(true));

        // Get the block's droprate of the current level from GamingManager
        //mDropRate = GamingManager.Instance.DropRate;//GamingManager.Instance.GetCurrentDroppingRate();

        //mNextVerticalButtonDownTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;//mButtonDownDropRate;
        //mNextHorizontalButtonDownTime = Time.time + Constants.BUTTON_DOWN_INTERVAL;
        ControlManager.Ins.ResetButtonPressTimer();
        mNextDropTime = GamingManager.Instance.BlockNextDropTime;//Time.time + GamingManager.Instance.DropRate;//mDropRate;

        mBlockLanded = false;

        BlockManager.Instance.ResetCombo();
    }

    private void RegistrateNewLandedBlock()
    {
        mCurrentBlock.name = "Block " + mBlockCount.ToString();
        RecordingManager.Instance.Record(new TurnData(mCurrentBlock));
        BlockManager.Instance.AddNewOriginalBlock(mCurrentBlock);
        GamingManager.Instance.LandedBlockCount++;
        UpdateDebugBoard();

        mCurrentBlock = null;
        mBlockLanded = true;
        GuideBlockObject.SetActive(GameSettings.Instance.GetGuideBlockVisible(false));
    }

    /// <summary>
    /// For debug purpose
    /// </summary>
    private void UpdateDebugBoard()
    {
        blockLandedDebug?.Invoke(GridManager.Instance.Grid);
    }

    private void ResetGame()
    {
        BlockManager.Instance.Reset();
        GamingManager.Instance.Reset();
        GameSettings.Instance.Reset();
    }

    private void RewindTurn()
    {
        if (RecordingManager.Instance.RecordCount <= 0)
            return;

        int childs = transform.childCount;
        for (int i = childs - 1; i > -1; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        GridManager.Instance.GenerateGrid();
        BlockManager.Instance.ResetBlockList();

        RewindProgress();
    }

    private void RewindProgress()
    {
        TurnData data = RecordingManager.Instance.Rewind();
        mRevindBlocks.Clear();
        foreach (BlockData b in data.Blocks)
        {
            Block rewindBlock = Instantiate(BlockObject, new Vector3(b.RootCubePosition.x * Constants.CUBE_GAP_DISTANCE, b.RootCubePosition.y * Constants.CUBE_GAP_DISTANCE, 0f), Quaternion.identity, transform).GetComponent<Block>();
            rewindBlock.gameObject.name = b.BlockName;
            rewindBlock.SetBlockWithRewindData(b);
            mRevindBlocks.Add(rewindBlock);
            //BlockManager.Instance.AddRewindBlock(rewindBlock);
        }

        mCurrentBlock = Instantiate(BlockObject, GridManager.Instance.StartWorldPosition, Quaternion.identity, transform).GetComponent<Block>();
        mCurrentBlock.gameObject.name = data.ThisTurnFallingBlock.BlockName;
        mCurrentBlock.SetBlockWithRewindData(data.ThisTurnFallingBlock, true);

        //GridManager.Instance.SetGridAfterData(data.Blocks);
        GamingManager.Instance.RewindNextNumber(data.ThisTurnNextBlock);

        mIsRewinding = true;
    }
}
