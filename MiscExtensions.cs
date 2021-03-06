using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using Marvel.Api;
using Marvel.Api.Results;

namespace httpdeveloper.marvel.comdocsAPIMVCnewestversion.Controllers
{
    public static class MiscExtensions
    {
#if !WINDOWS_PHONE
		/// <summary>
		/// Save a byte array to a file
		/// </summary>
		/// <param name="input">Bytes to save</param>
		/// <param name="path">Full path to save file to</param>
		public static void SaveAs(this byte[] input, string path)
		{
			File.WriteAllBytes(path, input);
		}
#endif

		/// <summary>
		/// Read a stream into a byte array
		/// </summary>
		/// <param name="input">Stream to read</param>
		/// <returns>byte[]</returns>
		public static byte[] ReadAsBytes(this Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Copies bytes from one stream to another
		/// </summary>
		/// <param name="input">The input stream.</param>
		/// <param name="output">The output stream.</param>
		public static void CopyTo(this Stream input, Stream output)
		{
			var buffer = new byte[32768];
			while (true)
			{
				var read = input.Read(buffer, 0, buffer.Length);
				if (read <= 0)
					return;
				output.Write(buffer, 0, read);
			}
		}
		/// <summary>
		/// Gets string value from JToken
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		/// 

		//public static string AsString(this Byte[] marvelByte/*MarvelRestClient client*/)
		//{
		//	return /*marvelByte*//*.Equals*//*(String)*//*.Type*/ /*== JTokenType.String ? marvelByte.Value<string>() : */marvelByte.ToString();
		//}


		public static string AsString(this JToken token)
		{
			return token.Type == JTokenType.String ? token.Value<string>() : token.ToString();
		}

		/// <summary>
		/// Gets string value from JToken
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static string AsString(this JToken token, CultureInfo culture)
		{
			var value = token as JValue;
			if (value != null)
			{
				return (string)value.Value/*ChangeType*//*(typeof(string)*//*, culture*//*)*/;
			}

			return token.Type == JTokenType.String ? token.Value<string>() : token.ToString();
		}

		/// <summary>
		/// Converts a byte array to a string, using its byte order mark to convert it to the right encoding.
		/// http://www.shrinkrays.net/code-snippets/csharp/an-extension-method-for-converting-a-byte-array-to-a-string.aspx
		/// </summary>
		/// <param name="buffer">An array of bytes to convert</param>
		/// <returns>The byte as a string.</returns>
		public static string AsByteString(this byte[] buffer)
		{
			if (buffer == null) return "";

			// Ansi as default
			Encoding encoding = Encoding.UTF8;

#if FRAMEWORK
			return encoding.GetString(buffer);
#else
			if (buffer == null || buffer.Length == 0)
				return "";

			/*
				EF BB BF		UTF-8 
				FF FE UTF-16	little endian 
				FE FF UTF-16	big endian 
				FF FE 00 00		UTF-32, little endian 
				00 00 FE FF		UTF-32, big-endian 
				*/

			if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
			{
				encoding = Encoding.UTF8;
			}
			else if (buffer[0] == 0xfe && buffer[1] == 0xff)
			{
				encoding = Encoding.Unicode;
			}
			else if (buffer[0] == 0xfe && buffer[1] == 0xff)
			{
				encoding = Encoding.BigEndianUnicode; // utf-16be
			}

			using (MemoryStream stream = new MemoryStream())
			{
				stream.Write(buffer, 0, buffer.Length);
				stream.Seek(0, SeekOrigin.Begin);
				using (StreamReader reader = new StreamReader(stream, encoding))
				{
					return reader.ReadToEnd();
				}
			}
#endif
		}



		public static string AsAByteString(this CharacterResult characterResult/*this byte[] buffer*/)
		{
			if (characterResult == null) return "";

			// Ansi as default
			Encoding encoding = Encoding.UTF8;

#if FRAMEWORK
			return encoding.GetString(buffer);
#else
			if (characterResult == null/* || characterResult.Length == 0*/)
				return "";

			/*
				EF BB BF		UTF-8 
				FF FE UTF-16	little endian 
				FE FF UTF-16	big endian 
				FF FE 00 00		UTF-32, little endian 
				00 00 FE FF		UTF-32, big-endian 
				*/

			//if (characterResult[0] == 0xef && characterResult[1] == 0xbb && characterResult[2] == 0xbf)
			//{
			//	encoding = Encoding.UTF8;
			//}
			//else if (characterResult[0] == 0xfe && characterResult[1] == 0xff)
			//{
			//	encoding = Encoding.Unicode;
			//}
			//else if (characterResult[0] == 0xfe && characterResult[1] == 0xff)
			//{
			//	encoding = Encoding.BigEndianUnicode; // utf-16be
			//}

			using (MemoryStream stream = new MemoryStream())
			{

				string characterResultString = characterResult.ToString();
				// Convert a C# string to a byte array    
				byte[] characterResultByteArray = Encoding.ASCII.GetBytes(characterResultString);
				//foreach (byte b in characterResultByteArray)
				//{
				//	Console.WriteLine(b);
				//}

				stream.Write(characterResultByteArray/*.Code.*//*.AsAByteString()*/, 0, characterResult.Data.Count.Length/*, characterResult.Length*/);
				stream.Seek(0, SeekOrigin.Begin);
				using (StreamReader reader = new StreamReader(stream, encoding))
				{
					return reader.ReadToEnd();
				}
			}
#endif
		}

	}
}

