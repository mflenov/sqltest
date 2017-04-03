using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SqlTest
{
    public class SqlTestCase
    {
		public string Name { get; set; }

		public string SqlQuery { get; set; }

		List<SqlTestAssert> asserts = new List<SqlTestAssert>();
		public List<SqlTestAssert> Asserts {
			get { return asserts; }
			set { asserts = value; }
		}

		public void LoadFromXElement(XElement node) {
			this.Name = (string)node.Element("name").Value;
			this.SqlQuery = (string)node.Element("sql").Value;

			foreach (var test in node.Element("asserts").Elements("assert")) {
				SqlTestAssert assert = new SqlTestAssert();
				assert.LoadFromXElement(test);
				asserts.Add(assert);
			}
		}
	}
}
