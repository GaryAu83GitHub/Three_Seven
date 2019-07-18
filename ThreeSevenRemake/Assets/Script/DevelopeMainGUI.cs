using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevelopeMainGUI : MonoBehaviour
{
    public BlockGUI NextBlockGUI;
    public Text ScoreText;
    public Text TimeText;
    public Text LevelText;
    public Image LevelUpFillingImage;

    public GameObject ScoreAddOn;

    public Text ComboCountText;
    public Text ComboTitleText;
    public Text ComboScoreText;

    public GameObject DebugPanel;

    private float mGameTimer = 0f;
    private bool mGameIsPlaying;

    private Color mComboTextColor = Color.black;
    private Color mTransparentColor = new Color(0f, 0f, 0f, 0f);
    private bool mComboAppear = false;
    private float mComboTextFadingTime = 0f;

    private bool mUpdateDisplayScore = false;
   
    private int mCurrentDisplayScore = 0;
    private int mCurrentTotalScore = 0;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateLevel(GameManager.Instance.CurrentLevel, 0, 1);

        GameManager.comboOccuring += ComboAppear;
        GameManager.levelChanging += UpdateLevel;
        GameManager.scoreChanging += UpdateScore;

        DevelopeMain.createNewBlock += TransferNewBlock;
        DevelopeMain.gameIsPlaying += GameIsPlaying;

        GameOverMenu.leaveTheGame += ResetGameTime;

        //TestImage.color = Color.green;
    }

    private void OnDisable()
    {
        GameManager.comboOccuring -= ComboAppear;
        GameManager.levelChanging -= UpdateLevel;
        GameManager.scoreChanging -= UpdateScore;

        DevelopeMain.createNewBlock -= TransferNewBlock;
        DevelopeMain.gameIsPlaying -= GameIsPlaying;

        GameOverMenu.leaveTheGame -= ResetGameTime;
    }

    // Update is called once per frame
    private void Update()
    {
        Clock();

        if(mComboAppear)
            ComboTextFading();

        //if(Input.GetKeyDown(KeyCode.Insert))
        //{
        //    if (DebugPanel.GetComponent<CanvasGroup>().alpha >= 1f)
        //        DebugPanel.GetComponent<CanvasGroup>().alpha = 0f;
        //    else
        //        DebugPanel.GetComponent<CanvasGroup>().alpha = 1f;
        //    //mLerpValue += .125f;
        //    //if (mLerpValue > 1)
        //    //    mLerpValue = 0f;
        //    //TestImage.color = Color.Lerp(Color.green, Color.red, mLerpValue);
        //}
        if(mUpdateDisplayScore)
        {
            UpdateDisplayScore();
        }
    }

    public void UpdateScore(int aNewTotalScore, int anAddOnScore)
    {
        mCurrentTotalScore = aNewTotalScore;

        
        ScoreAddOn.GetComponent<Text>().text = "+" + anAddOnScore.ToString();
        ScoreAddOn.GetComponent<Animation>().Play();

        mUpdateDisplayScore = true;
    }

    public void UpdateLevel(int aNewLevel, int aCurrentLevelScore, int aNextLevelUpScore)
    {
        float filling = (float)aCurrentLevelScore / (float)aNextLevelUpScore;
        LevelUpFillingImage.fillAmount = filling;
        LevelText.text = aNewLevel.ToString();
    }

    public void ComboAppear(int aComboCount, int aComboScore, string aComboText)
    {
        ComboCountText.text = aComboCount.ToString();
        ComboTitleText.text = aComboText;
        ComboScoreText.text = aComboScore.ToString();

        mComboTextFadingTime = 0f;
        mComboTextColor.a = 1f;
        mComboAppear = true;
    }

    public void TransferNewBlock(Block aNewBlock)
    {
        aNewBlock.SetCubeNumbers(NextBlockGUI.NewNumber());
    }

    public void GameIsPlaying(bool anIsPlaying)
    {
        mGameIsPlaying = anIsPlaying;
    }

    private void UpdateDisplayScore()
    {
        if(mCurrentDisplayScore < mCurrentTotalScore)
        {
            mCurrentDisplayScore++;
            if(mCurrentDisplayScore >= mCurrentTotalScore)
            {
                mUpdateDisplayScore = false;
                mCurrentDisplayScore = mCurrentTotalScore;
            }
        }
        ScoreText.text = mCurrentDisplayScore.ToString();
    }

    private void Clock()
    {
        if (!mGameIsPlaying || BlockManager.Instance.BlockPassedGameOverLine())
            return;

        mGameTimer += Time.deltaTime;

        int seconds = (int)(mGameTimer % 60);
        int minutes = (int)((mGameTimer / 60) % 60);
        int hours = (int)((mGameTimer / 3600) % 60);

        GameManager.Instance.GameTimeString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        TimeText.text = GameManager.Instance.GameTimeString;
    }

    private void ComboTextFading()
    {  
        ComboCountText.color = mComboTextColor;
        ComboTitleText.color = mComboTextColor;
        ComboScoreText.color = mComboTextColor;

        mComboTextColor = Color.Lerp(mComboTextColor, mTransparentColor, mComboTextFadingTime);
        if (mComboTextFadingTime < 1f)
            mComboTextFadingTime += Time.deltaTime / 50f;
        else
            mComboAppear = false;
    }

    private void ResetGameTime()
    {
        mGameTimer = 0f;
    }
}
