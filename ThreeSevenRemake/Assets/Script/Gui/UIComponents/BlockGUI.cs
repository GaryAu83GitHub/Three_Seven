using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    public List<int> NextNumber { get { return mNextNumbers; } }
    private List<int> mNextNumbers = new List<int>();
    
    private void Start()
    {
        Block.swapWithPreviewBlock += SwapWithOriginalBlock;
        DevelopeMainGUI.changeNextBlock += NewNumber;

        NewNumber();
    }

    private void OnDestroy()
    {
        Block.swapWithPreviewBlock -= SwapWithOriginalBlock;
        DevelopeMainGUI.changeNextBlock -= NewNumber;
    }

    private void Update()
    {
    }
    
    public void NewNumber()
    {
        mNextNumbers.Clear();
        mNextNumbers = new List<int>(GameManager.Instance.GenerateNewCubeNumber());

        // randomize a new number for the root cube
        mRootNumber = mNextNumbers[0];
        RootNumberText.text = RootNumber.ToString();
        RootCube.color = SupportTools.GetCubeHexColorOf(RootNumber);

        // randomize a new number for the sub cube
        mSubNumber = mNextNumbers[1];
        SubNumberText.text = SubNumber.ToString();
        SubCube.color = SupportTools.GetCubeHexColorOf(SubNumber);

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
