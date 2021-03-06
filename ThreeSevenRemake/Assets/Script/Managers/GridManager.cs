﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GridManager
{
    // The static instance for this manager class
    public static GridManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GridManager();
            }
            return mInstance;
        }
    }
    private static GridManager mInstance;

    
    /// <summary>
    /// Dictionary of list to the both axis on the grid
    /// The keys ID to the dictionary stands for the columns "X"
    /// the index in the list in the dictionary stands for the rows "Y"
    /// </summary>
    private Dictionary<int, List<Cube>> mGrid;

    /// <summary>
    /// The vector that hold the size of the grid size
    /// </summary>
    private Vector2Int mGridSize = new Vector2Int(10, 20);
    public Vector2Int GridSize { get { return mGridSize; } }

    private Vector2Int mBlockStartPosition = new Vector2Int(Constants.BLOCK_START_POSITION_X, Constants.BLOCK_START_POSITION_Y);
    public Vector2Int GridStartPosition { get { return mBlockStartPosition; } }
    public Vector3 StartWorldPosition { get { return new Vector3((mBlockStartPosition.x * Constants.CUBE_GAP_DISTANCE) - Constants.WORLD_OFF_X, (mBlockStartPosition.y * Constants.CUBE_GAP_DISTANCE) - Constants.WORLD_OFF_Y, 0f); } }
        
    /// <summary>
    /// The Griddata storage constructed with a dictionary holding a list on each item
    /// The Key to the Dictionary stands of the column (x) value
    /// The Index in the list stand for the row (y) value
    /// </summary>
    public Dictionary<int, List<Cube>> Grid { get { return mGrid; } }

    /// <summary>
    ///  Generate the grid with default value along with the list of the
    ///  highest row to each column which is 0
    /// </summary>
    public void GenerateGrid()
    {
        if(mGrid != null)
            mGrid.Clear();

        mGrid = new Dictionary<int, List<Cube>>();
        for (int x = 0; x < mGridSize.x; x++)
        {
            mGrid.Add(x, new List<Cube>());
            for (int y = 0; y < mGridSize.y; y++)
            {
                mGrid[x].Add(null);
            }
        }
    }
    
    /// <summary>
    /// Registrate the cell's value with the cube's value
    /// in the grid according to the cube's grid position
    ///  (still under develop with the "mCurrentTallestCellInGrid" variable for optimize purpose)
    /// </summary>
    /// <param name="aCube">The cube that will be registrate</param>
    public void RegistrateCell(Cube aCube)
    {
        mGrid[aCube.GridPos.x][aCube.GridPos.y] = aCube;
        //Debug.LogFormat("Current tallest cell row: {0}", mCurrentTallestCellInGrid);
    }

    /// <summary>
    /// Set the cell on the requesting position to default value
    /// (still under develop with the "mCurrentTallestCellInGrid" variable for optimize purpose)
    /// </summary>
    /// <param name="aPos"></param>
    public void UnregistrateCell(Vector2Int aPos)
    {
        mGrid[aPos.x][aPos.y] = null;
    }

    /// <summary>
    /// Check if the requesting cell is vacant (if the cell has it default
    /// value)
    /// </summary>
    /// <param name="aPos">The requesting cell's position</param>
    /// <returns>If the position is within the boundary of the grid
    /// AND if the cell currently have the default value,
    /// this will return true, or else false</returns>
    public bool IsCellVacant(Vector2Int aPos)
    {
        // Boundary check
        if (aPos.x < 0 || aPos.x >= mGridSize.x || aPos.y < 0)
            return false;

        // Checking cells
        if (mGrid[aPos.x][aPos.y] != null)
            return false;
        return true;
    }

    /// <summary>
    /// This is to check if the block is enable to rotate from it root position 
    /// and the angle it'll rotate
    /// </summary>
    /// <param name="aPos"></param>
    /// <param name="anAngle"></param>
    /// <returns></returns>
    public bool IsRotateAvailable(Vector2Int aPos, float anAngle)
    {
        // the block is about to rotate from 0(360) to 90 degree
        // the restriction will be that if:
        // - the block is at the rightmost cell of the table
        // - the cell to the right of the rotate cube (CoreCube) is not vacant
        if (anAngle == 90 && !IsCellVacant(aPos + Vector2Int.right))
            return false;

        // the block is about to rotate from 90 to 180 degree
        // the restriction will be that if:
        // - the block is at the bottom row of the table
        // - the cell below the rotate cube (CoreCube) is not vacant
        if (anAngle == 180 && !IsCellVacant(aPos + Vector2Int.down))
            return false;

        // the block is about to rotate from 180 to 270 degree
        // the restriction will be that if:
        // - the block is at the leftmost cell of the table
        // - the cell to the left of the rotate cube (CoreCube) is not vacant
        if (anAngle == 270 && !IsCellVacant(aPos + Vector2Int.left))
            return false;

        // the block is about to rotate from 270 to 0(360) degree (the odds for this too occure is very thin)
        // the restriction will be that if:
        // - the cell above the rotate cube (CoreCube) is not vacant
        if (anAngle == 360 && !IsCellVacant(aPos + Vector2Int.up))
            return false;

        return true;
    }

    public bool IsRotateAvailableUppgrade(Vector2Int aPos, float anAngle)
    {
        // the block is about to rotate from 0(360) to 90 degree
        // the restriction will be that if:
        // - the block is at the rightmost cell of the table
        // - the cell to the right of the rotate cube (CoreCube) is not vacant
        if (anAngle == 90 && !IsCellVacant(aPos + Vector2Int.right) && !IsCellVacant(aPos + Vector2Int.left))
            return false;

        // the block is about to rotate from 90 to 180 degree
        // the restriction will be that if:
        // - the block is at the bottom row of the table
        // - the cell below the rotate cube (CoreCube) is not vacant
        if (anAngle == 180 && !IsCellVacant(aPos + Vector2Int.down) && !IsCellVacant(aPos + Vector2Int.up))
            return false;

        // the block is about to rotate from 180 to 270 degree
        // the restriction will be that if:
        // - the block is at the leftmost cell of the table
        // - the cell to the left of the rotate cube (CoreCube) is not vacant
        if (anAngle == 270 && !IsCellVacant(aPos + Vector2Int.left) && !IsCellVacant(aPos + Vector2Int.right))
            return false;

        // the block is about to rotate from 270 to 0(360) degree (the odds for this too occure is very thin)
        // the restriction will be that if:
        // - the cell above the rotate cube (CoreCube) is not vacant
        if (anAngle == 360 && !IsCellVacant(aPos + Vector2Int.up) && !IsCellVacant(aPos + Vector2Int.down))
            return false;

        return true;
    }

    public List<ScoringGroupAchieveInfo> GetListOfScoringPositionGroups(Block aBlock)
    {
        List<ScoringGroupAchieveInfo> someGroupOfPosition = new List<ScoringGroupAchieveInfo>();
        if (aBlock.Cubes.Count == 2)
            ScoreWithBlock(aBlock, ref someGroupOfPosition);

        foreach (Cube c in aBlock.Cubes)
            ScoreWithCube(c, ref someGroupOfPosition);

        return someGroupOfPosition;
    }

    /// <summary>
    /// Check after the current tallest row on the requested cell (x pos)
    /// It begin to check from the current tallest row on the whole table
    ///  (still under develop with the "mCurrentTallestCellInGrid" variable for optimize purpose)
    /// </summary>
    /// <param name="aXPosition">Requested x position</param>
    /// <returns>The the tallest row of the requested x position</returns>
    public int TallestRowOnColumn(int aXPosition)
    {
        //int row = mCurrentTallestCellInGrid;
        int row = 0;

        for (int y = mGridSize.y - 1/*mCurrentTallestCellInGrid*/; y >= 0; y--)
        {
            if (mGrid[aXPosition][y] == null)
                row = y;
            else
                break;
        }

        return row;
    }

    public int TallestRowFromCubePos(Vector2Int aGridPos)
    {
        //int row = mCurrentTallestCellInGrid;
        int row = 0;

        for (int y = aGridPos.y; y >= 0; y--)
        {
            if (mGrid[aGridPos.x][y] == null)
                row = y;
            else
                break;
        }

        return row;
    }

    public int GetCubeValueOn(Vector2Int aPos)
    {
        return GetCubeOn(aPos).Number;
    }

    /// <summary>
    /// Get the value from the cell on the given position on the grid,
    /// but if the given position is out of boundary it'll return a
    /// default value
    /// </summary>
    /// <param name="aPos">Given position</param>
    /// <returns>return the value of the cell from the given position,
    /// and if the position is out of boundary, it'll return 
    /// the default value</returns>
    private Cube GetCubeOn(Vector2Int aPos)
    {
        // Boundary check
        if (aPos.x < 0 || aPos.x >= mGridSize.x || aPos.y < 0 || aPos.y >= mGridSize.y)
            return null;

        //return mGridInt[aPos.x, aPos.y];
        return mGrid[aPos.x][aPos.y];
    }
    
    private void ScoreWithBlock(Block aBlock, ref List<ScoringGroupAchieveInfo> infoList)
    {
        List<List<Vector2Int>> scoreCombinationPositions = GenerateScoreCombinationPositions.Instance.GetScorePositionListForBlock(aBlock);

        foreach (List<Vector2Int> pos in scoreCombinationPositions)
            RegistrateInfo(aBlock, null, ref infoList, pos);
    }

    private void ScoreWithCube(Cube aCube, ref List<ScoringGroupAchieveInfo> infoList)
    {
        List<List<Vector2Int>> scoreCombinationPositions = GenerateScoreCombinationPositions.Instance.GetScorePositionListForCube(aCube);
        
        foreach (List<Vector2Int> pos in scoreCombinationPositions)
            RegistrateInfo(null, aCube, ref infoList, pos);
    }

    private void RegistrateInfo(Block aBlock, Cube aCube, ref List<ScoringGroupAchieveInfo> someScoringInfos, List<Vector2Int> somePositions)
    {
        ScoringType scoringType = ((aBlock != null) ? ScoringType.BLOCK_SCORING : ScoringType.CUBE_SCORING);
        TaskRank achieveRank = TaskRank.X1;
        int slotIndex = -1;

        int cubeCount = somePositions.Count(); //+ ((scoringType == ScoringType.BLOCK_SCORING) ? 2 : 1);
        int resultValue = 0; //= ((scoringType == ScoringType.BLOCK_SCORING) ? TotalValueWithBlock(aBlock, somePositions) : TotalValueWithCube(aCube, somePositions));

        if (scoringType == ScoringType.BLOCK_SCORING)
        {
            cubeCount += 2;
            resultValue = TotalValueWithBlock(aBlock, somePositions);
        }
        else
        {
            cubeCount += 1;
            resultValue = TotalValueWithCube(aCube, somePositions);
        }

        if (!TaskManagerNew.Instance.MatchTaskData(ref achieveRank, ref slotIndex, resultValue, cubeCount))
            return;

        if (!ThisGroupHasNotbeenRegistrate(scoringType, ref someScoringInfos, somePositions))
            return;


        ScoringGroupAchieveInfo registrateInfo = ((scoringType == ScoringType.BLOCK_SCORING) ? 
            new ScoringGroupAchieveInfo(achieveRank, slotIndex, aBlock, somePositions) : 
            new ScoringGroupAchieveInfo(achieveRank, slotIndex, aCube, somePositions));

        someScoringInfos.Add(registrateInfo);
        return;
    }
    
    private bool ThisGroupHasNotbeenRegistrate(ScoringType aScoringType, ref List<ScoringGroupAchieveInfo> someGroupOfPosition, List<Vector2Int> someScoringPositions)
    {
        //var a = ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
        foreach (ScoringGroupAchieveInfo info in someGroupOfPosition)
        {
            if (info.ScoringType == aScoringType && info.GroupPosition.SequenceEqual(someScoringPositions))  
                return false;            
        }
        TaskManagerNew.Instance.ConfirmAchiveTask();
        return true;
    }

    /// <summary>
    /// Return the sum value of the two requesting position
    /// </summary>
    /// <param name="aPos1">position 1</param>
    /// <param name="aPos2">position 2</param>
    /// <returns></returns>
    private int TotalValueFromTwoPositions(Vector2Int aPos1, Vector2Int aPos2)
    {
        if (GetCubeOn(aPos1) == null || GetCubeOn(aPos2) == null)
            return -1;

        int value = GetCubeOn(aPos1).Number + GetCubeOn(aPos2).Number;

        return value;
    }

    /// <summary>
    /// Return the sum value of the three requesting position
    /// </summary>
    /// <param name="aPos1">position 1</param>
    /// <param name="aPos2">position 2</param>
    /// <param name="aPos3">position 3</param>
    /// <returns></returns>
    private int TotalValueFromThreePositions(Vector2Int aPos1, Vector2Int aPos2, Vector2Int aPos3)
    {
        if (GetCubeOn(aPos1) == null || GetCubeOn(aPos2) == null || GetCubeOn(aPos3) == null)
            return -1;

        int value = GetCubeOn(aPos1).Number + GetCubeOn(aPos2).Number + GetCubeOn(aPos3).Number;
        
        return value;
    }

    /// <summary>
    /// Return the sum value of the four requesting position
    /// </summary>
    /// <param name="aPos1">position 1</param>
    /// <param name="aPos2">position 2</param>
    /// <param name="aPos3">position 3</param>
    /// <param name="aPos4">position 4</param>
    /// <returns></returns>
    private int TotalValueFromFourPositions(Vector2Int aPos1, Vector2Int aPos2, Vector2Int aPos3, Vector2Int aPos4)
    {
        if (GetCubeOn(aPos1) == null || GetCubeOn(aPos2) == null || GetCubeOn(aPos3) == null || GetCubeOn(aPos4) == null)
            return -1;

        int value = GetCubeOn(aPos1).Number + GetCubeOn(aPos2).Number + GetCubeOn(aPos3).Number + GetCubeOn(aPos4).Number;

        return value;
    }

    /// <summary>
    /// Return the sum value of the five requesting position
    /// </summary>
    /// <param name="aPos1">position 1</param>
    /// <param name="aPos2">position 2</param>
    /// <param name="aPos3">position 3</param>
    /// <param name="aPos4">position 4</param>
    /// <param name="aPos5">position 5</param>
    /// <returns></returns>
    private int TotalValueFromFivePositions(Vector2Int aPos1, Vector2Int aPos2, Vector2Int aPos3, Vector2Int aPos4, Vector2Int aPos5)
    {
        if (GetCubeOn(aPos1) == null || GetCubeOn(aPos2) == null || GetCubeOn(aPos3) == null || GetCubeOn(aPos4) == null || GetCubeOn(aPos5) == null)
            return -1;

        int value = GetCubeOn(aPos1).Number + GetCubeOn(aPos2).Number + GetCubeOn(aPos3).Number + GetCubeOn(aPos4).Number + GetCubeOn(aPos5).Number;

        return value;
    }

    private int TotalValue(List<Vector2Int> someScoringPositions)
    {
        int value = 0;
        for (int i = 0; i < someScoringPositions.Count; i++)
        {
            if (GetCubeOn(someScoringPositions[i]) == null)
                return -1;
            else
                value += GetCubeOn(someScoringPositions[i]).Number;
        }
        return value;
    }

    private int TotalValueWithBlock(Block aBlock, List<Vector2Int> someScoringPositions)
    {
        int value = aBlock.BlockValue;
        for (int i = 0; i < someScoringPositions.Count; i++)
        {
            if (GetCubeOn(someScoringPositions[i]) == null)
                return -1;
            else
                value += GetCubeOn(someScoringPositions[i]).Number;
        }
        return value;
    }

    private int TotalValueWithCube(Cube aCube, List<Vector2Int> someScoringPositions)
    {
        int value = aCube.Number;
        for (int i = 0; i < someScoringPositions.Count; i++)
        {
            if (GetCubeOn(someScoringPositions[i]) == null)
                return -1;
            else
                value += GetCubeOn(someScoringPositions[i]).Number;
        }
        return value;
    }
}
