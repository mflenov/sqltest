using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SqlTest
{
    public class TestParameter
    {
		public string Name { get; set; }

		public SqlTestDataType DataType { get; set; }

		public string Value { get; set; }

		public void LoadFromXElement(XElement node) {
			Name = (string)node.Attribute("name");
			Value = (string)node.Value;
			DataType = (SqlTestDataType)Enum.Parse(typeof(SqlTestDataType), (string)node.Attribute("type"));
		}
    }
}
