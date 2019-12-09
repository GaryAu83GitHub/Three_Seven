using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationScript : MonoBehaviour
{
    private enum StateIndex
    {
        CREATE_NEW,
        CHANGE_ORDER,
        SWAP_BLOCK,
    }

    public List<BlockGUI> Blocks;

    public Animator Animator;

    public int Test;

    private List<List<int>> mBlockNumbers = new List<List<int>>();

    // Start is called before the first frame update
    void Start()
    {
        LabMenu.createNewBlock += CreateNewBlock;
        LabMenu.swapTheBlock += SwapTheBlock;

        for (int i = 0; i < Blocks.Count; i++)
        {
            mBlockNumbers.Add(new List<int>(GamingManager.Instance.GenerateNewCubeNumber()));
            Blocks[i].SetNumber(mBlockNumbers[i]);
        }
    }

    private void OnDestroy()
    {
        LabMenu.createNewBlock -= CreateNewBlock;
        LabMenu.swapTheBlock -= SwapTheBlock;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    ChangeAnimationState("SwapBlock");
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    ChangeAnimationState("ChangeOrder");
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    PlayAnimation("IdleBlock");
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    ChangeAnimationState("CreateNew");
        //}

        
        //Animator.SetInteger("Animation", Test);
    }

    public void CreateNewEventEnd()
    {
        mBlockNumbers[0] = new List<int>(mBlockNumbers[1]);
        mBlockNumbers[1] = new List<int>(mBlockNumbers[2]);
        mBlockNumbers[2] = new List<int>(mBlockNumbers[3]);
        mBlockNumbers[3] = new List<int>(GamingManager.Instance.GenerateNewCubeNumber());

        for (int i = 0; i < Blocks.Count; i++)
        {
            Blocks[i].SetNumber(mBlockNumbers[i]);
        }
    }

    public void ChangeOrderEvent()
    {
        List<int> tempNumbers = new List<int>(mBlockNumbers[0]);
        mBlockNumbers[0] = new List<int>(mBlockNumbers[1]);
        mBlockNumbers[1] = new List<int>(mBlockNumbers[2]);
        mBlockNumbers[2] = new List<int>(tempNumbers);

        for (int i = 0; i < Blocks.Count; i++)
        {
            Blocks[i].SetNumber(mBlockNumbers[i]);
        }
    }

    public void SwapBlockEvent()
    {
        mBlockNumbers[0] = new List<int>(mBlockNumbers[1]);
        mBlockNumbers[1] = new List<int>(mBlockNumbers[2]);
        mBlockNumbers[2] = new List<int>(mBlockNumbers[3]);
        mBlockNumbers[3] = new List<int>(GamingManager.Instance.GenerateNewCubeNumber());

        for (int i = 0; i < Blocks.Count; i++)
        {
            Blocks[i].SetNumber(mBlockNumbers[i]);
        }
    }


    private void CreateNewBlock(Block aNewBlock)
    {
        if (aNewBlock == null)
            return;

        aNewBlock.SetCubeNumbers(mBlockNumbers[0]);
        ChangeAnimationState("CreateNew");
    }

    private void SwapTheBlock(Block aSwapingBlock)
    {
        if (aSwapingBlock == null)
            return;

        mBlockNumbers[3] = new List<int>(aSwapingBlock.CubeNumbers);
        Blocks[3].SetNumber(mBlockNumbers[3]);

        aSwapingBlock.SetCubeNumbers(mBlockNumbers[0]);
        ChangeAnimationState("SwapBlock");
    }

    private void ChangePreviewOrder()
    {
        ChangeAnimationState("ChangeOrder");
    }

    private void PlayAnimation(int aStateIndex)
    {
        Animator.SetInteger("AnimationStates", aStateIndex);
    }

    private void ChangeAnimationState(string aStateName)
    {
        Animator.SetTrigger(aStateName);
    }
}
