using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SqlTest
{
    public class SqlUnitTest
    {
		public string filename;
		TestProject project;
		IOutput output;

		public SqlUnitTest(string filename, IOutput output) {
			this.filename = filename;
			this.output = output;
			LoadFile();
		}

		void LoadFile() {
			project = new TestProject();

			var doc = XDocument.Load(filename);
			project.LoadFromXElement(doc.Root);
		}

		public void RunTests() {
			output.PrintMessage("Starting test");
			project.RunTests(output);
		}
	}
}
