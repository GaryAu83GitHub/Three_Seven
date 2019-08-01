using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoringGroupAchieveInfo
{
    private readonly TaskRank mTaskRank = TaskRank.X1;
    public TaskRank TaskRank { get { return mTaskRank; } }

    private readonly List<Vector2Int> mGroupPositions = new List<Vector2Int>();
    public List<Vector2Int> GroupPosition { get { return mGroupPositions; } }

    private readonly Block mBlock = null;
    public Block Block { get { return mBlock; } }

    private readonly Cube mCube = null;
    public Cube Cube { get { return mCube; } }

    public ScoringGroupAchieveInfo(TaskRank anObjectiveRank, List<Vector2Int> someGroupPositions)
    {
        mTaskRank = anObjectiveRank;
        mGroupPositions = someGroupPositions;
    }

    public ScoringGroupAchieveInfo(TaskRank aTaskRank, Block aBlock, List<Vector2Int> someGroupPosition)
    {
        mTaskRank = aTaskRank;
        mBlock = aBlock;
        mGroupPositions = someGroupPosition;
    }

    public ScoringGroupAchieveInfo(TaskRank aTaskRank, Cube aCube, List<Vector2Int> someGroupPosition)
    {
        mTaskRank = aTaskRank;
        mCube = aCube;
        mGroupPositions = someGroupPosition;
    }
}
