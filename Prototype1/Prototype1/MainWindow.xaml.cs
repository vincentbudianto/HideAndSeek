using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using Msagl = Microsoft.Msagl.Drawing;

namespace Prototype1
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graf map;
        private ExQuery quests;
        private Query quest;
        private Driver solver;
        public Boolean B1, B2;
        public String dirGraph, dirExQuery;
        public Msagl.Graph graph = new Msagl.Graph("graph");

        private int counter;
        private int checker;
        private List<int> solution;
        private DispatcherTimer _timer;

        public Boolean temp;

        public MainWindow()
        {
            B1 = false;
            B2 = false;
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += timer_Tick;
            solver = new Driver();
        }

        private void Load_Graph_Click(object sender, RoutedEventArgs e)
        {
            map = new Graf(dirGraph);
            Enter_Query.IsEnabled = true;
            Next.IsEnabled = false;
            this.gViewer.Graph = null;
            graph = new Msagl.Graph("graph");


            for (int i = 1; i < map.getHouses(); i++)
            {
                for (int j = 0; j < map.getPath(i).Count(); j++)
                {
                    string str1 = i.ToString();
                    string str2 = map.getPath(i)[j].ToString();
                    graph.AddEdge(str1, str2).Attr.ArrowheadAtTarget = Msagl.ArrowStyle.None;
                    graph.FindNode(str1).Attr.FillColor = Microsoft.Msagl.Drawing.Color.IndianRed;
                    graph.FindNode(str2).Attr.FillColor = Microsoft.Msagl.Drawing.Color.IndianRed;
                }
            }
            this.gViewer.Graph = graph;
        }

        private void Load_Query_Click(object sender, RoutedEventArgs e)
        {
            if (B2)
            {
                Result.Content = "";
                B2 = false;
            }

            B1 = true;
            quests = new ExQuery(dirExQuery);

            Result.Content += "Total query = " + quests.getNum() + "\n";
            counter = 0;
            Next.IsEnabled = true;
        }

        private void Open_Graph_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == true) {
                dirGraph = openFileDialog1.FileName;
                Graph_Text.Text = System.IO.Path.GetFileName(dirGraph);
                Load_Graph.IsEnabled = true;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (quests.getMove(counter) == 0)
            {
                bool found2 = false;
                List<int> res = new List<int>();
                solver.recurseSolve(1, quests.getFrom(counter), ref found2, map, ref res);
                solution = null;
                solution = new List<int>(res);
                if (!res.Contains(quests.getTo(counter)))
                {
                    resetGraph();
                    String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter) + "   TIDAK\n";
                    Result.Content += ans;
                    this.gViewer.Graph = graph;
                }
                else
                {
                    resetGraph();
                    String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter) + "   YA\n";
                    Result.Content += ans;
                    checker = 0;
                    _timer.Start();
                }
            }
            else if (quests.getMove(counter) == 1)
            {
                bool found3 = false;
                List<int> res = new List<int>();
                solver.recurseSolve(quests.getFrom(counter), quests.getTo(counter), ref found3, map, ref res);
                solution = null;
                solution = new List<int>(res);

                if (found3)
                {
                    resetGraph();
                    String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter) + "   YA\n";
                    Result.Content += ans;

                    checker = 0;
                    _timer.Start();
                }
                else
                {
                    resetGraph();
                    String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter) + "   TIDAK\n";
                    Result.Content += ans;
                    this.gViewer.Graph = graph;
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
            counter++;
            if (counter == quests.getNum())
            {
                Next.IsEnabled = false;
            }
        }

        private void Open_Query_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog2.ShowDialog() == true)
            {
                dirExQuery = openFileDialog2.FileName;
                ExQuery_Text.Text = System.IO.Path.GetFileName(dirExQuery);
                Load_Query.IsEnabled = true;
            }
        }

        private void Enter_Query_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = false;
            if (B1)
            {
                Result.Content = "";
                B1 = false;
            }

            B2 = true;
            quest = new Query(Query_Text.Text);

            if (quest.getMove() == 0)
            {
                bool found2 = false;
                List<int> res = new List<int>();
                solver.recurseSolve(1, quest.getFrom(), ref found2, map, ref res);
                solution = null;
                solution = new List<int>(res);
                if (!res.Contains(quest.getTo()))
                {
                    resetGraph();
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   TIDAK\n";
                    Result.Content = ans + Result.Content;
                    this.gViewer.Graph = graph;
                }
                else
                {
                    resetGraph();
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   YA\n";
                    Result.Content = ans + Result.Content;

                    checker = 0;
                    _timer.Start();
                }
            }
            else if (quest.getMove() == 1)
            {

                bool found3 = false;
                List<int> res = new List<int>();
                solver.recurseSolve(quest.getFrom(), quest.getTo(), ref found3, map, ref res);
                solution = null;
                solution = new List<int>(res);

                if (found3)
                {
                    resetGraph();
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   YA\n";
                    Result.Content = ans + Result.Content;

                    checker = 0;
                    _timer.Start();
                    
                }
                else
                {
                    resetGraph();
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   TIDAK\n";
                    Result.Content = ans + Result.Content;
                    this.gViewer.Graph = graph;
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }

        public void resetGraph()
        {
            for (int i = 1; i < map.getHouses(); i++)
            {
                for (int j = 0; j < map.getPath(i).Count(); j++)
                {
                    string str1 = i.ToString();
                    string str2 = map.getPath(i)[j].ToString();
                    graph.FindNode(str1).Attr.FillColor = Microsoft.Msagl.Drawing.Color.IndianRed;
                    graph.FindNode(str2).Attr.FillColor = Microsoft.Msagl.Drawing.Color.IndianRed;
                }
            }

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (checker < solution.Count())
            {
                graph.FindNode((solution[checker]).ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightGreen;
                this.gViewer.Graph = graph;
            }
            
            checker++;
            if (checker >= solution.Count())
            {
                _timer.Stop();
            }
        }

        private void timer_Tick_Red(object sender, EventArgs e)
        {
            if (checker < solution.Count())
            {
                graph.FindNode((solution[checker]).ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.IndianRed;
                this.gViewer.Graph = graph;
            }

            checker++;
            if (checker >= solution.Count())
            {
                _timer.Stop();
            }
        }
    }
}
