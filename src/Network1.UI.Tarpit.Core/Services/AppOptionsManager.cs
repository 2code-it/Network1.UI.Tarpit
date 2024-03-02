using Network1.UI.Tarpit.Core.Models;
using Network1.UI.Tarpit.Core.Services.Internals;
using System;
using System.Text.Json;

namespace Network1.UI.Tarpit.Core.Services
{
	public class AppOptionsManager : IAppOptionsManager
	{
		public AppOptionsManager(IObjectMapper objectMapper) : this(objectMapper, new FileSystem()) { }
		internal AppOptionsManager(IObjectMapper objectMapper, IFileSystem fileSystem)
		{
			_objectMapper = objectMapper;
			_fileSystem = fileSystem;

			LoadFromFile();
		}

		private readonly IObjectMapper _objectMapper;
		private readonly IFileSystem _fileSystem;

		private const string _sourceFilePath = $"./{nameof(AppOptions)}.json";
		private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { WriteIndented = true };

		private AppOptions _options = new AppOptions();


		public void Load(object target, Type? alternateTargetType = null)
		{
			Type targetType = alternateTargetType ?? target.GetType();

			object source = true switch
			{
				true when typeof(TarpitOptions) == targetType => _options.TarpitOptions,
				true when typeof(ViewOptions) == targetType => _options.ViewOptions,
				true when typeof(ConnectionLogOptions) == targetType => _options.ConnectionLogOptions,
				_ => throw new InvalidOperationException($"Type '{target.GetType()}' not supported")
			};
			_objectMapper.Map(source, target);
		}

		public void Save(object source)
		{
			object target = source switch
			{
				TarpitOptions => _options.TarpitOptions,
				ViewOptions => _options.ViewOptions,
				ConnectionLogOptions => _options.ConnectionLogOptions,
				_ => throw new InvalidOperationException($"Type '{source.GetType()}' not supported")
			};
			_objectMapper.Map(source, target);
			SaveToFile();
		}

		private void LoadFromFile()
		{
			string filePath = _fileSystem.PathGetFullPath(_sourceFilePath);
			string json = _fileSystem.FileReadAllText(filePath);
			_options = JsonSerializer.Deserialize<AppOptions>(json, _serializerOptions)!;
		}

		private void SaveToFile()
		{
			string filePath = _fileSystem.PathGetFullPath(_sourceFilePath);
			string json = JsonSerializer.Serialize(_options, _serializerOptions);
			_fileSystem.FileWriteAllText(filePath, json);
		}

	}
}
