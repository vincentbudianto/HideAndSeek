using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    public class Graf
    {
        // Atributes
        private int houses;
        private List<int>[] paths;

        // Constructor
        public Graf(string filename)
        {
            System.IO.StreamReader fileRead = new System.IO.StreamReader(filename);

            string fileLine;
            fileLine = fileRead.ReadLine();
            houses = Int32.Parse(fileLine);

            // Initialize list
            paths = new List<int>[houses + 1];
			
            for (int i = 1; i <= houses; i++)
            {
                paths[i] = new List<int>();
            }

            // Add to list
            while ((fileLine = fileRead.ReadLine()) != null)
            {
                string[] info = fileLine.Split();
                paths[Int32.Parse(info[0])].Add(Int32.Parse(info[1]));
                paths[Int32.Parse(info[1])].Add(Int32.Parse(info[0]));
            }

            // Sort and remove duplicates
            for (int i = 1; i <= houses; i++)
            {
                paths[i] = paths[i].Distinct().ToList();
                paths[i].Sort();
            }

            // 1-way Graph
            delReverse(1);
        }

        // Destructor
        ~Graf()
        {
            for (int i = 1; i <= houses; i++)
            {
                paths[i] = null;
                GC.Collect();
            }
			
            paths = null;
            GC.Collect();
        }

        // Getter-setter
        public List<int> getPath(int node)
        {
            return (paths[node]);
        }
		
        public int getHouses()
        {
            return (houses);
        }

        // Other functions
        private void delReverse(int node)
        {
            for (int i = 0; i < paths[node].Count; i++)
			{
                paths[(paths[node][i])].Remove(node);
                delReverse(paths[node][i]);
            }
        }
        
        public void print()
        {
            Console.WriteLine("Houses = {0}", houses);
            Console.WriteLine("Palace is connected to: ");

            for (int i = 1; i <= houses; i++)
            {
                Console.Write(i + ": ");
                for (int j = 0; j < paths[i].Count; j++)
                {
                    Console.Write(paths[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
