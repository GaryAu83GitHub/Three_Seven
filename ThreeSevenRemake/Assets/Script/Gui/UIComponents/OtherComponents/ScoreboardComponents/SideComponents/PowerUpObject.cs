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

    public bool PowerUpUseable { get { return (mCurrentCharges == ChargeTimes); } }

    /// <summary>
    /// The uncharged power up icon image component
    /// </summary>
    private Image BaseIconImage;

    /// <summary>
    /// The instance for the CanvasGroup component, use for the object appearence
    /// when been selected or not
    /// </summary>
    private CanvasGroup mCG;

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

    private const float CHARGING_SPEED = .01f;
    private const float DISCHARGE_SPEED = .05f;

    void Awake()
    {
        mFillingSectionValue = 1f / (float)ChargeTimes;
    }

    void Start()
    {
        BaseIconImage = GetComponent<Image>();
        mCG = GetComponent<CanvasGroup>();
        Reset();
    }

    void Update()
    {
        FillingIcon();
        DepleteIcon();
    }

    /// <summary>
    /// Reset this objects changeable value
    /// </summary>
    public void Reset()
    {
        mCurrentFillingValue = 0;
        mCurrentCharges = 0;
        Selected(false);
        IconFilling();
    }

    public void Selected(bool isSelected)
    {
        if (!isSelected)
            mCG.alpha = .5f;
        else
            mCG.alpha = 1f;
    }

    /// <summary>
    /// Increase this object's charging count value and active the charging trigger.
    /// If the charge count surpass the max number of charge times this item has, the 
    /// current charges will be equal to the max charge number 
    /// </summary>
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

    /// <summary>
    /// Active the discharge trigger and return the type of this item
    /// </summary>
    public PowerUpType Discharge()
    {
        mIsDischarging = true;
        mCurrentCharges = 0;
        return Type;
    }

    /// <summary>
    /// Change the filling value with the filling constant value when the trigger for
    /// charging is active. The trigger will deactive once the filling value pass over
    /// the current fillup value.
    /// </summary>
    private void FillingIcon()
    {
        if (!mIsCharging)
            return;

        if (mCurrentFillingValue >= mFillUpValue)
        {
            mCurrentFillingValue = mFillUpValue;
            mIsCharging = false;
        }
        else
            mCurrentFillingValue += CHARGING_SPEED;

        IconFilling();
    }

    /// <summary>
    /// Change the filling value with the deplete constant value when the trigger for 
    /// discharge is active. The trigger will deactive once the filling value falls 
    /// till equal or below 0.
    /// </summary>
    private void DepleteIcon()
    {
        if (!mIsDischarging)
            return;

        if (mCurrentFillingValue <= 0f)
        {
            mCurrentFillingValue = 0f;
            mIsDischarging = false;
        }
        else
            mCurrentFillingValue -= DISCHARGE_SPEED;

        IconFilling();
    }

    /// <summary>
    /// Fill the Filling icon image with the current filling value
    /// </summary>
    private void IconFilling()
    {
        IconFillingImage.fillAmount = mCurrentFillingValue;
    }
}
