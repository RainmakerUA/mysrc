using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace RM.InputRobot.Modules
{
	internal sealed class Logger
	{
		public enum Level
		{
			None = 0,
			Error,
			Warning,
			Info,
			Debug
		}

		private const Level _maxLevel = Level.Debug; //TODO: Move out to settings

		private const string _logEntryFormat = "{0:yyyy-MM-dd HH:mm:ss,fff} {1} {2} :: {3}{4}";

		private static readonly string _newLine = Environment.NewLine;

		private readonly bool _useUtcTime;

		private readonly string _sourceName;

		private readonly string _fileName;

		public Logger(string sourceName)
			: this(sourceName, null)
		{
		}

		public Logger(string sourceName, string fileName)
		{
			_useUtcTime = true; //TODO: Add field or hardcode to always use UTC
			_sourceName = sourceName;
			_fileName = fileName ?? GetDefaultLogFileName();

			LogInternal(Level.Debug, "Logger", _fileName, _useUtcTime, _sourceName + " - Logger is created.");
		}

		public void Log(Level level, string message)
		{
			LogInternal(level, _sourceName, _fileName, _useUtcTime, message);
		}

		public void Log(Level level, string format, params object[] args)
		{
			Log(level, String.Format(format, args));
		}

		public void Log(Level level, Exception exception)
		{
			Log(level, String.Format(
									"{0}: {1}{2}{3}",
									exception.GetType().FullName,
									exception.Message,
									_newLine,
									exception.StackTrace
								));
		}

		private static string GetLevelCode(Level level)
		{
			var levelName = level.ToString().ToUpperInvariant();
			return levelName.Length > 5 ? levelName.Substring(0, 4) : levelName;
		}

		private static string GetDefaultLogFileName()
		{
			var exeName = Assembly.GetEntryAssembly().Location;
			return Path.ChangeExtension(exeName, "log");
		}

		private static void LogInternal(Level level, string source, string fileName, bool useUtcTime, string message)
		{
			try
			{
				if (level <= _maxLevel)
				{
					var entry = String.Format(
											_logEntryFormat,
											useUtcTime ? DateTime.UtcNow : DateTime.Now,
											GetLevelCode(level),
											source,
											message,
											_newLine
										);
					File.AppendAllText(fileName, entry, Encoding.UTF8);
				}
			}
			catch (Exception)
			{
				// Suppress all exceptions
			}
		}
	}
}
