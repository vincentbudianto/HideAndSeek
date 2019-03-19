using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Msagl = Microsoft.Msagl.Drawing;

namespace Prototype1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public class AutoClosingMessageBox
    // Created by : DmitryGaravsky
    // To install AutoClosingMessageBox, run the following command in the Package Manager Console:
    // PM> Install-Package AutoClosingMessageBox -Version 1.0.0.2
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
                MessageBox.Show(text, caption);
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }

    public partial class MainWindow : Window
    {
        public Boolean B1;
        private DispatcherTimer _timer, _timer2;
        private ExQuery quests;
        private Graf map;
        private int checker, counter, now, prev;
        private List<int> solution;
        public Msagl.Graph graph = new Msagl.Graph("graph");
        private Query quest;
        public String dirExQuery, dirGraph, nowpath, prevpath;
        
        public MainWindow()
        {
            B1 = false;
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(0.0000009);
            _timer.Tick += timer_Tick_Green;

            _timer2 = new DispatcherTimer();
            _timer2.Interval = TimeSpan.FromMilliseconds(0.0000009);
            _timer2.Tick += timer_Tick_Red;
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

        private void Load_Graph_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                map = new Graf(dirGraph);
                Enter_Query.IsEnabled = true;
                Open_Query.IsEnabled = true;
                Next.IsEnabled = false;
                this.gViewer.Graph = null;
                graph = new Msagl.Graph("graph");

                for (int i = map.getHouses()-1; i>0; i--)
                {
                    for (int j = map.getPath(i).Count()-1; j >=0 ; j--)
                    {
                        string str1 = i.ToString();
                        string str2 = map.getPath(i)[j].ToString();

                        graph.AddEdge(str1, str2).Attr.ArrowheadAtTarget = Msagl.ArrowStyle.None;

                        Microsoft.Msagl.Drawing.Node from = graph.FindNode(str1);
                        Microsoft.Msagl.Drawing.Node to = graph.FindNode(str2);

                        from.Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
                        from.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                        to.Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
                        to.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                    }
                }
                this.gViewer.Graph = graph;
            }
            catch
            {
                MessageBox.Show("      Error Code 0x05021999\n             File Input Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Load_Query_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                B1 = true;
                quests = new ExQuery(dirExQuery);
                Next.IsEnabled = true;
                Result.Content = "";

                Result.Content += "Total query = " + quests.getNum() + "\n";
                counter = 0;

                Next_Click(sender, e);
            }
            catch
            {
                MessageBox.Show("      Error Code 0x28041999\n             File Input Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            resetGraph();
            if (quests.getMove(counter) == 0)
            {
                bool found2 = false;
                List<int> res = new List<int>();
                String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter);
                Result.Content += ans;
                recurseSolve(1, quests.getFrom(counter), ref found2, map, ref res);
                solution = null;
                solution = new List<int>(res);


                if (!res.Contains(quests.getTo(counter)))
                {
                    AutoClosingMessageBox.Show("Solution Not Found", "Result", 1500);
                    ans = "   TIDAK\n";
                    Result.Content += ans;
                    resetGraph();
                    this.gViewer.Graph = graph;
                }
                else
                {
                    AutoClosingMessageBox.Show("Solution Found", "Result", 1500);
                    ans = "   YA\n";
                    Result.Content += ans;
                }
            }
            else if (quests.getMove(counter) == 1)
            {
                bool found3 = false;
                List<int> res = new List<int>();
                String ans = quests.getMove(counter) + " " + quests.getTo(counter) + " " + quests.getFrom(counter);
                Result.Content += ans;
                recurseSolve(quests.getFrom(counter), quests.getTo(counter), ref found3, map, ref res);
                solution = null;
                solution = new List<int>(res);


                if (found3)
                {
                    AutoClosingMessageBox.Show("Solution Found", "Result", 1500);
                    ans = "   YA\n";
                    Result.Content += ans;
                }
                else
                {
                    AutoClosingMessageBox.Show("Solution Not Found", "Result", 1500);
                    ans = "   TIDAK\n";
                    Result.Content += ans;
                    resetGraph();
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

                resetGraph();
                quest = new Query(Query_Text.Text);
                Next.IsEnabled = false;

                if (quest.getMove() == 0)
                {
                    bool found2 = false;
                    List<int> res = new List<int>();
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom();
                    Result.Content += ans;
                    recurseSolve(1, quest.getFrom(), ref found2, map, ref res);
                    solution = null;
                    solution = new List<int>(res);

                    if (!res.Contains(quest.getTo()))
                    {
                        AutoClosingMessageBox.Show("Solution Not Found", "Result", 1500);
                        ans = "   TIDAK\n";
                        Result.Content += ans;
                        resetGraph();
                        this.gViewer.Graph = graph;
                    }
                    else
                    {
                        AutoClosingMessageBox.Show("Solution Found", "Result", 1500);
                        ans = "   YA\n";
                        Result.Content += ans;
                    }
                }
                else if (quest.getMove() == 1)
                {
                    bool found3 = false;
                    List<int> res = new List<int>();
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom();
                    Result.Content += ans;
                    recurseSolve(quest.getFrom(), quest.getTo(), ref found3, map, ref res);
                    solution = null;
                    solution = new List<int>(res);

                    if (found3)
                    {
                        AutoClosingMessageBox.Show("Solution Found", "Result", 1500);
                        ans = "   YA\n";
                        Result.Content += ans;
                    }
                    else
                    {
                        AutoClosingMessageBox.Show("Solution Not Found", "Result", 1500);
                        ans = "   TIDAK\n";
                        Result.Content += ans;
                        resetGraph();
                        this.gViewer.Graph = graph;
                    }
                }
            }
            catch
            {
                MessageBox.Show("      Error Code 0x14031999\n                Input Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    graph.FindNode(str1).Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
                    graph.FindNode(str2).Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
                }
            }

        }

        private void timer_Tick_Green(object sender, EventArgs e)
        {
            graph.FindNode(now.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightGreen;
            this.gViewer.Graph = graph;
            checker++;
            if (checker >= 1)
            {
                _timer.Stop();
            }
        }

        private void timer_Tick_Red(object sender, EventArgs e)
        {
            graph.FindNode(prev.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.IndianRed;
            this.gViewer.Graph = graph;
            checker++;
            if (checker >= 1)
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
                now = target;
                checker = 0;
                _timer.Start();
                result.Add(target);

                if (result.Count() != 0)
                {
                    nowpath = result[0].ToString();
                }
                else
                {
                    nowpath = "";
                }

                for (int j = 1; j < result.Count(); j++)
                {
                    nowpath += " -> " + result[j];
                }

                //AutoClosingMessageBox.Show("Hijau " + now, "Path", 1000);
                AutoClosingMessageBox.Show(nowpath, "Path", 1000);
            }
            else if (neighbor == null)
            {
                found = false;
                prev = curr;
                checker = 0;
                _timer2.Start();
                result.Remove(curr);

                if (result.Count() != 0)
                {
                    prevpath = result[0].ToString();
                }
                else
                {
                    prevpath = "";
                }

                for (int j = 1; j < result.Count(); j++)
                {
                    prevpath += " -> " + result[j];
                }

                //AutoClosingMessageBox.Show("Merah " + prev, "Path", 1000);
                AutoClosingMessageBox.Show(prevpath, "Path", 1000);
            }
            else
            {
                now = curr;
                checker = 0;
                _timer.Start();
                result.Add(curr);
                
                if (result.Count() != 0)
                {
                    nowpath = result[0].ToString();
                }
                else
                {
                    nowpath = "";
                }

                for (int j = 1; j < result.Count(); j++)
                {
                    nowpath += " -> " + result[j];
                }

                //AutoClosingMessageBox.Show("Hijau " + now, "Path", 1000);
                AutoClosingMessageBox.Show(nowpath, "Path", 1000);
                int i = 0;

                while ((i < neighbor.Count) && (!found))
                {
                    recurseSolve(neighbor[i], target, ref found, path, ref result);
                    i++;
                }
                if (!found)
                {
                    prev = curr;
                    checker = 0;
                    _timer2.Start();
                    result.Remove(curr);

                    if (result.Count() != 0)
                    {
                        prevpath = result[0].ToString();
                    }
                    else
                    {
                        prevpath = "";
                    }

                    for (int j = 1; j < result.Count(); j++)
                    {
                        prevpath += " -> " + result[j];
                    }

                    //AutoClosingMessageBox.Show("Merah " + prev, "Path", 1000);
                    AutoClosingMessageBox.Show(prevpath, "Path", 1000);
                }
            }
        }
    }
}
