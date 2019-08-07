using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBlock : MonoBehaviour
{
    [SerializeField]
    private List<GuideCube> mCubes = new List<GuideCube>();
    public List<GuideCube> Cubes { get { return mCubes; } }
    public GuideCube RootCube { get { return (mCubes[0] ?? null); } }
    public GuideCube SubCube { get { return (mCubes[1] ?? null); } }

    private float mCubeGap = 0f;
    // Start is called before the first frame update
    void Start()
    {
        mCubes.Add(transform.GetChild(0).GetComponent<GuideCube>());
        mCubes[0].name = "RootCube";

        mCubes.Add(transform.GetChild(1).GetComponent<GuideCube>());
        mCubes[1].name = "SubCube";

        mCubeGap = Constants.CUBE_GAP_DISTANCE;//GridData.Instance.CubeGapDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupGuideBlock(Block anActiveBlock)
    {
        RootCube.SetCubeColor(anActiveBlock.RootCube.Color);
        SubCube.SetCubeColor(anActiveBlock.SubCube.Color);
    }

    public void SetPosition(Block anActiveBlock)
    {
        RootCube.GridPos = anActiveBlock.RootCube.GridPos;
        SubCube.GridPos = anActiveBlock.SubCube.GridPos;

        int placingPosition = 0;

        // the block is laying horisontal, since both have different column value, it need to check both cube's
        // tallest row on their respective column value and compare which one have the tallest row beneath them
        // will determined which y position the block will be placed.
        if (RootCube.GridPos.y == SubCube.GridPos.y)
        {
            int rootCubeTallestRow = GridData.Instance.TallestRowFromCubePos(RootCube.GridPos);//GridData.Instance.TallestRowOnColumn(RootCube.GridPos.x);
            int subCubeTallestRow = GridData.Instance.TallestRowFromCubePos(SubCube.GridPos);//GridData.Instance.TallestRowOnColumn(SubCube.GridPos.x);

            placingPosition = (rootCubeTallestRow > subCubeTallestRow) ? rootCubeTallestRow : subCubeTallestRow;
        }
        // the block is standing vertical, since both cube has the same column value, it need only to check
        // on one column value
        else if(RootCube.GridPos.x == SubCube.GridPos.x)
            placingPosition = GridData.Instance.TallestRowFromCubePos((RootCube.GridPos.y < SubCube.GridPos.y) ? RootCube.GridPos : SubCube.GridPos);//GridData.Instance.TallestRowOnColumn((RootCube.GridPos.y < SubCube.GridPos.y) ? RootCube.GridPos.x : SubCube.GridPos.x);

        if (anActiveBlock.BlockRotation == 0)
        {
            RootCube.GridPos = new Vector2Int(RootCube.GridPos.x, placingPosition);
            SubCube.GridPos = new Vector2Int(SubCube.GridPos.x, placingPosition + 1);
        }
        else if(anActiveBlock.BlockRotation == 90)
        {
            RootCube.GridPos = new Vector2Int(RootCube.GridPos.x, placingPosition);
            SubCube.GridPos = new Vector2Int(SubCube.GridPos.x, placingPosition);
        }
        else if(anActiveBlock.BlockRotation == 180)
        {
            RootCube.GridPos = new Vector2Int(RootCube.GridPos.x, placingPosition + 1);
            SubCube.GridPos = new Vector2Int(SubCube.GridPos.x, placingPosition);
        }
        else if (anActiveBlock.BlockRotation == 270)
        {
            RootCube.GridPos = new Vector2Int(RootCube.GridPos.x, placingPosition);
            SubCube.GridPos = new Vector2Int(SubCube.GridPos.x, placingPosition);
        }

        transform.position = new Vector3(RootCube.GridPos.x * mCubeGap, RootCube.GridPos.y * mCubeGap, 10f);
        RootCube.transform.position = new Vector3(RootCube.GridPos.x * mCubeGap, RootCube.GridPos.y * mCubeGap, 10f);
        SubCube.transform.position = new Vector3(SubCube.GridPos.x * mCubeGap, SubCube.GridPos.y * mCubeGap, 10f);

    }
}
