using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DInvoke.ManualMap
{
	// Token: 0x02000002 RID: 2
	public class Map
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static IntPtr AllocateFileToMemory(string FilePath)
		{
			if (!File.Exists(FilePath))
			{
				throw new InvalidOperationException("Filepath not found.");
			}
			return Map.AllocateBytesToMemory(File.ReadAllBytes(FilePath));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		public static IntPtr AllocateBytesToMemory(byte[] FileByteArray)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(FileByteArray.Length);
			Marshal.Copy(FileByteArray, 0, intPtr, FileByteArray.Length);
			return intPtr;
		}
	}
}
