using System;
using GH_Game.Chart;

namespace Chart_View
{
    class Test_Chart_Class
    {
        public static void test()
        {
            Chart test = new Chart("chart.chart");
            test.print_info();
            Console.WriteLine("SUCCESS!! Press enter to quit...");
            Console.ReadLine();
        }
    }
}
