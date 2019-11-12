using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewNormalSlot : GuiSlotBase
{
    public List<BlockGUI> PreviewBlocks;
    public Animator Animator;

    private List<List<int>> mPreviewNumbers = new List<List<int>>();

    public override void Start()
    {
        base.Start();

        LabMenu.createNewBlock += CreateNewBlock;
        LabMenu.swapTheBlock += SwapTheBlock;
        LabMenu.changePreviewOrder += ChangePreviewOrder;

        for (int i = 0; i < PreviewBlocks.Count; i++)
        {
            mPreviewNumbers.Add(new List<int>(GameManager.Instance.GenerateNewCubeNumber()));
            PreviewBlocks[i].SetNumber(mPreviewNumbers[i]);
        }
    }

    private void OnDestroy()
    {
        LabMenu.createNewBlock -= CreateNewBlock;
        LabMenu.swapTheBlock -= SwapTheBlock;
        LabMenu.changePreviewOrder -= ChangePreviewOrder;
    }

    public override void Update()
    {
        base.Update();
    }

    public void CreateNewEventEnd()
    {
        SwapPreviewList();
        SetBlockNumbers();
    }

    public void ChangeOrderEvent()
    {
        List<int> tempNumbers = new List<int>(mPreviewNumbers[0]);
        mPreviewNumbers[0] = new List<int>(mPreviewNumbers[1]);
        mPreviewNumbers[1] = new List<int>(mPreviewNumbers[2]);
        mPreviewNumbers[2] = new List<int>(tempNumbers);

        SetBlockNumbers();
    }

    public void SwapBlockEvent()
    {
        SwapPreviewList();
        SetBlockNumbers();
    }

    private void CreateNewBlock(Block aNewBlock)
    {
        if (aNewBlock == null)
            return;

        aNewBlock.SetCubeNumbers(mPreviewNumbers[0]);
        ChangeAnimationState("CreateNew");
    }

    private void SwapTheBlock(Block aSwapingBlock)
    {
        if (aSwapingBlock == null)
            return;

        mPreviewNumbers[3] = new List<int>(aSwapingBlock.CubeNumbers);
        PreviewBlocks[3].SetNumber(mPreviewNumbers[3]);

        aSwapingBlock.SetCubeNumbers(mPreviewNumbers[0]);
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

    private void SetBlockNumbers()
    {
        for (int i = 0; i < PreviewBlocks.Count; i++)
        {
            PreviewBlocks[i].SetNumber(mPreviewNumbers[i]);
        }
    }

    private void SwapPreviewList()
    {
        mPreviewNumbers[0] = new List<int>(mPreviewNumbers[1]);
        mPreviewNumbers[1] = new List<int>(mPreviewNumbers[2]);
        mPreviewNumbers[2] = new List<int>(mPreviewNumbers[3]);
        mPreviewNumbers[3] = new List<int>(GameManager.Instance.GenerateNewCubeNumber());
    }
}
