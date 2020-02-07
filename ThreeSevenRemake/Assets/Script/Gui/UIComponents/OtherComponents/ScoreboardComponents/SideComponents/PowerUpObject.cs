using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUpType
{
    SWAP_BLOCK,
    POWER_2,
    POWER_3,
}

public class PowerUpObject : MonoBehaviour
{
    public Image IconFillingImage;

    /// <summary>
    /// Index of type this power up object is
    /// </summary>
    public PowerUpType Type;

    /// <summary>
    /// Number of times of charges to enable this power up
    /// </summary>
    public int ChargeTimes;

    /// <summary>
    /// The uncharged power up icon image component
    /// </summary>
    private Image BaseIconImage;

    /// <summary>
    /// The value of how much the image is filling
    /// </summary>
    private float mCurrentFillingValue = 0f;

    /// <summary>
    /// This total filling value the filling image has to fill up depending on 
    /// the charge count
    /// </summary>
    private float mFillUpValue = 0f;

    /// <summary>
    /// The filling value of how much the image fills of each charge by divide
    /// the icon image max filling which is 1 divided with the value of ChargeCount
    /// </summary>
    private float mFillingSectionValue = 0f;

    /// <summary>
    /// The current charge count this item has charged.
    /// </summary>
    private int mCurrentCharges = 0;

    /// <summary>
    /// Triggering the charging filling to the filling image
    /// </summary>
    private bool mIsCharging = false;

    /// <summary>
    /// Triggering the discharging depleate to the filling image
    /// </summary>
    private bool mIsDischarging = false;

    private const float CHARGING_SPEED = .05f;
    private const float DISCHARGE_SPEED = .1f;

    void Awake()
    {
        mFillingSectionValue = 1f / (float)ChargeTimes;
    }

    void Start()
    {
        BaseIconImage = GetComponent<Image>();    
    }

    void Update()
    {
        FillingIcon();
        DepleteIcon();
    }

    public void Charging()
    {
        mCurrentCharges++;
        if (mCurrentCharges > ChargeTimes)
        {
            mCurrentCharges = ChargeTimes;
            return;
        }
        mFillUpValue = mCurrentCharges * mFillingSectionValue;
        mIsCharging = true;
    }

    public void Discharge()
    {
        mIsDischarging = true;
    }

    private void FillingIcon()
    {
        if (!mIsCharging)
            return;

        if (mCurrentFillingValue > mFillUpValue)
        {
            mCurrentFillingValue = mFillUpValue;
            mIsCharging = false;
        }
        else
            mCurrentFillingValue += CHARGING_SPEED;

        IconFilling();
    }

    private void DepleteIcon()
    {
        if (!mIsDischarging)
            return;

        if (mCurrentFillingValue < 0f)
        {
            mCurrentFillingValue = 0f;
            mIsDischarging = false;
        }
        else
            mCurrentFillingValue -= DISCHARGE_SPEED;

        IconFilling();
    }

    private void IconFilling()
    {
        IconFillingImage.fillAmount = mCurrentFillingValue;
    }
}
