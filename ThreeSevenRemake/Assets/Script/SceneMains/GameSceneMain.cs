using Assets.Script.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMain : MonoBehaviour
{
    public GameObject BlockObject;
    public GameObject GuideBlockObject;
    public GameObject LimitLine;
    public GameObject TableCover;
    public GameObject TableDebugPanel;

    public delegate void OnSetGameMode(GameMode aMode);
    public static OnSetGameMode setGameMode;

    public delegate void OnSetLevelUpMode(LevelUpMode aMode);
    public static OnSetLevelUpMode setLevelUpMode;

    public delegate void OnCreateNewBlock(Block aNewBlock);
    public static OnCreateNewBlock createNewBlock;

    public delegate void OnGetNextDropTime(float aNextDropTime);
    public static OnGetNextDropTime getNextDropTime;

    public delegate void OnSwapingWithPreview(Block aCurrentBlock);
    public static OnSwapingWithPreview swapingWithPreview;

    public delegate void OnActiveTimer(bool activeTimer);
    public static OnActiveTimer activeTimer;

    public delegate void OnRoundStart();
    public static OnRoundStart roundStart;

    public delegate void OnPassingTheTop();
    public static OnPassingTheTop passingTheTop;

    public delegate void OnGameTimeOver();
    public static OnGameTimeOver gameTimeOver;

    public delegate void OnGameRoundOver();
    public static OnGameRoundOver gameRoundOver;

    private Block mCurrentBlock;
    private GuideBlock mGuideBlock;
    private TableDebugPanel mDebugTable;

    private int mBlockCount = 0;
    //private float mNextDropTime = 0f;
    private float mBlockDropTimer = 0f;

    private bool mBlockLanded = false;
    private bool mIsAccomplishAnimationPlaying = false;
    private bool mTimeOver = false;
    private bool mGameIsPausing = false;

    private void Awake()
    {
        GridManager.Instance.GenerateGrid();

        // only for debug purpose
        GameRoundManager.Instance.SetUpGameRound(GameMode.SURVIVAL);
        //GenerateScoreCombinationPositions.Instance.GenerateCombinationPositions();
        //TaskManagerNew.Instance.PrepareNewTaskSubjects();
    }

    void Start()
    {
        MainGamePanel.roundStart += CreateNewBlock;
        MainGamePanel.blockMoveHorizontal += BlockMoveHorizontal;
        MainGamePanel.blockDropGradually += BlockDropGradually;
        MainGamePanel.blockDropInstantly += BlockDropInstantly;
        MainGamePanel.blockRotate += BlockRotate;
        MainGamePanel.blockInvert += BlockInvert;
        MainGamePanel.blockSwaping += BlockSwaping;
        MainGamePanel.gamePause += GamePause;

        TaskManagerNew.createNewBlock += CreateNewBlock;
        TaskBox.createNewBlock += CreateNewBlock;
        ResultPanel.leaveGameScene += LeaveGameScene;
        TimeTextBox.timeOver += TimeOver;

        LimitLine.transform.position += new Vector3(0f, .625f + (Constants.CUBE_GAP_DISTANCE * GameSettings.Instance.LimitHigh), 0f);

        mDebugTable = TableDebugPanel.GetComponent<TableDebugPanel>();

        StartCoroutine(GameStart());
    }

    private void OnDestroy()
    {
        MainGamePanel.roundStart -= CreateNewBlock;
        MainGamePanel.blockMoveHorizontal -= BlockMoveHorizontal;
        MainGamePanel.blockDropGradually -= BlockDropGradually;
        MainGamePanel.blockDropInstantly -= BlockDropInstantly;
        MainGamePanel.blockRotate -= BlockRotate;
        MainGamePanel.blockInvert -= BlockInvert;
        MainGamePanel.blockSwaping -= BlockSwaping;
        MainGamePanel.gamePause += GamePause;

        TaskManagerNew.createNewBlock -= CreateNewBlock;
        TaskBox.createNewBlock -= CreateNewBlock;
        ResultPanel.leaveGameScene -= LeaveGameScene;
        TimeTextBox.timeOver -= TimeOver;

        BlockManager.Instance.ResetBlockList();

    }

    void Update()
    {
        TableCover.SetActive(mGameIsPausing/*PauseMenu.GameIsPause*/);
        if (mGameIsPausing)
        {
            if(Input.GetKeyDown(KeyCode.F4))
            {
                TableDebugPanel.SetActive(true);
                mDebugTable.GridUpdate(GridManager.Instance.Grid);
            }
            return;
        }

        if(mTimeOver && !BlockManager.Instance.GameOver)
        {
            BlockManager.Instance.TowerCollapse();
            gameTimeOver?.Invoke();
            return;
        }

        if (mCurrentBlock == null && !BlockManager.Instance.GameOver)
        {
            CurrentBlockStatus();
            return;
        }

        //if (BlockManager.Instance.BlockPassedGameOverLine())
        //{
        //    passingTheTop?.Invoke();
        //    return;
        //}
        //InputHandle()
        if (mCurrentBlock != null)
        {
            if (mBlockDropTimer > 0)
            {
                mBlockDropTimer -= Time.deltaTime;
                getNextDropTime?.Invoke(mBlockDropTimer);
            }

            mGuideBlock.SetupGuideBlock(mCurrentBlock);
            mGuideBlock.SetPosition(mCurrentBlock);
        }
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1f);
        setGameMode?.Invoke(GameSettings.Instance.GameMode);
        setLevelUpMode?.Invoke(GameSettings.Instance.LevelUpMode);

        GUIPanelManager.Instance.StartWithPanel(GUIPanelIndex.MAIN_GAME_PANEL);
        TaskManagerNew.Instance.StartFirstSetOfTask();
        roundStart?.Invoke();

        mTimeOver = false;
        GamingManager.Instance.Reset();
        //CreateNewBlock();
    }

    private void CreateNewBlock()
    {
        if (mCurrentBlock != null)
            return;

        GameObject newBlock = Instantiate(BlockObject, GridManager.Instance.StartWorldPosition, Quaternion.identity);
        newBlock.name = "FallingBlock";
        mBlockCount++;

        createNewBlock?.Invoke(newBlock.GetComponent<Block>());

        if (newBlock.GetComponent<Block>() != null)
            mCurrentBlock = newBlock.GetComponent<Block>();

        mGuideBlock = GuideBlockObject.GetComponent<GuideBlock>();
        GuideBlockObject.SetActive(true);

        ControlManager.Ins.ResetButtonPressTimer();
        //mNextDropTime = GamingManager.Instance.BlockNextDropTime;//Time.time + GamingManager.Instance.DropRate;//mDropRate;
        mBlockDropTimer = GamingManager.Instance.DropRate;
        getNextDropTime?.Invoke(mBlockDropTimer);
        activeTimer?.Invoke(true);
        mBlockLanded = false;
    }

    private void CurrentBlockStatus()
    {
        if (mBlockLanded)
        {
            { // these skall be encaps into the blackmanager
                if (BlockManager.Instance.AnyBlockPlayingAnimation())
                    return;

                if (BlockManager.Instance.CheckIfAnyBlocksIsFloating())
                    BlockManager.Instance.RearrangeBlock();
                else if (BlockManager.Instance.IsScoring())
                    BlockManager.Instance.ScoreCalculationProgression();
                else if(!BlockManager.Instance.BlockPassedGameOverLine())
                    TaskManagerNew.Instance.ChangeTask();
                else
                    passingTheTop?.Invoke();
            }
        }
        //else
        //    BlockDropGradually();
    }

    // These are the method subscribed to MainGamePanels delegate for navigate the block
    private void BlockMoveHorizontal(Vector3 aDir)
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.Move(aDir);
    }

    private void BlockDropGradually()
    {
        if (mCurrentBlock == null)
            return;

        if (!mCurrentBlock.CheckIfCellIsVacantBeneath())
            RegistrateNewLandedBlock();
        else
            mCurrentBlock.DropDown();

        mBlockDropTimer = GamingManager.Instance.DropRate;
        getNextDropTime?.Invoke(mBlockDropTimer);
        //getNextDropTime?.Invoke(GamingManager.Instance.BlockNextDropTime);
    }

    private void BlockDropInstantly()
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.InstantDrop();
        RegistrateNewLandedBlock();
        mBlockDropTimer = GamingManager.Instance.DropRate;
        //getNextDropTime?.Invoke(GamingManager.Instance.BlockNextDropTime);
    }

    private void BlockRotate()
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.RotateBlockUpgrade();
    }

    private void BlockInvert()
    {
        if (mCurrentBlock == null)
            return;

        mCurrentBlock.InvertBlock();
    }

    private void BlockSwaping()
    {
        if (mCurrentBlock == null)
            return;

        swapingWithPreview?.Invoke(mCurrentBlock);
    }

    private void GamePause(bool isGamePausing)
    {
        mGameIsPausing = isGamePausing;
        if (!mGameIsPausing)
            TableDebugPanel.SetActive(false);
    }

    private void BlockSwapWithPreview(List<int> givenNumbers, ref List<int> retrieveNumbers)
    {
        if (mCurrentBlock == null)
            return;

        retrieveNumbers = new List<int>(mCurrentBlock.CubeNumbers);
        mCurrentBlock.SetCubeNumbers(givenNumbers);
    }

    private void AccomplishAnimationIsPlaying(bool isPlaying)
    {
        mIsAccomplishAnimationPlaying = isPlaying;
    }

    private void RegistrateNewLandedBlock()
    {
        mCurrentBlock.name = "Block " + mBlockCount.ToString();
        BlockManager.Instance.AddNewOriginalBlock(mCurrentBlock);
        GamingManager.Instance.LandedBlockCount++;
        //UpdateDebugBoard();

        mCurrentBlock = null;
        mBlockLanded = true;
        activeTimer?.Invoke(false);
        GuideBlockObject.SetActive(GameSettings.Instance.GetGuideBlockVisible(false));
    }

    private void GameRoundOver()
    {
        mCurrentBlock = null;
    }

    private void LeaveGameScene()
    {
        StartCoroutine(LeaveGame());
    }

    private IEnumerator LeaveGame()
    {
        yield return new WaitForSeconds(.5f);
        ScreenTransistor.Instance.FadeToSceneWithIndex(0);
    }

    private void TimeOver(bool aTimeIsOver)
    {
        mTimeOver = aTimeIsOver;
    }
}
