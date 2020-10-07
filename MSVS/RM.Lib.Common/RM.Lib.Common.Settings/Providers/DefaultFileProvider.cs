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
		private const string _localAppFolder = "%LOCALAPPDATA%\\RM\\";

		private readonly TSerializer _serializer;
		private readonly Func<TApp> _defaultAppSettings;
		private readonly string _appSettingsFile;
		private readonly string _userSettingsFile;
		private readonly TApp _appSettings;

		public DefaultFileProvider(TSerializer serializer = null, string appName = null, bool? useAppData = null, Func<TApp> defaultAppSettings = null)
		{
			var entryAssembly = Assembly.GetEntryAssembly();
			_defaultAppSettings = defaultAppSettings;
			_serializer = serializer ?? CreateSerializer();

			var fileName = _serializer.FileName;
			_appSettingsFile = Path.Combine(GetAppSettingsPath(entryAssembly), fileName);
			_appSettings = GetApplicationSettingsInternal();
			_userSettingsFile = useAppData ?? _appSettings.UseLocalAppData
									? Path.Combine(GetAppDataUserSettingsPath(appName ?? GetApplicationName(entryAssembly)), fileName)
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

		private TApp GetApplicationSettingsInternal() => _serializer.ReadFile<TApp>(_appSettingsFile) ?? _defaultAppSettings?.Invoke() ?? new TApp();

		private static TSerializer CreateSerializer()
		{
			var serializerType = typeof(TSerializer);
			var ctor = serializerType.GetConstructor(Array.Empty<Type>());
			return ctor?.Invoke(null) as TSerializer ?? throw new ArgumentException($"Cannot instantiate serializer of class {serializerType.FullName}");
		}

		private static string GetApplicationName(Assembly entryAssembly)
		{
			var name = entryAssembly?.GetName().Name;

			if (!String.IsNullOrEmpty(name))
			{
				return name;
			}

			name = Assembly.GetExecutingAssembly().Location;

			return $"{name.GetHashCode():X8}";
		}

		private static string GetAppSettingsPath(Assembly entryAssembly)
		{
			return Path.GetDirectoryName((entryAssembly ?? Assembly.GetExecutingAssembly()).Location);
		}

		private static string GetAppDataUserSettingsPath(string appName) => Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(_localAppFolder + appName)).FullName;
	}
}
