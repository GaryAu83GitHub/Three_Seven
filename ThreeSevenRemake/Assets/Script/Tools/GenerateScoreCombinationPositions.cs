using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public enum ScorePosIndex
{
    Z_R,
    L_Z,
    U_Z,
    Z_D,

    Z_R_R2,
    L_Z_R,
    L2_L_Z,
    U2_U_Z,
    U_Z_D,
    Z_D_D2,

    Z_R_R2_R3,
    L_Z_R_R2,
    L2_L_Z_R,
    L3_L2_L_Z,
    U3_U2_U_Z,
    U2_U_Z_D,
    U_Z_D_D2,
    Z_D_D2_D3,

    Z_R_R2_R3_R4,
    L_Z_R_R2_R3,
    L2_L_Z_R_R2,
    L3_L2_L_Z_R,
    L4_L3_L2_L_Z,
    U4_U3_U2_U_Z,
    U3_U2_U_Z_D,
    U2_U_Z_D_D2,
    U_Z_D_D2_D3,
    Z_D_D2_D3_D4,
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

    private Dictionary<ScorePosIndex, List<Vector2Int>> mScoreCombinationPositionAddons = new Dictionary<ScorePosIndex, List<Vector2Int>>();

    public GenerateScoreCombinationPositions()
    {
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT])
        {
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_R, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L_Z, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U_Z, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_D, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down });
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT])
        {
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_R_R2, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L_Z_R, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L2_L_Z, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U2_U_Z, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U_Z_D, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_D_D2, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT])
        {
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_R_R2_R3, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L_Z_R_R2, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L2_L_Z_R, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero, Vector2Int.right });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L3_L2_L_Z, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U3_U2_U_Z, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U2_U_Z_D, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero, Vector2Int.down });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U_Z_D_D2, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_D_D2_D3, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT])
        {
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_R_R2_R3_R4, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3, Vector2Int.right * 4 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L_Z_R_R2_R3, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L2_L_Z_R_R2, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L3_L2_L_Z_R, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero, Vector2Int.right });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.L4_L3_L2_L_Z, new List<Vector2Int> { Vector2Int.left * 4, Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U4_U3_U2_U_Z, new List<Vector2Int> { Vector2Int.up * 4, Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U3_U2_U_Z_D, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero, Vector2Int.down });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U2_U_Z_D_D2, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.U_Z_D_D2_D3, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
            mScoreCombinationPositionAddons.Add(ScorePosIndex.Z_D_D2_D3_D4, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3, Vector2Int.down * 4 });
        }
    }

    public List<List<Vector2Int>> GetScorePositionListFrom(Vector2Int aStartPosition)
    {
        List<List<Vector2Int>> positionList = new List<List<Vector2Int>>();
        
        foreach(ScorePosIndex index in mScoreCombinationPositionAddons.Keys)
        {
            positionList.Add(AddPositionToList(index, aStartPosition));
        }

        return positionList;
    }

    private List<Vector2Int> AddPositionToList(ScorePosIndex index, Vector2Int aStartPosition)
    {
        List<Vector2Int> posList = new List<Vector2Int>();

        for (int i = 0; i < mScoreCombinationPositionAddons[index].Count; i++)
            posList.Add(aStartPosition + mScoreCombinationPositionAddons[index][i]);

        return posList;
    }
}
