﻿
namespace RM.Lib.Common.Settings.Providers
{
	public interface IFileSerializer
	{
		string FileName { get; }

		T? ReadFile<T>(string fileName);

		void WriteFile(string fileName, object data);
	}
}
