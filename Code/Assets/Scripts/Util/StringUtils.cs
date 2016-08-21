using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


namespace Soulgame.Util
{
	public static class StringUtils
	{
		public static readonly string[] EmptyStrings = new string[0];
		
		public static List<string> Split(string s, string delimiters, bool trimTokens, bool ignoreEmptyTokens)
		{
			return StringUtils.Split(s, delimiters, trimTokens, ignoreEmptyTokens, null);
		}
		
		public static List<string> Split(string s, string delimiters, bool trimTokens, bool ignoreEmptyTokens, string quoteChars)
		{
			if (s == null)
			{
				return new List<string>(0);
			}
			if (string.IsNullOrEmpty(delimiters))
			{
				return new List<string>
				{
					s
				};
			}
			if (quoteChars == null)
			{
				quoteChars = string.Empty;
			}
			Assert.IsTrue(quoteChars.Length % 2 == 0, "the number of quote characters must be even", new object[0]);
			char[] delimiters2 = delimiters.ToCharArray();
			int[] array = new int[s.Length];
			int num = StringUtils.MakeDelimiterPositionList(s, delimiters2, quoteChars, array);
			List<string> list = new List<string>(num + 1);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				string text = s.Substring(num2, array[i] - num2);
				if (trimTokens)
				{
					text = text.Trim();
				}
				if (!ignoreEmptyTokens || text.Length != 0)
				{
					list.Add(text);
				}
				num2 = array[i] + 1;
			}
			if (num2 < s.Length)
			{
				string text2 = s.Substring(num2);
				if (trimTokens)
				{
					text2 = text2.Trim();
				}
				if (!ignoreEmptyTokens || text2.Length != 0)
				{
					list.Add(text2);
				}
			}
			else if (num2 == s.Length && !ignoreEmptyTokens)
			{
				list.Add(string.Empty);
			}
			return list;
		}

		private static int MakeDelimiterPositionList(string s, char[] delimiters, string quoteChars, int[] delimiterPositions)
		{
			int num = 0;
			int num2 = 0;
			char c = '\0';
			char c2 = '\0';
			for (int i = 0; i < s.Length; i++)
			{
				char c3 = s[i];
				for (int j = 0; j < delimiters.Length; j++)
				{
					if (delimiters[j] == c3 && num2 == 0)
					{
						delimiterPositions[num] = i;
						num++;
						break;
					}
					if (num2 == 0)
					{
						for (int k = 0; k < quoteChars.Length; k += 2)
						{
							if (quoteChars[k] == c3)
							{
								num2++;
								c = c3;
								c2 = quoteChars[k + 1];
								break;
							}
						}
					}
					else if (c3 == c)
					{
						num2++;
					}
					else if (c3 == c2)
					{
						num2--;
					}
				}
			}
			return num;
		}

		public static bool HasLength(string target)
		{
			return target != null && target.Length > 0;
		}
		
		public static bool HasText(string target)
		{
			return target != null && StringUtils.HasLength(target.Trim());
		}
		
		public static bool IsNullOrEmpty(string target)
		{
			return !StringUtils.HasText(target);
		}

		public static List<string> CommaDelimitedListToStringList(string s)
		{
			return StringUtils.Split(s, ",", false, false, "\"\"");
		}

		public static string ToUniString(bool value)
		{
			return (!value) ? "false" : "true";
		}
		
		public static string ToUniString(short value)
		{
			return value.ToString("D", NumberFormatInfo.InvariantInfo);
		}
		
		public static string ToUniString(int value)
		{
			return value.ToString("D", NumberFormatInfo.InvariantInfo);
		}
		
		public static string ToUniString(long value)
		{
			return value.ToString("D", NumberFormatInfo.InvariantInfo);
		}
		
		public static string ToUniString(float value)
		{
			return value.ToString("R", NumberFormatInfo.InvariantInfo);
		}
		
		public static string ToUniString(double value)
		{
			return value.ToString("R", NumberFormatInfo.InvariantInfo);
		}

		public static string CollectionToDelimitedString<T>(ICollection<T> c, string delimiter)
		{
			if (c == null)
			{
				return "null";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (T current in c)
			{
				if (num++ > 0)
				{
					stringBuilder.Append(delimiter);
				}
				stringBuilder.Append(current);
			}
			return stringBuilder.ToString();
		}

		public static string CollectionToCommaDelimitedString<T>(ICollection<T> collection)
		{
			return StringUtils.CollectionToDelimitedString<T>(collection, ",");
		}

		public static string CombineStringWithCount(string value, int count)
		{
			Assert.IsTrue(count >= 1, "count must be >= 1", new object[0]);
			Assert.IsTrue(value.IndexOf('=') == -1, "value must not contain '='", new object[0]);
			if (count == 1)
			{
				return value;
			}
			return string.Format("{0}={1}", value, StringUtils.ToUniString(count));
		}

		public static bool TryParsingCombinedStringWithCount(string combined, out string value, out int count)
		{
			if (string.IsNullOrEmpty(combined))
			{
				value = combined;
				count = 0;
				return false;
			}
			string[] array = combined.Split(new char[]{
				'='
			});
			value = array[0];
			if (array.Length == 1)
			{
				count = 1;
				return true;
			}
			return int.TryParse(array[1], out count);
		}
	}
}

