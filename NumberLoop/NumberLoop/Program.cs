using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberLoop
{
    class Program
    {
        static Dictionary<int, List<string>> AllCubeList = new Dictionary<int, List<string>>();

        static void AddCombination(int aKey, string aString)
        {
            if (!AllCubeList.ContainsKey(aKey))
                AllCubeList.Add(aKey, new List<string>());

            if (!AllCubeList[aKey].Contains(aString))
                AllCubeList[aKey].Add(aString);
        }

        static IEnumerable<IEnumerable<T>> GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutationsWithRept(list, length - 1)
                .SelectMany(t => list,
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        static int RecrusiveSum(int anIndex, int aSumSoFar, List<int> someValue)
        {   
            int currentSum = aSumSoFar + someValue[anIndex];
            int nextIndex = anIndex + 1;

            if (nextIndex >= someValue.Count)
                return currentSum;

            return RecrusiveSum(nextIndex, currentSum, someValue);
        }

        static string RecrusiveEquationString(int anIndex, string anEquationText, List<int> someValue)
        {
            int nextIndex = anIndex + 1;
            if (nextIndex >= someValue.Count)
                return anEquationText;

            string equationString = anEquationText + " + " + someValue[nextIndex].ToString();

            return RecrusiveEquationString(nextIndex, equationString, someValue);
        }

        static bool BothListAreMatched(List<List<int>> anOrgiginalList, List<int> testingList)
        {
            for(int i = 0; i < anOrgiginalList.Count; i++)
            {
                if (anOrgiginalList[i].SequenceEqual(testingList))
                    return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            Dictionary<int, List<string>> TwoCubeList = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> ThreeCubeList = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> FourCubeList = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> FiveCubeList = new Dictionary<int, List<string>>();

            IEnumerable<IEnumerable<int>> result = GetPermutationsWithRept(Enumerable.Range(0, 10), 3);//GetPermutationsWithRept(Enumerable.Range(0, 10), 2).Concat(GetPermutationsWithRept(Enumerable.Range(0, 10), 3));.Concat(GetPermutationsWithRept(Enumerable.Range(0, 10), 4)).Concat(GetPermutationsWithRept(Enumerable.Range(0, 10), 5));

            List<List<int>> lists = result.Select(i => i.ToList()).ToList();
            Dictionary<int, List<List<int>>> summary = new Dictionary<int, List<List<int>>>();

            for(int i = 0; i < lists.Count; i++)
            {
                lists[i].Sort();
                int sumKey = RecrusiveSum(0, 0, lists[i]);
                if (!summary.ContainsKey(sumKey))
                    summary.Add(sumKey, new List<List<int>>());

                if(!BothListAreMatched(summary[sumKey], lists[i]))
                    summary[sumKey].Add(lists[i]);
            }

            Console.WriteLine("Press a button");
            Console.ReadKey();

            string text = "\t";
            for (int i = 0; i < 10; i++)
                text += i.ToString() + "\t";
            Console.WriteLine(text);
            foreach (int key in summary.Keys)
            {
                

                int[] numberCount = new int[10];
                for (int i = 0; i < summary[key].Count; i++)
                {
                    for (int j = 0; j < summary[key][i].Count; j++)
                    {
                        int index = summary[key][i][j];
                        numberCount[index]++;
                    }
                }

                text = key.ToString() + "(" + (summary[key].Count).ToString() + "):\t";
                for (int i = 0; i < numberCount.Length; i++)
                    text += numberCount[i].ToString() + "\t";
                text += "\n";

                //text = key.ToString() + "(" + (summary[key].Count).ToString() + "):\t";
                //for (int i = 0; i < summary[key].Count; i++)
                //    text += "[" + RecrusiveEquationString(0, summary[key][i][0].ToString(), summary[key][i]) + "] ";
                //text += "\n";
                Console.WriteLine(text);
            }
            //for (int i = 0; i < 10; i++)
            //    text += i.ToString() + "\t";
            //Console.WriteLine(text+"\n");

            //foreach (int key in summary.Keys)
            //{
            //    int[] numberCount = new int[10];
            //    for(int i = 0; i < summary[key].Count; i++)
            //    {
            //        for (int j = 0; j < summary[key][i].Count; j++)
            //        {
            //            int index = summary[key][i][j];
            //            numberCount[index]++;
            //        }
            //    }

            //    text = key.ToString() + "(" + (summary[key].Count).ToString() + "):\t";
            //    for (int i = 0; i < numberCount.Length; i++)
            //        text += numberCount[i].ToString() + "\t";
            //    Console.WriteLine(text);
            //}
            //for(int i = 0; i < 22; i++)
            //{
            //    string addition = i.ToString() + "(" + (ThreeCubeList[i].Count).ToString() + "):\t";
            //    for (int t = 0; t < ThreeCubeList[i].Count; t++)
            //    {
            //        addition += ThreeCubeList[i][t] + "\t";
            //    }
            //    Console.WriteLine(addition);
            //}

            //Console.WriteLine("2 Cubes");
            //foreach (int key in TwoCubeList.Keys)
            //{
            //    string addition = key.ToString() + "(" + (TwoCubeList[key].Count).ToString() + "):\t";
            //    //for (int i = 0; i < TwoCubeList[key].Count; i++)
            //    //{
            //    //    addition += TwoCubeList[key][i] + "\t";
            //    //}
            //    Console.WriteLine(addition);
            //}

            //Console.WriteLine("\n3 Cubes");
            //foreach (int key in ThreeCubeList.Keys)
            //{
            //    string addition = key.ToString() + "(" + (ThreeCubeList[key].Count).ToString() + "):\t";
            //    //for (int i = 0; i < ThreeCubeList[key].Count; i++)
            //    //{
            //    //    addition += ThreeCubeList[key][i] + "\t";
            //    //}
            //    Console.WriteLine(addition);
            //}

            //Console.WriteLine("\n4 Cubes");
            //foreach (int key in FourCubeList.Keys)
            //{
            //    string addition = key.ToString() + "(" + (FourCubeList[key].Count).ToString() + "):\t";
            //    //for (int i = 0; i < FourCubeList[key].Count; i++)
            //    //{
            //    //    addition += FourCubeList[key][i] + "\t";
            //    //}
            //    Console.WriteLine(addition);
            //}

            //Console.WriteLine("\n5 Cubes");
            //foreach (int key in FiveCubeList.Keys)
            //{
            //    string addition = key.ToString() + "(" + (FiveCubeList[key].Count).ToString() + "):\t";
            //    //for (int i = 0; i < FiveCubeList[key].Count; i++)
            //    //{
            //    //    addition += FiveCubeList[key][i] + "\t";
            //    //}
            //    Console.WriteLine(addition);
            //}

            //Console.WriteLine("\nAll Cubes");
            //foreach (int key in AllCubeList.Keys)
            //{
            //    string addition = key.ToString() + "(" + (AllCubeList[key].Count).ToString() + "):\t";
            //    //for (int i = 0; i < FiveCubeList[key].Count; i++)
            //    //{
            //    //    addition += FiveCubeList[key][i] + "\t";
            //    //}
            //    Console.WriteLine(addition);
            //}

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
