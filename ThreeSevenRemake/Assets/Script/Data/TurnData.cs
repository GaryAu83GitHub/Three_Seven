using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TurnData
{
    private readonly Dictionary<int, List<Cube>> Grid = new Dictionary<int, List<Cube>>();

    private readonly Block ThisTurnFallingBlock = new Block();

    private readonly List<int> ThisTurnNextBlock = new List<int>();

    public TurnData(Block thisTurnsBlock)
    {
        Grid = new Dictionary<int, List<Cube>>(GridData.Instance.Grid);
        ThisTurnFallingBlock = thisTurnsBlock;

        //ThisTurnNextBlock = new List<int>()
    }
}
