using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    public class Query
    {
        // Atributes
        private int move;
        private int from;
        private int to;

        // Constructor
        public Query(string input)
        {
            string[] info = input.Split();
            move = Int32.Parse(info[0]);
            to = Int32.Parse(info[1]);
            from = Int32.Parse(info[2]);
        }

        // getter-setter
        public int getMove()
        {
            return move;
        }
        public int getFrom()
        {
            return from;
        }
        public int getTo()
        {
            return to;
        }
    }

    // Class dari sebuah query
    // Query dimuat dalam class ini dari index 0
    public class ExQuery
    {
        // Atributes
        private int num_queries;
        private int[] move;
        private int[] from;
        private int[] to;

        // Constructor
        public ExQuery (string filename)
        {
            System.IO.StreamReader fileRead = new System.IO.StreamReader(filename);

            string fileLine;
            fileLine = fileRead.ReadLine();
            num_queries = Int32.Parse(fileLine);

            // Initialize array
            move = new int[num_queries];
            from = new int[num_queries];
            to = new int[num_queries];

            // Add to array
            for (int i = 0; i < num_queries; i++)
            {
                fileLine = fileRead.ReadLine();
                string[] info = fileLine.Split();
                move[i] = Int32.Parse(info[0]);
                to[i] = Int32.Parse(info[1]);
                from[i] = Int32.Parse(info[2]);
            }
        }

        // Destruktor
        ~ExQuery()
        {
            move = null;
            from = null;
            to = null;
            GC.Collect();
        }

        // getter-setter
        public int getNum ()
        {
            return num_queries;
        }
        public int getMove(int i)
        {
            return move[i];
        }
        public int getFrom(int i)
        {
            return from[i];
        }
        public int getTo(int i)
        {
            return to[i];
        }

        // fungsi lain
        public void print() {
            Console.WriteLine("Printing Queries...");
            for (int i = 0; i < num_queries; i++)
            {
                Console.WriteLine("Move = {0}, From = {1}, To = {2}", move[i], from[i], to[i]);
            }
        }
    }
}
