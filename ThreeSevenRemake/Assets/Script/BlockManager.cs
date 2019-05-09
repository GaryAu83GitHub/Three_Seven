using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// This class is use to managing the all blocks that had been created
/// It'll store all blocks that had landed into it collection and remove
/// blocks that don't have any cube in it.
/// 
/// It'll also registrate/unregistrate the blocks cube of the grid when the
/// block has landed or scoring
/// 
/// When cube are scoring, it call the play animation from cube, unregistrate
/// then from the grid and rearrange and sort the blocks according to which
/// row they currently are placed at
/// </summary>
public class BlockManager
{
    // The static instance for this manager class
    public static BlockManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new BlockManager();
            }
            return mInstance;
        }
    }
    private static BlockManager mInstance;

    public int BlockCount { get { return mBlocks.Count; } }

    private List<BlockDeveloping> mBlocks = new List<BlockDeveloping>();
    private List<Cube> mScoringsCubes = new List<Cube>();

    // Add in the new landed and rearranged block
    public void Add(BlockDeveloping aBlock)
    {
        mBlocks.Add(aBlock);
        foreach (Cube c in aBlock.Cubes)
            GridData.Instance.RegistrateCell(c);
    }

    // Add in cubes that had scored
    public void AddScoringCubes(Cube aCube)
    {
        mScoringsCubes.Add(aCube);
    }

    // All scoring cube play their scoring animation
    public void PlayScoringAnimation()
    {
        foreach(Cube c in mScoringsCubes)
        {
            c.PlayAnimation();
        }
    }

    public void RearrangeBlocks()
    { }
}
