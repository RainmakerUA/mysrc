using System;
using System.IO;
using System.Reflection;

namespace RM.Lib.Common.Settings.Providers
{
	public class DefaultFileProvider<TUser, TApp, TSerializer> : ISettingsProvider<TUser, TApp>
			where TUser : class, ICloneable
			where TApp : class, TUser, IUseLocalAppData, new()
			where TSerializer : class, IFileSerializer
	{
		private const string _localAppFolder = @"%LOCALAPPDATA%\RM\";

		private readonly TSerializer _serializer;
		private readonly Func<TApp>? _defaultAppSettings;
		private readonly string _appSettingsFile;
		private readonly string _userSettingsFile;
		private readonly TApp _appSettings;

		public DefaultFileProvider(TSerializer? serializer = null, string? appName = null, bool? useAppData = null, Func<TApp>? defaultAppSettings = null)
		{
			_defaultAppSettings = defaultAppSettings;
			_serializer = serializer ?? CreateSerializer();

			var fileName = _serializer.FileName;
			_appSettingsFile = Path.Combine(ProcessLocation.ProcessExePath ?? AppContext.BaseDirectory, fileName);
			_appSettings = GetApplicationSettingsInternal();
			_userSettingsFile = useAppData ?? _appSettings.UseLocalAppData
									? Path.Combine(GetAppDataUserSettingsPath(appName ?? GetApplicationName()), fileName)
									: _appSettingsFile;
		}

		public TApp GetApplicationSettings() => _appSettings;

		public TUser GetUserSettings()
		{
			var settings = _serializer.ReadFile<TUser>(_userSettingsFile);

			if (settings == null)
			{
				settings = _appSettings.Clone() as TUser ?? GetApplicationSettingsInternal();
				SaveUserSettings(settings);
			}

			return settings;
		}

		public void SaveUserSettings(TUser settings) => _serializer.WriteFile(_userSettingsFile, settings);

		private TApp GetApplicationSettingsInternal() => _serializer.ReadFile<TApp>(_appSettingsFile)
															?? _defaultAppSettings?.Invoke()
															?? new TApp();

		private static TSerializer CreateSerializer()
		{
			var serializerType = typeof(TSerializer);
			var ctor = serializerType.GetConstructor(Type.EmptyTypes);
			return ctor?.Invoke(null) as TSerializer
						?? throw new ArgumentException($"Cannot instantiate serializer of class {serializerType.FullName}");
		}

		private static string GetApplicationName()
		{
			var name = ProcessLocation.ProcessExeName;

			if (!String.IsNullOrEmpty(name))
			{
				return name;
			}

			name = Assembly.GetExecutingAssembly().Location ?? AppContext.BaseDirectory;

			return $"{name.GetHashCode():X8}";
		}

		private static string GetAppDataUserSettingsPath(string appName) => Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(_localAppFolder + appName)).FullName;
	}
}
