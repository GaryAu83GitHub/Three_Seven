using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum ClockDirection
    {
        CLOCK_12,
        CLOCK_3,
        CLOCK_6,
        CLOCK_9
    }

    private List<Cube> mCubes = new List<Cube>();
    public List<Cube> Cubes { get { return mCubes; } }
    public Cube RootCube { get { return (mCubes[0] != null ? mCubes[0] : null); } }
    public Cube SubCube { get { return (mCubes[1] != null ? mCubes[1] : null); } }

    private Transform Joint;
    private GameObject Limb;

    private Vector2Int mGridPosition;
    public Vector2Int GridPos { get { return mGridPosition; } }

    private Vector2Int mMaxPosition;
    public Vector2Int MaxGridPos { get { return mMaxPosition; } }

    private Vector2Int mMinPosition;
    public Vector2Int MinGridPos { get { return mMinPosition; } }

    private ClockDirection mCurrentClockDirection;
    public ClockDirection ClockDir { get { return mCurrentClockDirection; } }

    private List<int> mCubeNumbers = new List<int>();
    public List<int> CubeNumbers { get { return mCubeNumbers; } }

    void Start ()
    {
        mCubes.Add(transform.GetChild(0).GetComponent<Cube>());
        mCubes[0].GridPos = GridManager.Instance.StartPosition;
        mCubes[0].name = "RootCube";
        mCubes[0].Init(this, mCubeNumbers[0]);

        mCubes.Add(transform.GetChild(1).GetComponent<Cube>());
        mCubes[1].name = "SubCube";
        mCubes[1].Init(this, mCubeNumbers[1]);

        mCurrentClockDirection = ClockDirection.CLOCK_12;
    }
	
    public void DropDown()
    {
        Move(Vector3.down);
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    private void Move(Vector3 aDir)
    {
        transform.Translate(aDir * GridManager.Instance.CubeDistance);
        Vector2Int dir = new Vector2Int((int)aDir.x, (int)aDir.y);
        mCubes[0].GridPos += dir;
        mCubes[1].GridPos += dir;

        mMinPosition += dir;
        mMaxPosition += dir;
    }
}
