using System;
using System.Collections.Generic;
using System.Json;
using System.Linq.Expressions;
using RM.UzTicket.Lib.Model;

namespace RM.UzTicket.Console.Tests
{
	internal static class ModelTest
	{
		//private class TestClass : ModelBase
		//{
		//	private int _field = 0;

		//	public void Promote()
		//	{
		//		Salary += Salary == 0m ? 1000m : 0.05m * Salary;
		//	}
		//	/*
		//	[ModelProperty("thizz")]
		//	public string this[bool ist]
		//	{
		//		get { return _field.ToString(ist ? "X" : "D"); }
		//		set { _field = Convert.ToInt32(value, ist ? 16 : 10); }
		//	}
		//	*/
		//	private decimal Salary { get; set; }

		//	public string FirstName { get; set; }

		//	public string LastName { get; set; }

		//	public int Age { get; set; }

		//	[ModelProperty("sechs")]
		//	public bool? Gender { get; set; }

		//	public int Field => _field;

		//	public int SField
		//	{
		//		set { _field = value; }
		//	}
		//}

		private static readonly IDictionary<string, object> _props = new Dictionary<string, object>
																		{
																			{"Name", "John Smith"},
																			{"Age", 23},
																			{"Gender", true}
																		};

		//public static void Run()
		//{
		//	var json = "{\"str\":\"some string\",\"num_int\":12.34,\"num_float\":123.321,\"num_str\":\"4.32\"}";
		//	var jsonObj = JsonValue.Parse(json) as JsonObject;

		//	object str, num_int, num_float, num_str;

		//	if (jsonObj["str"].TryReadAs(typeof(string), out str))
		//	{
		//		System.Console.WriteLine($"['str'] = {(string)str}");
		//	}

		//	if (jsonObj["num_int"].TryReadAs(typeof(int), out num_int))
		//	{
		//		System.Console.WriteLine($"['num_int'] = {(int)num_int}");
		//	}

		//	if (jsonObj["num_float"].TryReadAs(typeof(double), out num_float))
		//	{
		//		System.Console.WriteLine($"['num_float'] = {(double)num_float}");
		//	}

		//	if (jsonObj["num_str"].TryReadAs(typeof(int), out num_str))
		//	{
		//		System.Console.WriteLine($"['num_str'] = {(int)num_str}");
		//	}
		//}

		public static void Run()
		{
			//var model = new TestClass
			//				{
			//					FirstName = ((string) _props["Name"]).Split(' ')[0],
			//					LastName = ((string)_props["Name"]).Split(' ')[1],
			//					Age = (int) _props["Age"],
			//					Gender = (bool?) _props["Gender"],
			//					//[false] = "123"
			//				};
			//var dict = model.ToDictionary();

			//model.Promote();
			//model.Promote();

			//dict = model.ToDictionary();

			//var model2 = dict.ToModel<TestClass>();
			//var model3 = new TestClass();
			//model3.LoadFromDictionary(dict);

			//var dic2 = model2.ToDictionary();
			//var dic3 = model3.ToDictionary();
			//var bl = dic2.EqualsTo(dic3);

			//if (!bl)
			//{
			//	throw new Exception("Dict Equals test failed:\nDictionaries must be equal");
			//}

			//dic2["salary"] = model2;

			//bl = dic2.EqualsTo(dic3);

			//if (bl)
			//{
			//	throw new Exception("Dict Equals test failed:\nDictionaries must not be equal");
			//}
		}
	}
}
