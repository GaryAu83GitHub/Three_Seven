using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCircle : MonoBehaviour
{
    public Image LevelPreviewValueFillingImage;
    public Image LevelValueFillingImage;
    public TextMeshProUGUI LevelText;
    public Animation LevelUpAnimation;

    //public Text LevelText;
    private float PreviewFillupAmount { get { return mPreviewSectionFillingValue * mPreviewDividedCount; } }

    private float mPreviewCurrentValue = 0f;
    private float mPreviewSectionFillingValue = 0f;

    private float mMainCurrentValue = 0f;
    private float mMainSectionFillingValue = 0f;
    private float mMainFillingValue = 0f;
    private float mMainFillingValueRest = 0f;

    private int mPreviewDividedCount = 1;
    //private int mMainDividedCount = 1;

    private const int mBaseDividValue = 10;

    private bool mFillupPreviewCircle = false;
    private bool mFillupMainCircle = false;

    private float mCurrentLevelSectionValue = 0f;
    private float mNextLevelSectionValue = 0f;


    // Start is called before the first frame update
    void Start()
    {
        BlockManager.addLevelScore += FillPreviewCircle;
        GameManager.levelChangingNew += FillMainCircle;
        mPreviewSectionFillingValue = 1f / (float)(mBaseDividValue * (GameManager.Instance.CurrentLevel + 1));
        mMainSectionFillingValue = 1f / (float)(mBaseDividValue * (GameManager.Instance.CurrentLevel + 1));

        mCurrentLevelSectionValue = 1f / (float)(mBaseDividValue * (GameManager.Instance.CurrentLevel + 1));
        mNextLevelSectionValue = 1f / (float)(mBaseDividValue * (GameManager.Instance.CurrentLevel + 2));

        LevelText.text = GameManager.Instance.CurrentLevel.ToString();
    }

    private void OnDestroy()
    {
        BlockManager.addLevelScore -= FillPreviewCircle;
        GameManager.levelChangingNew -= FillMainCircle;
    }

    // Update is called once per frame
    void Update()
    {
        PreviewFilling();
        MainFilling();

        //if(Input.GetKeyDown(KeyCode.Y))
        //    LevelUpAnimation.Play();
    }

    private void PreviewFilling()
    {
        if (!mFillupPreviewCircle)
            return;

        mPreviewCurrentValue += Time.deltaTime;
        if (mPreviewCurrentValue >= 1f)
        {
            mPreviewCurrentValue = 0f;
            //mDividing += 10;
            mPreviewSectionFillingValue = 1f / (float)(mBaseDividValue + (GameManager.Instance.CurrentLevel + 1));
            mPreviewDividedCount = 1;
            mFillupPreviewCircle = false;
        }
        else if (mPreviewCurrentValue >= PreviewFillupAmount)
        {
            mPreviewCurrentValue = PreviewFillupAmount;
            mPreviewDividedCount++;
            mFillupPreviewCircle = false;
        }
        LevelPreviewValueFillingImage.fillAmount = mPreviewCurrentValue;
    }

    private void PreviewCircleFilling()
    {
        if (!mFillupPreviewCircle)
            return;

        mPreviewCurrentValue += Time.deltaTime;



        LevelPreviewValueFillingImage.fillAmount = mPreviewCurrentValue;
    }

    private void MainFilling()
    {
        if (!mFillupMainCircle)
            return;

        mMainCurrentValue += Time.deltaTime;
        if(mMainCurrentValue >= 1f)
        {
            mMainCurrentValue = 0f;
            mMainSectionFillingValue = 1f / (float)(mBaseDividValue + (GameManager.Instance.CurrentLevel + 1));

            if(mMainFillingValueRest > 0)
                mMainFillingValue = mMainFillingValueRest;
            else
                mFillupMainCircle = false;

            LevelText.text = (GameManager.Instance.CurrentLevel).ToString();
            LevelUpAnimation.Play();
        }
        else if(mMainCurrentValue >= mMainFillingValue)
        {
            mMainCurrentValue = mMainFillingValue;
            mFillupMainCircle = false;
        }

        LevelValueFillingImage.fillAmount = mMainCurrentValue;
    }

    private void FillPreviewCircle()
    {
        mFillupPreviewCircle = true;
    }

    private void FillMainCircle(int anAddOn)
    {
        mMainFillingValue = anAddOn * mMainSectionFillingValue;
        if ((mMainCurrentValue + mMainFillingValue >= 1))
        {
            mMainFillingValueRest = (mMainCurrentValue + mMainFillingValue) - 1;
            mMainFillingValue = 2;
        }
        else
        {
            mMainFillingValue += mMainCurrentValue;
        }
        mFillupMainCircle = true;
    }
}
