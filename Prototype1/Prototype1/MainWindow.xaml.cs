using System;
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

namespace Prototype1
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graph map;
        private ExQuery quests;
        private Query quest;
        private Driver solver;

        public MainWindow()
        {
            InitializeComponent();
            solver = new Driver();
        }

        private void Load_Graph_Click(object sender, RoutedEventArgs e)
        {
            map = new Graph("../../" + Graph_Text.Text);
            MessageBox.Show("Graph built!");
        }

        private void Load_Query_Click(object sender, RoutedEventArgs e)
        {
            quests = new ExQuery("../../" + ExQuery_Text.Text);
            MessageBox.Show("Query File read!");


        }

        private void Enter_Query_Click(object sender, RoutedEventArgs e)
        {
            quest = new Query(Query_Text.Text);
            MessageBox.Show("Query Console read!");

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
                    /*Console.Write("Path : ");
                    int j = res.Count - 1;
                    while(res[j] != quest.getTo())
                    {
                        Console.Write(res[j] + " ");
                        j--;
                    }
                    Console.WriteLine(res[j]);*/
                }
            }
            else if (quest.getMove() == 1) 
            {
  
                bool found3 = false;
                List<int> res = new List<int>();
                solver.recurseSolve(quest.getFrom(), quest.getTo(), ref found3, map, ref res);
  
                if(found3){
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   YA\n";
                    Result.Content += ans;
                }
                else
                {
                    String ans = quest.getMove() + " " + quest.getTo() + " " + quest.getFrom() + "   TIDAK\n";
                    Result.Content += ans;
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }
}
