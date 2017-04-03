using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlTest;

namespace CyDSqlTest
{
    public class TestOutput: IOutput
    {
		public void PrintMessage(string Message) {
			Console.WriteLine(Message);
		}
	}
}
