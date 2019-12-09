using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TurnData
{
    public readonly List<BlockData> Blocks = new List<BlockData>();

    public readonly BlockData ThisTurnFallingBlock;

    public readonly List<int> ThisTurnNextBlock = new List<int>();

    public TurnData(Block thisTurnsBlock)
    {
        foreach(Block b in BlockManager.Instance.Blocks)
        {
            Blocks.Add(new BlockData(b));
        }

        ThisTurnFallingBlock = new BlockData(thisTurnsBlock);

        ThisTurnNextBlock = new List<int>(GamingManager.Instance.NextCubeNumbers);
    }
}
