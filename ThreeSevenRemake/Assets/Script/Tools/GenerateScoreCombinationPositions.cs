using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// These pos index is for when checking with the block with one cube
/// </summary>
public enum ScorePosWithCubeIndex
{
    U1_Ze,
    Ze_D1,
    Ze_R1,
    L1_Ze,

    U2_U1_Ze,
    U1_Ze_D1,
    Ze_D1_D2,
    Ze_R1_R2,
    L1_Ze_R1,
    L2_L1_Ze,

    U3_U2_U1_Ze,
    U2_U1_Ze_D1,
    U1_Ze_D1_D2,
    Ze_D1_D2_D3,
    Ze_R1_R2_R3,
    L1_Ze_R1_R2,
    L2_L1_Ze_R1,
    L3_L2_L1_Ze,

    U4_U3_U2_U1_Ze,
    U3_U2_U1_Ze_D1,
    U2_U1_Ze_D1_D2,
    U1_Ze_D1_D2_D3,
    Ze_D1_D2_D3_D4,
    Ze_R1_R2_R3_R4,
    L1_Ze_R1_R2_R3,
    L2_L1_Ze_R1_R2,
    L3_L2_L1_Ze_R1,
    L4_L3_L2_L1_Ze,
}

/// <summary>
/// These pos index is for when checking with the original or floating block with two cubes
/// </summary>
public enum ScorePosWithBlockIndex
{
    // 3 Links
    Link_3_VERT_MI_MA_U1,
    Link_3_VERT_D1_MI_MA,
    Link_3_HORI_MI_MA_R1,
    Link_3_HORI_L1_MI_MA,

    // 4 Links
    LINK_4_VERT_MI_MA_U1_U2,
    LINK_4_VERT_D1_MI_MA_U1,
    LINK_4_VERT_D2_D1_MI_MA,
    LINK_4_HORI_MI_MA_R1_R2,
    LINK_4_HORI_L1_MI_MA_R1,
    LINK_4_HORI_L2_L1_MI_MA,

    // 5 Links
    LINK_5_VERT_MI_MA_U1_U2_U3,
    LINK_5_VERT_D1_MI_MA_U1_U2,
    LINK_5_VERT_D2_D1_MI_MA_U1,
    LINK_5_VERT_D3_D2_D1_MI_MA,
    LINK_5_HORI_MI_MA_R1_R2_R3,
    LINK_5_HORI_L1_MI_MA_R1_R2,
    LINK_5_HORI_L2_L1_MI_MA_R1,
    LINK_5_HORI_L3_L2_L1_MI_MA,
}

