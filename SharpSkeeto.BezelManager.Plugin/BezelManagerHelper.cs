using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SharpSkeeto.BezelManager.Plugin
{
	internal class BezelManagerHelper
	{
		internal SupportedSystemBezelData GetBezelData(string filePath)
		{
			string content = string.Empty;
			using (StreamReader sr = new StreamReader(filePath))
			{
				content = sr.ReadToEnd();
			}
			return JsonSerializer.Deserialize<SupportedSystemBezelData>(content);
		}

		internal static class JsonSerializer
		{
			public static string Serialize<T>(T obj) where T : class, new()
			{
				using (MemoryStream ms = new MemoryStream())
				{
					DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
					ser.WriteObject(ms, obj);
					return Encoding.UTF8.GetString(ms.ToArray());
				}
			}

			public static T Deserialize<T>(string json) where T : class, new()
			{
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
				using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
				{
					return ser.ReadObject(stream) as T;
				}
			}
		}
	}
}
