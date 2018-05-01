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

    private void NewNumber()
    {
        mRootNumber = SupportTools.RNG(0, 8);
        RootNumberText.text = RootNumber.ToString();
        RootCube.color = SupportTools.GetCubeColorOf(RootNumber);

        mSubNumber = SupportTools.RNG(0, 8);
        SubNumberText.text = SubNumber.ToString();
        SubCube.color = SupportTools.GetCubeColorOf(SubNumber);


    }
}
