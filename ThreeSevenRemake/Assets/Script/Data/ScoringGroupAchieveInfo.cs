using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ScoringType
{
    BLOCK_SCORING,
    CUBE_SCORING,
    NONE_SCORING
}

public class ScoringGroupAchieveInfo
{   
    private readonly TaskRank mTaskRank = TaskRank.X1;
    public TaskRank TaskRank { get { return mTaskRank; } }

    private readonly int mSlotIndex = -1;
    public int TaskSlotIndex { get { return mSlotIndex; } }

    private readonly List<Vector2Int> mGroupPositions = new List<Vector2Int>();
    public List<Vector2Int> GroupPosition { get { return mGroupPositions; } }

    private readonly Block mBlock = null;
    public Block Block { get { return mBlock; } }

    private readonly Cube mCube = null;
    public Cube Cube { get { return mCube; } }

    private readonly ScoringType mScoringType = ScoringType.NONE_SCORING;
    public ScoringType ScoringType { get { return mScoringType; } }

    public ScoringGroupAchieveInfo(TaskRank anObjectiveRank, List<Vector2Int> someGroupPositions)
    {
        mTaskRank = anObjectiveRank;
        mGroupPositions = someGroupPositions;
    }

    public ScoringGroupAchieveInfo(TaskRank aTaskRank, int aSlotIndex, Block aBlock, List<Vector2Int> someGroupPosition)
    {
        mScoringType = ScoringType.BLOCK_SCORING;
        mSlotIndex = aSlotIndex;
        mTaskRank = aTaskRank;
        mBlock = aBlock;
        mGroupPositions = someGroupPosition;
    }

    public ScoringGroupAchieveInfo(TaskRank aTaskRank, int aSlotIndex, Cube aCube, List<Vector2Int> someGroupPosition)
    {
        mScoringType = ScoringType.CUBE_SCORING;
        mSlotIndex = aSlotIndex;
        mTaskRank = aTaskRank;
        mCube = aCube;
        mGroupPositions = someGroupPosition;
    }
}
