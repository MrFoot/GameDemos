
using System;
using FootStudio.Threading;
using UnityEngine;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using SimpleJson;
using System.Xml.Serialization;
using System.IO;


namespace FootStudio.Util
{
	public static class UserPrefs
	{
		private const string Tag = "UserPrefs";
		
		private const string EmptyCollectionValue = "eric,soul,Game,Test,Bear,l";
		
		private static bool SavePending;
		
		public static MainThread MainExecutor
		{
			get;
			set;
		}
		
		public static void Clear()
		{
			PlayerPrefs.DeleteAll();
		}
		
		public static void Remove(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}
		
		public static void RemoveHash(string key)
		{
			UserPrefs.Remove(UserPrefs.CreateHashKey(key));
		}
		
		public static bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}
		
		public static void Save()
		{
			if (UserPrefs.MainExecutor != null && UserPrefs.SavePending)
			{
				UserPrefs.MainExecutor.RemoveAllSchedules(new Action(UserPrefs.Save));
			}
			PlayerPrefs.Save();
			UserPrefs.SavePending = false;
		}
		
		public static void SaveDelayed()
		{
			UserPrefs.SaveDelayed(1.2f);
		}
		
		public static void SaveDelayed(float delaySecs)
		{
			if (UserPrefs.MainExecutor != null)
			{
				if (UserPrefs.SavePending)
				{
					return;
				}
				UserPrefs.MainExecutor.PostDelayed(new Action(UserPrefs.Save), (double)delaySecs);
				UserPrefs.SavePending = true;
			}
			else
			{
				UserPrefs.Save();
			}
		}
		
