﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RM.UzTicket.Bot
{
	internal class SettingsProvider
	{
		private const string _varPrefix = "UZTB_";
#if DEBUG
		private const string _env = ".env";
		private const string _envPattern = @"^\s*{0}\s*=\s*(?'val'\S*)\s*$";
#endif
		private static readonly string[] _varNames;
		
		private readonly IDictionary<string, string> _variables = new Dictionary<string, string>();

		static SettingsProvider()
		{
			_varNames = new[]
						{
							"PROXYSRC",
							"PROXYSCRIPTPATH",
							"PROXYPATH",
							"PROXYRE",
							
							"TELEBOTKEY",
							"TELEMASTER",
							
							"REDISURL",
							"REDISPWD",
							
							"UZBASEURL",
							"UZSESSIONCOOKIE",
							"UZSCANDELAY",
							"TEMP_SCAN"
						}.Select(s => $"{_varPrefix}{s}").ToArray();
		}

		public SettingsProvider()
		{
			foreach (var varName in _varNames)
			{
				_variables.Add(varName, Environment.GetEnvironmentVariable(varName));
			}
		}

//#if DEBUG
		public SettingsProvider(string envContent)
		{
			foreach (var varName in _varNames)
			{
				_variables.Add(varName, GetMatch(envContent, String.Format(_envPattern, varName)));
			}
		}

		private static string GetMatch(string content, string pattern)
		{
			var re = new Regex(pattern, RegexOptions.Multiline);
			var match = re.Match(content);

			return match.Success ? match.Groups["val"].Value : null;
		}
//#endif
		
		public SettingsData GetSettings()
		{
			return new SettingsData()
						.SetProxySettings(
									GetVariable("PROXYSRC"), GetVariable("PROXYSCRIPTPATH"),
									GetVariable("PROXYPATH"), GetVariable("PROXYRE")
								)
						.SetTelegramSettings(GetVariable("TELEBOTKEY"), GetVariable("TELEMASTER"))
						.SetPersistenceSettings(GetVariable("REDISURL"), GetVariable("REDISPWD"))
						.SetUzSettings(GetVariable("UZBASEURL"), GetVariable("UZSESSIONCOOKIE"), GetVariable("UZSCANDELAY"), GetVariable("TEMP_SCAN"));
		}

		private string GetVariable(string name)
		{
			return _variables.TryGetValue(_varPrefix + name, out var value) ? value : null;
		}

		public static SettingsProvider Load()
		{
//#if DEBUG
			var envFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _env);

			if (File.Exists(envFile))
			{
				return new SettingsProvider(File.ReadAllText(envFile));
			}
//#endif
			return new SettingsProvider();
		}
	}
}
