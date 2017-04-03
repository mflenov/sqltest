using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlTest;

namespace CyDSqlTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
			Console.WriteLine("CyD Sql Unit Test, http://www.cydsoft.com");

			if (args.Length == 0) {
				PrintHelp();
				return;
			}

			SqlUnitTest test = new SqlUnitTest(args[1], new TestOutput());
			test.RunTests();

			Console.WriteLine("Complete");
			Console.ReadLine();
		}

		static List<string> help = new List<string>() {
			"",
			"SYNTAX",
			"\t CyDSqlTest FILENAME",
			"",
		};

		public static void PrintHelp() {
			foreach(string line in help) {
				Console.WriteLine(line);
			}
		}
	}
}
