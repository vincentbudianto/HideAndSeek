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
        private DispatcherTimer _timer1, _timer2;
        private ExQuery quests;
        private Graf map;
        private int counter, checker;
        private List<int> solution;
        private Query quest;
        public Boolean B1, B2;
        public Msagl.Graph graph = new Msagl.Graph("graph");
        public String dirGraph, dirExQuery;

        public MainWindow()
        {
            B1 = false;
            B2 = false;
            InitializeComponent();

            _timer1 = new DispatcherTimer();
            _timer1.Interval = TimeSpan.FromSeconds(0.5);
            _timer1.Tick += timer_Tick_Green;

            _timer2 = new DispatcherTimer();
            _timer2.Interval = TimeSpan.FromSeconds(0.5);
            _timer2.Tick += timer_Tick_Red;
        }

        private void Load_Graph_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch
            {
                MessageBox.Show("      Error Code 0x05021999\n             File Input Error");
            }
        }

        private void Load_Query_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (B2)
                {
                    Result.Content = "";
                    B2 = false;
                }

                B1 = true;
                quests = new ExQuery(dirExQuery);
                Next.IsEnabled = true;

                Result.Content += "Total query = " + quests.getNum() + "\n";
                counter = 0;
            }
            catch
            {
                MessageBox.Show("      Error Code 0x28041999\n             File Input Error");
            }
        }

        private void Open_Graph_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == true)
            {
                dirGraph = openFileDialog1.FileName;
                Graph_Text.Text = System.IO.Path.GetFileName(dirGraph);
                Load_Graph.IsEnabled = true;
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

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (quests.getMove(counter) == 0)
            {
                bool found2 = false;
                List<int> res = new List<int>();
                recurseSolve(1, quests.getFrom(counter), ref found2, map, ref res);
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
                    _timer1.Start();
                }
            }
            else if (quests.getMove(counter) == 1)
            {
                bool found3 = false;
                List<int> res = new List<int>();
                recurseSolve(quests.getFrom(counter), quests.getTo(counter), ref found3, map, ref res);
                solution = null;
                solution = new List<int>(res);

                if (found3)
                {
                    resetGraph();
                    String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter) + "   YA\n";
                    Result.Content += ans;

                    checker = 0;
                    _timer1.Start();
                }
                else
                {
                    resetGraph();
                    String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter) + "   TIDAK\n";
                    Result.Content += ans;
                    this.gViewer.Graph = graph;
                }
            }

            counter++;

            if (counter == quests.getNum())
            {
                Next.IsEnabled = false;
            }
        }

        private void Enter_Query_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (B1)
                {
                    Result.Content = "";
                    B1 = false;
                }

                B2 = true;
                quest = new Query(Query_Text.Text);
                Next.IsEnabled = false;

                if (quest.getMove() == 0)
                {
                    bool found2 = false;
                    List<int> res = new List<int>();
                    recurseSolve(1, quest.getFrom(), ref found2, map, ref res);
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
                        _timer1.Start();
                    }
                }
                else if (quest.getMove() == 1)
                {
                    bool found3 = false;
                    List<int> res = new List<int>();
                    recurseSolve(quest.getFrom(), quest.getTo(), ref found3, map, ref res);
                    solution = null;
                    solution = new List<int>(res);

                    if (found3)
                    {
                        resetGraph();
                        String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   YA\n";
                        Result.Content = ans + Result.Content;

                        checker = 0;
                        _timer1.Start();

                    }
                    else
                    {
                        resetGraph();
                        String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   TIDAK\n";
                        Result.Content = ans + Result.Content;
                        this.gViewer.Graph = graph;
                    }
                }
            }
            catch
            {
                MessageBox.Show("      Error Code 0x14031999\n                Input Error");
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

        private void timer_Tick_Green(object sender, EventArgs e)
        {
            if (checker < solution.Count())
            {
                graph.FindNode((solution[checker]).ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightGreen;
                this.gViewer.Graph = graph;
            }
            
            checker++;

            if (checker >= solution.Count())
            {
                _timer1.Stop();
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
                _timer2.Stop();
            }
        }

        public void recurseSolve(int curr, int target, ref bool found, Graf path, ref List<int> result)
        {
            List<int> neighbor = path.getPath(curr);

            if (curr == target)
            {
                found = true;
                result.Add(target);
            }
            else if (neighbor == null)
            {
                found = false;
                result.Remove(curr);
            }
            else
            {
                result.Add(curr);
                int i = 0;

                while ((i < neighbor.Count) && (!found))
                {
                    recurseSolve(neighbor[i], target, ref found, path, ref result);
                    i++;
                }

                if (!found)
                {
                    result.Remove(curr);
                }
            }
        }
    }
}
