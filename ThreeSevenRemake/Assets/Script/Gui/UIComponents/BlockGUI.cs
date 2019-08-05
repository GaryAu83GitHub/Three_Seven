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
    private List<int> mNextNumbers = new List<int>();
    
    private void Start()
    {
        Block.swapWithPreviewBlock += SwapWithOriginalBlock;

        mRootNumber = CubeNumberGenerator.Instance.GetNewRootNumber;    //RandomNewRootCubeNumber();
        RootNumberText.text = RootNumber.ToString();
        RootCube.color = SupportTools.GetCubeHexColorOf(RootNumber);

        // randomize a new number for the sub cube
        mSubNumber = CubeNumberGenerator.Instance.GetNewSubNumber;      //RandomNewRootCubeNumber();
        SubNumberText.text = SubNumber.ToString();
        SubCube.color = SupportTools.GetCubeHexColorOf(SubNumber);
    }

    private void OnDestroy()
    {
        Block.swapWithPreviewBlock -= SwapWithOriginalBlock;
    }

    private void Update()
    {
    }

    public List<int> NewNumber()
    {
        // clear the previous cubenumber and store the current number
        mPreviousNumber.Clear();
        mPreviousNumber.Add(RootNumber);
        mPreviousNumber.Add(SubNumber);

        mNextNumbers.Clear();
        mNextNumbers.Add(CubeNumberGenerator.Instance.GetNewRootNumber);
        mNextNumbers.Add(CubeNumberGenerator.Instance.GetNewSubNumber);

        // randomize a new number for the root cube
        mRootNumber = mNextNumbers[0];
        RootNumberText.text = RootNumber.ToString();
        RootCube.color = SupportTools.GetCubeHexColorOf(RootNumber);

        // randomize a new number for the sub cube
        mSubNumber = mNextNumbers[1];
        SubNumberText.text = SubNumber.ToString();
        SubCube.color = SupportTools.GetCubeHexColorOf(SubNumber);

        // return the previous number for the new creating cube
        return mPreviousNumber;
    }

    private void SwapWithOriginalBlock(Block anOrignalBlock)
    {
        List<int> tempCubeNumbers = new List<int>(anOrignalBlock.CubeNumbers);
        anOrignalBlock.SetCubeNumbers(mNextNumbers);

        mNextNumbers.Clear();
        mNextNumbers = new List<int>(tempCubeNumbers);

        mRootNumber = mNextNumbers[0];
        RootNumberText.text = RootNumber.ToString();
        RootCube.color = SupportTools.GetCubeHexColorOf(RootNumber);

        // randomize a new number for the sub cube
        mSubNumber = mNextNumbers[1];
        SubNumberText.text = SubNumber.ToString();
        SubCube.color = SupportTools.GetCubeHexColorOf(SubNumber);
    }
}
