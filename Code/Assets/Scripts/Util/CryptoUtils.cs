
using System;
using System.Text;
using System.Security.Cryptography;


namespace FootStudio.Util
{
	public static class CryptoUtils
	{
		public static string Sha1(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			return CryptoUtils.Sha1(bytes);
		}
		
		public static string Sha1(byte[] value)
		{
			SHA1Managed sHA1Managed = new SHA1Managed();
			byte[] value2 = sHA1Managed.ComputeHash(value);
			string text = BitConverter.ToString(value2);
			return text.Replace("-", string.Empty).ToLowerInvariant();
		}
	}
}

