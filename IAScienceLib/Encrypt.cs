using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace IAScience
{
	/// <summary>
	/// A simple encryption class that can be used to two-encode/decode strings and byte buffers
	/// with single method calls.
	/// </summary>
	public class Encrypt
	{
		/// <summary>
		/// Replace this value with some unique key of your own
		/// Best set this in your App start up in a Static constructor
		/// </summary>
		// public static string Key = "0a1f131c";
		public static string Key = "D116149136CF4065AE6FC74B744D4B2D";	// 128 bit

		/// <summary>
		/// Encodes a stream of bytes using DES encryption with a pass key. Lowest level method that 
		/// handles all work.
		/// </summary>
		/// <param name="InputString"></param>
		/// <param name="EncryptionKey"></param>
		/// <returns></returns>
		public static byte[] EncryptBytes(byte[] InputString, string EncryptionKey) 
		{
			if (EncryptionKey == null)
				EncryptionKey = Key;

			//TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			//MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			var des = TripleDES.Create();
			var hashmd5 = MD5.Create();

			des.Key = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(EncryptionKey));
			des.Mode = CipherMode.ECB;
			
			ICryptoTransform Transform = des.CreateEncryptor();

			byte[] Buffer = InputString;
			return Transform.TransformFinalBlock(Buffer,0,Buffer.Length);
		}
		
		/// <summary>
		/// Encrypts a string into bytes using DES encryption with a Passkey. 
		/// </summary>
		/// <param name="InputString"></param>
		/// <param name="EncryptionKey"></param>
		/// <returns></returns>
		public static byte[] EncryptBytes(string DecryptString, string EncryptionKey) 
		{
			return EncryptBytes(Encoding.ASCII.GetBytes(DecryptString),EncryptionKey);
		}

		/// <summary>
		/// Encrypts a string using Triple DES encryption with a two way encryption key.String is returned as Base64 encoded value
		/// rather than binary.
		/// </summary>
		/// <param name="InputString"></param>
		/// <param name="EncryptionKey"></param>
		/// <returns></returns>
		public static string EncryptString(string InputString, string EncryptionKey) 
		{
			return Convert.ToBase64String( EncryptBytes(Encoding.ASCII.GetBytes(InputString),EncryptionKey) );
		}


		
		/// <summary>
		/// Decrypts a Byte array from DES with an Encryption Key.
		/// </summary>
		/// <param name="DecryptBuffer"></param>
		/// <param name="EncryptionKey"></param>
		/// <returns></returns>
		public static byte[] DecryptBytes(byte[] DecryptBuffer, string EncryptionKey) 
		{
			if (EncryptionKey == null)
				EncryptionKey = Key;

			//TripleDESCryptoServiceProvider des =  new TripleDESCryptoServiceProvider();
			//MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			var des =  TripleDES.Create();
			var hashmd5 = MD5.Create();

			des.Key = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(EncryptionKey));
			des.Mode = CipherMode.ECB;

			ICryptoTransform Transform = des.CreateDecryptor();
			
			return  Transform.TransformFinalBlock(DecryptBuffer,0,DecryptBuffer.Length);
		}
		
		public static byte[] DecryptBytes(string DecryptString, string EncryptionKey) 
		{	
				return DecryptBytes(Convert.FromBase64String(DecryptString),EncryptionKey);
		}

		/// <summary>
		/// Decrypts a string using DES encryption and a pass key that was used for 
		/// encryption.
		/// <seealso>Class wwEncrypt</seealso>
		/// </summary>
		/// <param name="DecryptString"></param>
		/// <param name="EncryptionKey"></param>
		/// <returns>String</returns>
		public static string DecryptString(string DecryptString, string EncryptionKey) 
		{
			try 
			{
				byte[] bytes = DecryptBytes(Convert.FromBase64String(DecryptString), EncryptionKey);
				return Encoding.ASCII.GetString(bytes,0,bytes.Length );
			}
			catch { return ""; }  // Probably not encoded
		}
		public static string MD5HashOfFile(string fileName) { 
			//MD5 md5 = new MD5CryptoServiceProvider(); 

			using (var md5 = MD5.Create()) {
				using (FileStream file = new FileStream(fileName, FileMode.Open,FileAccess.Read)) {
					byte[] retVal = md5.ComputeHash(file);
					return BitConverter.ToString(retVal).Replace("-", "");	// hex string
					}
				}

			//file.Close();


			//StringBuilder sb = new StringBuilder(); 
			//for (int i = 0; i < retVal.Length; i++) { 
			//    sb.Append(retVal[i].ToString("x2")); 
			//    } 
			//return sb.ToString(); }
			}
	}
}
