using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTermBox : ScoreboardComponentBase
{
    public Image LevelBar;
    public Image UpArrowImage;
    public Image DownArrowImage;

    //public Animation LevelUpAnimation;

    private int mLevel = 10;
    private float mFillingAmountValue = 0f;
    private float mFillupBarValue = 0f;
    private bool mFillingBar = false;

    public override void Start()
    {
        MainGamePanel.onAddLevelFilling += FillingBarWith;
    }

    private void OnDestroy()
    {
        MainGamePanel.onAddLevelFilling -= FillingBarWith;
    }

    protected override void ComponentsDisplay()
    {
        ValueText.text = mLevel.ToString();
        BarFilling();
    }

    private void FillingBarWith(float aFillingValue)
    {
        mFillingAmountValue += aFillingValue;
        mFillingBar = true;
    }

    private void BarFilling()
    {
        if (!mFillingBar)
            return;

        mFillupBarValue += Time.deltaTime;
        if(mFillupBarValue >= 1f)
        {
            mFillupBarValue = 0f;
            mFillingAmountValue = 0f;
            mFillingBar = false;
        }
        else if(mFillupBarValue >= mFillingAmountValue)
        {
            mFillupBarValue = mFillingAmountValue;
            mFillingBar = false;
        }

        LevelBar.fillAmount = mFillupBarValue;
    }
}
