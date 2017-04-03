using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SqlTest
{
    public class SqlTestAssert
    {
		public enum SqlAssertType { Rownum, Value }

		public string Name { get; set; }

		public SqlAssertType AssertType { get; set; }

		public SqlTestDataType DataType { get; set; }

		public string Value { get; set; }

		public void LoadFromXElement(XElement node) {
			this.Name = (string)node.Attribute("name").Value;
			this.AssertType = (SqlAssertType)Enum.Parse(typeof(SqlAssertType), (string)node.Attribute("type"));
			this.DataType = (SqlTestDataType)Enum.Parse(typeof(SqlTestDataType), (string)node.Attribute("datatype"));
			this.Value = (string)node.Value;
		}
	}
}