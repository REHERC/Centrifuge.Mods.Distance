using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Distance.TrackMusic.Util
{
	//
	// Base16k.cpp : Variant of base64 used to efficiently encode  binary into Unicode UTF16 strings. Based on work by
	// Markus Scherer at https://sites.google.com/site/markusicu/unicode/base16k
	//
	// This code is hereby placed in the Public Domain.
	// Jim Beveridge, November 29, 2011.
	//
	// C# port of http://qualapps.blogspot.com/2011/11/base64-for-unicode-utf16.html
	// This code is hereby placed in the Public Domain.
	// J. Daniel Smith, February 23, 2015
	//

	public static partial class Base16k
	{
		/// <summary>
		/// Encode a binary array into a Base16k string for Unicode.
		/// </summary>
		public static string ToBase16kString(byte[] inArray)
		{
			int len = inArray.Length;

			var sb = new StringBuilder(len * 6 / 5);
			sb.Append(len);

			int code = 0;

			for (int i = 0; i < len; ++i)
			{
				byte byteValue = inArray[i];
				switch (i % 7)
				{
					case 0:
						code = byteValue << 6;
						break;

					case 1:
						code |= byteValue >> 2;
						code += 0x5000;
						sb.Append(System.Convert.ToChar(code));
						code = (byteValue & 3) << 12;
						break;

					case 2:
						code |= byteValue << 4;
						break;

					case 3:
						code |= byteValue >> 4;
						code += 0x5000;
						sb.Append(System.Convert.ToChar(code));
						code = (byteValue & 0xf) << 10;
						break;

					case 4:
						code |= byteValue << 2;
						break;

					case 5:
						code |= byteValue >> 6;
						code += 0x5000;
						sb.Append(System.Convert.ToChar(code));
						code = (byteValue & 0x3f) << 8;
						break;

					case 6:
						code |= byteValue;
						code += 0x5000;
						sb.Append(System.Convert.ToChar(code));
						code = 0;
						break;
				}
			}

			// emit a character for remaining bits
			if (len % 7 != 0)
			{
				code += 0x5000;
				sb.Append(System.Convert.ToChar(code));
			}

			return sb.ToString();
		}

		/// <summary>
		///  Decode a Base16k string for Unicode into a binary array.
		/// </summary>
		public static byte[] FromBase16kString(string s)
		{
			// read the length
			var r = new Regex(@"^\d+", RegexOptions.None);
			Match m = r.Match(s);
			if (!m.Success)
			{
				return null;
			}

			if (!Int32.TryParse(m.Value, out int length))
			{
				return null;
			}

			var buf = new List<byte>(length);

			int pos = 0;  // position in s
			while ((pos < s.Length) && (s[pos] >= '0' && s[pos] <= '9'))
			{
				++pos;
			}

			// decode characters to bytes
			int i = 0;	// byte position modulo 7 (0..6 wrapping around)
			int code = 0;
			byte byteValue = 0;

			while (length-- > 0)
			{
				if (((1 << i) & 0x2b) != 0)
				{
					// fetch another Han character at i=0, 1, 3, 5
					if (pos >= s.Length)
					{
						// Too few Han characters representing binary data.
						System.Diagnostics.Debug.Assert(pos < s.Length);
						return null;
					}

					code = s[pos++] - 0x5000;
				}

				switch (i % 7)
				{
					case 0:
						byteValue = System.Convert.ToByte(code >> 6);
						buf.Add(byteValue);
						byteValue = System.Convert.ToByte((code & 0x3f) << 2);
						break;

					case 1:
						byteValue |= System.Convert.ToByte(code >> 12);
						buf.Add(byteValue);
						break;

					case 2:
						byteValue = System.Convert.ToByte((code >> 4) & 0xff);
						buf.Add(byteValue);
						byteValue = System.Convert.ToByte((code & 0xf) << 4);
						break;

					case 3:
						byteValue |= System.Convert.ToByte(code >> 10);
						buf.Add(byteValue);
						break;

					case 4:
						byteValue = System.Convert.ToByte((code >> 2) & 0xff);
						buf.Add(byteValue);
						byteValue = System.Convert.ToByte((code & 3) << 6);
						break;

					case 5:
						byteValue |= System.Convert.ToByte(code >> 8);
						buf.Add(byteValue);
						break;

					case 6:
						byteValue = System.Convert.ToByte(code & 0xff);
						buf.Add(byteValue);
						break;
				}

				// advance to the next byte position
				if (++i == 7)
				{
					i = 0;
				}
			}

			return buf.ToArray();
		}
	}
}
