using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GridData
{
    // The static instance for this manager class
    public static GridData Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GridData();
                //mInstance.GenerateGrid();
            }
            return mInstance;
        }
    }
    private static GridData mInstance;

    /// <summary>
    /// Dictionary of list to the both axis on the grid
    /// The keys ID to the dictionary stands for the columns "X"
    /// the index in the list in the dictionary stands for the rows "Y"
    /// </summary>
    private Dictionary<int, List<int>> mGrid;

    /// <summary>
    /// The vector that hold the size of the grid size
    /// </summary>
    private Vector2Int mGridSize = new Vector2Int(10, 20);
    public Vector2Int GridSize { get { return mGridSize; } }

    private Vector2Int mBlockStartPosition = new Vector2Int(6, 18);
    public Vector2Int GridStartPosition { get { return mBlockStartPosition; } }
    public Vector3 StartWorldPosition { get { return new Vector3(mBlockStartPosition.x * mCubeGapDistance, mBlockStartPosition.y * mCubeGapDistance, 0f); } }

    /// <summary>
    /// The distance between cubes.
    /// </summary>
    private const float mCubeGapDistance = .5f;
    public float CubeGapDistance { get { return mCubeGapDistance; } }

    public Dictionary<int, List<int>> Grid { get { return mGrid; } }

    /// <summary>
    ///  Generate the grid with default value along with the list of the
    ///  highest row to each column which is 0
    /// </summary>
    public void GenerateGrid()
    {
        mGrid = new Dictionary<int, List<int>>();
        for (int x = 0; x < mGridSize.x; x++)
        {
            mGrid.Add(x, new List<int>());
            for (int y = 0; y < mGridSize.y; y++)
            {
                mGrid[x].Add(-1);
            }
        }
    }

    /// <summary>
    /// Registrate the cell's value with the cube's value
    /// in the grid according to the cube's grid position
    /// </summary>
    /// <param name="aCube">The cube that will be registrate</param>
    public void RegistrateCell(Cube aCube)
    {
        mGrid[aCube.GridPos.x][aCube.GridPos.y] = aCube.Number;
    }

    /// <summary>
    /// Set the cell on the requesting position to default value
    /// </summary>
    /// <param name="aPos"></param>
    public void UnregistrateCell(Vector2Int aPos)
    {
        mGrid[aPos.x][aPos.y] = -1;
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
        if (mGrid[aPos.x][aPos.y] > -1)
            return false;
        return true;
    }

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

    /// <summary>
    /// Collecting the grid position of those cell that together with
    /// the cell of the given position would made a score
    /// Totally 6 times a given cube can maximalt score
    /// [ ][ ][N][ ][ ], [ ][ ][ ][ ][ ]
    /// [ ][ ][N][ ][ ], [ ][ ][N][ ][ ]
    /// [N][N][G][N][N], [ ][N][C][N][ ]
    /// [ ][ ][N][ ][ ], [ ][ ][N][ ][ ]
    /// [ ][ ][N][ ][ ], [ ][ ][ ][ ][ ]
    /// </summary>
    /// <param name="aPos">The given position</param>
    /// <returns></returns>
    public List<Vector2Int> ScoringMethodThreeSeven(List<Cube> someNewLandedCubes)
    {
        List<Vector2Int> scoringPositions = new List<Vector2Int>();

        foreach (Cube c in someNewLandedCubes)
        {
            // horizontal
            //  the horizont cross [N][G][N]
            if (TotalValueFromPositions(c.GridPos + Vector2Int.left, c.GridPos, c.GridPos + Vector2Int.right) == 7 ||
                TotalValueFromPositions(c.GridPos + Vector2Int.left, c.GridPos, c.GridPos + Vector2Int.right) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);
            }

            //  the right [G][N][N]
            if (TotalValueFromPositions(c.GridPos, c.GridPos + Vector2Int.right, c.GridPos + (Vector2Int.right * 2)) == 7 ||
                TotalValueFromPositions(c.GridPos, c.GridPos + Vector2Int.right, c.GridPos + (Vector2Int.right * 2)) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.right * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.right * 2));
            }

            //  the left [N][N][G]
            if (TotalValueFromPositions(c.GridPos + (Vector2Int.left * 2), c.GridPos + Vector2Int.left, c.GridPos) == 7 ||
                TotalValueFromPositions(c.GridPos + (Vector2Int.left * 2), c.GridPos + Vector2Int.left, c.GridPos) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.left * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.left * 2));
            }

            // vertical
            //  the verticla cross
            // [N]
            // [G]
            // [N]
            if (TotalValueFromPositions(c.GridPos + Vector2Int.up, c.GridPos, c.GridPos + Vector2Int.down) == 7 ||
                TotalValueFromPositions(c.GridPos + Vector2Int.up, c.GridPos, c.GridPos + Vector2Int.down) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);
            }

            //  the up
            // [N]
            // [N]
            // [G]
            if (TotalValueFromPositions(c.GridPos + (Vector2Int.up * 2), c.GridPos + Vector2Int.up, c.GridPos) == 7 ||
                TotalValueFromPositions(c.GridPos + (Vector2Int.up * 2), c.GridPos + Vector2Int.up, c.GridPos) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.up * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.up * 2));
            }

            //  the down
            // [G]
            // [N]
            // [N]
            if (TotalValueFromPositions(c.GridPos, c.GridPos + Vector2Int.down, c.GridPos + (Vector2Int.down * 2)) == 7 ||
                TotalValueFromPositions(c.GridPos, c.GridPos + Vector2Int.down, c.GridPos + (Vector2Int.down * 2)) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.down * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.down * 2));
            }
        }

        return scoringPositions;
    }

    /// <summary>
    /// Development purpose, work like the GetScoreingPositions method, but instead of get
    /// score for 3 cubes sum will be 7, here is to check the table rows is filled
    /// and gather it position.
    /// </summary>
    public List<Vector2Int> TempScoringMethodRowFilling()
    {
        List<Vector2Int> scoringPositions = new List<Vector2Int>();

        for(int y = 0; y < mGridSize.y; y++)
        {
            List<int> tempList = new List<int>();
            for (int x = 0; x < mGridSize.x; x++)
                tempList.Add(mGrid[x][y]);

            if (!tempList.Contains(-1))
            {
                for (int x = 0; x < tempList.Count; x++)
                {
                    scoringPositions.Add(new Vector2Int(x, y));
                }
            }

        }

        return scoringPositions;
    }

    /// <summary>
    /// This is a test method in use during the development for develpe the base of the original scoring method
    /// The main idea is to score when three aligned with each other and have a sum of 7
    /// The base of the original scoring method is to have three cube align with both horizontal and vertical
    /// to score.
    /// This method will serve as the core of the scoring method
    /// </summary>
    /// <param name="someNewLandedCubes">Cubes that just had landed will going through the scoring checks</param>
    /// <returns>A list of positions of cubes that had manage to score</returns>
    public List<Vector2Int> TempScoringMethodThreeInRows(List<Cube> someNewLandedCubes)
    {
        List<Vector2Int> scoringPositions = new List<Vector2Int>();

        foreach(Cube c in someNewLandedCubes)
        {
            // horizontal
            //  the horizont cross [N][G][N]
            if (GetValueOn(c.GridPos + Vector2Int.left) > -1 && GetValueOn(c.GridPos + Vector2Int.right) > -1)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);
            }

            //  the right [G][N][N]
            if (GetValueOn(c.GridPos + Vector2Int.right) > -1 && GetValueOn(c.GridPos + (Vector2Int.right * 2)) > -1)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.right * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.right * 2));
            }

            //  the left [N][N][G]
            if (GetValueOn(c.GridPos + Vector2Int.left) > -1 && GetValueOn(c.GridPos + (Vector2Int.left * 2)) > -1)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.left * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.left * 2));
            }

            // vertical
            //  the verticla cross
            // [N]
            // [G]
            // [N]
            if (GetValueOn(c.GridPos + Vector2Int.up) > -1 && GetValueOn(c.GridPos + Vector2Int.down) > -1)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);
            }

            //  the up
            // [N]
            // [N]
            // [G]
            if (GetValueOn(c.GridPos + Vector2Int.up) > -1 && GetValueOn(c.GridPos + (Vector2Int.up * 2)) > -1)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.up * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.up * 2));
            }

            //  the down
            // [G]
            // [N]
            // [N]
            if (GetValueOn(c.GridPos + Vector2Int.down) > -1 && GetValueOn(c.GridPos + (Vector2Int.down * 2)) > -1)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.down * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.down * 2));
            }
        }
        return scoringPositions;
    }

    /// <summary>
    /// Collecting the scoring position into the given list
    /// If the list already contain the position, it'll be ignored 
    /// </summary>
    /// <param name="aList">List to store the position</param>
    /// <param name="aPos">Position that want to be stored</param>
    private void ScoringPosCollection(List<Vector2Int> aList, Vector2Int aPos)
    {
        if (!aList.Contains(aPos))
            aList.Add(aPos);
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
    private int GetValueOn(Vector2Int aPos)
    {
        // Boundary check
        if (aPos.x < 0 || aPos.x >= mGridSize.x || aPos.y < 0 || aPos.y >= mGridSize.y)
            return -1;

        //return mGridInt[aPos.x, aPos.y];
        return mGrid[aPos.x][aPos.y];
    }

    /// <summary>
    /// Return a boolian of the value is equal with the scoring value
    /// </summary>
    /// <param name="aV1">value 1</param>
    /// <param name="aV2">value 2</param>
    /// <param name="aV3">value 3</param>
    /// <returns></returns>
    private int TotalValueFromPositions(Vector2Int aPos1, Vector2Int aPos2, Vector2Int aPos3)
    {
        int value = GetValueOn(aPos1) + GetValueOn(aPos2) + GetValueOn(aPos3);

        return value;
    }
}
