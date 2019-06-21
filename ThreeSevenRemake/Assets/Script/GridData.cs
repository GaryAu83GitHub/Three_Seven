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
    private Dictionary<int, List<Cube>> mGrid;

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

    /// <summary>
    /// The Griddata storage constructed with a dictionary holding a list on each item
    /// The Key to the Dictionary stands of the column (x) value
    /// The Index in the list stand for the row (y) value
    /// </summary>
    public Dictionary<int, List<Cube>> Grid { get { return mGrid; } }

    /// <summary>
    /// Keep track of the tallest cell row on the board
    /// </summary>
    //private int mCurrentTallestCellInGrid = 0;

    /// <summary>
    ///  Generate the grid with default value along with the list of the
    ///  highest row to each column which is 0
    /// </summary>
    public void GenerateGrid()
    {
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

        //if (aCube.GridPos.y > mCurrentTallestCellInGrid)
        //    mCurrentTallestCellInGrid = aCube.GridPos.y;

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

        //bool tallestCellHasChanged = true;
        //for(int x = 0; x < mGridSize.x; x++)
        //{
        //    if (mGrid[x][mCurrentTallestCellInGrid] != null)
        //    {
        //        tallestCellHasChanged = false;
        //        break;
        //    }
        //}

        //if(tallestCellHasChanged)
        //{
        //    int tallestRow = 0;

        //    for (int x = 0; x < mGridSize.x; x++)
        //    {
        //        int tallestRowOnThisColumn = TallestRowOnColumn(x);

        //        if (tallestRowOnThisColumn > tallestRow)
        //            tallestRow = tallestRowOnThisColumn;
        //    }

        //    mCurrentTallestCellInGrid = tallestRow;
            //Debug.LogFormat("Current tallest cell row: {0}", mCurrentTallestCellInGrid);
        //}
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

    /// <summary>
    /// </summary>
    /// <param name="someNewLandedCubes"></param>
    /// <param name="aComboCount"></param>
    /// <returns></returns>
    public List<Vector2Int> CompleteObjectiveScoringMethod(List<Cube> someNewLandedCubes, ref int aComboCount)
    {
        List<Vector2Int> scoringPositions = new List<Vector2Int>();

        foreach (Cube c in someNewLandedCubes)
        {
            // horizontal
            //  the horizont cross [N][G][N]
            if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + Vector2Int.left, c.GridPos, c.GridPos + Vector2Int.right)))
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);

                aComboCount++;
            }

            //  the right [G][N][N]
            if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.right, c.GridPos + (Vector2Int.right * 2))))
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.right * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.right * 2));

                aComboCount++;
            }

            //  the left [N][N][G]
            if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + (Vector2Int.left * 2), c.GridPos + Vector2Int.left, c.GridPos)))
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.left * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.left * 2));

                aComboCount++;
            }

            // vertical
            //  the verticla cross
            // [N]
            // [G]
            // [N]
            if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + Vector2Int.up, c.GridPos, c.GridPos + Vector2Int.down)))
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);

                aComboCount++;
            }

            //  the up
            // [N]
            // [N]
            // [G]
            if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + (Vector2Int.up * 2), c.GridPos + Vector2Int.up, c.GridPos)))
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.up * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.up * 2));

                aComboCount++;
            }

            //  the down
            // [G]
            // [N]
            // [N]
            if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.down, c.GridPos + (Vector2Int.down * 2))))
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.down * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.down * 2));

                aComboCount++;
            }
        }

        GameManager.Instance.SetComboScore(aComboCount);

        return scoringPositions;
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
    public List<Vector2Int> ScoringMethodThreeSeven(List<Cube> someNewLandedCubes, ref int aComboCount)
    {
        List<Vector2Int> scoringPositions = new List<Vector2Int>();

        foreach (Cube c in someNewLandedCubes)
        {
            // horizontal
            //  the horizont cross [N][G][N]
            if (TotalValueFromThreePositions(c.GridPos + Vector2Int.left, c.GridPos, c.GridPos + Vector2Int.right) == 7 ||
                TotalValueFromThreePositions(c.GridPos + Vector2Int.left, c.GridPos, c.GridPos + Vector2Int.right) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);

                aComboCount++;
            }

            //  the right [G][N][N]
            if (TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.right, c.GridPos + (Vector2Int.right * 2)) == 7 ||
                TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.right, c.GridPos + (Vector2Int.right * 2)) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.right * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.right * 2));

                aComboCount++;
            }

            //  the left [N][N][G]
            if (TotalValueFromThreePositions(c.GridPos + (Vector2Int.left * 2), c.GridPos + Vector2Int.left, c.GridPos) == 7 ||
                TotalValueFromThreePositions(c.GridPos + (Vector2Int.left * 2), c.GridPos + Vector2Int.left, c.GridPos) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.left * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.left * 2));

                aComboCount++;
            }

            // vertical
            //  the verticla cross
            // [N]
            // [G]
            // [N]
            if (TotalValueFromThreePositions(c.GridPos + Vector2Int.up, c.GridPos, c.GridPos + Vector2Int.down) == 7 ||
                TotalValueFromThreePositions(c.GridPos + Vector2Int.up, c.GridPos, c.GridPos + Vector2Int.down) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);

                aComboCount++;
            }

            //  the up
            // [N]
            // [N]
            // [G]
            if (TotalValueFromThreePositions(c.GridPos + (Vector2Int.up * 2), c.GridPos + Vector2Int.up, c.GridPos) == 7 ||
                TotalValueFromThreePositions(c.GridPos + (Vector2Int.up * 2), c.GridPos + Vector2Int.up, c.GridPos) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.up * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.up * 2));

                aComboCount++;
            }

            //  the down
            // [G]
            // [N]
            // [N]
            if (TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.down, c.GridPos + (Vector2Int.down * 2)) == 7 ||
                TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.down, c.GridPos + (Vector2Int.down * 2)) == 21)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.down * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.down * 2));

                aComboCount++;
            }
        }

        GameManager.Instance.SetComboScore(aComboCount);

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
            List<Cube> tempList = new List<Cube>();
            for (int x = 0; x < mGridSize.x; x++)
                tempList.Add(mGrid[x][y]);

            if (!tempList.Contains(null))
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
    public List<Vector2Int> TempScoringMethodThreeInRows(List<Cube> someNewLandedCubes, ref int aComboCount)
    {
        List<Vector2Int> scoringPositions = new List<Vector2Int>();
        if (!someNewLandedCubes.Any())
            return scoringPositions;

        foreach (Cube c in someNewLandedCubes)
        {
            // horizontal
            //  the horizont cross [N][G][N]
            if (GetCubeOn(c.GridPos + Vector2Int.left) != null && GetCubeOn(c.GridPos + Vector2Int.right) != null)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);

                aComboCount++;
            }

            //  the right [G][N][N]
            if (GetCubeOn(c.GridPos + Vector2Int.right) != null && GetCubeOn(c.GridPos + (Vector2Int.right * 2)) != null)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.right))
                    scoringPositions.Add(c.GridPos + Vector2Int.right);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.right * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.right * 2));

                aComboCount++;
            }

            //  the left [N][N][G]
            if (GetCubeOn(c.GridPos + Vector2Int.left) != null && GetCubeOn(c.GridPos + (Vector2Int.left * 2)) != null)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.left))
                    scoringPositions.Add(c.GridPos + Vector2Int.left);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.left * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.left * 2));

                aComboCount++;
            }

            // vertical
            //  the verticla cross
            // [N]
            // [G]
            // [N]
            if (GetCubeOn(c.GridPos + Vector2Int.up) != null && GetCubeOn(c.GridPos + Vector2Int.down) != null)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);

                aComboCount++;
            }

            //  the up
            // [N]
            // [N]
            // [G]
            if (GetCubeOn(c.GridPos + Vector2Int.up) != null && GetCubeOn(c.GridPos + (Vector2Int.up * 2)) != null)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.up))
                    scoringPositions.Add(c.GridPos + Vector2Int.up);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.up * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.up * 2));

                aComboCount++;
            }

            //  the down
            // [G]
            // [N]
            // [N]
            if (GetCubeOn(c.GridPos + Vector2Int.down) != null && GetCubeOn(c.GridPos + (Vector2Int.down * 2)) != null)
            {
                if (!scoringPositions.Contains(c.GridPos))
                    scoringPositions.Add(c.GridPos);
                if (!scoringPositions.Contains(c.GridPos + Vector2Int.down))
                    scoringPositions.Add(c.GridPos + Vector2Int.down);
                if (!scoringPositions.Contains(c.GridPos + (Vector2Int.down * 2)))
                    scoringPositions.Add(c.GridPos + (Vector2Int.down * 2));

                aComboCount++;
            }
        }

        GameManager.Instance.SetComboScore(aComboCount);
        return scoringPositions;
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
    private Cube GetCubeOn(Vector2Int aPos)
    {
        // Boundary check
        if (aPos.x < 0 || aPos.x >= mGridSize.x || aPos.y < 0 || aPos.y >= mGridSize.y)
            return null;

        //return mGridInt[aPos.x, aPos.y];
        return mGrid[aPos.x][aPos.y];
    }

    private void Scorings(Cube aCube, ref List<Vector2Int> scorePositionCollector)
    {
        if(GameSettings.Instance.Difficulty == GameSettings.Difficulties.EASY)
        {
            ScoreWithTwoCubes(aCube, ref scorePositionCollector);
            ScoreWithThreeCubes(aCube, ref scorePositionCollector);
            ScoreWithFourCubes(aCube, ref scorePositionCollector);
        }
        else if(GameSettings.Instance.Difficulty == GameSettings.Difficulties.NORMAL)
        {
            ScoreWithThreeCubes(aCube, ref scorePositionCollector);
            ScoreWithFourCubes(aCube, ref scorePositionCollector);
        }
        else if (GameSettings.Instance.Difficulty == GameSettings.Difficulties.HARD)
        {
            ScoreWithFourCubes(aCube, ref scorePositionCollector);
        }
    }

    private void ScoreWithTwoCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector)
    {
        //  to the right [G][N]
        if (GameManager.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.right)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
        }

        //  to the left [N][G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos + Vector2Int.left, aCube.GridPos)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
        }

        //  with above
        // [N]
        // [G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.up)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
        }

        //  with beneath
        // [G]
        // [N]
        if (GameManager.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.down)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
        }
    }

    private void ScoreWithThreeCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector)
    {
        // horizontal
        //  the horizont cross [N][G][N]
        if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + Vector2Int.left, aCube.GridPos, aCube.GridPos + Vector2Int.right)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
        }

        //  the right [G][N][N]
        if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
        }

        //  the left [N][N][G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left, aCube.GridPos)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
        }

        // vertical
        //  the verticla cross
        // [N]
        // [G]
        // [N]
        if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + Vector2Int.up, aCube.GridPos, aCube.GridPos + Vector2Int.down)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
        }

        //  the up
        // [N]
        // [N]
        // [G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up, aCube.GridPos)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
        }

        //  the down
        // [G]
        // [N]
        // [N]
        if (GameManager.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos, aCube.GridPos + Vector2Int.down, aCube.GridPos + (Vector2Int.down * 2))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
        }
    }

    private void ScoreWithFourCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector)
    {
        // horizontal
        // [G][R][2R][3R]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos, 
            aCube.GridPos + Vector2Int.right, 
            aCube.GridPos + (Vector2Int.right * 2), 
            aCube.GridPos + (Vector2Int.right * 3))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 3));
        }

        // [L][G][R][2R]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos + Vector2Int.left, 
            aCube.GridPos, 
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
        }

        // [2L][L][G][R]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.left * 2), 
            aCube.GridPos + Vector2Int.left, 
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
        }

        // [3L][2L][L][G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.left * 3),
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 3));
        }

        // [3U]
        // [2U]
        // [U]
        // [G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.up * 3), 
            aCube.GridPos + (Vector2Int.up * 2), 
            aCube.GridPos + Vector2Int.up, 
            aCube.GridPos)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 3));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
        }

        // [2U]
        // [U]
        // [G]
        // [D]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.up * 2), 
            aCube.GridPos + Vector2Int.up, 
            aCube.GridPos, 
            aCube.GridPos + Vector2Int.down)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
        }

        // [U]
        // [G]
        // [D]
        // [2D]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos + Vector2Int.up, 
            aCube.GridPos, 
            aCube.GridPos + Vector2Int.down, 
            aCube.GridPos + (Vector2Int.down * 2))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
        }

        // [G]
        // [D]
        // [2D]
        // [3D]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFourPositions(
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2),
            aCube.GridPos + (Vector2Int.down * 3))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 3));
        }
    }

    private void ScoreWithFiveCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector)
    {
        // [G][R][2R][3R][4R]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2),
            aCube.GridPos + (Vector2Int.right * 3),
            aCube.GridPos + (Vector2Int.right * 4))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 3));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 4)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 4));
        }

        // [L][G][R][2R][3R]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2),
            aCube.GridPos + (Vector2Int.right * 3))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 3));
        }

        // [2L][L][G][R][2R]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
        }

        // [3L][2L][L][G][R]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.left * 3),
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 3));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
        }

        // [4L][3L][2L][L][G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.left * 4), 
            aCube.GridPos + (Vector2Int.left * 3),
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 3));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 4)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 4));
        }

        // [4U]
        // [3U]
        // [2U]
        // [U]
        // [G]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.up * 4), 
            aCube.GridPos + (Vector2Int.up * 3),
            aCube.GridPos + (Vector2Int.up * 2),
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 4)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 4));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 3));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
        }

        // [3U]
        // [2U]
        // [U]
        // [G]
        // [D]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.up * 3), 
            aCube.GridPos + (Vector2Int.up * 2),
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down)))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 3));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
        }

        // [2U]
        // [U]
        // [G]
        // [D]
        // [2D]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.up * 2), 
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up * 2))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up * 2);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
        }

        // [U]
        // [G]
        // [D]
        // [2D]
        // [3D]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + Vector2Int.up, 
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2),
            aCube.GridPos + (Vector2Int.down * 3))))
        {
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 3));
        }

        // [G]
        // [D]
        // [2D]
        // [3D]
        // [4D]
        if (GameManager.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2),
            aCube.GridPos + (Vector2Int.down * 3),
            aCube.GridPos + (Vector2Int.down * 4))))
        {   
            if (!aScorePositionCollector.Contains(aCube.GridPos))
                aScorePositionCollector.Add(aCube.GridPos);
            if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
                aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 3)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 3));
            if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 4)))
                aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 4));
        }
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
}
