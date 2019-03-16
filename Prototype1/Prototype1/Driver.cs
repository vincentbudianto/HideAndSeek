using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Prototype1
{
    // Planning:
            // 1. DFS dari istana ke posisi curr
            // 2. Simpan di array
            // 3. Cek apakah 0 atau 1
            //    0 mendekati istana, 1 menjauhi istana
            //    posisi jose
            //    posisi ferdian
    
    class Driver
    {
        // 2 buah variabel utama
        private Graf map;
        private ExQuery queries;


  //      // Main program (tester)
  //      static void Main(string[] args)
  //      {
  //          map = new Graph("test100000.txt");
  //          map.print();
  //          Console.WriteLine("Press any key to continue");
  //          Console.ReadKey();
  //
  //          Console.WriteLine("Testing...");
  //          
  //          // Tes 1
  //          bool found = false;
  //          List<int> result = new List<int>();
  //          List<int> further = new List<int>();
  //
  //          recurseSolve(1,6,ref found,map,ref result);
  //          
  //          Console.Write("Path : ");
  //          foreach (int item in result)
  //          {
  //              Console.Write(item + " ");
  //          }
  //          Console.WriteLine();
  //          Console.WriteLine();
  //          
  //          // Tes 2
  //          
  //          ExQuery quests = new ExQuery("test1_1.txt");
  //          quests.print();
  //
  //          recursiveCont(5,map,ref further);
  //          Console.Write("Further : ");
  //
  //          for (int i = 0; i < further.Count; i++)
  //          {
  //              Console.Write(further[i] + " ");
  //          }
  //          Console.WriteLine();
  //          Console.WriteLine();
  //
  //          // Tes 3
  //          for (int i = 0; i < quests.getNum(); i++)
  //          {
  //              if (quests.getMove(i) == 0)
  //              {
  //                  bool found2 = false;
  //                  List<int> res = new List<int>();
  //                  recurseSolve(1, quests.getFrom(i), ref found2, map, ref res);
  //                  if (!res.Contains(quests.getTo(i)))
  //                  {
  //                      Console.WriteLine("{0} {1} {2}  TIDAK", quests.getMove(i), quests.getTo(i), quests.getFrom(i));
  //                  }
  //                  else
  //                  {
  //                      Console.WriteLine("{0} {1} {2}  YA", quests.getMove(i), quests.getTo(i), quests.getFrom(i));
  //                      Console.Write("Path : ");
  //                      int j = res.Count - 1;
  //                      while(res[j] != quests.getTo(i))
  //                      {
  //                          Console.Write(res[j] + " ");
  //                          j--;
  //                      }
  //                      Console.WriteLine(res[j]);
  //                  }
  //              }
  //              else if (quests.getMove(i) == 1) 
  //              {
  //                  //List<int> fur = new List<int>();
  //                  //recursiveCont(quests.getFrom(i), map, ref fur);
  //                  
  //                  // if(fur.Contains(quests.getTo(i)))
  //                  // {
  //                  //     bool found3 = false;
  //                  //     List<int> res = new List<int>();
  //                  //     recurseSolve(quests.getFrom(i), quests.getTo(i), ref found3, map, ref res);
  //
  //                  //     Console.WriteLine("{0} {1} {2}  YA", quests.getMove(i), quests.getTo(i), quests.getFrom(i));
  //                  //     Console.Write("Path : ");
  //                  //     for (int j = 0; j < res.Count; j++)
  //                  //     {
  //                  //         Console.Write(res[j] + " ");
  //                  //     }
  //                  //     Console.WriteLine();
  //                  // }
  //                  // else
  //                  // {
  //                  //     Console.WriteLine("{0} {1} {2}  TIDAK", quests.getMove(i), quests.getTo(i), quests.getFrom(i));
  //                  // }
  //
  //                  bool found3 = false;
  //                  List<int> res = new List<int>();
  //                  recurseSolve(quests.getFrom(i), quests.getTo(i), ref found3, map, ref res);
  //
  //                  if(found3){
  //                      Console.WriteLine("{0} {1} {2}  YA", quests.getMove(i), quests.getTo(i), quests.getFrom(i));
  //                      Console.Write("Path : ");
  //                      for (int j = 0; j < res.Count; j++)
  //                      {
  //                          Console.Write(res[j] + " ");
  //                      }
  //                      Console.WriteLine();
  //                  }else{
  //                      Console.WriteLine("{0} {1} {2}  TIDAK", quests.getMove(i), quests.getTo(i), quests.getFrom(i));
  //                  }
  //              }
  //              else
  //              {
  //                  Console.WriteLine("Invalid command");
  //              }
  //          }
  //      }

        public static bool checkExist(int target, List<int> result)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] == target)
                {
                    return true;
                }
            }
            return false;
        }

        public void recursiveCont(int curr, Graf path, ref List<int> further){
            List<int> neighbor = path.getPath(curr);
            further.Add(curr);
            further.Distinct().ToList();
            further.Sort();
            if (neighbor == null)
            {
            }
            else
            {
                int i = 0;
                while ((i < neighbor.Count))
                {
                    recursiveCont(neighbor[i], path, ref further);
                    i++;
                }
            }
        }
    }
}
