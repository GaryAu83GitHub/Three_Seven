using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TurnData
{
    public readonly List<Block> Blocks = new List<Block>();

    public readonly Block ThisTurnFallingBlock;

    public readonly List<int> ThisTurnNextBlock = new List<int>();

    public TurnData(Block thisTurnsBlock)
    {
        Blocks = new List<Block>(BlockManager.Instance.Blocks);
        ThisTurnFallingBlock = thisTurnsBlock;

        ThisTurnNextBlock = new List<int>(GameManager.Instance.NextCubeNumbers);
    }
}
