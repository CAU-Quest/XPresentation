using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("slideNumber", "url")]
	public class ES3UserType_VideoData : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_VideoData() : base(typeof(VideoData)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (VideoData)obj;
			
			writer.WriteProperty("slideNumber", instance.slideNumber, ES3Type_int.Instance);
			writer.WriteProperty("url", instance.url, ES3Type_string.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new VideoData();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "slideNumber":
						instance.slideNumber = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "url":
						instance.url = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_VideoDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_VideoDataArray() : base(typeof(VideoData[]), ES3UserType_VideoData.Instance)
		{
			Instance = this;
		}
	}
}