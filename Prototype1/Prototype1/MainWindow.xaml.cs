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
        public Boolean B1, B2;
        
        public MainWindow()
        {
            B1 = false;
            B2 = false;
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
            if (B2)
            {
                Result.Content = "";
                B2 = false;
            }

            B1 = true;
            quests = new ExQuery("../../" + ExQuery_Text.Text);
            
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
                        String ans = quests.getMove(i) + " " + quests.getTo(i) + " " + quests.getFrom(i) + "   YA\n";
                        Result.Content += ans;
                    }
                }
                else if (quests.getMove(i) == 1) 
                {
                    bool found3 = false;
                    List<int> res = new List<int>();
                    solver.recurseSolve(quests.getFrom(i), quests.getTo(i), ref found3, map, ref res);
  
                    if(found3)
                    {
                        String ans = quests.getMove(i) + " " + quests.getTo(i) + " " + quests.getFrom(i) + "   YA\n";
                        Result.Content += ans;
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
  
                if(found3){
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
