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
    /// Use as a temporary storing for the new original landed block
    /// </summary>
    private List<Vector2Int> mOriginalLandedBlockPositions = new List<Vector2Int>();

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

    public void AddOriginalBlockPosition(Vector2Int aBlockCubeGridPos)
    {
        if (mOriginalLandedBlockPositions.Count == 2)
            return;
        mOriginalLandedBlockPositions.Add(aBlockCubeGridPos);
    }

    //public List<Vector2Int> CompleteObjectiveScoringMethod(List<Cube> someNewLandedCubes, ref int aComboCount)
    //{
    //    List<Vector2Int> scoringPositions = new List<Vector2Int>();

    //    foreach (Cube c in someNewLandedCubes)
    //    {
    //        Scorings(c, ref scoringPositions, ref aComboCount);
    //    }

    //    GameManager.Instance.AddComboScore(aComboCount);

    //    return scoringPositions;
    //}

    public List<ScoringGroupAchieveInfo> GetListOfScoringPositionGroups(List<Cube> someNewLandedCubes)
    {
        List<ScoringGroupAchieveInfo> someGroupOfPosition = new List<ScoringGroupAchieveInfo>();

        foreach (Cube c in someNewLandedCubes)
        {
            Scoring(c, ref someGroupOfPosition);
        }

        mOriginalLandedBlockPositions.Clear();
        //GameManager.Instance.SetComboScore(aComboCount);

        return someGroupOfPosition;
    }

    /// <summary>
    /// </summary>
    /// <param name="someNewLandedCubes"></param>
    /// <param name="aComboCount"></param>
    /// <returns></returns>
    public List<Vector2Int> CompleteObjectiveScoringMethodWithThreeCube(List<Cube> someNewLandedCubes, ref int aComboCount)
    {
        List<Vector2Int> scoringPositions = new List<Vector2Int>();

        foreach (Cube c in someNewLandedCubes)
        {
            // horizontal
            //  the horizont cross [N][G][N]
            if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + Vector2Int.left, c.GridPos, c.GridPos + Vector2Int.right)))
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
            if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.right, c.GridPos + (Vector2Int.right * 2))))
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
            if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + (Vector2Int.left * 2), c.GridPos + Vector2Int.left, c.GridPos)))
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
            if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + Vector2Int.up, c.GridPos, c.GridPos + Vector2Int.down)))
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
            if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos + (Vector2Int.up * 2), c.GridPos + Vector2Int.up, c.GridPos)))
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
            if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(c.GridPos, c.GridPos + Vector2Int.down, c.GridPos + (Vector2Int.down * 2))))
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

        GameManager.Instance.AddComboScore(aComboCount);

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

        GameManager.Instance.AddComboScore(aComboCount);

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

        GameManager.Instance.AddComboScore(aComboCount);
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

    private void Scoring(Cube aCube, ref List<ScoringGroupAchieveInfo> someGroupOfPositions)
    {
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT])
            ScoreWithTwoCubes(aCube, ref someGroupOfPositions);
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT])
            ScoreWithThreeCubes(aCube, ref someGroupOfPositions);
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT])
            ScoreWithFourCubes(aCube, ref someGroupOfPositions);
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT])
            ScoreWithFiveCubes(aCube, ref someGroupOfPositions);

        List<ScoringGroupAchieveInfo> testingList = new List<ScoringGroupAchieveInfo>();
        ScoreWithAvailableCubes(aCube, ref testingList);

        return;
    }

    private void ScoreWithAvailableCubes(Cube aCube, ref List<ScoringGroupAchieveInfo> someGroupOfPositions)
    {
        List<List<Vector2Int>> scoreCombinationPositions = GenerateScoreCombinationPositions.Instance.GetScorePositionListFrom(aCube.GridPos);

        Objectives getObjectiveRank = Objectives.X1;
        ScoringGroupAchieveInfo newInfo;
        foreach(List<Vector2Int> pos in scoreCombinationPositions)
        {
            if(Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValue(pos)) && IsNotOnlyTheOriginal(pos))
            {
                newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, pos);
                someGroupOfPositions.Add(newInfo);
            }
        }
    }

    private void ScoreWithTwoCubes(Cube aCube, ref List<ScoringGroupAchieveInfo> someGroupOfPositions)
    {
        Objectives getObjectiveRank = Objectives.X1;
        ScoringGroupAchieveInfo newInfo;
        //  to the right [G][R] 
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.right)) &&
            IsNotOnlyTheOriginal(aCube.GridPos, aCube.GridPos + Vector2Int.right))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.right });
            someGroupOfPositions.Add(newInfo);
        }

        //  to the left [L][G]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromTwoPositions(aCube.GridPos + Vector2Int.left, aCube.GridPos)) &&
            IsNotOnlyTheOriginal(aCube.GridPos + Vector2Int.left, aCube.GridPos))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.left });
            someGroupOfPositions.Add(newInfo);
        }

        //  with above
        // [U]
        // [G]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromTwoPositions(aCube.GridPos + Vector2Int.up, aCube.GridPos)) &&
            IsNotOnlyTheOriginal(aCube.GridPos + Vector2Int.up, aCube.GridPos))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.up });
            someGroupOfPositions.Add(newInfo);
        }

        //  with beneath
        // [G]
        // [D]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.down)) &&
            IsNotOnlyTheOriginal(aCube.GridPos, aCube.GridPos + Vector2Int.down))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.down });
            someGroupOfPositions.Add(newInfo);
        }
    }

    private void ScoreWithThreeCubes(Cube aCube, ref List<ScoringGroupAchieveInfo> someGroupOfPositions)
    {
        Objectives getObjectiveRank = Objectives.X1;
        ScoringGroupAchieveInfo newInfo;
        // horizontal
        //  the horizont cross [L][G][R]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromThreePositions(aCube.GridPos + Vector2Int.left, aCube.GridPos, aCube.GridPos + Vector2Int.right)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.left, aCube.GridPos + Vector2Int.right });
            someGroupOfPositions.Add(newInfo);
        }

        //  the right [G][R][2R]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromThreePositions(aCube.GridPos, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.right, aCube.GridPos + Vector2Int.right * 2 });
            someGroupOfPositions.Add(newInfo);
        }

        //  the left [2L][L][G]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromThreePositions(aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left, aCube.GridPos)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.left * 2, aCube.GridPos + Vector2Int.left });
            someGroupOfPositions.Add(newInfo);
        }

        // vertical
        //  the verticla cross
        // [U]
        // [G]
        // [D]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromThreePositions(aCube.GridPos + Vector2Int.up, aCube.GridPos, aCube.GridPos + Vector2Int.down)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.up, aCube.GridPos + Vector2Int.down });
            someGroupOfPositions.Add(newInfo);
        }

        //  the up
        // [2U]
        // [U]
        // [G]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromThreePositions(
            aCube.GridPos + (Vector2Int.up * 2), 
            aCube.GridPos + Vector2Int.up, 
            aCube.GridPos)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up });
            someGroupOfPositions.Add(newInfo);
        }

        //  the down
        // [G]
        // [D]
        // [2D]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromThreePositions(
            aCube.GridPos, 
            aCube.GridPos + Vector2Int.down, 
            aCube.GridPos + (Vector2Int.down * 2))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.down, aCube.GridPos + Vector2Int.down * 2 });
            someGroupOfPositions.Add(newInfo);
        }
    }

    private void ScoreWithFourCubes(Cube aCube, ref List<ScoringGroupAchieveInfo> someGroupOfPositions)
    {
        Objectives getObjectiveRank = Objectives.X1;
        ScoringGroupAchieveInfo newInfo;
        // horizontal
        // [G][R][2R][3R]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2),
            aCube.GridPos + (Vector2Int.right * 3))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2), aCube.GridPos + (Vector2Int.right * 3) });
            someGroupOfPositions.Add(newInfo);
        }

        // [L][G][R][2R]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.left, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2) });
            someGroupOfPositions.Add(newInfo);
        }

        // [2L][L][G][R]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left, aCube.GridPos + Vector2Int.right });
            someGroupOfPositions.Add(newInfo);
        }

        // [3L][2L][L][G]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.left * 3),
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.left * 3), aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left });
            someGroupOfPositions.Add(newInfo);
        }

        // [3U]
        // [2U]
        // [U]
        // [G]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.up * 3),
            aCube.GridPos + (Vector2Int.up * 2),
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.up * 3), aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up });
            someGroupOfPositions.Add(newInfo);
        }

        // [2U]
        // [U]
        // [G]
        // [D]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos + (Vector2Int.up * 2),
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up, aCube.GridPos + Vector2Int.down });
            someGroupOfPositions.Add(newInfo);
        }

        // [U]
        // [G]
        // [D]
        // [2D]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.up, aCube.GridPos + Vector2Int.down, aCube.GridPos + (Vector2Int.down * 2) });
            someGroupOfPositions.Add(newInfo);
        }

        // [G]
        // [D]
        // [2D]
        // [3D]
        if (Objective.Instance.AchiveObjective(ref getObjectiveRank, TotalValueFromFourPositions(
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2),
            aCube.GridPos + (Vector2Int.down * 3))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.down, aCube.GridPos + (Vector2Int.down * 2), aCube.GridPos + (Vector2Int.down * 3) });
            someGroupOfPositions.Add(newInfo);
        }
    }

    private void ScoreWithFiveCubes(Cube aCube, ref List<ScoringGroupAchieveInfo> someGroupOfPositions)
    {
        Objectives getObjectiveRank = Objectives.X1;
        ScoringGroupAchieveInfo newInfo;
        // [G][R][2R][3R][4R]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2),
            aCube.GridPos + (Vector2Int.right * 3),
            aCube.GridPos + (Vector2Int.right * 4))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2), aCube.GridPos + (Vector2Int.right * 3), aCube.GridPos + (Vector2Int.right * 4) });
            someGroupOfPositions.Add(newInfo);
        }

        // [L][G][R][2R][3R]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2),
            aCube.GridPos + (Vector2Int.right * 3))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.left, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2), aCube.GridPos + (Vector2Int.right * 3) });
            someGroupOfPositions.Add(newInfo);
        }

        // [2L][L][G][R][2R]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right,
            aCube.GridPos + (Vector2Int.right * 2))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2) });
            someGroupOfPositions.Add(newInfo);
        }

        // [3L][2L][L][G][R]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.left * 3),
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.right)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.left * 3), aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left, aCube.GridPos + Vector2Int.right });
            someGroupOfPositions.Add(newInfo);
        }

        // [4L][3L][2L][L][G]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.left * 4),
            aCube.GridPos + (Vector2Int.left * 3),
            aCube.GridPos + (Vector2Int.left * 2),
            aCube.GridPos + Vector2Int.left,
            aCube.GridPos)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.left * 4), aCube.GridPos + (Vector2Int.left * 3), aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left, });
            someGroupOfPositions.Add(newInfo);
        }

        // [4U]
        // [3U]
        // [2U]
        // [U]
        // [G]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.up * 4),
            aCube.GridPos + (Vector2Int.up * 3),
            aCube.GridPos + (Vector2Int.up * 2),
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.up * 4), aCube.GridPos + (Vector2Int.up * 3), aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up, });
            someGroupOfPositions.Add(newInfo);
        }

        // [3U]
        // [2U]
        // [U]
        // [G]
        // [D]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.up * 3),
            aCube.GridPos + (Vector2Int.up * 2),
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down)))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.up * 3), aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up, aCube.GridPos + Vector2Int.down });
            someGroupOfPositions.Add(newInfo);
        }

        // [2U]
        // [U]
        // [G]
        // [D]
        // [2D]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + (Vector2Int.up * 2),
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up, aCube.GridPos + Vector2Int.down, aCube.GridPos + (Vector2Int.down * 2) });
            someGroupOfPositions.Add(newInfo);
        }

        // [U]
        // [G]
        // [D]
        // [2D]
        // [3D]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos + Vector2Int.up,
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2),
            aCube.GridPos + (Vector2Int.down * 3))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.up, aCube.GridPos + Vector2Int.down, aCube.GridPos + (Vector2Int.down * 2), aCube.GridPos + (Vector2Int.down * 3) });
            someGroupOfPositions.Add(newInfo);
        }

        // [G]
        // [D]
        // [2D]
        // [3D]
        // [4D]
        if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
            aCube.GridPos,
            aCube.GridPos + Vector2Int.down,
            aCube.GridPos + (Vector2Int.down * 2),
            aCube.GridPos + (Vector2Int.down * 3),
            aCube.GridPos + (Vector2Int.down * 4))))
        {
            newInfo = new ScoringGroupAchieveInfo(getObjectiveRank, new List<Vector2Int> { aCube.GridPos, aCube.GridPos + Vector2Int.down, aCube.GridPos + (Vector2Int.down * 2), aCube.GridPos + (Vector2Int.down * 3), aCube.GridPos + (Vector2Int.down * 4) });
            someGroupOfPositions.Add(newInfo);
        }
    }

    private bool IsNotOnlyTheOriginal(Vector2Int aPos1, Vector2Int aPos2)
    {
        if (mOriginalLandedBlockPositions.Contains(aPos1) && mOriginalLandedBlockPositions.Contains(aPos2))
            return false;
        return true;
    }

    private bool IsNotOnlyTheOriginal(List<Vector2Int> somePos)
    {
        if (somePos.Count > 2)
            return true;

        if (mOriginalLandedBlockPositions.Contains(somePos[0]) && mOriginalLandedBlockPositions.Contains(somePos[1]))
            return false;
        return true;
    }

    private bool ThisGroupIsAlreadyRegistrated(ref List<ScoringGroupAchieveInfo> someGroupOfPosition, List<Vector2Int> someScoringPositions)
    {
        foreach(ScoringGroupAchieveInfo info in someGroupOfPosition)
        {
            if (info.GroupPosition.SequenceEqual(someScoringPositions))
                return true;
        }
        return false;
    }

    //private void Scorings(Cube aCube, ref List<Vector2Int> scorePositionCollector, ref int aComboScore)
    //{
    //    if(GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT])
    //        ScoreWithTwoCubes(aCube, ref scorePositionCollector, ref aComboScore);
    //    if(GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT])
    //        ScoreWithThreeCubes(aCube, ref scorePositionCollector, ref aComboScore);
    //    if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT])
    //        ScoreWithFourCubes(aCube, ref scorePositionCollector, ref aComboScore);
    //    if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT])
    //        ScoreWithFiveCubes(aCube, ref scorePositionCollector, ref aComboScore);

    //}

    //private void ScoreWithTwoCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector, ref int aComboScore)
    //{
    //    //  to the right [G][N]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.right)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);

    //        aComboScore++;
    //    }

    //    //  to the left [N][G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos + Vector2Int.left, aCube.GridPos)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);

    //        aComboScore++;
    //    }

    //    //  with above
    //    // [N]
    //    // [G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.up)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);

    //        aComboScore++;
    //    }

    //    //  with beneath
    //    // [G]
    //    // [N]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromTwoPositions(aCube.GridPos, aCube.GridPos + Vector2Int.down)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);

    //        aComboScore++;
    //    }
    //}

    //private void ScoreWithThreeCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector, ref int aComboScore)
    //{
    //    // horizontal
    //    //  the horizont cross [N][G][N]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + Vector2Int.left, aCube.GridPos, aCube.GridPos + Vector2Int.right)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);

    //        aComboScore++;
    //    }

    //    //  the right [G][N][N]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos, aCube.GridPos + Vector2Int.right, aCube.GridPos + (Vector2Int.right * 2))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));

    //        aComboScore++;
    //    }

    //    //  the left [N][N][G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + (Vector2Int.left * 2), aCube.GridPos + Vector2Int.left, aCube.GridPos)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));

    //        aComboScore++;
    //    }

    //    // vertical
    //    //  the verticla cross
    //    // [N]
    //    // [G]
    //    // [N]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + Vector2Int.up, aCube.GridPos, aCube.GridPos + Vector2Int.down)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);

    //        aComboScore++;
    //    }

    //    //  the up
    //    // [N]
    //    // [N]
    //    // [G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos + (Vector2Int.up * 2), aCube.GridPos + Vector2Int.up, aCube.GridPos)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));

    //        aComboScore++;
    //    }

    //    //  the down
    //    // [G]
    //    // [N]
    //    // [N]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromThreePositions(aCube.GridPos, aCube.GridPos + Vector2Int.down, aCube.GridPos + (Vector2Int.down * 2))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));

    //        aComboScore++;
    //    }
    //}

    //private void ScoreWithFourCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector, ref int aComboScore)
    //{
    //    // horizontal
    //    // [G][R][2R][3R]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos, 
    //        aCube.GridPos + Vector2Int.right, 
    //        aCube.GridPos + (Vector2Int.right * 2), 
    //        aCube.GridPos + (Vector2Int.right * 3))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 3));

    //        aComboScore++;
    //    }

    //    // [L][G][R][2R]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos + Vector2Int.left, 
    //        aCube.GridPos, 
    //        aCube.GridPos + Vector2Int.right,
    //        aCube.GridPos + (Vector2Int.right * 2))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));

    //        aComboScore++;
    //    }

    //    // [2L][L][G][R]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos + (Vector2Int.left * 2), 
    //        aCube.GridPos + Vector2Int.left, 
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.right)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
    //    }

    //    // [3L][2L][L][G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos + (Vector2Int.left * 3),
    //        aCube.GridPos + (Vector2Int.left * 2),
    //        aCube.GridPos + Vector2Int.left,
    //        aCube.GridPos)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 3));

    //        aComboScore++;
    //    }

    //    // [3U]
    //    // [2U]
    //    // [U]
    //    // [G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos + (Vector2Int.up * 3), 
    //        aCube.GridPos + (Vector2Int.up * 2), 
    //        aCube.GridPos + Vector2Int.up, 
    //        aCube.GridPos)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 3));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);

    //        aComboScore++;
    //    }

    //    // [2U]
    //    // [U]
    //    // [G]
    //    // [D]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos + (Vector2Int.up * 2), 
    //        aCube.GridPos + Vector2Int.up, 
    //        aCube.GridPos, 
    //        aCube.GridPos + Vector2Int.down)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);

    //        aComboScore++;
    //    }

    //    // [U]
    //    // [G]
    //    // [D]
    //    // [2D]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos + Vector2Int.up, 
    //        aCube.GridPos, 
    //        aCube.GridPos + Vector2Int.down, 
    //        aCube.GridPos + (Vector2Int.down * 2))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));

    //        aComboScore++;
    //    }

    //    // [G]
    //    // [D]
    //    // [2D]
    //    // [3D]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFourPositions(
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.down,
    //        aCube.GridPos + (Vector2Int.down * 2),
    //        aCube.GridPos + (Vector2Int.down * 3))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 3));

    //        aComboScore++;
    //    }
    //}

    //private void ScoreWithFiveCubes(Cube aCube, ref List<Vector2Int> aScorePositionCollector, ref int aComboScore)
    //{
    //    // [G][R][2R][3R][4R]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.right,
    //        aCube.GridPos + (Vector2Int.right * 2),
    //        aCube.GridPos + (Vector2Int.right * 3),
    //        aCube.GridPos + (Vector2Int.right * 4))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 3));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 4)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 4));

    //        aComboScore++;
    //    }

    //    // [L][G][R][2R][3R]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + Vector2Int.left,
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.right,
    //        aCube.GridPos + (Vector2Int.right * 2),
    //        aCube.GridPos + (Vector2Int.right * 3))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 3));

    //        aComboScore++;
    //    }

    //    // [2L][L][G][R][2R]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + (Vector2Int.left * 2),
    //        aCube.GridPos + Vector2Int.left,
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.right,
    //        aCube.GridPos + (Vector2Int.right * 2))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.right * 2));

    //        aComboScore++;
    //    }

    //    // [3L][2L][L][G][R]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + (Vector2Int.left * 3),
    //        aCube.GridPos + (Vector2Int.left * 2),
    //        aCube.GridPos + Vector2Int.left,
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.right)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 3));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.right))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.right);

    //        aComboScore++;
    //    }

    //    // [4L][3L][2L][L][G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + (Vector2Int.left * 4), 
    //        aCube.GridPos + (Vector2Int.left * 3),
    //        aCube.GridPos + (Vector2Int.left * 2),
    //        aCube.GridPos + Vector2Int.left,
    //        aCube.GridPos)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.left))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.left);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.left * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 3));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.right * 4)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.left * 4));

    //        aComboScore++;
    //    }

    //    // [4U]
    //    // [3U]
    //    // [2U]
    //    // [U]
    //    // [G]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + (Vector2Int.up * 4), 
    //        aCube.GridPos + (Vector2Int.up * 3),
    //        aCube.GridPos + (Vector2Int.up * 2),
    //        aCube.GridPos + Vector2Int.up,
    //        aCube.GridPos)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 4)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 4));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 3));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);

    //        aComboScore++;
    //    }

    //    // [3U]
    //    // [2U]
    //    // [U]
    //    // [G]
    //    // [D]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + (Vector2Int.up * 3), 
    //        aCube.GridPos + (Vector2Int.up * 2),
    //        aCube.GridPos + Vector2Int.up,
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.down)))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 3));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.up * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.up * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);

    //        aComboScore++;
    //    }

    //    // [2U]
    //    // [U]
    //    // [G]
    //    // [D]
    //    // [2D]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + (Vector2Int.up * 2), 
    //        aCube.GridPos + Vector2Int.up,
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.down,
    //        aCube.GridPos + (Vector2Int.down * 2))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up * 2))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up * 2);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));

    //        aComboScore++;
    //    }

    //    // [U]
    //    // [G]
    //    // [D]
    //    // [2D]
    //    // [3D]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos + Vector2Int.up, 
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.down,
    //        aCube.GridPos + (Vector2Int.down * 2),
    //        aCube.GridPos + (Vector2Int.down * 3))))
    //    {
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.up))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.up);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 3));

    //        aComboScore++;
    //    }

    //    // [G]
    //    // [D]
    //    // [2D]
    //    // [3D]
    //    // [4D]
    //    if (Objective.Instance.AchiveObjective(TotalValueFromFivePositions(
    //        aCube.GridPos,
    //        aCube.GridPos + Vector2Int.down,
    //        aCube.GridPos + (Vector2Int.down * 2),
    //        aCube.GridPos + (Vector2Int.down * 3),
    //        aCube.GridPos + (Vector2Int.down * 4))))
    //    {   
    //        if (!aScorePositionCollector.Contains(aCube.GridPos))
    //            aScorePositionCollector.Add(aCube.GridPos);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + Vector2Int.down))
    //            aScorePositionCollector.Add(aCube.GridPos + Vector2Int.down);
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 2)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 2));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 3)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 3));
    //        if (!aScorePositionCollector.Contains(aCube.GridPos + (Vector2Int.down * 4)))
    //            aScorePositionCollector.Add(aCube.GridPos + (Vector2Int.down * 4));

    //        aComboScore++;
    //    }
    //}

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
}
