using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace WPFShared
{
	// Token: 0x02000045 RID: 69
	public static class XmlExtensions
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x00008D44 File Offset: 0x00006F44
		public static T GetElementValue<T>(this XmlElement element, string name, T defaultValue = default(T))
		{
			Type type = typeof(T);
			XmlElement xmlElement = (from XmlNode o in element.SelectNodes(name)
				where o is XmlElement
				select o).Cast<XmlElement>().FirstOrDefault<XmlElement>();
			if (xmlElement == null || string.IsNullOrWhiteSpace(xmlElement.InnerText))
			{
				return defaultValue;
			}
			if (type.IsEnum)
			{
				try
				{
					return (T)((object)Enum.Parse(type, xmlElement.InnerText));
				}
				catch
				{
					return defaultValue;
				}
			}
			if (type == typeof(string))
			{
				return (T)((object)xmlElement.InnerText);
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments().Single<Type>();
			}
			MethodInfo methodInfo = type.GetMethods().SingleOrDefault((MethodInfo o) => o.Name == "Parse" && o.GetParameters().Count<ParameterInfo>() == 1 && o.GetParameters()[0].ParameterType == typeof(string));
			if (methodInfo == null)
			{
				throw new Exception("Specified type does not implement Parse method.");
			}
			return (T)((object)methodInfo.Invoke(type, new object[] { xmlElement.InnerText }));
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008E64 File Offset: 0x00007064
		public static XmlElement NewElement(this XmlDocument doc, string name, object value)
		{
			XmlElement xmlElement = doc.CreateElement(name);
			if (value is DateTime)
			{
				xmlElement.InnerText = ((DateTime)value).ToString("s");
			}
			else if (value is DateTime? && (DateTime?)value != null)
			{
				xmlElement.InnerText = ((DateTime?)value).Value.ToString("s");
			}
			else if (value != null)
			{
				xmlElement.InnerText = value.ToString();
			}
			return xmlElement;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008EE8 File Offset: 0x000070E8
		public static void FormatToFile(this XmlDocument doc, string filename, int retries = 3, int retryWait = 250)
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.NewLineChars = Environment.NewLine;
			StreamWriter streamWriter = null;
			bool flag = false;
			while (retries-- > 0 && !flag)
			{
				try
				{
					streamWriter = new StreamWriter(filename);
					flag = true;
				}
				catch
				{
					Thread.Sleep(retryWait);
				}
			}
			if (!flag)
			{
				throw new IOException(string.Format("Could not open '{0}' for write.", filename));
			}
			XmlWriter xmlWriter = XmlWriter.Create(streamWriter, xmlWriterSettings);
			doc.Save(xmlWriter);
			xmlWriter.Close();
			streamWriter.Close();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008F74 File Offset: 0x00007174
		public static MemoryStream FormatToMemoryStream(this XmlDocument doc)
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.NewLineChars = Environment.NewLine;
			MemoryStream memoryStream = new MemoryStream();
			XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			doc.Save(xmlWriter);
			xmlWriter.Flush();
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00008FC0 File Offset: 0x000071C0
		public static string FormatToString(this XmlDocument doc)
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.NewLineChars = Environment.NewLine;
			StringWriter stringWriter = new StringWriter();
			XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
			doc.Save(xmlWriter);
			string text = stringWriter.ToString();
			xmlWriter.Close();
			stringWriter.Close();
			stringWriter.Dispose();
			return text;
		}
	}
}
