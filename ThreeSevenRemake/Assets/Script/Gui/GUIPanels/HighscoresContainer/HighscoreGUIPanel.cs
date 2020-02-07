using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is attach to the HighscoreGUIPanel
/// </summary>
public class HighscoreGUIPanel : MenuEnablePanelBase
{
    private enum ButtonIndex
    {
        SURVIVE_BUTTON,
        EXIT_BUTTON,
    }

    private enum SortMode
    {
        //BY_NAME,
        BY_DIGIT,
        BY_CHAIN,
        BY_TASK,
        BY_LEVEL,
        BY_TIME,
        BY_ODDS,
        BY_SCORE,
    }

    private enum DisplayChallenge
    {
        //CLASSIC_CHALLENGE,
        SURVIVE_CHALLENGE
    }

    public PanelButtonsContainer ButtonContainer;
    public CanvasGroup TableCanvasGroup;

    public GameObject SlotContainer;

    public Sprite SortButtonSelectedStateSprite;
    public Sprite SortButtonDeselectedStateSprite;
    public List<Button> SortButtons;

    public List<HighScoreListObjectSlot> SlotComponents;

    private List<SavingResultData> mHighScores = new List<SavingResultData>();
    private List<int> mDigitBinaryList = new List<int>();

    private int mListScrollCurrentValue = 0;
    private int mMaxScrollValue = 0;
    private int mDisplayingSlotCount = 0;
    private int mDisplayEnableDigitIndex = 0;
    private int mCurrentHighlightSlotIndex = 0;
    
    private bool mListIsDisplaying = false;
    private bool mSortCurrentListDescending = true;

    private SortMode mSortButtonCurrentIndex = SortMode.BY_SCORE;

    public override void Start()
    {
        mPanelIndex = GUIPanelIndex.HIGHSCORE_PANEL;

        base.Start();
    }

    public override void Enter()
    {
        base.Enter();
        mListIsDisplaying = false;
        TablePanelDisplay();
        SetSelectedButton(0);
        AllSlotInvisible();
    }

