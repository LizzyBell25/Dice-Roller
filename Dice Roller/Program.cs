using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace Dice_Roller
{
    public class Roller
    {
        static void Main(string[] args)
        {
            var LowestNumber = 0;
            var HighestNumber = 10000;
            var ListList = new List<List<int>>() { new() };
            NewNumber(ListList[0], LowestNumber, HighestNumber);

            PrintLists(ListList);

            var start = DateTime.Now;
            var CL = 0;
            ulong Checks = 0, CreatedLists = 1;
            while (true)
            {
                var HighestValue = LowestNumber - 1;
                var repeat = false;

                for (int i = 0; i < ListList[CL].Count; i++)
                {
                    if (HighestValue <= ListList[CL][i])
                    {
                        HighestValue = ListList[CL][i];
                        AddList(ListList, CL, ref CreatedLists, 1);
                        ListList[CL + 1].Add(ListList[CL][i]);
                    }
                    else
                    {
                        AddList(ListList, CL, ref CreatedLists, 1);
                        AddList(ListList, CL, ref CreatedLists, 2);
                        ListList[CL + 2].Add(ListList[CL][i]);
                        repeat = true;
                    }
                    Checks++;
                }

                ListList.Remove(ListList[CL]);
                CL++;

                Console.WriteLine(
                    "List Count : " + ListList.Count + 
                    " | Checks : " + Checks +
                    " | Total Lists : " + CreatedLists);

                if (!repeat)
                {
                    if (ListList.Count == 1)
                        break;

                    CL = 0;
                    var NewUnsorted = new List<int>();

                    for (int i = ListList.Count - 1; i >= 0; i--)
                    {
                        NewUnsorted.AddRange(ListList[i]);
                        ListList.Remove(ListList[i]);
                    }

                    ListList.Add(NewUnsorted);
                }
            }

            Console.WriteLine("Sorted List:");
            PrintLists(ListList);

            Console.WriteLine(DateTime.Now - start);
        }

        static void AddList(List<List<int>> ListList, int CL, ref ulong ListCount, int extra = 0)
        {
            if (ListList.Count <= CL + extra)
                ListList.Add([]);
            ListCount++;
        }

        static void NewNumber(List<int> CurrentList, int LowestNumber, int HighestNumber)
        {
            var NewList = new List<int>();
            for (int i = LowestNumber; i < HighestNumber; i++)
                NewList.Add(i);

            var rnd = new Random();
            while (NewList.Count > 0)
            {
                int index = rnd.Next(0, NewList.Count - 1);
                var num = NewList[index];
                CurrentList.Add(num);
                NewList.RemoveAt(index);
            }
        }

        static void PrintLists(List<List<int>> ListList)
        {
            for (int i = 0; i < ListList.Count; i++)
                Console.WriteLine("[{0}]", string.Join(", ", ListList[i]));
            Console.WriteLine("-----");
        }

        static void RunDiceRoller()
        {
            while (true)
            {
                Console.WriteLine("Roll Dice?");

                var input = Console.ReadLine();
                if (input == "F") break;

                RollDice();
            }
        }

        static void RollDice()
        {
            Console.WriteLine("Number of Dice and Type");
            var input = Console.ReadLine();
            if (input == null) input = "";
            input = input.ToUpper();

            string type = "";
            if (CheckType(ref input, "A")) type = "A";
            if (CheckType(ref input, "N")) type = "N";
            if (CheckType(ref input, "D")) type = "D";

            var DiceNumber = int.Parse(input);
            var rnd = new Random();
            int[] A = { -1, -1, 1, 1, 1, 2 };
            int[] N = { -1, -1, -1, 1, 1, 1 };
            int[] D = { -2, -1, -1, -1, 1, 1 };

            var Rolls = new int[DiceNumber];

            for (int i = 0; i < DiceNumber; i++)
            {
                var Roll = rnd.Next(0, 6);
                if (type == "A") Rolls[i] = A[Roll];
                if (type == "N") Rolls[i] = N[Roll];
                if (type == "D") Rolls[i] = D[Roll];
            }
            Array.Sort(Rolls);
            Print(Rolls);
        }

        static bool CheckType(ref string input, string type)
        {
            if (input != null && input.Contains(type))
            {
                input = input.Replace(type, "");
                return true;
            }
            return false;
        }

        static void Print(int[] Roll)
        {
            var Total = 0;
            Dictionary<int,int> Count = new Dictionary<int,int>();
            for (int i = 0; i < Roll.Length; i++)
            {
                Total += Roll[i];
                if(Count.TryGetValue(Roll[i], out int value)) Count[Roll[i]] = ++value;
                else Count.Add(Roll[i], 1);
            }

            var sortedDict = Count.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (KeyValuePair<int, int> pair in sortedDict)
            {
                var key = "" + pair.Key;
                Console.Write("{0} ; {1}  ", pair.Key, pair.Value);
            }

            Console.Write(" : ");
            if (Total > -1) Console.Write(" ");
            Console.WriteLine(Total);
        }
    }
}