		private static string CreateHashKey(string key)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
		}
		
		private static string CreateHashValue(string value, string hashSuffix)
		{
			return (!StringUtils.HasText(value)) ? null : CryptoUtils.Sha1(value + hashSuffix);
		}
		
		public static bool GetBool(string key, bool defaultValue)
		{
			if (UserPrefs.HasKey(key))
			{
				return PlayerPrefs.GetInt(key) >= 1;
			}
			return defaultValue;
		}
		
		public static int GetInt(string key, int defaultValue)
		{
			if (UserPrefs.HasKey(key))
			{
				return PlayerPrefs.GetInt(key);
			}
			return defaultValue;
		}
		
		public static long GetLong(string key, long defaultValue)
		{
			if (UserPrefs.HasKey(key))
			{
				string @string = PlayerPrefs.GetString(key);
				try
				{
					long result = long.Parse(@string, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
					return result;
				}
				catch (FormatException)
				{
					long result = defaultValue;
					return result;
				}
				catch (OverflowException)
				{
					long result = defaultValue;
					return result;
				}
				return defaultValue;
			}
			return defaultValue;
		}
		
		public static float GetFloat(string key, float defaultValue)
		{
			if (UserPrefs.HasKey(key))
			{
				return PlayerPrefs.GetFloat(key);
			}
			return defaultValue;
		}
		
		public static double GetDouble(string key, double defaultValue)
		{
			if (UserPrefs.HasKey(key))
			{
				string @string = PlayerPrefs.GetString(key);
				try
				{
					double result = double.Parse(@string, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
					return result;
				}
				catch (FormatException)
				{
					double result = defaultValue;
					return result;
				}
				catch (OverflowException)
				{
					double result = defaultValue;
					return result;
				}
				return defaultValue;
			}
			return defaultValue;
		}
		
		public static string GetString(string key, string defaultValue)
		{
			if (UserPrefs.HasKey(key))
			{
				return PlayerPrefs.GetString(key);
			}
			return defaultValue;
		}
		
		public static DateTime GetDateTime(string key, DateTime defaultValue)
		{
			return DateTime.FromBinary(UserPrefs.GetLong(key, defaultValue.ToBinary()));
		}
		
		public static JsonObject GetJson(string key, JsonObject defaultValue)
		{
			if (UserPrefs.HasKey(key))
			{
				string @string = PlayerPrefs.GetString(key);
				try
				{
					JsonObject result = SimpleJson.SimpleJson.DeserializeObject<JsonObject> (@string);
					return result;
				}
				catch
				{
					JsonObject result = defaultValue;
					return result;
				}
				return defaultValue;
			}
			return defaultValue;
		}
		
		public static List<string> GetCollectionAsList(string key, List<string> defaultCollection)
		{
			if (!UserPrefs.HasKey(key))
			{
				return defaultCollection;
			}
			string @string = PlayerPrefs.GetString(key);
			if (@string == EmptyCollectionValue)
			{
				return new List<string>(0);
			}
			return StringUtils.CommaDelimitedListToStringList(@string);
		}
		
		public static LinkedList<string> GetCollectionAsLinkedList(string key, LinkedList<string> defaultCollection)
		{
			if (!UserPrefs.HasKey(key))
			{
				return defaultCollection;
			}
			string @string = PlayerPrefs.GetString(key);
			if (@string == EmptyCollectionValue)
			{
				return new LinkedList<string>();
			}
			List<string> collection = StringUtils.CommaDelimitedListToStringList(@string);
			return new LinkedList<string>(collection);
		}
		
		public static HashSet<string> GetCollectionAsHashSet(string key, HashSet<string> defaultCollection)
		{
			if (!UserPrefs.HasKey(key))
			{
				return defaultCollection;
			}
			string @string = PlayerPrefs.GetString(key);
			if (@string == EmptyCollectionValue)
			{
				return new HashSet<string>();
			}
			List<string> collection = StringUtils.CommaDelimitedListToStringList(@string);
			return new HashSet<string>(collection);
		}
		
		public static Stack<string> GetCollectionAsStack(string key, Stack<string> defaultCollection)
		{
			if (!UserPrefs.HasKey(key))
			{
				return defaultCollection;
			}
			string @string = PlayerPrefs.GetString(key);
			if (@string == EmptyCollectionValue)
			{
				return new Stack<string>(0);
			}
			List<string> collection = StringUtils.CommaDelimitedListToStringList(@string);
			return new Stack<string>(collection);
		}
		
		public static Queue<string> GetCollectionAsQueue(string key, Queue<string> defaultCollection)
		{
			if (!UserPrefs.HasKey(key))
			{
				return defaultCollection;
			}
			string @string = PlayerPrefs.GetString(key);
			if (@string == EmptyCollectionValue)
			{
				return new Queue<string>(0);
			}
			List<string> collection = StringUtils.CommaDelimitedListToStringList(@string);
			return new Queue<string>(collection);
		}
		
		public static bool CheckHash(string originalKey, string value, string hashSuffix)
		{
			if (!StringUtils.HasText(value))
			{
				return true;
			}
			string @string = UserPrefs.GetString(UserPrefs.CreateHashKey(originalKey), null);
			string a = UserPrefs.CreateHashValue(value, hashSuffix);
			return a == @string;
		}
		
		public static void SetBool(string key, bool value)
		{
			PlayerPrefs.SetInt(key, (!value) ? 0 : 1);
		}
		
		public static bool? SetBoolAndReturnPrevious(string key, bool value)
		{
			bool? result = null;
			if (UserPrefs.HasKey(key))
			{
				result = new bool?(PlayerPrefs.GetInt(key) >= 1);
			}
			UserPrefs.SetBool(key, value);
			return result;
		}
		
		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}
		
		public static int? SetIntAndReturnPrevious(string key, int value)
		{
			int? result = null;
			if (UserPrefs.HasKey(key))
			{
				result = new int?(PlayerPrefs.GetInt(key));
			}
			UserPrefs.SetInt(key, value);
			return result;
		}
		
		public static void SetLong(string key, long value)
		{
			PlayerPrefs.SetString(key, StringUtils.ToUniString(value));
		}
		
		public static long? SetLongAndReturnPrevious(string key, long value)
		{
			long? result = null;
			if (UserPrefs.HasKey(key))
			{
				string @string = PlayerPrefs.GetString(key);
				try
				{
					result = new long?(long.Parse(@string, NumberStyles.Integer, NumberFormatInfo.InvariantInfo));
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			UserPrefs.SetLong(key, value);
			return result;
		}
		
		public static void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}
		
		public static float? SetFloatAndReturnPrevious(string key, float value)
		{
			float? result = null;
			if (UserPrefs.HasKey(key))
			{
				result = new float?(PlayerPrefs.GetFloat(key));
			}
			UserPrefs.SetFloat(key, value);
			return result;
		}
		
		public static void SetDouble(string key, double value)
		{
			PlayerPrefs.SetString(key, StringUtils.ToUniString(value));
		}
		
		public static double? SetDoubleAndReturnPrevious(string key, double value)
		{
			double? result = null;
			if (UserPrefs.HasKey(key))
			{
				string @string = PlayerPrefs.GetString(key);
				try
				{
					result = new double?(double.Parse(@string, NumberStyles.Float, NumberFormatInfo.InvariantInfo));
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			UserPrefs.SetDouble(key, value);
			return result;
		}
		
		public static void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}
		
		public static string SetStringAndReturnPrevious(string key, string value)
		{
			string result = null;
			if (UserPrefs.HasKey(key))
			{
				result = PlayerPrefs.GetString(key);
			}
			UserPrefs.SetString(key, value);
			return result;
		}
		
		public static void SetDateTime(string key, DateTime value)
		{
			if (value.Kind == DateTimeKind.Local)
			{
				value = value.ToUniversalTime();
			}
			UserPrefs.SetLong(key, value.ToBinary());
		}
		
		public static void SetJson(string key, JsonObject value)
		{
			UserPrefs.SetString(key, (!(value != null)) ? null : value.ToString());
		}
		
		public static void SetCollection(string key, ICollection<string> collection)
		{
			string value;
			if (collection == null || collection.Count == 0)
			{
				value = EmptyCollectionValue;
			}
			else
			{
				value = StringUtils.CollectionToCommaDelimitedString<string>(collection);
			}
			UserPrefs.SetString(key, value);
		}
		
		public static void SetHash(string originalKey, string value, string hashSuffix)
		{
			string value2 = UserPrefs.CreateHashValue(value, hashSuffix);
			UserPrefs.SetString(UserPrefs.CreateHashKey(originalKey), value2);
		}

		public static void SetXml<T>(string key, T obj) {
			XmlSerializer serializer = new XmlSerializer(typeof(T));    
			StringWriter sw = new StringWriter();    
			serializer.Serialize( sw, obj );
            //Debug.LogError(sw.ToString());
            Debug.Log("Save Obj : " + key);
			string en = EncryptUtils.Base64Encrypt (sw.ToString());
			UserPrefs.SetString (key, en);
		}

		public static T GetXml<T>(string key, T defaultT) {
			if (UserPrefs.HasKey (key)) {
				string en = UserPrefs.GetString (key, "");
				if (en == "")
					return defaultT;

				string de = EncryptUtils.Base64Decrypt (en);
				XmlSerializer ss = new XmlSerializer(typeof(T));    
				StringReader sr = new StringReader( de );    
				return (T)ss.Deserialize( sr );
			}
			return defaultT;
		}
	}
}

