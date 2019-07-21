using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask UnWalkableMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;

    private Node[,] mGrid;

    private float mNodeDiameter;
    private Vector2Int mGridSize;

    void Start()
    {
        mNodeDiameter = NodeRadius * 2;
        mGridSize = new Vector2Int(Mathf.RoundToInt(GridWorldSize.x / mNodeDiameter), Mathf.RoundToInt(GridWorldSize.y / mNodeDiameter));
        CreateGrid();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1f, GridWorldSize.y));

        if(mGrid != null)
        {
            foreach (Node n in mGrid)
            {
                Gizmos.color = ((n.Walkable) ? Color.white : Color.red);
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (mNodeDiameter - .1f));
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 aWorldPosition)
    {
        float percentX = (aWorldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (aWorldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((mGridSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((mGridSize.y - 1) * percentY);

        return mGrid[x, y];
    }

    private void CreateGrid()
    {
        mGrid = new Node[mGridSize.x, mGridSize.y];
        Vector3 worldButtonLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;

        for(int x = 0; x < mGridSize.x; x++)
        {
            for (int y = 0; y < mGridSize.x; y++)
            {
                Vector3 worldPoint = worldButtonLeft + Vector3.right * (x * mNodeDiameter + NodeRadius) + Vector3.forward * (y * mNodeDiameter + NodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnWalkableMask));
                mGrid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }
}
