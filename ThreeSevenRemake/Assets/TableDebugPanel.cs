using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDebugPanel : MonoBehaviour
{
    public TableDebugCube ProtoCube;

    private Dictionary<int, List<TableDebugCube>> mGrid = new Dictionary<int, List<TableDebugCube>>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid(10, 20);
        main.blockLandedDebug += GridUpdate;
    }

    private void OnDisable()
    {
        main.blockLandedDebug -= GridUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateGrid(int aWidth, int aHeigh)
    {
        mGrid.Clear();

       
        for (int y = 0; y < aHeigh + 1; y++)
        {
            mGrid.Add(y, new List<TableDebugCube>());
            for (int x = 0; x < aWidth; x++)
            {
                Instantiate(ProtoCube.gameObject, transform);
                transform.GetChild(transform.childCount - 1).GetComponent<TableDebugCube>().SetPosition(x, y);
                mGrid[y].Add(transform.GetChild(transform.childCount - 1).GetComponent<TableDebugCube>());
            }
        }
    }

    private void GridUpdate(Dictionary<int, List<Cube>> aGrid)
    {
        foreach(int y in aGrid.Keys)
        {
            for(int x = 0; x < aGrid[y].Count; x++)
            {
                Color tempColor = new Color();
                string tempString = "";

                if(aGrid[y][x] == null)
                {
                    tempColor = SupportTools.GetCubeColorOf(-1);
                    tempString = "n";
                }
                else
                {
                    tempColor = SupportTools.GetCubeColorOf(aGrid[y][x].Number);
                    tempString = aGrid[y][x].Number.ToString();
                }

                mGrid[y][x].SetCube(tempColor, tempString);
            }
        }
    }
}
