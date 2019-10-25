﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreGUIPanel : MenuPanelBase
{
    private enum ButtonIndex
    {
        EXIT_BUTTON,
    }

    private enum SortMode
    {
        BY_NAME,
        BY_LEVEL,
        BY_SCORE,
        BY_COMBO,
        BY_TIME,
    }

    public List<HighscoreListComponent> ListComponents;

    //private int mCurrentSortMode = 0;
    private List<RoundResultData> mHighscores = new List<RoundResultData>();
    private int mCurrentScrollValue = 0;
    private int mMaxScrollValue = 0;

    public override void Start()
    {
        mPanelIndex = MenuPanelIndex.HIGHSCORE_PANEL;

        //Buttons[(int)ButtonIndex.EXIT_BUTTON].onClick.AddListener(ExitHighscore);

        base.Start();

        UpdateList();
    }

    protected override void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    {
        if (ControlManager.Ins.MenuNavigation(CommandIndex.NAVI_UP))
            ScrollListUp();
        if (ControlManager.Ins.MenuNavigation(CommandIndex.NAVI_DOWN))
            ScrollListDown();
        if (ControlManager.Ins.MenuCancelButtonPressed())
            ExitHighscore();
    }

    private void ScrollListUp()
    {
        mCurrentScrollValue--;
        if (mCurrentScrollValue < 0)
            mCurrentScrollValue = 0;

        Display();
    }

    private void ScrollListDown()
    {
        mCurrentScrollValue++;
        if (mCurrentScrollValue > mMaxScrollValue)
            mCurrentScrollValue = mMaxScrollValue;

        Display();
    }

    private void ExitHighscore()
    {
        //gameObject.SetActive(false);
        MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
    }

    public void UpdateList()
    {
        mHighscores = new List<RoundResultData>(HighScoreManager.Instance.GetListSortBy(TableCategory.SCORE));
        if (mHighscores.Count > 10)
        {
            //ScrollButtonContain.SetActive(true);
            mMaxScrollValue = mHighscores.Count - 10;
        }

        mCurrentScrollValue = 0;
        //UpButton.interactable = false;
        //DownButton.interactable = true;

        Display();
    }

    private void Display()
    {
        for (int i = 0; i < ((mHighscores.Count < 10) ? mHighscores.Count : 10); i++)
        {
            ListComponents[i].gameObject.SetActive(true);
            ListComponents[i].SetData(mCurrentScrollValue + (i + 1), mHighscores[mCurrentScrollValue + i]);
        }
    }
}