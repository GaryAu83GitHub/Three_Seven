using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeveloping : MonoBehaviour
{
    [SerializeField]
    private List<Cube> mCubes = new List<Cube>();
    public List<Cube> Cubes { get { return mCubes; } }
    public Cube RootCube { get { return (mCubes[0] != null ? mCubes[0] : null); } }
    public Cube SubCube { get { return (mCubes[1] != null ? mCubes[1] : null); } }

    private Vector2Int mMaxPosition;
    public Vector2Int MaxGridPos { get { return mMaxPosition; } }

    private Vector2Int mMinPosition;
    public Vector2Int MinGridPos { get { return mMinPosition; } }

    private Transform Joint;
    private Transform Limb;

    // Start is called before the first frame update
    void Start()
    {
        mCubes.Add(transform.GetChild(0).GetComponent<Cube>());
        mCubes[0].GridPos = GridManager.Instance.StartPosition;
        mCubes[0].name = "RootCube";

        mCubes.Add(transform.GetChild(1).GetComponent<Cube>());
        mCubes[1].name = "SubCube";

        // this is for creating the first block when the game start
        mCubes[0].Init(this, SupportTools.RNG(0, 8));
        mCubes[1].Init(this, SupportTools.RNG(0, 8));
        

        //mCurrentClockDirection = ClockDirection.CLOCK_12;

        Joint = transform.GetChild(2);
        Limb = Joint.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
