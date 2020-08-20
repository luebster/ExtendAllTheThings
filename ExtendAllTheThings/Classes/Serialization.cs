using Microsoft.AspNetCore.Hosting;

using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace ExtendAllTheThings.Classes
{
	public static class Serialization
	{
		public static string Serialize<T>(T data, SerializeFormat format)
		{
			return format switch
			{
				SerializeFormat.XML => SerializeToXml(data),
				SerializeFormat.JSON => SerializeToJson(data),
				_ => string.Empty,
			};
		}

		public static T Deserialize<T>(string data, SerializeFormat format)
		{
			return format switch
			{
				SerializeFormat.XML => DeserializeToXml<T>(data),
				SerializeFormat.JSON => DeserializeFromJson<T>(data),
				_ => default,
			};
		}

		/*public static string SerializeToXml<T>(T data)
		{
				var serializer = new DataContractSerializer(data.GetType());
				var builder = new StringBuilder();
				var writer = XmlWriter.Create(builder);
				serializer.WriteObject(writer, data);
				writer.Flush();
				return builder.ToString();
		}*/

		private static string SerializeToXml<T>(T data)
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			using var stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, data);
			return stringWriter.ToString();
		}

		/*public static T DeserializeToXml<T>(string data)
		{
				var serializer = new DataContractSerializer(data.GetType());
				var writer = XmlReader.Create(GenerateStreamFromString(data));
				var result = serializer.ReadObject(writer);
				return (T)result;
		}*/

		private static T DeserializeToXml<T>(string data)
		{
			var xmlSerializer = new XmlSerializer(data.GetType());
			Stream stream = GenerateStreamFromString(data);
			var result = xmlSerializer.Deserialize(stream);
			return (T)result;
		}

		private static Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			using (var writer = new StreamWriter(stream))
			{
				writer.Write(s);
				writer.Flush();
			}
			stream.Position = 0;
			return stream;
		}

		private static string SerializeToJson<T>(T data)
		{
			return JsonSerializer.Serialize(data);
		}

		private static T DeserializeFromJson<T>(string data)
		{
			return JsonSerializer.Deserialize<T>(data);
		}

		public static T Deserialize<T>(string fileLocation, IHostingEnvironment _env)
		{
			var webRoot = _env.WebRootPath;
			var file = Path.Combine(webRoot, fileLocation);
			var jsonString = File.ReadAllText(file);

			return Deserialize<T>(jsonString, SerializeFormat.JSON);
		}
	}

	public enum SerializeFormat
	{
		XML = 1,
		JSON = 2
	}
}
