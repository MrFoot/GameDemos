using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;

public class AESTools {

	static AESTools() {
	
		string code_key = MD5Tool.GetMd5Hash ("soulgame");
		string code_iv = MD5Tool.GetMd5Hash ("bear");
		//Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("soulgame", Encoding.UTF8.GetBytes("garfield"));
		//Key = rfc.GetBytes (32);
		//IV = rfc.GetBytes (16);
		Key = Encoding.UTF8.GetBytes(code_key.Substring (0, 16));
		IV = Encoding.UTF8.GetBytes(code_iv.Substring (0, 16));
		//Debug.Log (Key.Length);
		//Debug.Log (IV.Length);
	}

	/// <summary>
	/// 获取密钥
	/// </summary>
	private static byte[] Key;
//	{
//		get { return @"SOULGAME2012?#*GARFIELD%$CAT2016"; }
//	}
	
	/// <summary>
	/// 获取向量
	/// </summary>
	private static byte[] IV;
//	{
//		get { return @"GARFIELD#$%CAT?&"; }
//	}

	public static string AESEncryptStrToStr(string str) {
		byte[] inputByteArray = Encoding.UTF8.GetBytes (str) ;
		return Encoding.UTF8.GetString (AESEncrypt(inputByteArray));
	}

	public static byte[] AESEncryptStrToByte(string str) {
		byte[] inputByteArray = Encoding.UTF8.GetBytes (str) ;
		return AESEncrypt (inputByteArray);
	}

	public static byte[] AESEncryptByteToByte(byte[] bytes) {
		return AESEncrypt (bytes);
	}

	public static string AESEncryptByteToStr(byte[] bytes) {
		return Encoding.UTF8.GetString (AESEncrypt(bytes));
		//return Convert.ToBase64String (bytes);
	}

	public static byte[] AESDecryptByteToByte(byte[] bytes) {
		return AESDecrypt (bytes);
	}

	public static string AESDecryptByteToStr(byte[] bytes) {
		return Encoding.UTF8.GetString (AESDecrypt (bytes));
	}

	public static string AESDecryptStrToStr(string str) {
		byte[] inputByteArray = Encoding.UTF8.GetBytes (str) ;
		return Encoding.UTF8.GetString(AESDecrypt (inputByteArray));
	}

	public static byte[] AESDecryptStrToByte(string str) {
		byte[] inputByteArray = Encoding.UTF8.GetBytes (str) ;
		//byte[] inputByteArray = Convert.FromBase64String (str);
		return AESDecrypt (inputByteArray);
	}

	/// <summary>
	/// 加密
	/// </summary>
	/// <returns>The encrypt.</returns>
	/// <param name="bytes">Bytes.</param>
	public static byte[] AESEncrypt(byte[] cipherText)
	{
		if (cipherText == null || cipherText.Length <= 0)
			throw new ArgumentNullException("plainText");
		if (Key == null || Key.Length <= 0)
			throw new ArgumentNullException("Key");
		if (IV == null || IV.Length <= 0)
			throw new ArgumentNullException("IV");
		
		byte[] encrypt = null;
		Rijndael aes = Rijndael.Create();
		aes.Key = Key;
		aes.IV = IV;
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.Zeros;
//		try
//		{
//			using (MemoryStream mStream = new MemoryStream())
//			{
//				using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
//				{
//					cStream.Write(cipherText, 0, cipherText.Length);
//					cStream.FlushFinalBlock();
//					encrypt = mStream.ToArray();
//				}
//			}
//		}
//		catch { }
		ICryptoTransform cTransform = aes.CreateEncryptor();
		encrypt = cTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
		aes.Clear();
		
		return encrypt;
	}

	/// <summary>
	/// 解密
	/// </summary>
	/// <returns>The decrypt.</returns>
	/// <param name="bytes">Bytes.</param>
	public static byte[] AESDecrypt(byte[] cipherText)
	{
		if (cipherText == null || cipherText.Length <= 0)
			throw new ArgumentNullException("cipherText");
		if (Key == null || Key.Length <= 0)
			throw new ArgumentNullException("Key");
		if (IV == null || IV.Length <= 0)
			throw new ArgumentNullException("IV");
		
		byte[] decrypt = null;
		Rijndael aes = Rijndael.Create();
		aes.Key = Key;
		aes.IV = IV;
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.Zeros;
//		try
//		{
//			using (MemoryStream mStream = new MemoryStream())
//			{
//				using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
//				{
//					cStream.Write(cipherText, 0, cipherText.Length);
//					cStream.FlushFinalBlock();
//					decrypt = mStream.ToArray();
//				}
//			}
//		}
//		catch { }
		ICryptoTransform cTransform = aes.CreateDecryptor();
		decrypt = cTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
		aes.Clear();
		
		return decrypt;
	}
}
