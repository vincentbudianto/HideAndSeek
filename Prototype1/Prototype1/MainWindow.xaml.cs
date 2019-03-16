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

        public MainWindow()
        {
            B1 = false;
            B2 = false;
            InitializeComponent();
            solver = new Driver();
        }

        private void Load_Graph_Click(object sender, RoutedEventArgs e)
        {
            map = new Graf(dirGraph);

            for (int i = 1; i < map.getHouses(); i++)
            {
                for (int j = 0; j < map.getPath(i).Count(); j++)
                {
                    string str1 = i.ToString();
                    string str2 = map.getPath(i)[j].ToString();
                    graph.AddEdge(str1, str2).Attr.ArrowheadAtTarget = Msagl.ArrowStyle.None;
                    //graph.AddEdge(str1, str2).Attr.Color = Microsoft.Msagl.Drawing.Color.DarkRed;
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

            Result.Content += quests.getNum() + "\n";

            for (int i = 0; i < quests.getNum(); i++)
            {
                if (quests.getMove(i) == 0)
                {
                    bool found2 = false;
                    List<int> res = new List<int>();
                    solver.recurseSolve(1, quests.getFrom(i), ref found2, map, ref res);
                    if (!res.Contains(quests.getTo(i)))
                    {
                        String ans = quests.getMove(i) + " " + quests.getTo(i) + " " + quests.getFrom(i) + "   TIDAK\n";
                        Result.Content += ans;
                    }
                    else
                    {
                        this.gViewer.Graph = null;
                        String ans = quests.getMove(i) + " " + quests.getTo(i) + " " + quests.getFrom(i) + "   YA\n";
                        Result.Content += ans;
                    }
                }
                else if (quests.getMove(i) == 1)
                {
                    bool found3 = false;
                    List<int> res = new List<int>();
                    solver.recurseSolve(quests.getFrom(i), quests.getTo(i), ref found3, map, ref res);

                    if (found3)
                    {
                        String ans = quests.getMove(i) + " " + quests.getTo(i) + " " + quests.getFrom(i) + "   YA\n";
                        Result.Content += ans;

                        for (int j = 0; j < res.Count(); j++)
                        {
                            //MessageBox.Show(res[j].ToString());
                            graph.FindNode((res[j]).ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightGreen;
                            this.gViewer.Graph = graph;
                        }
                    }
                    else
                    {
                        String ans = quests.getMove(i) + " " + quests.getTo(i) + " " + quests.getFrom(i) + "   TIDAK\n";
                        Result.Content += ans;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }

        private void Open_Graph_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true) {
                dirGraph = openFileDialog1.FileName;
                Graph_Text.Text = System.IO.Path.GetFileName(dirGraph);
            }
        }
        
        private void Open_Query_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            if (openFileDialog2.ShowDialog() == true)
            {
                dirExQuery = openFileDialog2.FileName;
                ExQuery_Text.Text = System.IO.Path.GetFileName(dirExQuery);
                Next.IsEnabled = true;
            }
        }

        private void Enter_Query_Click(object sender, RoutedEventArgs e)
        {
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
                if (!res.Contains(quest.getTo()))
                {
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   TIDAK\n";
                    Result.Content = ans + Result.Content;
                }
                else
                {
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   YA\n";
                    Result.Content = ans + Result.Content;
                }
            }
            else if (quest.getMove() == 1)
            {

                bool found3 = false;
                List<int> res = new List<int>();
                solver.recurseSolve(quest.getFrom(), quest.getTo(), ref found3, map, ref res);

                if (found3)
                {
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   YA\n";
                    Result.Content = ans + Result.Content;
                }
                else
                {
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   TIDAK\n";
                    Result.Content = ans + Result.Content;
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }
}
