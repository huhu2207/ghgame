using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
