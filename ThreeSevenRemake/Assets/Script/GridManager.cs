﻿using System.Collections;
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
    public Vector3 StartWorldPosition { get { return new Vector3(mStartGridPos.x * mCubeGap, mStartGridPos.y * mCubeGap, 0); } }

    private float mCubeGap = .5f;
    public float CubeGap { get { return mCubeGap; } }

    public Dictionary<int, List<Cube>> Grid { get { return mGrid; } }
    private Dictionary<int, List<Cube>> mGrid = new Dictionary<int, List<Cube>>();

    public void GenerateGrid()
    {
        mGridMax = mGridSize + new Vector2Int(-1, -1);

        for (int y = 0; y < mGridSize.y+1; y++)
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

    public bool AvailableMove(Vector2Int aDir, Block aBlock)
    {
        if (aDir == Vector2Int.left)
        {
            Vector2Int pos = aBlock.MinGridPos + aDir;

            if (pos.x > -1 && mGrid[pos.y][(pos.x)] == null)
                return true;
        }
        else if (aDir == Vector2Int.right)
        {
            Vector2Int pos = aBlock.MaxGridPos + aDir;

            if (pos.x < mGridSize.x && mGrid[pos.y][(pos.x)] == null)
                return true;
        }
        else if (aDir == Vector2Int.down)
        {
            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_3 || aBlock.ClockDir == Block.ClockDirection.CLOCK_9)
            {
                Vector2Int rootPos = aBlock.RootCube.GridPos + aDir;
                Vector2Int subPos = aBlock.SubCube.GridPos + aDir;

                if (rootPos.y > -1 && subPos.y > -1 &&
                    mGrid[rootPos.y][(rootPos.x)] == null &&
                    mGrid[subPos.y][(subPos.x)] == null)
                    return true;
            }
            else
            {
                Vector2Int pos = aBlock.MinGridPos + aDir;

                if (pos.y > -1 && mGrid[pos.y][(pos.x)] == null)
                    return true;
            }
        }
        return false;
    }

    public bool AvailableRot(int aTurningIndex, Block aBlock)
    {
        Vector2Int pos = aBlock.GridPos;

        if (aTurningIndex == -1)
        {
            // obvious of the block will be out of range when rotating counter clock wise at the first and last column of the table
            if ((aBlock.ClockDir == Block.ClockDirection.CLOCK_12 && pos.x == 0) ||
                (aBlock.ClockDir == Block.ClockDirection.CLOCK_6 && pos.x == mGridMax.x) ||
                (aBlock.ClockDir == Block.ClockDirection.CLOCK_9 && pos.y == 0))
                return false;

            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_12 && GetCubeFrom(pos + Vector2Int.left) == null)
                return true;
            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_9 && GetCubeFrom(pos + Vector2Int.down) == null)
                return true;
            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_6 && GetCubeFrom(pos + Vector2Int.right) == null)
                return true;
            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_3 && GetCubeFrom(pos + Vector2Int.up) == null)
                return true;
        }
        else
        {
            // obvious of the block will be out of range when rotating clock wise at the first and last column of the table
            if ((aBlock.ClockDir == Block.ClockDirection.CLOCK_12 && pos.x == mGridMax.x) ||
                (aBlock.ClockDir == Block.ClockDirection.CLOCK_6 && pos.x == 0) ||
                (aBlock.ClockDir == Block.ClockDirection.CLOCK_3 && pos.y == 0))
                return false;

            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_12 && GetCubeFrom(pos + Vector2Int.right) == null)
                return true;
            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_9 && GetCubeFrom(pos + Vector2Int.up) == null)
                return true;
            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_6 && GetCubeFrom(pos + Vector2Int.left) == null)
                return true;
            if (aBlock.ClockDir == Block.ClockDirection.CLOCK_3 && GetCubeFrom(pos + Vector2Int.down) == null)
                return true;
        }
        return false;
    }
}
