using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DInjector
{
	// Token: 0x02000019 RID: 25
	internal class AES
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00005689 File Offset: 0x00003889
		public AES(string password)
		{
			this.key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000056AC File Offset: 0x000038AC
		private byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(data, 0, data.Length);
					cryptoStream.FlushFinalBlock();
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00005714 File Offset: 0x00003914
		public byte[] Decrypt(byte[] data)
		{
			byte[] result;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				byte[] iv = data.Take(16).ToArray<byte>();
				byte[] data2 = data.Skip(16).Take(data.Length - 16).ToArray<byte>();
				aesCryptoServiceProvider.Key = this.key;
				aesCryptoServiceProvider.IV = iv;
				aesCryptoServiceProvider.Mode = CipherMode.CBC;
				aesCryptoServiceProvider.Padding = PaddingMode.PKCS7;
				using (ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateDecryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV))
				{
					result = this.PerformCryptography(cryptoTransform, data2);
				}
			}
			return result;
		}

		// Token: 0x04000002 RID: 2
		private byte[] key;
	}
}
