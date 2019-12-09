using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevelopeMainGUI : MonoBehaviour
{
    public BlockGUI NextBlockGUI;
    public Text ScoreText;
    public Text TimeText;
    //public Text LevelText;
    //public Image LevelUpFillingImage;

    public GameObject ScoreAddOn;
    public GameObject ComboFrame;
    
    public Text ComboCountText;
    //public Text ComboTitleText;
    //public Text ComboScoreText;

    public GameObject DebugPanel;
    public Text TempDebugText;


    private Animation mComboAnimation;

    public delegate void ChangeNextBlock();
    public static ChangeNextBlock changeNextBlock;

    private bool mGameIsPlaying;

    private Color mComboTextColor = Color.black;
    private Color mTransparentColor = new Color(0f, 0f, 0f, 0f);

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
        GamingManager.scoreChanging += UpdateScore;

        DevelopeMain.createNewBlock += TransferNewBlock;
        DevelopeMain.gameIsPlaying += GameIsPlaying;

        GameOverMenu.leaveTheGame += ResetGameTime;

        BlockManager.comboOccuring += UpdateCombo;
        CubeNumberManager.updateIntervall += UpdateDebugTextWithDicitonary;

        mComboAnimation = ComboFrame.GetComponent<Animation>();

        //TestImage.color = Color.green;
    }

    private void OnDisable()
    {
        GamingManager.scoreChanging -= UpdateScore;

        DevelopeMain.createNewBlock -= TransferNewBlock;
        DevelopeMain.gameIsPlaying -= GameIsPlaying;

        GameOverMenu.leaveTheGame -= ResetGameTime;

        BlockManager.comboOccuring -= UpdateCombo;
        CubeNumberManager.updateIntervall -= UpdateDebugTextWithDicitonary;
    }

    // Update is called once per frame
    private void Update()
    {
        Clock();
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (DebugPanel.GetComponent<CanvasGroup>().alpha >= 1f)
                DebugPanel.GetComponent<CanvasGroup>().alpha = 0f;
            else
                DebugPanel.GetComponent<CanvasGroup>().alpha = 1f;
            //mLerpValue += .125f;
            //if (mLerpValue > 1)
            //    mLerpValue = 0f;
            //TestImage.color = Color.Lerp(Color.green, Color.red, mLerpValue);
        }

        if (mUpdateDisplayScore)
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
    
    public void UpdateCombo(int aComboCount)
    {
        if (aComboCount < 1)
            return;

        if (aComboCount > GamingManager.Instance.MaxCombo)
            GamingManager.Instance.MaxCombo = aComboCount;

        ComboCountText.text = aComboCount.ToString();
        mComboAnimation.Play();
    }

    public void UpdateDebugTextWithDicitonary(Dictionary<int, int> originalValue, Dictionary<int, int> currentValue)
    {
        string displayText = "Num\tOri\tCur\n";
        foreach (int key in originalValue.Keys)
        {
            if (currentValue.ContainsKey(key))
                displayText += key.ToString() + " :\t\t" + originalValue[key] + "\t\t" + currentValue[key] + "\n";
            else
                displayText += key.ToString() + " :\t\t" + originalValue[key] + "\n";
        }
        TempDebugText.text = displayText;
    }

    //public void ComboAppear(int aComboCount, int aComboScore, string aComboText)
    //{
    //    ComboCountText.text = aComboCount.ToString();
    //    ComboTitleText.text = aComboText;
    //    ComboScoreText.text = aComboScore.ToString();

    //    mComboTextFadingTime = 0f;
    //    mComboTextColor.a = 1f;
    //    mComboAppear = true;
    //}

    public void TransferNewBlock(Block aNewBlock)
    {
        if (aNewBlock != null)
            aNewBlock.SetCubeNumbers(GamingManager.Instance.NextCubeNumbers);

        changeNextBlock?.Invoke();
    }

    public void GameIsPlaying(bool anIsPlaying)
    {
        mGameIsPlaying = anIsPlaying;
    }

    private void UpdateDisplayScore()
    {
        if(mCurrentDisplayScore < mCurrentTotalScore)
        {
            if((mCurrentTotalScore - mCurrentDisplayScore) > 1000)
                mCurrentDisplayScore += 101;
            else if((mCurrentTotalScore - mCurrentDisplayScore) > 100)
                mCurrentDisplayScore += 11;
            else
                mCurrentDisplayScore++;

            if (mCurrentDisplayScore >= mCurrentTotalScore)
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

        //mGameTimer += Time.deltaTime;
        GamingManager.Instance.GameTime += Time.deltaTime;

        int seconds = (int)(GamingManager.Instance.GameTime % 60);
        int minutes = (int)((GamingManager.Instance.GameTime / 60) % 60);
        int hours = (int)((GamingManager.Instance.GameTime / 3600) % 60);

        GamingManager.Instance.GameTimeString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        TimeText.text = GamingManager.Instance.GameTimeString;
    }

    //private void ComboTextFading()
    //{  
    //    ComboCountText.color = mComboTextColor;
    //    ComboTitleText.color = mComboTextColor;
    //    ComboScoreText.color = mComboTextColor;

    //    mComboTextColor = Color.Lerp(mComboTextColor, mTransparentColor, mComboTextFadingTime);
    //    if (mComboTextFadingTime < 1f)
    //        mComboTextFadingTime += Time.deltaTime / 50f;
    //    else
    //        mComboAppear = false;
    //}

    private void ResetGameTime()
    {
        GamingManager.Instance.GameTime = 0f;
    }
}
