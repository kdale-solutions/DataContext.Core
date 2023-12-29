using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Core.Utilities.Security
{
	public static class Sha512Utility
	{
		private static SHA512 _sha512 = SHA512.Create();

		public static byte[] HashString(this string str) 
		{
			return _sha512.ComputeHash(Encoding.ASCII.GetBytes(str));
		}

		public static Task<byte[]> HashStringAsync(this string str)
		{
			return _sha512.ComputeHashAsync(Stream.Synchronized(new MemoryStream(Encoding.ASCII.GetBytes(str))));
		}
	}
}
