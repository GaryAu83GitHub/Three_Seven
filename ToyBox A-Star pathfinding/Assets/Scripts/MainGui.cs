using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGui : MonoBehaviour
{
    public Image TextImage;

    private float mFillingValue = 0f;
    private float mFillingIntervalValue = 0f;
    private float mFillingSectionValue = 0f;
    private int mDividing = 10;
    private int mDividedCount = 1;
    private bool mFillup = false;
    // Start is called before the first frame update
    void Start()
    {
        mFillingSectionValue = 1f / (float)mDividing;
        mFillingIntervalValue = mFillingSectionValue / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            mFillup = true;

        Filling();
    }

    private void Filling()
    {
        if (!mFillup)
            return;

        mFillingValue += Time.deltaTime;
        if(mFillingValue >= 1f)
        {
            mFillingValue = 0f;
            mDividing += 10;
            mFillingSectionValue = 1f / (float)mDividing;
            mDividedCount = 1;
            mFillup = false;
        }
        else if(mFillingValue >= (mFillingSectionValue * mDividedCount))
        {
            mFillingValue = (mFillingSectionValue * mDividedCount);
            mDividedCount++;
            mFillup = false;
        }
        TextImage.fillAmount = mFillingValue;
    }
}
