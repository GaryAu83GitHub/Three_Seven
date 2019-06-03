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
    //public Image TestImage;

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
    //private float mLerpValue = 0f;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateLevel(GameManager.Instance.CurrentLevel);

        GameManager.comboOccuring += ComboAppear;
        GameManager.levelChanging += UpdateLevel;
        GameManager.scoreChanging += UpdateScore;

        DevelopeMain.createNewBlock += TransferNewBlock;
        DevelopeMain.gameIsPlaying += GameIsPlaying;

        //TestImage.color = Color.green;
    }

    private void OnDisable()
    {
        GameManager.comboOccuring -= ComboAppear;
        GameManager.levelChanging -= UpdateLevel;
        GameManager.scoreChanging -= UpdateScore;

        DevelopeMain.createNewBlock -= TransferNewBlock;
        DevelopeMain.gameIsPlaying -= GameIsPlaying;
    }

    // Update is called once per frame
    private void Update()
    {
        Clock();

        if(mComboAppear)
            ComboTextFading();

        if(Input.GetKeyDown(KeyCode.Insert))
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
    }

    public void UpdateScore(int aNewScore)
    {
        ScoreText.text = aNewScore.ToString();
    }

    public void UpdateLevel(int aNewLevel)
    {
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
}
