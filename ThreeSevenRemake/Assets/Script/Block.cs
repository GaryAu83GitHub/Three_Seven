using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
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

        if(mCubeNumbers.Count == 0)
        {
            mCubes[0].Init(this, SupportTools.RNG(0, 8));
            mCubes[1].Init(this, SupportTools.RNG(0, 8));
        }
        else
        {
            for (int i = 0; i < mCubeNumbers.Count; i++)
                mCubes[i].Init(this, mCubeNumbers[i]);
        }

        Joint = transform.GetChild(2);

        mCubeGap = GridData.Instance.CubeGapDistance;
    }
    
    /// <summary>
    /// A shortcut method to check if the cell beneath the block (both cubes) is
    /// vacant
    /// </summary>
    /// <returns>Return the result from the method</returns>
    public bool CheckIfCellIsVacantBeneath()
    {
        return CheckNeighborCellIsVacant(Vector2Int.down);
    }

    public bool IsAnimationPlaying()
    {
        foreach(Cube c in mCubes)
        {
            if (c.AnimationIsPlaying)
                return true;
        }
        return false;
    }
    
    public void SetCubeNumbers(List<int> someNumbers)
    {
        foreach (int n in someNumbers)
            mCubeNumbers.Add(n);
    }

    /// <summary>
    /// The block will rearrange or destroy itself depending on the status of the cubes in it
    /// If the block don't have any cube in it, it'll destroy itself.
    /// </summary>
    /// <returns>Return if the block is destroy or only rearranged itself</returns>
    public bool DestroyThisCube()
    {
        if ((mCubes.Count == 1 && mCubes[0] == null) ||
            (mCubes.Count == 2 && mCubes[0] == null && mCubes[1] == null))
        {
            Destroy(this.gameObject);
            return true;
        }

        if (mCubes[0] == null)
        {
            mCubes.RemoveAt(0);

            transform.position = new Vector3(mCubes[0].GridPos.x * mCubeGap, mCubes[0].GridPos.y * mCubeGap, 0f);
            mCubes[0].transform.localPosition = Vector3.zero;
        }
        if (mCubes.Count == 2 && mCubes[1] == null)
            mCubes.RemoveAt(1);

        if (mCubes.Count == 1)
        {
            mMinPosition = mCubes[0].GridPos;
            mMaxPosition = mCubes[0].GridPos;
        }
        return false;
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
    }

    private void Move(Vector3 aDir)
    {
        Vector2Int dir = new Vector2Int((int)aDir.x, (int)aDir.y);

        // check for the cell to move is vacant
        //if (!GridData.Instance.IsCellVacant(mMinPosition + dir) || !GridData.Instance.IsCellVacant(mMaxPosition + dir))
        if(!CheckNeighborCellIsVacant(dir))
            return;

        transform.Translate(aDir * mCubeGap);
        mCubes[0].GridPos += dir;
        if (mCubes.Count > 1)
            mCubes[1].GridPos += dir;

        mMinPosition += dir;
        mMaxPosition += dir;
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

    private bool CheckNeighborCellIsVacant(Vector2Int aDir)
    {
        // the block lies horizontal
        if(mCurrentRotation == 90 || mCurrentRotation == 270)
        {
            // check the cell to the right of the block maximal horizontal position is vacant
            if (aDir == Vector2Int.right && GridData.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
            // check the cell to the left of the block minimal horizontal position is vacant
            if (aDir == Vector2Int.left && GridData.Instance.IsCellVacant(mMinPosition + aDir))
                return true;
            // check the cell/cells below block min/max vertical position is vacant
            if (aDir == Vector2Int.down && GridData.Instance.IsCellVacant(mMinPosition + aDir) && GridData.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
        }
        // the block stands vertical
        if (mCurrentRotation == 0 || mCurrentRotation == 180)
        {
            // check the cell to the right of the block maximal horizontal position is vacant
            if (aDir == Vector2Int.right && GridData.Instance.IsCellVacant(mMinPosition + aDir) && GridData.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
            // check the cell to the left of the block minimal horizontal position is vacant
            if (aDir == Vector2Int.left && GridData.Instance.IsCellVacant(mMinPosition + aDir) && GridData.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
            // check the cell/cells below block min/max vertical position is vacant
            if (aDir == Vector2Int.down && GridData.Instance.IsCellVacant(mMinPosition + aDir))
                return true;
        }
        

        return false;
    }

    private void GridPositionDebugLog()
    {
        string output = String.Format("RootCube {0} : {1}, SubCube {2} : {3}.",
                              mCubes[0].GridPos.x, mCubes[0].GridPos.y,
                              mCubes[1].GridPos.x, mCubes[1].GridPos.y);
        Debug.Log(output);
    }
}
