using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ComboBaseScoreFactory
{
    public static ComboBaseScoreFactory Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new ComboBaseScoreFactory();
            return mInstance;
        }
    }
    private static ComboBaseScoreFactory mInstance;

    private List<uint> mComboBaseScoreList = new List<uint>() { 50 };

    /// <summary>
    /// This method is called when cubes scores throught aligning and met up the condition
    /// The function is to get the correct combo score accordingly by the number of combos the new landed
    /// cubes had invoke
    /// This is based on Get funciotn from the Factory pattern by take and seek for the requestedindex in 
    /// the list.
    /// But if the requesting index is out of bound of the list current limit, the list will be upgraded 
    /// through the Create method
    /// </summary>
    /// <param name="aCombo">Requested index</param>
    /// <returns>return the combo score base on the requested index</returns>
    public int GetComboBaseScore(int aCombo)
    {
        if (aCombo == 0)
            return 0;

        if (aCombo >= mComboBaseScoreList.Count)
            CreateNewBaseScore(aCombo);

        return (int)mComboBaseScoreList[aCombo - 1];
    }

    /// <summary>
    /// This method is use to upgrade the list of combo base scores.
    /// By sending in a requested combo limit value, the list will go through a iteration of adding new
    /// base score into it.
    /// </summary>
    /// <param name="aNewComboLimit">the new high combo that the player had made</param>
    private void CreateNewBaseScore(int aNewComboLimit)
    {
        do
        {
            int lastIndex = mComboBaseScoreList.Count - 1;
            uint nextBaseScore = mComboBaseScoreList[lastIndex] * ((uint)lastIndex + 1);
            mComboBaseScoreList.Add(nextBaseScore);

        } while (mComboBaseScoreList.Count <= aNewComboLimit);
    }
}
