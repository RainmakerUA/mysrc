using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace RM.Win.LuaScripting
{
	internal static class ProxyParser
	{
		private const string _filterMapFunctions = @"
			function filter(t, filterFunc)
			local res = {}
			for k, v in pairs(t) do
				if filterFunc(v, k, t) then
					if type(k) == ""number"" then
						tinsert(res, v)
					else
						res[k] = v
					end
				end
			end
			return res
		end

		function map(t, mapFunc)
			local res = { }
			for k, v in pairs(t) do
				res[k] = mapFunc(v, k, t)
			end
			return res
		end
		";

		static ProxyParser()
		{
			Script.WarmUp();
		}

		public static IEnumerable<string> GetProxies(string url, string regex, string luaCode)
		{
			var content = new WebClient().DownloadString(url);
			Func<Script, DynValue> inputProvider;

			if (String.IsNullOrWhiteSpace(regex))
			{
				if (String.IsNullOrWhiteSpace(luaCode))
				{
					throw new InvalidOperationException("Neither regex nor script are provided.\r\nCannot parse input!");
				}

				inputProvider = script => DynValue.NewString(content);
			}
			else
			{
				var re = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(10));
				var successfullMatches = re.Matches(content).Cast<Match>().Where(m => m.Success);

				if (String.IsNullOrWhiteSpace(luaCode))
				{
					return successfullMatches.Select(m => m.Value);
				}

				inputProvider = RegexMatchInputProvider(content, successfullMatches);
			}

			return RunLuaParser(luaCode, inputProvider);

			Func<Script, DynValue> RegexMatchInputProvider(string text, IEnumerable<Match> matches)
			{
				return script =>
				{
					var matchTable = new Table(script);

					foreach (var m in matches)
					{
						var groups = m.Groups;

						if (groups.Count > 1)
						{
							var table = new Table(script);

							for (var i = 1; i < groups.Count; i++)
							{
								table.Set(groups[i].Name, DynValue.NewString(groups[i].Value));
							}

							matchTable.Append(DynValue.NewTable(table));
						}
						else
						{
							matchTable.Append(DynValue.NewString(m.Value));
						}
					}

					return DynValue.NewTable(matchTable);
				};
			}
		}

		private static IEnumerable<string> RunLuaParser(string luaCode, Func<Script, DynValue> getInput)
		{
			var lua = new Script(CoreModules.Preset_HardSandbox);
			DynValue result;

			lua.Options.DebugPrint = ShowMessage;
			lua.DoString(_filterMapFunctions);
			lua.Globals["re_match"] = new Func<string, string, object>(RegexMatch);

			try
			{
				var func = lua.DoString(String.Concat("return function(input)\r\n", luaCode, "\r\nend"));
				var input = getInput(lua);
				result = lua.Call(func, input);
			}
			catch (SyntaxErrorException e)
			{
				throw new Exception($"Syntax error:\r\n{e.Message}", e);
			}
			catch (ScriptRuntimeException e)
			{
				throw new Exception($"Script error:\r\n{e.Message}", e);
			}

			switch (result.Type)
			{
				case DataType.Tuple:
					var tuple = result.Tuple;

					if (tuple.Length == 2 && tuple[0].Type == DataType.Boolean && tuple[1].Type == DataType.Table)
					{
						return tuple[0].Boolean
								? ParseResult(tuple[1].Table.Values)
								: throw new NotSupportedException("Lua parser reported failure!");
					}
					goto default;

				case DataType.Table:
					return ParseResult(result.Table.Values);

				default:
					throw new NotSupportedException("Lua result is not supported!");
			}

			IEnumerable<string> ParseResult(IEnumerable<DynValue> dynValues)
			{
				return dynValues.Select(dyn => dyn.CastToString());
			}
		}

		private static void ShowMessage(string message)
		{
			System.Windows.MessageBox.Show(
							System.Windows.Application.Current.MainWindow,
							message, "Lua print output",
							System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information
						);
		}

		private static object RegexMatch(string source, string pattern)
		{
			var re = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(10));

			var matches = new List<object>();

			foreach (var m in re.Matches(source).Cast<Match>().Where(m => m.Success))
			{
				var groups = m.Groups;

				if (groups.Count > 1)
				{
					var dict = new Dictionary<string, string>();

					for (var i = 1; i < groups.Count; i++)
					{
						dict.Add(groups[i].Name, groups[i].Value);
					}

					matches.Add(dict);
				}
				else
				{
					matches.Add(m.Value);
				}
			}

			return matches.ToArray();
		}
	}
}
