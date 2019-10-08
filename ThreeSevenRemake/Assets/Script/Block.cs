using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public delegate void OnSwapWithPreviewBlock(Block thisBlock);
    public static OnSwapWithPreviewBlock swapWithPreviewBlock;

    private List<int> mCubeNumbers = new List<int>();
    public List<int> CubeNumbers { get { return mCubeNumbers; } }

    public int BlockValue { get { return GetTotalBlockValue(); } }

    private int mCurrentRotation = 0;
    public int BlockRotation { get { return mCurrentRotation; } }

    public string BlockName { get { return mBlockName; } }
    private string mBlockName = "";
    
    private Transform Joint;

    private readonly float mCubeGap = Constants.CUBE_GAP_DISTANCE;

    private Vector2Int mPlacingPosition = GridManager.Instance.GridStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        mBlockName = gameObject.name;

        mCubes.Add(transform.GetChild(0).GetComponent<Cube>());
        mCubes[0].GridPos = mPlacingPosition;
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

        Rotate();

        mCubes[0].ConnectSiblingCube(mCubes[1]);
        mCubes[1].ConnectSiblingCube(mCubes[0]);

        Joint = transform.GetChild(2);

        //mCubeGap = //GridManager.Instance.CubeGapDistance;
    }

    public void SetBlockWithRewindData(BlockData aData, bool isActiveBlock = false)
    {
        mBlockName = aData.BlockName;
        SetCubeNumbers(aData.CubeNumbers);
        mCurrentRotation = 0;

        if (!isActiveBlock)
        {
            mPlacingPosition = aData.RootCubePosition;
            mCurrentRotation = aData.Rotation;
        }
    }

    public void SetCubeNumbers(List<int> someNumbers)
    {
        if (!mCubeNumbers.Any())
        {
            foreach (int n in someNumbers)
                mCubeNumbers.Add(n);
        }
        else
        {
            mCubeNumbers[0] = someNumbers[0];
            mCubeNumbers[1] = someNumbers[1];

            mCubes[0].SetCubeNumber(mCubeNumbers[0]);
            mCubes[1].SetCubeNumber(mCubeNumbers[1]);
        }

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

    /// <summary>
    /// Iterate through the cubes and play the vanish animation of those that had been part in a scoring
    /// and unregistrate them from the GridManager
    /// </summary>
    public void PlayCubeAnimation()
    {
        foreach (Cube c in mCubes)
        {
            if (c.IsScoring)
            {
                c.PlayVanishAnimation();
                GridManager.Instance.UnregistrateCell(c.GridPos);
            }
        }
    }

    public void PlayParticleEffect()
    {
        foreach (Cube c in mCubes)
            c.PlayActiveParticlar();
    }

    public bool IsThisBlockScoringAlone(List<Vector2Int> somePos)
    {
        if (somePos.Count != 2)
            return false;

        if(somePos.Contains(mMinPosition) && somePos.Contains(mMaxPosition))
        {
            if ((GridManager.Instance.GetCubeValueOn(somePos[0]) == RootCube.Number && GridManager.Instance.GetCubeValueOn(somePos[1]) == SubCube.Number))
                return true;
            else if ((GridManager.Instance.GetCubeValueOn(somePos[0]) == SubCube.Number && GridManager.Instance.GetCubeValueOn(somePos[1]) == RootCube.Number))
                return true;
        }

        return false;
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
            Vector2Int subCubeDir = mCubes[1].GridPos - mCubes[0].GridPos;

            mCubes.RemoveAt(0);

            mCubes[0].transform.localPosition = Vector3.zero;
            transform.position += new Vector3(subCubeDir.x * mCubeGap, subCubeDir.y * mCubeGap, 0f);
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

    public void SwapWithPreviewBlock()
    {
        swapWithPreviewBlock?.Invoke(this);
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

    // this will be replace by RotateBlockUpgrade
    public void RotateBlock()
    {
        // check if the rotaion is possible with the block's current position and it current
        // add the comming degree of rotaion from the GridManager
        if (!GridManager.Instance.IsRotateAvailable(mCubes[0].GridPos, mCurrentRotation + 90))
            return;

        Joint.Rotate(Vector3.back, 90);
        
        mCurrentRotation += 90;
        if (mCurrentRotation >= 360)
            mCurrentRotation = 0;
        
        Rotate();
    }

    public void RotateBlockUpgrade()
    {
        if (!GridManager.Instance.IsRotateAvailableUppgrade(mCubes[0].GridPos, mCurrentRotation + 90))
            return;
        
        mCurrentRotation += 90;
        if (mCurrentRotation >= 360)
            mCurrentRotation = 0;

        Joint.Rotate(Vector3.back, 90);
        MoveRootCubeAtRotation();
        Rotate();
    }

    private void MoveRootCubeAtRotation()
    {
        //if(mCurrentRotation == 0 && !GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.up) && GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.down))
        //{
        //    Move(Vector3.down);
        //}
        if (mCurrentRotation == 90 && !GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.right) && GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.left))
        {
            Move(Vector3.left);
        }
        if (mCurrentRotation == 180 && !GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.down) && GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.up))
        {
            Move(Vector3.up);
        }
        if (mCurrentRotation == 270 && !GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.left) && GridManager.Instance.IsCellVacant(mCubes[0].GridPos + Vector2Int.right))
        {
            Move(Vector3.right);
        }
    }

    public void Move(Vector3 aDir)
    {
        Vector2Int dir = new Vector2Int((int)aDir.x, (int)aDir.y);

        // check for the cell to move is vacant
        if(!CheckNeighborCellIsVacant(dir))
            return;

        //transform.Translate(aDir * mCubeGap);
        transform.position += (aDir * mCubeGap);
        mCubes[0].GridPos += dir;
        if (mCubes.Count > 1)
            mCubes[1].GridPos += dir;

        mMinPosition += dir;
        mMaxPosition += dir;
    }

    private void Rotate()
    {
        switch (mCurrentRotation)
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
            if (aDir == Vector2Int.right && GridManager.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
            // check the cell to the left of the block minimal horizontal position is vacant
            if (aDir == Vector2Int.left && GridManager.Instance.IsCellVacant(mMinPosition + aDir))
                return true;
            // check the cell/cells below block min/max vertical position is vacant
            if (aDir == Vector2Int.down && GridManager.Instance.IsCellVacant(mMinPosition + aDir) && GridManager.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
            // check the cell/cells above block min/max vertical position is vacant
            if (aDir == Vector2Int.up && GridManager.Instance.IsCellVacant(mMinPosition + aDir) && GridManager.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
        }
        // the block stands vertical
        if (mCurrentRotation == 0 || mCurrentRotation == 180)
        {
            // check the cell to the right of the block maximal horizontal position is vacant
            if (aDir == Vector2Int.right && GridManager.Instance.IsCellVacant(mMinPosition + aDir) && GridManager.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
            // check the cell to the left of the block minimal horizontal position is vacant
            if (aDir == Vector2Int.left && GridManager.Instance.IsCellVacant(mMinPosition + aDir) && GridManager.Instance.IsCellVacant(mMaxPosition + aDir))
                return true;
            // check the cell/cells below block min/max vertical position is vacant
            if (aDir == Vector2Int.down && GridManager.Instance.IsCellVacant(mMinPosition + aDir))
                return true;
            // check the cell/cells above block min/max vertical position is vacant
            if (aDir == Vector2Int.up && GridManager.Instance.IsCellVacant(mMaxPosition + aDir))
            return true;
        }
        

        return false;
    }

    private int GetTotalBlockValue()
    {
        int value = 0;
        foreach (Cube c in mCubes)
            value += c.Number;

        return value;
    }

    private void GridPositionDebugLog()
    {
        string output = String.Format("RootCube {0} : {1}, SubCube {2} : {3}.",
                              mCubes[0].GridPos.x, mCubes[0].GridPos.y,
                              mCubes[1].GridPos.x, mCubes[1].GridPos.y);
        Debug.Log(output);
    }
}
