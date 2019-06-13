using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBlock : MonoBehaviour
{
    [SerializeField]
    private List<Cube> mCubes = new List<Cube>();
    public List<Cube> Cubes { get { return mCubes; } }
    public Cube RootCube { get { return (mCubes[0] ?? null); } }
    public Cube SubCube { get { return (mCubes[1] ?? null); } }

    [SerializeField]
    private Vector2Int mMinPosition;
    public Vector2Int MinGridPos { get { return mMinPosition; } }

    [SerializeField]
    private Vector2Int mMaxPosition;
    public Vector2Int MaxGridPos { get { return mMaxPosition; } }

    private List<int> mCubeNumbers = new List<int>();
    public List<int> CubeNumbers { get { return mCubeNumbers; } }

    // Start is called before the first frame update
    void Start()
    {
        mCubes.Add(transform.GetChild(0).GetComponent<Cube>());
        mCubes[0].name = "RootCube";

        mCubes.Add(transform.GetChild(1).GetComponent<Cube>());
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

    }
}
