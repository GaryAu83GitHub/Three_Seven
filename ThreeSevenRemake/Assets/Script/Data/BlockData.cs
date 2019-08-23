using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockData
{
    public string BlockName { get { return mName; } }
    private readonly string mName = "";

    public List<int> CubeNumbers { get { return mCubeNumbers; } }
    private readonly List<int> mCubeNumbers = new List<int>();

    public Vector2Int RootCubePosition { get { return mRootCubePosition; } }
    private readonly Vector2Int mRootCubePosition = new Vector2Int();

    public int Rotation { get { return mRotation; } }
    private readonly int mRotation = 0;
    
    public BlockData(Block aBlock)
    {
        mName = aBlock.BlockName;
        mCubeNumbers = new List<int>(aBlock.CubeNumbers);
        mRootCubePosition = ((!aBlock.Cubes.Any()) ? GridManager.Instance.GridStartPosition : aBlock.RootCube.GridPos);
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
