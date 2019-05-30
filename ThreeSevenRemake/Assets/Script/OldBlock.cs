using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBlock : MonoBehaviour
{
    public enum ClockDirection
    {
        CLOCK_12,
        CLOCK_3,
        CLOCK_6,
        CLOCK_9
    }

    [SerializeField]
    private List<Cube> mCubes = new List<Cube>();
    public List<Cube> Cubes { get { return mCubes; } }
    public Cube RootCube { get { return (mCubes[0] ?? null); } }
    public Cube SubCube { get { return (mCubes[1] ?? null); } }

    private Transform Joint;
    private Transform Limb;

    public Vector2Int GridPos { get { return RootCube.GridPos; } }

    private Vector2Int mMaxPosition;
    public Vector2Int MaxGridPos { get { return mMaxPosition; } }

    private Vector2Int mMinPosition;
    public Vector2Int MinGridPos { get { return mMinPosition; } }

    private List<int> mCubeNumbers = new List<int>();
    public List<int> CubeNumbers { get { return mCubeNumbers; } }

    private ClockDirection mCurrentClockDirection;
    public ClockDirection ClockDir { get { return mCurrentClockDirection; } }

    private int mScoringTimes = 0;
    public int ScoringTimes { get { return mScoringTimes; } set { mScoringTimes = value; } }

    public bool IsScoring
    {
        get
        {
            // Checking if any of the cube in the block is scoring
            if (AnyCubeScoring())
                return true;

            // Check if both cube has been removed from the first fall
            if (mCubes.Count == 0)
                return true;

            if (mScoringTimes > 0)
                return true;

            return false;
        }
    }

    void Start ()
    {
        mCubes.Add(transform.GetChild(0).GetComponent<Cube>());
        mCubes[0].GridPos = GridManager.Instance.StartPosition;
        mCubes[0].name = "RootCube";

        mCubes.Add(transform.GetChild(1).GetComponent<Cube>());
        mCubes[1].name = "SubCube";
        
        // this is for creating the first block when the game start
        if(mCubeNumbers.Count == 0)
        {
            mCubes[0].Init(this, SupportTools.RNG(0, 8));
            mCubes[1].Init(this, SupportTools.RNG(0, 8));
        }
        else
        {
            mCubes[0].Init(this, mCubeNumbers[0]);
            mCubes[1].Init(this, mCubeNumbers[1]);
        }

        mCurrentClockDirection = ClockDirection.CLOCK_12;

        Joint = transform.GetChild(2);
        Limb = Joint.GetChild(0);

        ClockPos(0);
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.G))
        //    mCubes[0].PlayAnimation();
        //if (Input.GetKey(KeyCode.H))
        //    mCubes[1].PlayAnimation();

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetCubeNumbers(List<int> someNumbers)
    {
        foreach (int n in someNumbers)
            mCubeNumbers.Add(n);
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
        ClockPos(1);
    }

    public void TurnCounterClockWise()
    {
        ClockPos(-1);
    }

    public void Landing()
    {
        foreach(Cube c in mCubes)
        {
            GridManager.Instance.PutInCube(c);
        }
    }

    /// <summary>
    /// Check
    /// </summary>
    public void Scoring()
    {
        foreach (Cube c in mCubes)
            GridManager.Instance.CubeScoring(c.GridPos);
    }

    public void Swap()
    {
        int rootNum = RootCube.Number;
        int subNum = SubCube.Number;

        mCubes[0].SetCubeNumber(subNum);
        mCubes[1].SetCubeNumber(rootNum);

    }

    /// <summary>
    /// After had check if the block had scored it'll need to remove the scoring cube and rearrange it't min and max position
    /// and reset the remaining cube for next scoring purpose.
    /// </summary>
    public void AfterScoreChange()
    {
        if(Joint != null)
            Destroy(Joint.gameObject);

        if (mCubes.Count == 2)
        {
            if (RootCube.IsScoring && !SubCube.IsScoring)
            {
                mMinPosition = SubCube.GridPos;
                mMaxPosition = SubCube.GridPos;

                transform.position = new Vector3(SubCube.GridPos.x * GridManager.Instance.CubeGap, SubCube.GridPos.y * GridManager.Instance.CubeGap, 0f);
                SubCube.transform.position = RootCube.transform.position;

                GridManager.Instance.NullifyGridWithCubeAt(RootCube.GridPos);
                Destroy(RootCube.gameObject);
                mCubes.Remove(RootCube);
            }
            else if (!RootCube.IsScoring && SubCube.IsScoring)
            {
                mMinPosition = RootCube.GridPos;
                mMaxPosition = RootCube.GridPos;

                GridManager.Instance.NullifyGridWithCubeAt(SubCube.GridPos);
                Destroy(SubCube.gameObject);
                mCubes.Remove(SubCube);
            }
            else if (RootCube.IsScoring && SubCube.IsScoring)
            {
                GridManager.Instance.NullifyGridWithCubeAt(RootCube.GridPos);
                GridManager.Instance.NullifyGridWithCubeAt(SubCube.GridPos);
                Destroy(SubCube.gameObject);
                Destroy(RootCube.gameObject);
                mCubes.Clear();
            }
        }
        else
        {
            GridManager.Instance.NullifyGridWithCubeAt(mCubes[0].GridPos);
            Destroy(mCubes[0].gameObject);
            mCubes.Clear();
        }

        mScoringTimes = 0;
        if(mCubes.Count != 0)
        {
            foreach (Cube c in mCubes)
                c.IsScoring = false;
        }
    }

    private void Move(Vector3 aDir)
    {
        transform.Translate(aDir * GridManager.Instance.CubeGap);
        Vector2Int dir = new Vector2Int((int)aDir.x, (int)aDir.y);
        mCubes[0].GridPos += dir;
        if(mCubes.Count > 1)
            mCubes[1].GridPos += dir;

        mMinPosition += dir;
        mMaxPosition += dir;
    }

    /// <summary>
    /// Rearrange the subcube's in the block and change the min and max
    /// grid position according to the block's current rotation
    /// </summary>
    /// <param name="aDir"></param>
    private void ClockPos(int aDir)
    {
        Joint.Rotate(Vector3.back, aDir * 90);
        mCurrentClockDirection += aDir;

        if (mCurrentClockDirection > ClockDirection.CLOCK_9)
            mCurrentClockDirection = ClockDirection.CLOCK_12;
        else if (mCurrentClockDirection < ClockDirection.CLOCK_12)
            mCurrentClockDirection = ClockDirection.CLOCK_9;

        //foreach (Cube c in mCubes)
        //    c.RotateCube(aDir);

        switch (mCurrentClockDirection)
        {
            case ClockDirection.CLOCK_12:
                SubCubPosition(Vector2Int.up);
                mMinPosition = RootCube.GridPos;
                mMaxPosition = SubCube.GridPos;
                break;
            case ClockDirection.CLOCK_3:
                SubCubPosition(Vector2Int.right);
                mMinPosition = RootCube.GridPos;
                mMaxPosition = SubCube.GridPos;
                break;
            case ClockDirection.CLOCK_6:
                SubCubPosition(Vector2Int.down);
                mMinPosition = SubCube.GridPos;
                mMaxPosition = RootCube.GridPos;
                break;
            case ClockDirection.CLOCK_9:
                SubCubPosition(Vector2Int.left);
                mMinPosition = SubCube.GridPos;
                mMaxPosition = RootCube.GridPos;
                break;
        }
    }

    /// <summary>
    /// Set the subcubes grid position according to the direction
    /// </summary>
    /// <param name="aDir"></param>
    private void SubCubPosition(Vector2Int aDir)
    {
        mCubes[1].transform.position = mCubes[0].transform.position + new Vector3(aDir.x * GridManager.Instance.CubeGap, aDir.y * GridManager.Instance.CubeGap, 0f);
        mCubes[1].GridPos = mCubes[0].GridPos + aDir;
    }

    private bool AnyCubeScoring()
    {
        foreach(Cube c in mCubes)
        {
            if (c.IsScoring)
                return true;
        }
        return false;
    }

    private void DestroyScoringCube(Cube aCube)
    {
        GridManager.Instance.NullifyGridWithCubeAt(aCube.GridPos);
        Destroy(aCube.gameObject);
        
        if (mCubes.Count == 0)
            mCubes.Clear();
        else
            mCubes.Remove(aCube);
    }
}
