using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGUI : MonoBehaviour
{
    public Image RootCube;
    public Image SubCube;

    public Text RootNumberText;
    public Text SubNumberText;

    private int mRootNumber = 0;
    public int RootNumber { get { return mRootNumber; } }

    private int mSubNumber = 0;
    public int SubNumber { get { return mSubNumber; } }

    private List<int> mPreviousNumber = new List<int>();
    private List<bool> mUsedNumber = new List<bool>();

    private void Start()
    {
        for (int i = 0; i < 8; i++)
            mUsedNumber.Add(false);

        mRootNumber = RandomNewNumber();
        RootNumberText.text = RootNumber.ToString();
        RootCube.color = SupportTools.GetCubeColorOf(RootNumber);

        // add in the new number for the root cube
        //mPreviousNumber.Add(RootNumber);

        // randomize a new number for the sub cube
        mSubNumber = RandomNewNumber();
        SubNumberText.text = SubNumber.ToString();
        SubCube.color = SupportTools.GetCubeColorOf(SubNumber);
    }

    public List<int> NewNumber()
    {
        // clear the previous cubenumber and store the current number
        mPreviousNumber.Clear();
        mPreviousNumber.Add(RootNumber);
        mPreviousNumber.Add(SubNumber);

        // randomize a new number for the root cube
        mRootNumber = RandomNewNumber();
        RootNumberText.text = RootNumber.ToString();
        RootCube.color = SupportTools.GetCubeColorOf(RootNumber);

        // add in the new number for the root cube
        //mPreviousNumber.Add(RootNumber);

        // randomize a new number for the sub cube
        mSubNumber = RandomNewNumber();
        SubNumberText.text = SubNumber.ToString();
        SubCube.color = SupportTools.GetCubeColorOf(SubNumber);

        // return the previous number for the new creating cube
        return mPreviousNumber;
    }

    private int RandomNewNumber()
    {
        int newNumber = SupportTools.RNG(0, 8);
        while (mUsedNumber[newNumber] == true)
        {
            newNumber = SupportTools.RNG(0, 8);
        }

        mUsedNumber[newNumber] = true;
        if(!mUsedNumber.Contains(false))
        {
            for (int i = 0; i < mUsedNumber.Count; i++)
                mUsedNumber[i] = false;
        }

        return newNumber;
    }
}
