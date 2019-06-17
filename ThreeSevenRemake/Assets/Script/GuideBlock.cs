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

    [SerializeField]
    private Vector2Int mMinPosition;
    public Vector2Int MinGridPos { get { return mMinPosition; } }

    [SerializeField]
    private Vector2Int mMaxPosition;
    public Vector2Int MaxGridPos { get { return mMaxPosition; } }

    // Start is called before the first frame update
    void Start()
    {
        mCubes.Add(transform.GetChild(0).GetComponent<GuideCube>());
        mCubes[0].name = "RootCube";

        mCubes.Add(transform.GetChild(1).GetComponent<GuideCube>());
        mCubes[1].name = "SubCube";

        mMinPosition = mCubes[0].GridPos;
        mMaxPosition = mCubes[1].GridPos;
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



    }
}
