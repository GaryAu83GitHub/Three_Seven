using System.Collections.Generic;
using UnityEngine;

public class BlockData
{
    private readonly string mName = "";
    private readonly List<int> mCubeNumbers = new List<int>();
    private readonly Vector2Int mRootCubePosition = new Vector2Int();
    private readonly int mRotation = 0;
    
    public BlockData(Block aBlock)
    {
        mName = aBlock.BlockName;
        mCubeNumbers = new List<int>(aBlock.CubeNumbers);
        mRootCubePosition = aBlock.RootCube.GridPos;
        mRotation = aBlock.BlockRotation;
    }

    public BlockData(string aBlockName, List<int> someCubeNumbers, Vector2Int aRootPosition, int aBlockRotation)
    {
        mName = aBlockName;
        mCubeNumbers = someCubeNumbers;
        mRootCubePosition = aRootPosition;
        mRotation = aBlockRotation;
    }
}
