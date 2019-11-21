using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewNormalSlot : GuiSlotBase
{
    public List<BlockGUI> PreviewBlocks;
    public Animator Animator;

    public List<int> FirstPreviewBlock { get { return mPreviewNumbers[0]; } }
    public List<int> LastPreviewBlock { set { mPreviewNumbers[3] = value; } }

    private List<List<int>> mPreviewNumbers = new List<List<int>>();

    private float mSwapCommandTimer = 0;
    private List<int> mStoredSwapingNumbers = new List<int>();

    public override void Start()
    {
        base.Start();

        // thses delegates are for the main game
        GameSceneMain.createNewBlock += CreateNewBlock;
        GameSceneMain.swapingWithPreview += SwapBlock;//SwapTheBlock;

        MainGamePanel.changePreviewOrder += ChangePreviewOrder;
        MainGamePanel.dumpPreviewBlock += DumbPreviewBlock;

        // these delegates are used in the test scene
        LabMenu.createNewBlock += CreateNewBlock;
        LabMenu.swapTheBlock += SwapTheBlock;
        LabMenu.changePreviewOrder += ChangePreviewOrder;

        for (int i = 0; i < PreviewBlocks.Count; i++)
        {
            if (i < PreviewBlocks.Count - 1)
            {
                mPreviewNumbers.Add(new List<int>(GameManager.Instance.GenerateNewCubeNumber()));
                PreviewBlocks[i].SetNumber(mPreviewNumbers[i]);
            }
            else if (i == PreviewBlocks.Count - 1)
                mPreviewNumbers.Add(new List<int>());
        }
    }

    private void OnDestroy()
    {
        // thses delegates are for the main game
        GameSceneMain.createNewBlock -= CreateNewBlock;
        GameSceneMain.swapingWithPreview -= SwapBlock;//SwapTheBlock;

        MainGamePanel.changePreviewOrder -= ChangePreviewOrder;
        MainGamePanel.dumpPreviewBlock -= DumbPreviewBlock;

        // these are used in the test scene
        LabMenu.createNewBlock -= CreateNewBlock;
        LabMenu.swapTheBlock -= SwapTheBlock;
        LabMenu.changePreviewOrder -= ChangePreviewOrder;
    }

    public override void Update()
    {
        base.Update();
        if (mSwapCommandTimer > 0)
            mSwapCommandTimer -= Time.deltaTime;
    }

    public void CreateNewEventEnd()
    {
        UpdatePreviewList(true);
        SetBlockNumbers();
    }

    public void DumpPreviewEvent()
    {
        UpdatePreviewList(true);
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

    public void SwapBlockEventNew()
    {
        mPreviewNumbers[0] = new List<int>(mStoredSwapingNumbers);
        SetBlockNumbers();
    }

    public void SwapBlockEvent()
    {
        UpdatePreviewList(false);
        SetBlockNumbers();
    }

    private void CreateNewBlock(Block aNewBlock)
    {
        if (aNewBlock == null)
            return;

        aNewBlock.SetCubeNumbers(mPreviewNumbers[0]);
        mPreviewNumbers[3] = new List<int>(GameManager.Instance.GenerateNewCubeNumber());

        SetBlockNumbers();
        ChangeAnimationState("CreateNew");
    }

    private void SwapBlock(Block aSwapingBlock)
    {
        if (aSwapingBlock == null || mSwapCommandTimer > 0)
            return;

        mSwapCommandTimer = .75f;

        mStoredSwapingNumbers = new List<int>(aSwapingBlock.CubeNumbers);
        aSwapingBlock.SetCubeNumbers(mPreviewNumbers[0]);
        ChangeAnimationState("SwapBlock");
    }

    // this will be replaced by SwapBlock
    private void SwapTheBlock(Block aSwapingBlock)
    {
        if (aSwapingBlock == null || mSwapCommandTimer > 0)
            return;

        mSwapCommandTimer = .75f;

        mPreviewNumbers[3] = new List<int>(aSwapingBlock.CubeNumbers);
        PreviewBlocks[3].SetNumber(mPreviewNumbers[3]);

        aSwapingBlock.SetCubeNumbers(mPreviewNumbers[0]);
        SetBlockNumbers();
        ChangeAnimationState("SwapBlock");

        //Vector3 a = new Vector3(0, 1, 0);
        //Vector3 b = new Vector3(0, 1, 1);
        // s = v * t
        // t = s / v
        //Vector3 c = Vector3.Lerp(a, b, Vector3.Distance(a, b) / (Time.deltaTime * 1));
    }

    private void ChangePreviewOrder()
    {
        ChangeAnimationState("ChangeOrder");
    }

    private void DumbPreviewBlock()
    {
        mPreviewNumbers[3] = new List<int>(GameManager.Instance.GenerateNewCubeNumber());

        SetBlockNumbers();
        ChangeAnimationState("Dumping");
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

    private void UpdatePreviewList(bool isCreatingNewBlock)
    {
        //if(isCreatingNewBlock)
        //    mPreviewNumbers[3] = new List<int>(GameManager.Instance.GenerateNewCubeNumber());

        mPreviewNumbers[0] = new List<int>(mPreviewNumbers[1]);
        mPreviewNumbers[1] = new List<int>(mPreviewNumbers[2]);
        mPreviewNumbers[2] = new List<int>(mPreviewNumbers[3]);
    }

    
}
