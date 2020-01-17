using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusDisplayBox : ScoreboardComponentBase
{
    public Image BarFillingImage;
    public List<GameObject> BonusDisplayObjects;

    public int ModulosValue = 1;

    public delegate void OnChangeBonusTerm(int aNewBonusTerms);
    public static OnChangeBonusTerm changeBonusTerm;

    private Rect mBarImageRect;

    private List<BonusDisplayObject> mBonusDisplayObjects = new List<BonusDisplayObject>();
    private int mBonus = 1;

    private float mHeightInterval = 0f;

    private float mBarFillingIntervalValue = 0;
    private float mBarCurrentAmountValue = 0;
    private bool mFillTheBar = false;

    public override void Start()
    {
        base.Start();

        ChainDisplayBox.updateChainCount += UpdateBonusBar;

        mBarImageRect = BarFillingImage.rectTransform.rect;
        mHeightInterval = (mBarImageRect.height / 9);
        float bonusObjectActiveValueFragment = (1f / 9f);
        mBarFillingIntervalValue = (bonusObjectActiveValueFragment / ModulosValue);

        mBonusDisplayObjects.Clear();

        for(int i = 0; i < BonusDisplayObjects.Count; i++)
        {
            mBonusDisplayObjects.Add(BonusDisplayObjects[i].GetComponent<BonusDisplayObject>());

            if(i==0)
                mBonusDisplayObjects[i].SetupBonusObject(new Vector2(0, 0), 1, 0f);
            else if(i==9)
                mBonusDisplayObjects[i].SetupBonusObject(new Vector2(0, mBarImageRect.height), 10, 9 * bonusObjectActiveValueFragment);
            else
                mBonusDisplayObjects[i].SetupBonusObject(new Vector2(0, (i * mHeightInterval)), i + 1, i * bonusObjectActiveValueFragment);
        }

        //mBonusDisplayObjects[0].SetupBonusObject(new Vector2(0, 0), 1, 0f);

        //for (int i = 1; i < 9; i++)
        //    mBonusDisplayObjects[i].SetupBonusObject(new Vector2(0, (i * mHeightInterval)), i + 1, i * bonusObjectActiveValueFragment);

        //mBonusDisplayObjects[9].SetupBonusObject(new Vector2(0, mBarImageRect.height), 10, 9 * bonusObjectActiveValueFragment);

        ActiveBonusObjects();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        ChainDisplayBox.updateChainCount -= UpdateBonusBar;
    }

    protected override void ComponentsDisplay()
    {
        if (mFillTheBar)
        {
            if (BarFillingImage.fillAmount < mBarCurrentAmountValue)
            {
                BarFillingImage.fillAmount += 0.001f;
                if (BarFillingImage.fillAmount >= mBarCurrentAmountValue)
                    BarFillingImage.fillAmount = mBarCurrentAmountValue;
            }
            else if(BarFillingImage.fillAmount > mBarCurrentAmountValue)
            {
                BarFillingImage.fillAmount -= 0.001f;
                if (BarFillingImage.fillAmount <= mBarCurrentAmountValue)
                    BarFillingImage.fillAmount = mBarCurrentAmountValue;
            }

            if (BarFillingImage.fillAmount == mBarCurrentAmountValue)
                mFillTheBar = false;
        }
    }

    private void UpdateBonusBar(int aCurrentChainCount)
    {
        mBarCurrentAmountValue = aCurrentChainCount * mBarFillingIntervalValue;
        ActiveBonusObjects();

        if (mBarCurrentAmountValue >= 1)
            return;
        else
            mFillTheBar = true;

        
        if ((aCurrentChainCount % ModulosValue) != 0)
            return;

        mBonus = (1 + (aCurrentChainCount / ModulosValue));
        changeBonusTerm?.Invoke(mBonus);

    }

    private void ActiveBonusObjects()
    {
        for (int i = 1; i < mBonusDisplayObjects.Count; i++)
            mBonusDisplayObjects[i].ActiveBonus(mBarCurrentAmountValue);
    }
}
