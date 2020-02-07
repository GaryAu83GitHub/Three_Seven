using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreListObjectSlot : GuiSlotBase
{
    public TextMeshProUGUI RankText;
    public TextMeshProUGUI ChainText;
    public TextMeshProUGUI TaskText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI OddsText;
    public TextMeshProUGUI ScoreText;

    public List<Image> DigitImages;

    public Sprite HighlightSprite;
    public Sprite DefaultSprite;

    private Image mBG;

    private bool mIsSelected = false;
    private bool mLoopPlayerName = false;

    private string mPlayerNameString = "";
    private string mDisplayName = "";

    private int mCurrentNameStringStartIndex = 0;

    private float mTimer = 0f;

    private const float mLoopTime = .085f;

    public override void Awake()
    {
        base.Awake();
        mBG = GetComponent<Image>();
        mBG.sprite = DefaultSprite;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (mIsSelected && mLoopPlayerName)
            PlayerNameLoopAnimation();
    }

    /// <summary>
    /// Set the displaying data on this slot object and it current place in the list
    /// </summary>
    /// <param name="aRankNumber">the number of list this data is places at</param>
    /// <param name="aData">result data this object shall display</param>
    public void SetSlotData(int aRankNumber, SavingResultData aData)
    {
        mPlayerNameString = aData.PlayerName;
        mCurrentNameStringStartIndex = 0;

        mLoopPlayerName = LoopThePlayerName();        

        RankText.text = aRankNumber.ToString();
        TitleText.text = mDisplayName;
        ChainText.text = aData.LongestChains.ToString();
        TaskText.text = aData.CompletedTaskCount.ToString();
        LevelText.text = aData.GainedLevel.ToString();
        TimeText.text = TimeTool.TimeString(aData.PlayTime);
        float oddsProcent = (Mathf.Round(aData.AverageOdds * 1000) / 1000f);
        OddsText.text = (oddsProcent * 100).ToString();
        ScoreText.text = aData.TotalScores.ToString();

        DigitImages[0].gameObject.SetActive(aData.EnableDigit2);
        DigitImages[1].gameObject.SetActive(aData.EnableDigit3);
        DigitImages[2].gameObject.SetActive(aData.EnableDigit4);
        DigitImages[3].gameObject.SetActive(aData.EnableDigit5);
    }

    /// <summary>
    /// Change the object background image depending on if this object is set as
    /// selected or not
    /// </summary>
    /// <param name="isSelected">indicating if this object is selected</param>
    public void SetAsSelected(bool isSelected)
    {
        mIsSelected = isSelected;
        mBG.sprite = DefaultSprite;

        if (mIsSelected)
            mBG.sprite = HighlightSprite;        
    }

    /// <summary>
    /// Change the alpha value of the object's canvas group depending the given
    /// parameter value
    /// </summary>
    /// <param name="isVisible">requesting of the object's visibility, if true the
    /// object is visible, else not </param>
    public void SlotVisible(bool isVisible)
    {
        mCG.alpha = 1f;

        if (!isVisible)
            mCG.alpha = 0f;
    }

    /// <summary>
    /// CHeck if the player name lenght is greater than the display name string's 
    /// lenght. If yes, the display name string will display the player name's first
    /// 10 character and set the boolian for active the loop animation to true, else
    /// it'll only display the player name.
    /// </summary>
    /// <returns>Set the loopanimation to active or not</returns>
    private bool LoopThePlayerName()
    {
        mDisplayName = "";
        if (mPlayerNameString.Length > 10)
        {   
            mTimer = 0f;
            for (int i = 0; i < 10; i++)
                mDisplayName += mPlayerNameString[i];
            return true;
        }

        mDisplayName = mPlayerNameString;
        return false;
    }

    /// <summary>
    /// Loop the name that has more character then the count of available displaying
    /// character.
    /// The index of where the start index increase when timer reach or pass 0, and 
    /// once the start index has reached the last character index, it'll reset to 
    /// the first index.
    /// </summary>
    private void PlayerNameLoopAnimation()
    {
        mTimer -= Time.deltaTime;
        if (mTimer < 0f)
            mTimer = mLoopTime;
        else
            return;
        mDisplayName = "";
        
        if (mCurrentNameStringStartIndex >= mPlayerNameString.Length)
            mCurrentNameStringStartIndex = 0;

        // if the current start index plus 10 is lesser then the player name's lenght
        // then the display string will display the 10 char from where the player name
        // start index is
        if (((mCurrentNameStringStartIndex + 10) < mPlayerNameString.Length))
        {
            for (int i = mCurrentNameStringStartIndex; i < (mCurrentNameStringStartIndex + 10); i++)
                mDisplayName += mPlayerNameString[i];
        }
        else
        {
            for (int i = mCurrentNameStringStartIndex; i != mPlayerNameString.Length; i++)
            {
                if(i < mPlayerNameString.Length)
                    mDisplayName += mPlayerNameString[i];
                else
                    mDisplayName = "";
            }
            if(mDisplayName.Length < 10)
            {
                for (int i = mDisplayName.Length - 1; i < 10; i++)
                    mDisplayName += " ";
            }
        }

        // the text component will display what the display name has stored
        TitleText.text = mDisplayName;
        mCurrentNameStringStartIndex++;
    }
}
