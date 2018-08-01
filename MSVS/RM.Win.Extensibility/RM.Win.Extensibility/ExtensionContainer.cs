using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using RM.Win.Extensibility.Contracts;

namespace RM.Win.Extensibility
{
	internal sealed class ExtensionContainer<T> : MarshalByRefObject, IDisposable
	{
		public sealed class AsmLoader : MarshalByRefObject
		{
			public Assembly LoadAssembly(string asmPath)
			{
				return Assembly.LoadFrom(asmPath);
			}
		}

		private static readonly Type _thisGenericType;
		private static readonly Type _iExtGenericType;
		private static readonly Type _typeParam;
		private static readonly Type _iExtType;
		private static readonly Assembly _entryAssembly;
		private static readonly StrongName[] _fullTrustAssemblies;

		static ExtensionContainer()
		{
			_thisGenericType = typeof(ExtensionContainer<>);
			_iExtGenericType = typeof(IExtension<>);
			_typeParam = typeof(T);
			_iExtType = typeof(IExtension<T>);
			_entryAssembly = Assembly.GetEntryAssembly();
			_fullTrustAssemblies = new[]
									{
										GetAssemblyStrongName(_entryAssembly),
										GetTypeAssemblyStrongName(_thisGenericType),
										GetTypeAssemblyStrongName(_iExtGenericType),
										GetTypeAssemblyStrongName(_typeParam)
									};
		}

		private readonly AppDomain _domain;
		private bool _isDisposed;
		private IReadOnlyList<string> _names;
		private IReadOnlyDictionary<string, IExtension<T>> _extensions;
		
		public ExtensionContainer(string assemblyPath, T app)
		{
			_domain = CreateRestrictedDomain(GetAssemblyName(assemblyPath));
			LoadExtensionAssembly(assemblyPath, app);
		}

		public IExtension<T> Extension { get; private set; }

		public IReadOnlyList<string> Names => _names;

		public IExtension<T> this[string name] => _extensions.TryGetValue(name, out var ext) ? ext : null;

		private static string GetAssemblyName(string assemblyPath)
		{
			return Path.GetFileNameWithoutExtension(assemblyPath);
		}

		private static AppDomain CreateRestrictedDomain(string name /*,permissions*/)
		{
			var appDir = Path.GetDirectoryName(_entryAssembly.Location);
			var adSetup = new AppDomainSetup
							{
								ApplicationBase = appDir,
								PrivateBinPath = appDir,
							};
			var permissions = new PermissionSet(PermissionState.None);
			permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			//permissions.AddPermission(new FileIOPermission(PermissionState.None) {AllLocalFiles = FileIOPermissionAccess.AllAccess});

			return AppDomain.CreateDomain(name, null, adSetup, permissions, _fullTrustAssemblies);
		}
		
		private static StrongName GetTypeAssemblyStrongName(Type type)
		{
			return GetAssemblyStrongName(type.Assembly);
		}

		private static StrongName GetAssemblyStrongName(Assembly assembly)
		{
			return assembly.Evidence.GetHostEvidence<StrongName>();
		}

		private void LoadExtensionAssembly(string assemblyPath, T app)
		{
			//var loader = (AsmLoader)_domain.CreateInstanceAndUnwrap(_thisGenericType.Assembly.FullName, typeof(AsmLoader).FullName);
			var asm = _domain.Load(File.ReadAllBytes(assemblyPath));//_domain.Load(new AssemblyName() { CodeBase = assemblyPath });//loader.LoadAssembly(assemblyPath);
			var extensions = new Dictionary<string, IExtension<T>>();

			foreach (var type in asm.ExportedTypes.Where(t => Array.IndexOf(t.GetInterfaces(), _iExtType) >= 0))
			{
				var typeName = type.FullName;

				try
				{
					var ext = _domain.CreateInstanceAndUnwrap(asm.FullName, typeName) as IExtension<T>;

					if (ext != null)
					{
						ext.OnInitializing(app);
						extensions.Add(typeName, ext);
					}
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine($"Cannot instantiate {typeName}:\r\n{e}");
				}
			}

			foreach (var ext in extensions.Values)
			{
				ext.OnInitialized();
			}

			_extensions = new ReadOnlyDictionary<string, IExtension<T>>(extensions);
		}

		private void UnloadDomain()
		{
			// Clear plugin references
			_names = null;
			_extensions = null;

			_isDisposed = true;
			AppDomain.Unload(_domain);
		}

		public void Dispose()
		{
			UnloadDomain();
			GC.SuppressFinalize(this);
		}

		~ExtensionContainer()
		{
			UnloadDomain();
		}
	}

	internal static class ExtensionContainer
	{
		public static ExtensionContainer<TApp> LoadExtensionAssembly<TApp>(string assemblyPath, TApp app)
		{
			return new ExtensionContainer<TApp>(assemblyPath, app);
		}
	}
}
