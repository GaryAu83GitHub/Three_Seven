using System;
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

    private List<int> mCubeNumbers = new List<int>();
    public List<int> CubeNumbers { get { return mCubeNumbers; } }
    
    private Transform Joint;

    private float mCubeGap = 0f;
    private int mCurrentRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        mCubes.Add(transform.GetChild(0).GetComponent<Cube>());
        mCubes[0].GridPos = GridData.Instance.GridStartPosition;
        mCubes[0].name = "RootCube";

        mCubes.Add(transform.GetChild(1).GetComponent<Cube>());
        mCubes[1].GridPos = mCubes[0].GridPos + Vector2Int.up;
        mCubes[1].name = "SubCube";

        mMinPosition = mCubes[0].GridPos;
        mMaxPosition = mCubes[1].GridPos;

        // this is for creating the first block when the game start
        mCubes[0].Init(this, SupportTools.RNG(0, 8));
        mCubes[1].Init(this, SupportTools.RNG(0, 8));
        

        //mCurrentClockDirection = ClockDirection.CLOCK_12;

        Joint = transform.GetChild(2);

        mCubeGap = GridData.Instance.CubeGapDistance;

        GridPositionDebugLog();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGridStartPosition(Vector2Int aPosition)
    {

    }

    public void SetCubeNumbers(List<int> someNumbers)
    {
        foreach (int n in someNumbers)
            mCubeNumbers.Add(n);
    }

    public void ChangeCubeArrangement()
    {
        if(mCubes[0] == null)
        {
            Cube tempCube = mCubes[1];
            mCubes[0] = mCubes[1];
            mCubes.RemoveAt(1);
        }
        if(mCubes[1] == null)
        {
            mCubes.RemoveAt(1);
        }
        if (mCubes[0] == null && mCubes[1] == null)
            Destroy(this);
    }

    public void DestroyJoint()
    {
        if (Joint != null)
            Destroy(Joint.gameObject);
    }

    public void Swap()
    {
        int rootNumber = RootCube.Number;
        int subNumber = SubCube.Number;

        mCubes[0].SetCubeNumber(subNumber);
        mCubes[1].SetCubeNumber(rootNumber);
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

    public void Rotate()
    {
        // check if the rotaion is possible with the block's current position and it current
        // add the comming degree of rotaion from the GridData
        if (!GridData.Instance.IsRotateAvailable(mCubes[0].GridPos, mCurrentRotation + 90))
            return;

        Joint.Rotate(Vector3.back, 90);
        mCurrentRotation += 90;
        if (mCurrentRotation >= 360)
            mCurrentRotation = 0;

        switch(mCurrentRotation)
        {
            case 0:
                SetSubCubPosition(Vector2Int.up);
                break;
            case 90:
                SetSubCubPosition(Vector2Int.right);
                break;
            case 180:
                SetSubCubPosition(Vector2Int.down);
                break;
            case 270:
                SetSubCubPosition(Vector2Int.left);
                break;

        }
        GridPositionDebugLog();
    }

    private void Move(Vector3 aDir)
    {
        Vector2Int dir = new Vector2Int((int)aDir.x, (int)aDir.y);

        // check for the cell to move is vacant
        if (!GridData.Instance.IsCellVacant(mMinPosition + dir) || !GridData.Instance.IsCellVacant(mMaxPosition + dir))
            return;

        transform.Translate(aDir * mCubeGap);
        mCubes[0].GridPos += dir;
        if (mCubes.Count > 1)
            mCubes[1].GridPos += dir;

        mMinPosition += dir;
        mMaxPosition += dir;

        GridPositionDebugLog();
    }

    private void SetSubCubPosition(Vector2Int aDir)
    {
        mCubes[1].transform.position = mCubes[0].transform.position + new Vector3(aDir.x * mCubeGap, aDir.y * mCubeGap, 0f);
        mCubes[1].GridPos = mCubes[0].GridPos + aDir;

        if(mCubes[1].GridPos.x > mCubes[0].GridPos.x || 
            mCubes[1].GridPos.y > mCubes[0].GridPos.y)
        {
            mMinPosition = RootCube.GridPos;
            mMaxPosition = SubCube.GridPos;
        }
        else
        {
            mMinPosition = SubCube.GridPos;
            mMaxPosition = RootCube.GridPos;
        }
    }

    private void GridPositionDebugLog()
    {
        string output = String.Format("RootCube {0} : {1}, SubCube {2} : {3}.",
                              mCubes[0].GridPos.x, mCubes[0].GridPos.y,
                              mCubes[1].GridPos.x, mCubes[1].GridPos.y);
        Debug.Log(output);
    }
}
