using System;
using MinGH.ChartImpl;

namespace MinGH
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
