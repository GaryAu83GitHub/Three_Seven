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
    ROT_0_U2_Su_Ro,
    ROT_0_Su_Ro_D1,

    ROT_90_Ro_Su_R2,
    ROT_90_L1_Ro_Su,

    ROT_180_U1_Ro_Su,
    ROT_180_Ro_Su_D2,

    ROT_270_Su_Ro_R1,
    ROT_270_L2_Su_Ro,

    // 4 Links
    ROT_0_U3_U2_Su_Ro,
    ROT_0_U2_Su_Ro_D1,
    ROT_0_Su_Ro_D1_2D,

    ROT_90_Ro_Su_R2_R3,
    ROT_90_L1_Ro_Su_R2,
    ROT_90_L2_L1_Ro_Su,

    ROT_180_U2_U1_Ro_Su,
    ROT_180_U1_Ro_Su_D2,
    ROT_180_Ro_Su_D2_D3,

    ROT_270_Su_Ro_R1_R2,
    ROT_270_L2_Su_Ro_R1,
    ROT_270_L3_L2_Su_Ro,

    // 5 Links
    ROT_0_U4_U3_U2_Su_Ro,
    ROT_0_U3_U2_Su_Ro_D1,
    ROT_0_U2_Su_Ro_D1_D2,
    ROT_0_Su_Ro_D1_D2_D3,

    ROT_90_Ro_Su_R2_R3_R4,
    ROT_90_L1_Ro_Su_R2_R3,
    ROT_90_L2_L1_Ro_Su_R2,
    ROT_90_L3_L2_L1_Ro_Su,

    ROT_180_U3_U2_U1_Ro_Su,
    ROT_180_U2_U1_Ro_Su_2D,
    ROT_180_U1_Ro_Su_D2_D3,
    ROT_180_Ro_Su_D2_D3_D4,

    ROT_270_Su_Ro_R1_R2_R3,
    ROT_270_L2_Su_Ro_R1_R2,
    ROT_270_L3_L2_Su_Ro_R1,
    ROT_270_L4_L3_L2_Su_Ro,
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

    private Dictionary<ScorePosWithCubeIndex, List<Vector2Int>> mOldScorePosAddons = new Dictionary<ScorePosWithCubeIndex, List<Vector2Int>>();
    private Dictionary<ScorePosWithCubeIndex, List<Vector2Int>> mScorePosWithCubeAddons = new Dictionary<ScorePosWithCubeIndex, List<Vector2Int>>();
    //private Dictionary<ScorePosWithBlockIndex, List<Vector2Int>> mScorePosWithBlockAddons = new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>();
    private Dictionary<int, Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>> mScorePosWithBlockAddons = new Dictionary<int, Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>>()
    {
        { 0, new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>() },
        { 90, new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>() },
        { 180, new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>() },
        { 270, new Dictionary<ScorePosWithBlockIndex, List<Vector2Int>>() }
    };

    public GenerateScoreCombinationPositions()
    {
    }

    public void GenerateCompinationPositions()
    {
        mOldScorePosAddons.Clear();

        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT])
        {
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.U1_Ze, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_D1, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_R1, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.L1_Ze, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero });

            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U1_Ze, new List<Vector2Int> { Vector2Int.up});
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_D1, new List<Vector2Int> { Vector2Int.down });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_R1, new List<Vector2Int> { Vector2Int.right });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L1_Ze, new List<Vector2Int> { Vector2Int.left });

        }
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT])
        {
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });

            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1, new List<Vector2Int> { Vector2Int.up, Vector2Int.down });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2, new List<Vector2Int> { Vector2Int.down, Vector2Int.down * 2 });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2, new List<Vector2Int> { Vector2Int.right, Vector2Int.right * 2 });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1, new List<Vector2Int> { Vector2Int.left, Vector2Int.right });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left });

            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_U2_Su_Ro, new List<Vector2Int> { Vector2Int.up * 2 });         //ROT_0_U2_Su_Ro,
            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_Su_Ro_D1, new List<Vector2Int> { Vector2Int.down });           //ROT_0_Su_Ro_D1,

            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_Ro_Su_R2, new List<Vector2Int> { Vector2Int.right * 2 });    //ROT_90_Ro_Su_R2,
            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_L1_Ro_Su, new List<Vector2Int> { Vector2Int.left });         //ROT_90_L1_Ro_Su,

            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_U1_Ro_Su, new List<Vector2Int> { Vector2Int.up });         //ROT_180_U1_Ro_Su,
            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_Ro_Su_D2, new List<Vector2Int> { Vector2Int.down * 2 });   //ROT_180_Ro_Su_D2,

            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_Su_Ro_R1, new List<Vector2Int> { Vector2Int.right });      //ROT_270_Su_Ro_R1,
            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_L2_Su_Ro, new List<Vector2Int> { Vector2Int.left * 2 });   //ROT_270_L2_Su_Ro,
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT])
        {
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.U3_U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze_D1, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.zero, Vector2Int.down });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1_D2, new List<Vector2Int> { Vector2Int.up, Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2 });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2_D3, new List<Vector2Int> { Vector2Int.zero, Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2_R3, new List<Vector2Int> { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1_R2, new List<Vector2Int> { Vector2Int.left, Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze_R1, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero, Vector2Int.right });
            mOldScorePosAddons.Add(ScorePosWithCubeIndex.L3_L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left, Vector2Int.zero });

            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U3_U2_U1_Ze, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2, Vector2Int.up });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U2_U1_Ze_D1, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up, Vector2Int.down });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.U1_Ze_D1_D2, new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.down * 2 });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_D1_D2_D3, new List<Vector2Int> { Vector2Int.down, Vector2Int.down * 2, Vector2Int.down * 3 });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.Ze_R1_R2_R3, new List<Vector2Int> { Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L1_Ze_R1_R2, new List<Vector2Int> { Vector2Int.left, Vector2Int.right, Vector2Int.right * 2 });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L2_L1_Ze_R1, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left, Vector2Int.right });
            mScorePosWithCubeAddons.Add(ScorePosWithCubeIndex.L3_L2_L1_Ze, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2, Vector2Int.left });

            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_U3_U2_Su_Ro, new List<Vector2Int> { Vector2Int.up * 3, Vector2Int.up * 2 });           //ROT_0_U3_U2_Su_Ro,
            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_U2_Su_Ro_D1, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.down });             //ROT_0_U2_Su_Ro_D1,
            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_Su_Ro_D1_2D, new List<Vector2Int> { Vector2Int.down, Vector2Int.down * 2 });           //ROT_0_Su_Ro_D1_2D,

            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_Ro_Su_R2_R3, new List<Vector2Int> { Vector2Int.right * 2, Vector2Int.right * 3 });   //ROT_90_Ro_Su_R2_R3,
            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_L1_Ro_Su_R2, new List<Vector2Int> { Vector2Int.left, Vector2Int.right * 2 });        //ROT_90_L1_Ro_Su_R2,
            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_L2_L1_Ro_Su, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.left });         //ROT_90_L2_L1_Ro_Su,

            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_U2_U1_Ro_Su, new List<Vector2Int> { Vector2Int.up * 2, Vector2Int.up });           //ROT_180_U2_U1_Ro_Su,
            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_U1_Ro_Su_D2, new List<Vector2Int> { Vector2Int.up, Vector2Int.down * 2 });         //ROT_180_U1_Ro_Su_D2,
            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_Ro_Su_D2_D3, new List<Vector2Int> { Vector2Int.down * 2, Vector2Int.down * 3 });   //ROT_180_Ro_Su_D2_D3,

            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_Su_Ro_R1_R2, new List<Vector2Int> { Vector2Int.right, Vector2Int.right * 2 });     //ROT_270_Su_Ro_R1_R2,
            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_L2_Su_Ro_R1, new List<Vector2Int> { Vector2Int.left * 2, Vector2Int.right });      //ROT_270_L2_Su_Ro_R1,
            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_L3_L2_Su_Ro, new List<Vector2Int> { Vector2Int.left * 3, Vector2Int.left * 2 });   //ROT_270_L3_L2_Su_Ro,
        }
        if (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT])
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

            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_U4_U3_U2_Su_Ro, new List<Vector2Int> { Vector2Int.up * 4,  Vector2Int.up * 3,      Vector2Int.up * 2 });   //ROT_0_U4_U3_U2_Su_Ro,
            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_U3_U2_Su_Ro_D1, new List<Vector2Int> { Vector2Int.up * 3,  Vector2Int.up * 2,      Vector2Int.down });     //ROT_0_U3_U2_Su_Ro_D1,
            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_U2_Su_Ro_D1_D2, new List<Vector2Int> { Vector2Int.up * 2,  Vector2Int.down,        Vector2Int.down * 2 }); //ROT_0_U2_Su_Ro_D1_2D,
            mScorePosWithBlockAddons[0].Add(ScorePosWithBlockIndex.ROT_0_Su_Ro_D1_D2_D3, new List<Vector2Int> { Vector2Int.down,    Vector2Int.down * 2,    Vector2Int.down * 3 }); //ROT_0_Su_Ro_D1_2D_3D,

            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_Ro_Su_R2_R3_R4, new List<Vector2Int> { Vector2Int.right * 2, Vector2Int.right * 3,   Vector2Int.right * 4 });//ROT_90_Ro_Su_R2_R3_R4,
            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_L1_Ro_Su_R2_R3, new List<Vector2Int> { Vector2Int.left,      Vector2Int.right * 2,   Vector2Int.right * 3 });//ROT_90_L1_Ro_Su_R2_R3,
            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_L2_L1_Ro_Su_R2, new List<Vector2Int> { Vector2Int.left * 2,  Vector2Int.left,        Vector2Int.right * 2 });//ROT_90_L2_L1_Ro_Su_R2,
            mScorePosWithBlockAddons[90].Add(ScorePosWithBlockIndex.ROT_90_L3_L2_L1_Ro_Su, new List<Vector2Int> { Vector2Int.left * 3,  Vector2Int.left * 2,    Vector2Int.left });     //ROT_90_L3_L2_L1_Ro_Su,

            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_U3_U2_U1_Ro_Su, new List<Vector2Int> { Vector2Int.up * 3,      Vector2Int.up * 2,      Vector2Int.up });       //ROT_180_U3_U2_U1_Ro_Su,
            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_U2_U1_Ro_Su_2D, new List<Vector2Int> { Vector2Int.up * 2,      Vector2Int.up,          Vector2Int.down * 2 }); //ROT_180_U2_U1_Ro_Su_2D,
            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_U1_Ro_Su_D2_D3, new List<Vector2Int> { Vector2Int.up,          Vector2Int.down * 2,    Vector2Int.down * 3 }); //ROT_180_U1_Ro_Su_D2_D3,
            mScorePosWithBlockAddons[180].Add(ScorePosWithBlockIndex.ROT_180_Ro_Su_D2_D3_D4, new List<Vector2Int> { Vector2Int.down * 2,    Vector2Int.down * 3,    Vector2Int.down * 4 }); //ROT_180_Ro_Su_D2_D3_D4,

            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_Su_Ro_R1_R2_R3, new List<Vector2Int> { Vector2Int.right,       Vector2Int.right * 2,   Vector2Int.right * 3 });//ROT_270_Su_Ro_R1_R2_R3,
            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_L2_Su_Ro_R1_R2, new List<Vector2Int> { Vector2Int.left * 2,    Vector2Int.right,       Vector2Int.right * 2 });//ROT_270_L2_Su_Ro_R1_R2,
            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_L3_L2_Su_Ro_R1, new List<Vector2Int> { Vector2Int.left * 3,    Vector2Int.left * 2,    Vector2Int.right });//ROT_270_L3_L2_Su_Ro_R1,
            mScorePosWithBlockAddons[270].Add(ScorePosWithBlockIndex.ROT_270_L4_L3_L2_Su_Ro, new List<Vector2Int> { Vector2Int.left * 4,    Vector2Int.left * 3,    Vector2Int.left });//ROT_270_L4_L3_L2_Su_Ro,
        }
    }

    public List<List<Vector2Int>> GetScorePositionListFor(Block aBlock)
    {
        List<List<Vector2Int>> positionList = new List<List<Vector2Int>>();

        return positionList;
    }

    public List<List<Vector2Int>> GetScorePositionListFor(Cube aCube)
    {
        List<List<Vector2Int>> positionList = new List<List<Vector2Int>>();
        foreach (ScorePosWithCubeIndex index in mScorePosWithCubeAddons.Keys)
        {
            positionList.Add(AddPositionToListForCube(aCube, index));
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

    private List<Vector2Int> AddPositionToListForBlock(Block aBlock, ScorePosWithBlockIndex index)
    {
        List<Vector2Int> posList = new List<Vector2Int>();
        int blockRotation = aBlock.BlockRotation;
        List<Vector2Int> comboPos = mScorePosWithBlockAddons[blockRotation][index];
        for (int i = 0; i < comboPos.Count; i++)
        {
            if(blockRotation == 0 || blockRotation == 90)
            {
                if(comboPos[i].x < 0 || comboPos[i].y < 0)
                {
                    posList.Add(aBlock.RootCube.GridPos + comboPos[i]);
                }
                else
                {
                    posList.Add(aBlock.SubCube.GridPos + comboPos[i]);
                }
            }
            else if (blockRotation == 180 || blockRotation == 270)
            {
                if (comboPos[i].x < 0 || comboPos[i].y < 0)
                {
                    posList.Add(aBlock.SubCube.GridPos + comboPos[i]);
                }
                else
                {
                    posList.Add(aBlock.RootCube.GridPos + comboPos[i]);
                }
            }


        }
        return posList;
    }

    private List<Vector2Int> AddPositionToListForCube(Cube aCube, ScorePosWithCubeIndex index)
    {
        List<Vector2Int> posList = new List<Vector2Int>();

        for (int i = 0; i < mOldScorePosAddons[index].Count; i++)
            posList.Add(aCube.GridPos + mOldScorePosAddons[index][i]);

        return posList;
    }
}
