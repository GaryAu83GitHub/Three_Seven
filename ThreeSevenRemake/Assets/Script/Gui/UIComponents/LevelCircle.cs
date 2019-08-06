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

    private float mPreviewCurrentValue = 0f;
    private float mMainCurrentValue = 0f;

    private bool mFillupPreviewCircle = false;
    private bool mFillupMainCircle = false;

    private float mCurrentLevelSectionValue = 0f;

    private bool mLevelUp = false;


    // Start is called before the first frame update
    void Start()
    {
        //LevelManager.levelUp += LevelUp;
        LevelManager.addLevelScore += AddLevelScore;
        LevelManager.fillUpTheMain += FillupMainBar;

        LevelText.text = LevelManager.Instance.CurrentLevel.ToString();

        mCurrentLevelSectionValue = LevelManager.Instance.GetCurrentLevelData.UIBarFillingSectionValue;
    }

    private void OnDestroy()
    {
        //LevelManager.levelUp -= LevelUp;
        LevelManager.addLevelScore -= AddLevelScore;
        LevelManager.fillUpTheMain -= FillupMainBar;
    }

    // Update is called once per frame
    void Update()
    {
        PreviewCircleFilling();
        MainCircleFilling();
    }

    private void PreviewCircleFilling()
    {
        if (!mFillupPreviewCircle)
            return;

        mPreviewCurrentValue += Time.deltaTime;

        if(mPreviewCurrentValue > 1f)
        {
            mPreviewCurrentValue = 0;
            mFillupPreviewCircle = false;
        }
        else if(mPreviewCurrentValue >= LevelManager.Instance.CurrentFillupAmount)
        {
            mPreviewCurrentValue = LevelManager.Instance.CurrentFillupAmount;
            mFillupPreviewCircle = false;
        }
        
        LevelPreviewValueFillingImage.fillAmount = mPreviewCurrentValue;
    }

    private void MainCircleFilling()
    {
        if (!mFillupMainCircle)
            return;

        mMainCurrentValue += Time.deltaTime;
        if(mMainCurrentValue >= 1f)
        {
            LevelText.text = (LevelManager.Instance.CurrentLevel).ToString();
            LevelUpAnimation.Play();

            mMainCurrentValue = 0f;
            mFillupMainCircle = false;
            mLevelUp = false;
        }
        else if (!mLevelUp && mMainCurrentValue >= LevelManager.Instance.CurrentFillupAmount)
        {
            mMainCurrentValue = LevelManager.Instance.CurrentFillupAmount;
            mFillupMainCircle = false;
        }

        LevelValueFillingImage.fillAmount = mMainCurrentValue;
    }
    
    private void AddLevelScore()
    {
        mFillupPreviewCircle = true;
    }

    private void FillupMainBar(bool isLevelUp)
    {
        mFillupMainCircle = true;
        mLevelUp = isLevelUp;
    }
}
