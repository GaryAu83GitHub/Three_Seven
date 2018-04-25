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
    private Transform Limb;

    public Vector2Int GridPos { get { return RootCube.GridPos; } }

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
        mCubes[0].Init(this, Random.Range(0, 8));
        //mCubes[0].Init(this, mCubeNumbers[0]);

        mCubes.Add(transform.GetChild(1).GetComponent<Cube>());
        mCubes[1].name = "SubCube";
        mCubes[1].Init(this, Random.Range(0, 8));
        //mCubes[1].Init(this, mCubeNumbers[1]);

        mCurrentClockDirection = ClockDirection.CLOCK_12;

        Joint = transform.GetChild(2);
        Limb = Joint.GetChild(0);

        ClockPos(0);
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

    public void TurnClockWise()
    {
        //Joint.Rotate(Vector3.back, 90f);
        //mCurrentClockDirection++;
        //if (mCurrentClockDirection > ClockDirection.CLOCK_9)
        //    mCurrentClockDirection = ClockDirection.CLOCK_12;

        ClockPos(1);
    }

    public void TurnCounterClockWise()
    {
        //Joint.Rotate(Vector3.back, -90f);
        //mCurrentClockDirection--;
        //if (mCurrentClockDirection < ClockDirection.CLOCK_12)
        //    mCurrentClockDirection = ClockDirection.CLOCK_9;

        ClockPos(-1);
    }

    public void Landing()
    {
        foreach(Cube c in mCubes)
        {
            GridManager.Instance.PutInCube(c);
        }
    }

    private void Move(Vector3 aDir)
    {
        transform.Translate(aDir * GridManager.Instance.CubeGap);
        Vector2Int dir = new Vector2Int((int)aDir.x, (int)aDir.y);
        mCubes[0].GridPos += dir;
        mCubes[1].GridPos += dir;

        mMinPosition += dir;
        mMaxPosition += dir;
    }

    private void ClockPos(int aDir)
    {
        Joint.Rotate(Vector3.back, aDir * 90);
        mCurrentClockDirection += aDir;

        if (mCurrentClockDirection > ClockDirection.CLOCK_9)
            mCurrentClockDirection = ClockDirection.CLOCK_12;
        else if (mCurrentClockDirection < ClockDirection.CLOCK_12)
            mCurrentClockDirection = ClockDirection.CLOCK_9;

        switch (mCurrentClockDirection)
        {
            case ClockDirection.CLOCK_12:
                SubCubPosition(Vector2Int.up);
                mMinPosition = mCubes[0].GridPos;
                mMaxPosition = mCubes[1].GridPos;
                break;
            case ClockDirection.CLOCK_3:
                SubCubPosition(Vector2Int.right);
                mMinPosition = mCubes[0].GridPos;
                mMaxPosition = mCubes[1].GridPos;
                break;
            case ClockDirection.CLOCK_6:
                SubCubPosition(Vector2Int.down);
                mMinPosition = mCubes[1].GridPos;
                mMaxPosition = mCubes[0].GridPos;
                break;
            case ClockDirection.CLOCK_9:
                SubCubPosition(Vector2Int.left);
                mMinPosition = mCubes[1].GridPos;
                mMaxPosition = mCubes[0].GridPos;
                break;
        }
    }

    private void SubCubPosition(Vector2Int aDir)
    {
        mCubes[1].transform.position = mCubes[0].transform.position + new Vector3(aDir.x * GridManager.Instance.CubeGap, aDir.y * GridManager.Instance.CubeGap, 0f);
        mCubes[1].GridPos = mCubes[0].GridPos + aDir;
    }
}
