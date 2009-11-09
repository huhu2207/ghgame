using System;
using MinGH.ChartImpl;

namespace MinGH
{
	/// <remarks>
	/// A test class used to print out all chart information.
	/// </remarks>
    class Test_Chart_Class
    {
		/// <summary>
		/// Prints out all information on a chart and waits before quitting.
		/// </summary>
        public static void test()
        {
            Chart test = new Chart("chart.chart");
            test.print_info();
            Console.WriteLine("SUCCESS!! Press enter to quit...");
            Console.ReadLine();
        }
    }
}
