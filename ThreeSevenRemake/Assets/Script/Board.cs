using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject CubeObject;
    private int[,] mGrid;

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int gridSize = new Vector2Int(10, 20);
        mGrid = new int[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
            {
                mGrid[x, y] = -1;
                CubeObject.name = "Cube" + x.ToString() + ":" + y.ToString();
                Instantiate(CubeObject, new Vector2(x * .5f, y * .5f), Quaternion.identity, transform);
                transform.GetChild(transform.childCount-1).GetComponent<Cube>().SetCubeNumber(mGrid[x, y]);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