    protected override void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    {
        if (mListIsDisplaying)
        {
            if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_UP))
                NavigateTableSlots(-1);//ScrollList(1);
            if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_DOWN))
                NavigateTableSlots(1);//ScrollList(-1);
            if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
                NavigateSortButtons(1);
            if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
                NavigateSortButtons(-1);

            if (ControlManager.Ins.MenuCancelButtonPressed())
                ExitHighscore();
        }
        else
        {
            base.NavigateMenuButtons(CommandIndex.NAVI_DOWN, CommandIndex.NAVI_UP);
        }
    }

    protected override void SelectButtonPressed()
    {
        if (!mListIsDisplaying)
        {
            switch (mCurrentSelectButtonIndex)
            {
                case (int)ButtonIndex.EXIT_BUTTON:
                    GUIPanelManager.Instance.GoTo(GUIPanelIndex.TITLE_PANEL);//ExitHighscore();
                    break;
                default:
                    DisplayList();
                    break;
            }
            return;
        }
        if(mSortButtonCurrentIndex == SortMode.BY_DIGIT)
        {
            mDisplayEnableDigitIndex++;
            if (mDisplayEnableDigitIndex >= mDigitBinaryList.Count)
                mDisplayEnableDigitIndex = 0;

            mHighScores = new List<SavingResultData>(HighScoreManager.Instance.GetListOfEnableDigit(mDigitBinaryList[mDisplayEnableDigitIndex]));
            mDisplayingSlotCount = ((mHighScores.Count < 10) ? mHighScores.Count : 10);

            AllSlotInvisible();
            UpdateList();
        }
        else
        {
            mHighScores = GetSortedList();
            AllSlotInvisible();
            UpdateList();
        }
    }
    
    private void TablePanelDisplay()
    {
        ButtonContainer.ActiveContainer(!mListIsDisplaying);
        SlotContainer.SetActive(mListIsDisplaying);

        if (mListIsDisplaying)
            TableCanvasGroup.alpha = 1f;
        else
            TableCanvasGroup.alpha = .5f;

    }

    private void DisplayList()
    {
        mListIsDisplaying = !mListIsDisplaying;
        
        TablePanelDisplay();
        if (mListIsDisplaying)
        {
            // if the button for survive button is highlighted, it'll sort the list of survival challenge
            if (mCurrentSelectButtonIndex == (int)ButtonIndex.SURVIVE_BUTTON)
                FillListOf(GameMode.SURVIVAL);
            // if the button for classic button is highlighted, it'll sort the list of classic challenge
            //if (mCurrentSelectButtonIndex == (int)ButtonIndex.CLASSIC_CHALLENGE)
            UpdateList();
        }
    }

    private void NavigateTableSlots(int aDirection)
    {
        if (mCurrentHighlightSlotIndex != -1)
            SlotComponents[mCurrentHighlightSlotIndex].SetAsSelected(false);

        mCurrentHighlightSlotIndex += aDirection;
        if(mCurrentHighlightSlotIndex < 0)
            mCurrentHighlightSlotIndex = 0;
        else if (mCurrentHighlightSlotIndex >= mDisplayingSlotCount)
            mCurrentHighlightSlotIndex = mDisplayingSlotCount-1;

        SlotComponents[mCurrentHighlightSlotIndex].SetAsSelected(true);

        if (mMaxScrollValue > 0)
        {
            if (mCurrentHighlightSlotIndex < 0)
            {
                mCurrentHighlightSlotIndex = 0;
                ScrollList(-1);
            }
            if (mCurrentHighlightSlotIndex > 9)
            {
                mCurrentHighlightSlotIndex = 9;
                ScrollList(1);
            }
        }
    }

    private void ScrollList(int aDirection)
    {
        mListScrollCurrentValue += aDirection;
        if (mListScrollCurrentValue < 0)
            mListScrollCurrentValue = 0;
        else if (mListScrollCurrentValue > mMaxScrollValue)
            mListScrollCurrentValue = mMaxScrollValue;
        DisplayTable();
    }

    /// <summary>
    /// This is to navigate through the number of sorting mode buttons.
    /// The list will be sorted after the highlight button.
    /// </summary>
    /// <param name="aDirection">The direction of how the sort button index will move to</param>
    private void NavigateSortButtons(int aDirection)
    {
        mSortButtonCurrentIndex += aDirection;
        if (mSortButtonCurrentIndex > SortMode.BY_SCORE)
            mSortButtonCurrentIndex = SortMode.BY_DIGIT;
        else if(mSortButtonCurrentIndex < SortMode.BY_DIGIT)
            mSortButtonCurrentIndex = SortMode.BY_SCORE;

        SortButtonDisplay();

        DeselectCurrentSelectedSlot();
        UpdateList();

        //AllSlotInvisible();
    }

    /// <summary>
    /// This will deactive the score table panel and reactive the main button panel
    /// </summary>
    private void ExitHighscore()
    {
        mListIsDisplaying = false;
        DeselectCurrentSelectedSlot();

        TablePanelDisplay();
    }

    /// <summary>
    /// This is more of a reset function to reset the display of the slot after game or
    /// sorting mode have change, and update the max scroll value depending if the
    /// number of data surpass 10 and reset the scroll value to the first
    /// </summary>
    public void UpdateList()
    {
        if (mHighScores.Count > 10)
        {
            mMaxScrollValue = mHighScores.Count - 10;
        }

        mListScrollCurrentValue = 0;
        DisplayTable();
    }

    /// <summary>
    /// This will turn the number of displaying slot visible and set the displaying
    /// data into the slot object
    /// </summary>
    private void DisplayTable()
    {
        for (int i = 0; i < mDisplayingSlotCount; i++)
        {
            SlotComponents[i].SlotVisible(true);
            SlotComponents[i].SetSlotData(mListScrollCurrentValue + (i + 1), mHighScores[mListScrollCurrentValue + i]);
        }
    }

    /// <summary>
    /// Call the highscore manager to fill up its active score list from the requested 
    /// challenge text file score list.
    /// The list will be sorted by score value as it default
    /// </summary>
    /// <param name="aMode">Requested game mode</param>
    private void FillListOf(GameMode aMode)
    {
        mHighScores.Clear();
        mDigitBinaryList.Clear();

        HighScoreManager.Instance.SetActiveList(aMode);
        mDigitBinaryList = HighScoreManager.Instance.EnableDigitsList;
        mDigitBinaryList.Sort();

        mSortButtonCurrentIndex = SortMode.BY_SCORE;
        SortButtonDisplay();
        mHighScores = GetSortedList();

        mDisplayingSlotCount = ((mHighScores.Count < 10) ? mHighScores.Count : 10);

        AllSlotInvisible();
    }

    /// <summary>
    /// Turn of the visibility of every highscore slot in the container
    /// </summary>
    private void AllSlotInvisible()
    {
        for (int i = 0; i < SlotComponents.Count; i++)
            SlotComponents[i].SlotVisible(false);
        mCurrentHighlightSlotIndex = -1;
    }

    /// <summary>
    /// Reset the current selected slot object to unselected appearence and change
    /// the highlight slot index to -1 so no slot object will be highlighted
    /// </summary>
    private void DeselectCurrentSelectedSlot()
    {
        if (mCurrentHighlightSlotIndex != -1)
            SlotComponents[mCurrentHighlightSlotIndex].SetAsSelected(false);
        mCurrentHighlightSlotIndex = -1;
    }

    /// <summary>
    /// Get a sorted list from the HighScoreManager by the current highlighted
    /// sort button
    /// </summary>
    /// <returns>List got from HighScoreManager</returns>
    private List<SavingResultData> GetSortedList()
    {
        TableCategory sortBy = TableCategory.SCORE;
        switch(mSortButtonCurrentIndex)
        {
            case SortMode.BY_CHAIN:
                sortBy = TableCategory.CHAIN;
                break;
            case SortMode.BY_TASK:
                sortBy = TableCategory.TASK;
                break;
            case SortMode.BY_LEVEL:
                sortBy = TableCategory.LEVEL;
                break;
            case SortMode.BY_TIME:
                sortBy = TableCategory.TIME;
                break;
            case SortMode.BY_ODDS:
                sortBy = TableCategory.ODDS;
                break;
            default:
                sortBy = TableCategory.SCORE;
                break;
        }
        mSortCurrentListDescending = !mSortCurrentListDescending;

        return HighScoreManager.Instance.GetListSortBy(sortBy, mSortCurrentListDescending);
    }

    /// <summary>
    /// Change the appearence of the sort buttons.
    /// All button's background image will change to the transparant sprite
    /// except the button that with the current sort mode
    /// </summary>
    private void SortButtonDisplay()
    {
        for(int i = 0; i < SortButtons.Count; i++)
        {
            SortButtons[i].image.sprite = SortButtonDeselectedStateSprite;
            if (i == (int)mSortButtonCurrentIndex)
                SortButtons[i].image.sprite = SortButtonSelectedStateSprite;
        }

        mSortCurrentListDescending = true;
    }
}
