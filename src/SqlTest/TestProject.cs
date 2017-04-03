using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SqlTest
{
    public class TestProject
    {
		public string Name { get; set; }

		public string DatabaseConnectionString { get; set; }

		public string Sql { get; set; }

		private List<TestParameter> parameters = new List<TestParameter>();
		public List<TestParameter> Parameters {
			get { return parameters; }
			set { parameters = value; }
		}

		public string Init { get; set; }

		List<SqlTestCase> testcases = new List<SqlTestCase>();
		public List<SqlTestCase> Testcases {
			get { return testcases; }
			set { testcases = value; }
		}

		public void LoadFromXElement(XElement documentRoot) {
			this.Name = (string)documentRoot.Element("configuration").Element("name").Value;
			this.DatabaseConnectionString = (string)documentRoot.Element("configuration").Element("connectionString").Value;
			this.Sql = (string)documentRoot.Element("configuration").Element("sql").Value;
			foreach (var param in documentRoot.Element("configuration").Elements("parameters").Elements("parameter")) {
				TestParameter testparam = new TestParameter();
				testparam.LoadFromXElement(param);
				parameters.Add(testparam);
			}

			this.Init = (string)documentRoot.Element("init").Value;

			foreach (var test in documentRoot.Element("testcases").Elements("testcase")) {
				SqlTestCase testcase = new SqlTestCase();
				testcase.LoadFromXElement(test);
				Testcases.Add(testcase);
			}
		}

		private void InitTestCase(SqlConnection connection) {
			if (String.IsNullOrEmpty(this.Init))
				return;
			using (var cmd = new SqlCommand { CommandText = this.Init, CommandType = CommandType.Text, Connection = connection }) {
				ApplyParameters(cmd);
				cmd.ExecuteNonQuery();
			}
		}

		void ApplyParameters(SqlCommand cmd) {
			foreach (var p in this.parameters) {
				cmd.Parameters.Add(new SqlParameter() { ParameterName = p.Name, Value = p.Value });
			}
		}

		public void RunTests(IOutput output) {
			foreach (SqlTestCase testCase in testcases) {
				using (SqlConnection connection = new SqlConnection(this.DatabaseConnectionString)) {
					connection.Open();
					InitTestCase(connection);

					SqlTransaction transaction = connection.BeginTransaction();

					SqlCommand initCommand = new SqlCommand { CommandText = testCase.SqlQuery, CommandType = CommandType.Text, Connection = connection, Transaction = transaction };
					ApplyParameters(initCommand);
					initCommand.ExecuteNonQuery();

					using (var testCmd = new SqlCommand {CommandText = this.Sql, CommandType = CommandType.Text, Connection = connection, Transaction = transaction }) {
						ApplyParameters(testCmd);


						using (SqlDataReader reader = testCmd.ExecuteReader()) {
							int failedCases = 0;
							if (reader.Read()) {
								foreach (var assert in testCase.Asserts) {
									if (assert.AssertType == SqlTestAssert.SqlAssertType.Rownum) {
									}
									if (assert.AssertType == SqlTestAssert.SqlAssertType.Value) {
										string result = reader.GetString(0);
										if (result != assert.Value) {
											output.PrintMessage("FAIL: " + testCase.Name + ": Expected result: '" + assert.Value + "' actual result: '" + result + "'");
											failedCases++;
										}
									}
								}

							}
							if (failedCases == 0) {
								output.PrintMessage("SUCCESS: " + testCase.Name);
							}
						}
					}

					transaction.Rollback();
				}
			}
		}
	}
}
