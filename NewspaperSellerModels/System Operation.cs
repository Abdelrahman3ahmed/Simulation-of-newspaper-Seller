using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class System_Operation
    {
        public SimulationSystem simsys;
        List<SimulationCase> simtab;
        PerformanceMeasures performence;
        public System_Operation()
        {
            Load_Simulation_System_Inputs();
            Enter_SimulationTable();
            simsys.SimulationTable = simtab;
            simsys.PerformanceMeasures = performence;
        }
        public void Load_Simulation_System_Inputs()
        {
            simsys = new SimulationSystem();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"E:\FCIS\CS\4th Year - 1st Semester\Modeling and Simulation\Labs\Task 2\NewspaperSellerSimulation_Students\NewspaperSellerSimulation_Students\NewspaperSellerSimulation\TestCases\TestCase3.txt");
            string[] files = File.ReadAllLines(path);
            simsys.NumOfNewspapers = int.Parse(files[1]);
            simsys.NumOfRecords = int.Parse(files[4]);
            simsys.PurchasePrice = decimal.Parse(files[7]);
            simsys.ScrapPrice = decimal.Parse(files[10]);
            simsys.SellingPrice = decimal.Parse(files[13]);
            simsys.UnitProfit = simsys.SellingPrice - simsys.PurchasePrice;
            string daydis = files[16];
            string[] days = daydis.Split(new string[] { ", " }, StringSplitOptions.None);
            simsys.DayTypeDistributions = new List<DayTypeDistribution>();

            for (int i = 0; i < days.Length; i++)
            {
                simsys.DayTypeDistributions.Add(new DayTypeDistribution());
            }

            simsys.DayTypeDistributions[0].DayType = Enums.DayType.Good;
            simsys.DayTypeDistributions[1].DayType = Enums.DayType.Fair;
            simsys.DayTypeDistributions[2].DayType = Enums.DayType.Poor;

            //
            for (int i = 0; i < days.Length; i++)
            {
                simsys.DayTypeDistributions[i].Probability = decimal.Parse(days[i]);
                if (i > 0)
                {
                    simsys.DayTypeDistributions[i].CummProbability = simsys.DayTypeDistributions[i].Probability + simsys.DayTypeDistributions[i - 1].CummProbability;
                    simsys.DayTypeDistributions[i].MaxRange = (int)(simsys.DayTypeDistributions[i].CummProbability * 100);
                    simsys.DayTypeDistributions[i].MinRange = simsys.DayTypeDistributions[i - 1].MaxRange + 1;


                }
                else
                {
                    simsys.DayTypeDistributions[i].CummProbability = simsys.DayTypeDistributions[i].Probability;
                    simsys.DayTypeDistributions[i].MaxRange = (int)(simsys.DayTypeDistributions[i].CummProbability * 100);
                    simsys.DayTypeDistributions[i].MinRange = 1;
                }


            }
            simsys.DemandDistributions = new List<DemandDistribution>();

            for (int i = 0; i < 7; i++)
            {
                simsys.DemandDistributions.Add(new DemandDistribution());
            }

            for (int i = 0; i < 7; i++)
            {
                string demand = files[19 + i];
                string[] demands = demand.Split(new string[] { ", " }, StringSplitOptions.None);
                simsys.DemandDistributions[i].Demand = int.Parse(demands[0]);


                List<DayTypeDistribution> DayTypeDistribution_demand = new List<DayTypeDistribution>();

                for (int j = 0; j < 3; j++)
                {
                    DayTypeDistribution_demand.Add(new DayTypeDistribution());
                }

                DayTypeDistribution_demand[0].DayType = Enums.DayType.Good;
                DayTypeDistribution_demand[1].DayType = Enums.DayType.Fair;
                DayTypeDistribution_demand[2].DayType = Enums.DayType.Poor;


                for (int j = 0; j < 3; j++)
                {
                    DayTypeDistribution_demand[j].Probability = decimal.Parse(demands[j + 1]);
                    if (i > 0)
                    {
                        DayTypeDistribution_demand[j].CummProbability = simsys.DemandDistributions[i - 1].DayTypeDistributions[j].CummProbability + DayTypeDistribution_demand[j].Probability;
                        DayTypeDistribution_demand[j].MaxRange = (int)(DayTypeDistribution_demand[j].CummProbability * 100);
                        DayTypeDistribution_demand[j].MinRange = simsys.DemandDistributions[i - 1].DayTypeDistributions[j].MaxRange + 1;

                    }
                    else
                    {
                        DayTypeDistribution_demand[j].CummProbability = DayTypeDistribution_demand[j].Probability;
                        DayTypeDistribution_demand[j].MaxRange = (int)(DayTypeDistribution_demand[j].CummProbability * 100);
                        DayTypeDistribution_demand[j].MinRange = 1;
                    }
                }
                simsys.DemandDistributions[i].DayTypeDistributions = DayTypeDistribution_demand;
            }
        }

        public void Enter_SimulationTable()
        {
            simtab = new List<SimulationCase>();
            performence = new PerformanceMeasures();
            int num_of_records = simsys.NumOfRecords;
            Random rnd = new Random();
            decimal dailyCost = simsys.NumOfNewspapers * simsys.PurchasePrice;
            for (int i = 0; i < num_of_records; i++)
            {
               simtab.Add(new SimulationCase());
            }
            for (int i = 0; i < num_of_records; i++)
            {
                simtab[i].DayNo = i + 1;
                simtab[i].RandomNewsDayType = rnd.Next(1, 101);
                for(int j=0;j<simsys.DayTypeDistributions.Count;j++)
                {
                    if(simtab[i].RandomNewsDayType >= simsys.DayTypeDistributions[j].MinRange && simtab[i].RandomNewsDayType <= simsys.DayTypeDistributions[j].MaxRange)
                    {
                        simtab[i].NewsDayType = simsys.DayTypeDistributions[j].DayType;
                    }
                }
                simtab[i].RandomDemand = rnd.Next(1, 101);
                for(int j=0;j<simsys.DemandDistributions.Count;j++)
                {//
                    for(int k=0;k<simsys.DemandDistributions[j].DayTypeDistributions.Count;k++)
                    {
                        if(simsys.DemandDistributions[j].DayTypeDistributions[k].DayType == simtab[i].NewsDayType)
                        {
                            if(simtab[i].RandomDemand  >= simsys.DemandDistributions[j].DayTypeDistributions[k].MinRange && simtab[i].RandomDemand  <= simsys.DemandDistributions[j].DayTypeDistributions[k].MaxRange)
                            {
                                simtab[i].Demand = simsys.DemandDistributions[j].Demand;
                            }
                        }
                    }
                }//
                simtab[i].DailyCost = simsys.NumOfNewspapers * simsys.PurchasePrice;
                simtab[i].SalesProfit = (simsys.NumOfNewspapers - simtab[i].Demand >=0)? simtab[i].Demand : simsys.NumOfNewspapers;
                simtab[i].SalesProfit *= simsys.SellingPrice;

                simtab[i].LostProfit = (simtab[i].Demand - simsys.NumOfNewspapers) * (simsys.SellingPrice - simsys.PurchasePrice);
                simtab[i].LostProfit = (simtab[i].LostProfit < 0) ? 0 : simtab[i].LostProfit;

                simtab[i].ScrapProfit = (simsys.NumOfNewspapers - simtab[i].Demand) * (simsys.ScrapPrice);
                simtab[i].ScrapProfit = (simtab[i].ScrapProfit < 0) ? 0 : simtab[i].ScrapProfit;

                simtab[i].DailyNetProfit = simtab[i].SalesProfit - simtab[i].DailyCost - simtab[i].LostProfit + simtab[i].ScrapProfit;


                performence.TotalSalesProfit += simtab[i].SalesProfit;
                performence.TotalCost += simtab[i].DailyCost;
                performence.TotalLostProfit += simtab[i].LostProfit;
                performence.TotalScrapProfit += simtab[i].ScrapProfit;
                performence.TotalNetProfit += simtab[i].DailyNetProfit;
                if (simtab[i].Demand > simsys.NumOfNewspapers)
                    performence.DaysWithMoreDemand += 1;
                if (simtab[i].Demand < simsys.NumOfNewspapers)
                    performence.DaysWithUnsoldPapers += 1;
            }   
        }
    }
}
