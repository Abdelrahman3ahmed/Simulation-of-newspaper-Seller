using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;
using System.Runtime.InteropServices;

namespace NewspaperSellerSimulation
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        System_Operation sysop;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
            SimulationCase obj = new SimulationCase();

            sysop = new System_Operation();
            string testingResult = TestingManager.Test(sysop.simsys, Constants.FileNames.TestCase3);
            MessageBox.Show(testingResult);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Days", "Day");
            dataGridView1.Columns.Add("RD", "Random Digits for Type of Newsday");
            dataGridView1.Columns.Add("TOND", "Type of Newsday");
            dataGridView1.Columns.Add("RDFD", "Random Digits for Demand");
            dataGridView1.Columns.Add("Demands", "Demand");
            dataGridView1.Columns.Add("RFS", "Revenue From Sales");
            dataGridView1.Columns.Add("LPFED", "Lost From Profit From Excess Demand");
            dataGridView1.Columns.Add("SFSOS", "Salvage From Sale of Scrap");
            dataGridView1.Columns.Add("DP", "Daily Profit");
            for(int i = 0; i < sysop.simsys.NumOfRecords;i++)
            {
                string [] row = new string []{sysop.simsys.SimulationTable[i].DayNo.ToString(),
                                                            sysop.simsys.SimulationTable[i].RandomNewsDayType.ToString(),
                                                            sysop.simsys.SimulationTable[i].NewsDayType.ToString(),
                                                            sysop.simsys.SimulationTable[i].RandomDemand.ToString(),
                                                            sysop.simsys.SimulationTable[i].Demand.ToString(),
                                                            sysop.simsys.SimulationTable[i].SalesProfit.ToString(),
                                                            sysop.simsys.SimulationTable[i].LostProfit.ToString(),
                                                            sysop.simsys.SimulationTable[i].ScrapProfit.ToString(),
                                                            sysop.simsys.SimulationTable[i].DailyNetProfit.ToString()};
                dataGridView1.Rows.Add(row);
            }
            string[] row1 = new string[] {"","","","","",                                                        
                                                            sysop.simsys.PerformanceMeasures.TotalSalesProfit.ToString(),
                                                            sysop.simsys.PerformanceMeasures.TotalLostProfit.ToString(),
                                                            sysop.simsys.PerformanceMeasures.TotalScrapProfit.ToString(),
                                                            sysop.simsys.PerformanceMeasures.TotalNetProfit.ToString()};
            dataGridView1.Rows.Add(row1);
            textBox1.Text = sysop.simsys.PerformanceMeasures.DaysWithMoreDemand.ToString();
            textBox2.Text = sysop.simsys.PerformanceMeasures.DaysWithUnsoldPapers.ToString();
            textBox3.Text = sysop.simsys.SimulationTable[0].DailyCost.ToString();
            textBox4.Text = sysop.simsys.PerformanceMeasures.TotalCost.ToString();
        }
    }

}
