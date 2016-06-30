using System;
using System.Collections.Generic;
using RM.UzTicket.Lib.Model;

namespace RM.UzTicket.Console.Tests
{
	internal static class ModelTest
	{
		private class TestClass : ModelBase
		{
			private int _field = 0;

			public void Promote()
			{
				Salary += Salary == 0m ? 1000m : 0.05m * Salary;
			}
			/*
			[ModelProperty("thizz")]
			public string this[bool ist]
			{
				get { return _field.ToString(ist ? "X" : "D"); }
				set { _field = Convert.ToInt32(value, ist ? 16 : 10); }
			}
			*/
			private decimal Salary { get; set; }

			public string Name { get; set; }

			public int Age { get; set; }

			[ModelProperty("sechs")]
			public bool? Gender { get; set; }

			public int Field => _field;

			public int SField
			{
				set { _field = value; }
			}
		}

		private static readonly IDictionary<string, object> _props = new Dictionary<string, object>
																		{
																			{"Name", "John Smith"},
																			{"Age", 23},
																			{"Gender", true}
																		};

		public static void Run()
		{
			var model = new TestClass
							{
								Name = (string) _props["Name"],
								Age = (int) _props["Age"],
								Gender = (bool?) _props["Gender"],
								//[false] = "123"
							};
			var dict = model.ToDictionary();

			model.Promote();
			model.Promote();

			dict = model.ToDictionary();

			var model2 = dict.ToModel<TestClass>();
			var model3 = new TestClass();
			model3.LoadFromDictionary(dict);

			var dic2 = model2.ToDictionary();
			var dic3 = model3.ToDictionary();
			var bl = dic2.EqualsTo(dic3);

			if (!bl)
			{
				throw new Exception("Dict Equals test failed:\nDictionaries must be equal");
			}

			dic2["salary"] = model2;

			bl = dic2.EqualsTo(dic3);

			if (bl)
			{
				throw new Exception("Dict Equals test failed:\nDictionaries must not be equal");
			}
		}
	}
}