public class GenerateScoreCombinationPositions
{
    public static GenerateScoreCombinationPositions Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GenerateScoreCombinationPositions();
            }
            return mInstance;
        }
    }
    private static GenerateScoreCombinationPositions mInstance;

    private enum Axis
    {
        HORI,
        VERT,
        NONE
    }
    private Dictionary<ScorePosWithCubeIndex, List<Vector2Int>> mOldScorePosAddons = new Dictionary<ScorePosWithCubeIndex, List<Vector2Int>>();
    private Dictionary<ScorePosWithCubeIndex, List<Vector2Int>> mScorePosWithCubeAddons = new Dictionary<ScorePosWithCubeIndex, List<Vector2Int>>();
    //private Dictionary<ScorePosWithBlockIndex, List<Vector2Int>> mScorePosWithBlockAddons = new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>();
    private Dictionary<Axis, Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>> mScorePosWithBlockAddons = new Dictionary<Axis, Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>>()
    {
        { Axis.HORI, new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>() },
        { Axis.VERT, new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>() },
    };

    public GenerateScoreCombinationPositions()
    {
    }

    public void GenerateCombinationPositions()
    {
        mOldScorePosAddons.Clear();
        mScorePosWithCubeAddons.Clear();
        
        foreach(Axis i in mScorePosWithBlockAddons.Keys)
        {
            mScorePosWithBlockAddons[i].Clear();
        }


        if (GameSettings.Instance.EnableScoringMethods[(int)LinkIndexes.LINK_2_DIGIT])
        {
            {
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U1_Ze, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_D1, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_R1, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L1_Ze, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero });
            }
            {
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U1_Ze, new List<Vector2Int> { Vector2Int.up });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_D1, new List<Vector2Int> { Vector2Int.down });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_R1, new List<Vector2Int> { Vector2Int.right });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L1_Ze, new List<Vector2Int> { Vector2Int.left });
            }
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)LinkIndexes.LINK_3_DIGIT])
        {
            {
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });
            }
            {
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1, new List<Vector2Int> { Vector2Int.up, Vector2Int.down });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2, new List<Vector2Int> { Vector2Int.down, Vector2Int.down * 2 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2, new List<Vector2Int> { Vector2Int.right, Vector2Int.right * 2 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1, new List<Vector2Int> { Vector2Int.left, Vector2Int.right });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left });
            }
            {
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.Link_3_HORI_MI_MA_R1, new List<Vector2Int> { Vector2Int.right * 1 });
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.Link_3_HORI_L1_MI_MA, new List<Vector2Int> { Vector2Int.left * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.Link_3_VERT_D1_MI_MA, new List<Vector2Int> { Vector2Int.down * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.Link_3_VERT_MI_MA_U1, new List<Vector2Int> { Vector2Int.up * 1 });
            }
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)LinkIndexes.LINK_4_DIGIT])
        {
            {
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U3_U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze_D1, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero, Vector2Int.down });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1_D2, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2_D3, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2_R3, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1_R2, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze_R1, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero, Vector2Int.right });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L3_L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });
            }
            {
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U3_U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze_D1, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.down });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1_D2, new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.down * 2 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2_D3, new List<Vector2Int> { Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2_R3, new List<Vector2Int> { Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1_R2, new List<Vector2Int> { Vector2Int.left, Vector2Int.right, Vector2Int.right * 2 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze_R1, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.right });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L3_L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left });
            }
            {
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.LINK_4_HORI_MI_MA_R1_R2, new List<Vector2Int> { Vector2Int.right * 1, Vector2Int.right * 2 });
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.LINK_4_HORI_L1_MI_MA_R1, new List<Vector2Int> { Vector2Int.left * 1, Vector2Int.right * 1 });
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.LINK_4_HORI_L2_L1_MI_MA, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.LINK_4_VERT_D2_D1_MI_MA, new List<Vector2Int> { Vector2Int.down * 2, Vector2Int.down * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.LINK_4_VERT_D1_MI_MA_U1, new List<Vector2Int> { Vector2Int.down * 1, Vector2Int.up * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.LINK_4_VERT_MI_MA_U1_U2, new List<Vector2Int> { Vector2Int.up * 1, Vector2Int.up * 2 });
            }
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)LinkIndexes.LINK_5_DIGIT])
        {
            {
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U4_U3_U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 4, Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U3_U2_U1_Ze_D1, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero, Vector2Int.down });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze_D1_D2, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1_D2_D3, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2_D3_D4, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3, Vector2Int.down * 4 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2_R3_R4, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3, Vector2Int.right * 4 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1_R2_R3, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze_R1_R2, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L3_L2_L1_Ze_R1, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero, Vector2Int.right });
                mOldScorePosAddons.Add(ScorePosWithCubeIndex.L4_L3_L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 4, Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });
            }
            {
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U4_U3_U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 4, Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U3_U2_U1_Ze_D1, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.down });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze_D1_D2, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.down, Vector2Int.down * 2 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1_D2_D3, new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2_D3_D4, new List<Vector2Int> { Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3, Vector2Int.down * 4 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2_R3_R4, new List<Vector2Int> { Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3, Vector2Int.right * 4 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1_R2_R3, new List<Vector2Int> { Vector2Int.left, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze_R1_R2, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.right, Vector2Int.right * 2 });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L3_L2_L1_Ze_R1, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.right });
                mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L4_L3_L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 4, Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left });
            }
            {
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.LINK_5_HORI_MI_MA_R1_R2_R3, new List<Vector2Int> { Vector2Int.right * 1, Vector2Int.right * 2, Vector2Int.right * 3 });
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.LINK_5_HORI_L1_MI_MA_R1_R2, new List<Vector2Int> { Vector2Int.left * 1, Vector2Int.right * 1, Vector2Int.right * 2 });
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.LINK_5_HORI_L2_L1_MI_MA_R1, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left * 1, Vector2Int.right * 1 });
                mScorePosWithBlockAddons[Axis.HORI].Add(ScorePosWithBlockIndex.LINK_5_HORI_L3_L2_L1_MI_MA, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.LINK_5_VERT_D3_D2_D1_MI_MA, new List<Vector2Int> { Vector2Int.down * 3, Vector2Int.down * 2, Vector2Int.down * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.LINK_5_VERT_D2_D1_MI_MA_U1, new List<Vector2Int> { Vector2Int.down * 2, Vector2Int.down * 1, Vector2Int.up * 1 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.LINK_5_VERT_D1_MI_MA_U1_U2, new List<Vector2Int> { Vector2Int.down * 1, Vector2Int.up * 1, Vector2Int.up * 2 });
                mScorePosWithBlockAddons[Axis.VERT].Add(ScorePosWithBlockIndex.LINK_5_VERT_MI_MA_U1_U2_U3, new List<Vector2Int> { Vector2Int.up * 1, Vector2Int.up * 2, Vector2Int.up * 3 });
            }
        }
    }
    
    public List<List<Vector2Int>> GetScorePositionListForBlock(Block aBlock)
    {
        if (aBlock.Cubes.Count == 1)
            return new List<List<Vector2Int>>();

        List<List<Vector2Int>> positionList = new List<List<Vector2Int>>();
        Axis axis = ((aBlock.BlockRotation == 0 || aBlock.BlockRotation == 180) ? Axis.VERT : Axis.HORI);

        foreach (ScorePosWithBlockIndex index in mScorePosWithBlockAddons[axis].Keys)
        {
            positionList.Add(AddPositionToListForBlock(aBlock, axis, index));
        }
        return positionList;
    }

    public List<List<Vector2Int>> GetScorePositionListForCube(Cube aCube)
    {
        if (aCube == null)
            return new List<List<Vector2Int>>();

        List<List<Vector2Int>> positionList = new List<List<Vector2Int>>();
        foreach (ScorePosWithCubeIndex index in mScorePosWithCubeAddons.Keys)
        {
            positionList.Add(AddPositionToListForCube(aCube, index));
            if (!positionList[positionList.Count - 1].Any())
                positionList.RemoveAt(positionList.Count - 1);
        }
        return positionList;
    }

    public List<List<Vector2Int>> GetScorePositionListFrom(Vector2Int aStartPosition)
    {
        List<List<Vector2Int>> positionList = new List<List<Vector2Int>>();
        
        foreach(ScorePosWithCubeIndex index in mOldScorePosAddons.Keys)
        {
            positionList.Add(AddPositionToList(index, aStartPosition));
        }

        return positionList;
    }

    private List<Vector2Int> AddPositionToList(ScorePosWithCubeIndex index, Vector2Int aStartPosition)
    {
        List<Vector2Int> posList = new List<Vector2Int>();

        for (int i = 0; i < mOldScorePosAddons[index].Count; i++)
            posList.Add(aStartPosition + mOldScorePosAddons[index][i]);

        return posList;
    }

    private List<Vector2Int> AddPositionToListForBlock(Block aBlock, Axis anAxis, ScorePosWithBlockIndex index)
    {
        List<Vector2Int> posList = new List<Vector2Int>();
        List<Vector2Int> comboPos = mScorePosWithBlockAddons[anAxis][index];
        for (int i = 0; i < comboPos.Count; i++)
        {
            if(comboPos[i].x < 0 || comboPos[i].y < 0)
            {
                posList.Add(aBlock.MinGridPos + comboPos[i]);
            }
            else
            {
                posList.Add(aBlock.MaxGridPos + comboPos[i]);
            }
        }
        return posList;
    }

    private List<Vector2Int> AddPositionToListForCube(Cube aCube, ScorePosWithCubeIndex index)
    {
        List<Vector2Int> posList = new List<Vector2Int>();
        Vector2Int tempVector = new Vector2Int();
        for (int i = 0; i < mScorePosWithCubeAddons[index].Count; i++)
        {
            tempVector = aCube.GridPos + mScorePosWithCubeAddons[index][i];
            if (aCube.IsSiblingsPosition(tempVector))
                return new List<Vector2Int>();
            posList.Add(tempVector);
        }
        return posList;
    }
}
