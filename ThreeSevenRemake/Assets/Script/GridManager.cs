using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    public static GridManager Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = new GridManager();
            }
            return mInstance;
        }
    }
    private static GridManager mInstance;

    private Vector2Int mGridSize = new Vector2Int(10, 19);

    private Vector2Int mGridMax;

    private Vector2Int mStartGridPos = new Vector2Int(6, 18);
    public Vector2Int StartPosition { get { return mStartGridPos; } }

    private float mCubeDistance = .5f;
    public float CubeDistance { get { return mCubeDistance; } }

    private Dictionary<int, List<Cube>> mGrid = new Dictionary<int, List<Cube>>();

    public void GenerateGrid()
    {
        mGridMax = mGridSize + new Vector2Int(-1, -1);

        for (int y = 0; y < mGridSize.y; y++)
        {
            mGrid.Add(y, new List<Cube>());
            for(int x = 0; x < mGridSize.x; x++)
            {
                mGrid[y].Add(null);
            }
        }
    }

    public Cube GetCubeFrom(Vector2Int aGridPos)
    {
        if (!GridIsVacantAt(aGridPos))
            return mGrid[aGridPos.y][aGridPos.x];
        return null;
    }

    public bool GridIsVacantAt(Vector2Int aGridPos)
    {
        if (mGrid[aGridPos.y][aGridPos.x] == null)
            return true;
        return false;
    }

    public void NullifyCubeAt(Vector2Int aGridpos)
    {
        if(!GridIsVacantAt(aGridpos))
            mGrid[aGridpos.y][aGridpos.x] = null;
    }

    public void PutInCube(Cube aCube)
    {
        Vector2Int pos = aCube.GridPos;
        if (GridIsVacantAt(pos))
            mGrid[pos.y][pos.x] = aCube;
    }

    public void RevoveCube(Cube aCube)
    {
        Vector2Int pos = aCube.GridPos;
        if (GridIsVacantAt(pos) && GetCubeFrom(pos) == aCube)
            NullifyCubeAt(pos);
    }
}
