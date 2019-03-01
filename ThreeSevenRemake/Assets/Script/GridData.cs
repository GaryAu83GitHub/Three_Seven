using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class GridData
    {
        private int[,] mGrid;
        private Vector2Int mGridSize = new Vector2Int(10, 20);

        // The static instance for this manager class
        public static GridData Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new GridData();
                    mInstance.GenerateGrid();
                }
                return mInstance;
            }
        }
        private static GridData mInstance;

        /// <summary>
        ///  Generate the grid with default value
        /// </summary>
        private void GenerateGrid()
        {
            mGrid = new int[mGridSize.x, mGridSize.y];
            for (int y = 0; y < mGridSize.y; y++)
            {
                for (int x = 0; x < mGridSize.x; x++)
                {
                    mGrid[x, y] = -1;
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
            mGrid[aCube.GridPos.x, aCube.GridPos.y] = aCube.Number;
        }

        /// <summary>
        /// Set the cell on the requesting position to default value
        /// </summary>
        /// <param name="aPos"></param>
        public void UnregistrateCell(Vector2Int aPos)
        {
            mGrid[aPos.x, aPos.y] = -1;
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
            if (aPos.x < 0 || aPos.x > mGridSize.x || aPos.y <0)
                return false;

            // Checking cells
            if (mGrid[aPos.x, aPos.y] > -1)
                return false;

            return true;
        }


    }

    
}
