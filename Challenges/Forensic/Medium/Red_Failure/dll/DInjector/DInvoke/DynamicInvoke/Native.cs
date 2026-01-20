using System;
using System.Runtime.InteropServices;
using DInvoke.Data;

namespace DInvoke.DynamicInvoke
{
	// Token: 0x02000004 RID: 4
	public class Native
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002C54 File Offset: 0x00000E54
		public static void RtlInitUnicodeString(ref Native.UNICODE_STRING DestinationString, [MarshalAs(UnmanagedType.LPWStr)] string SourceString)
		{
			object[] array = new object[]
			{
				DestinationString,
				SourceString
			};
			Generic.DynamicAPIInvoke("ntdll.dll", "RtlInitUnicodeString", typeof(Native.DELEGATES.RtlInitUnicodeString), ref array, false, true);
			DestinationString = (Native.UNICODE_STRING)array[0];
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public static Native.NTSTATUS LdrLoadDll(IntPtr PathToFile, uint dwFlags, ref Native.UNICODE_STRING ModuleFileName, ref IntPtr ModuleHandle)
		{
			object[] array = new object[]
			{
				PathToFile,
				dwFlags,
				ModuleFileName,
				ModuleHandle
			};
			Native.NTSTATUS result = (Native.NTSTATUS)Generic.DynamicAPIInvoke("ntdll.dll", "LdrLoadDll", typeof(Native.DELEGATES.LdrLoadDll), ref array, false, true);
			ModuleHandle = (IntPtr)array[3];
			return result;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002D14 File Offset: 0x00000F14
		public static void RtlZeroMemory(IntPtr Destination, int Length)
		{
			object[] array = new object[]
			{
				Destination,
				Length
			};
			Generic.DynamicAPIInvoke("ntdll.dll", "RtlZeroMemory", typeof(Native.DELEGATES.RtlZeroMemory), ref array, false, true);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002D58 File Offset: 0x00000F58
		public static Native.NTSTATUS NtQueryInformationProcess(IntPtr hProcess, Native.PROCESSINFOCLASS processInfoClass, out IntPtr pProcInfo)
		{
			uint num = 0U;
			int num2;
			if (processInfoClass != Native.PROCESSINFOCLASS.ProcessBasicInformation)
			{
				if (processInfoClass != Native.PROCESSINFOCLASS.ProcessWow64Information)
				{
					throw new InvalidOperationException(string.Format("Invalid ProcessInfoClass: {0}", processInfoClass));
				}
				pProcInfo = Marshal.AllocHGlobal(IntPtr.Size);
				Native.RtlZeroMemory(pProcInfo, IntPtr.Size);
				num2 = IntPtr.Size;
			}
			else
			{
				Native.PROCESS_BASIC_INFORMATION process_BASIC_INFORMATION = default(Native.PROCESS_BASIC_INFORMATION);
				pProcInfo = Marshal.AllocHGlobal(Marshal.SizeOf(process_BASIC_INFORMATION));
				Native.RtlZeroMemory(pProcInfo, Marshal.SizeOf(process_BASIC_INFORMATION));
				Marshal.StructureToPtr(process_BASIC_INFORMATION, pProcInfo, true);
				num2 = Marshal.SizeOf(process_BASIC_INFORMATION);
			}
			object[] array = new object[]
			{
				hProcess,
				processInfoClass,
				pProcInfo,
				num2,
				num
			};
			Native.NTSTATUS ntstatus = (Native.NTSTATUS)Generic.DynamicAPIInvoke("ntdll.dll", "NtQueryInformationProcess", typeof(Native.DELEGATES.NtQueryInformationProcess), ref array, false, true);
			if (ntstatus != Native.NTSTATUS.Success)
			{
				throw new UnauthorizedAccessException("Access is denied.");
			}
			pProcInfo = (IntPtr)array[2];
			return ntstatus;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002E5C File Offset: 0x0000105C
		public static bool NtQueryInformationProcessWow64Information(IntPtr hProcess)
		{
			IntPtr ptr;
			if (Native.NtQueryInformationProcess(hProcess, Native.PROCESSINFOCLASS.ProcessWow64Information, out ptr) != Native.NTSTATUS.Success)
			{
				throw new UnauthorizedAccessException("Access is denied.");
			}
			return !(Marshal.ReadIntPtr(ptr) == IntPtr.Zero);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002E98 File Offset: 0x00001098
		public static Native.PROCESS_BASIC_INFORMATION NtQueryInformationProcessBasicInformation(IntPtr hProcess)
		{
			IntPtr ptr;
			if (Native.NtQueryInformationProcess(hProcess, Native.PROCESSINFOCLASS.ProcessBasicInformation, out ptr) != Native.NTSTATUS.Success)
			{
				throw new UnauthorizedAccessException("Access is denied.");
			}
			return (Native.PROCESS_BASIC_INFORMATION)Marshal.PtrToStructure(ptr, typeof(Native.PROCESS_BASIC_INFORMATION));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002ED0 File Offset: 0x000010D0
		public static IntPtr NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect)
		{
			object[] array = new object[]
			{
				ProcessHandle,
				BaseAddress,
				ZeroBits,
				RegionSize,
				AllocationType,
				Protect
			};
			Native.NTSTATUS ntstatus = (Native.NTSTATUS)Generic.DynamicAPIInvoke("ntdll.dll", "NtAllocateVirtualMemory", typeof(Native.DELEGATES.NtAllocateVirtualMemory), ref array, false, true);
			if (ntstatus == (Native.NTSTATUS)3221225506U)
			{
				throw new UnauthorizedAccessException("Access is denied.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225505U)
			{
				throw new InvalidOperationException("The specified address range is already committed.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225773U)
			{
				throw new InvalidOperationException("Your system is low on virtual memory.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225496U)
			{
				throw new InvalidOperationException("The specified address range conflicts with the address space.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225626U)
			{
				throw new InvalidOperationException("Insufficient system resources exist to complete the API call.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225480U)
			{
				throw new InvalidOperationException("An invalid HANDLE was specified.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225541U)
			{
				throw new InvalidOperationException("The specified page protection was not valid.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225495U)
			{
				throw new InvalidOperationException("Not enough virtual memory or paging file quota is available to complete the specified operation.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225508U)
			{
				throw new InvalidOperationException("There is a mismatch between the type of object that is required by the requested operation and the type of object that is specified in the request.");
			}
			if (ntstatus != Native.NTSTATUS.Success)
			{
				throw new InvalidOperationException("An attempt was made to duplicate an object handle into or out of an exiting process.");
			}
			BaseAddress = (IntPtr)array[1];
			return BaseAddress;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003004 File Offset: 0x00001204
		public static uint NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength)
		{
			uint num = 0U;
			object[] array = new object[]
			{
				ProcessHandle,
				BaseAddress,
				Buffer,
				BufferLength,
				num
			};
			Native.NTSTATUS ntstatus = (Native.NTSTATUS)Generic.DynamicAPIInvoke("ntdll.dll", "NtWriteVirtualMemory", typeof(Native.DELEGATES.NtWriteVirtualMemory), ref array, false, true);
			if (ntstatus != Native.NTSTATUS.Success)
			{
				throw new InvalidOperationException("Failed to write memory, " + ntstatus.ToString());
			}
			return (uint)array[4];
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003094 File Offset: 0x00001294
		public static uint NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect)
		{
			uint num = 0U;
			object[] array = new object[]
			{
				ProcessHandle,
				BaseAddress,
				RegionSize,
				NewProtect,
				num
			};
			Native.NTSTATUS ntstatus = (Native.NTSTATUS)Generic.DynamicAPIInvoke("ntdll.dll", "NtProtectVirtualMemory", typeof(Native.DELEGATES.NtProtectVirtualMemory), ref array, false, true);
			if (ntstatus != Native.NTSTATUS.Success)
			{
				throw new InvalidOperationException("Failed to change memory protection, " + ntstatus.ToString());
			}
			return (uint)array[4];
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003128 File Offset: 0x00001328
		public static void NtFreeVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint FreeType)
		{
			object[] array = new object[]
			{
				ProcessHandle,
				BaseAddress,
				RegionSize,
				FreeType
			};
			Native.NTSTATUS ntstatus = (Native.NTSTATUS)Generic.DynamicAPIInvoke("ntdll.dll", "NtFreeVirtualMemory", typeof(Native.DELEGATES.NtFreeVirtualMemory), ref array, false, true);
			if (ntstatus == (Native.NTSTATUS)3221225506U)
			{
				throw new UnauthorizedAccessException("Access is denied.");
			}
			if (ntstatus == (Native.NTSTATUS)3221225480U)
			{
				throw new InvalidOperationException("An invalid HANDLE was specified.");
			}
			if (ntstatus != Native.NTSTATUS.Success)
			{
				throw new InvalidOperationException("There is a mismatch between the type of object that is required by the requested operation and the type of object that is specified in the request.");
			}
		}

		// Token: 0x0200001B RID: 27
		public struct DELEGATES
		{
			// Token: 0x02000084 RID: 132
			// (Invoke) Token: 0x0600015F RID: 351
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate void RtlInitUnicodeString(ref Native.UNICODE_STRING DestinationString, [MarshalAs(UnmanagedType.LPWStr)] string SourceString);

			// Token: 0x02000085 RID: 133
			// (Invoke) Token: 0x06000163 RID: 355
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate uint LdrLoadDll(IntPtr PathToFile, uint dwFlags, ref Native.UNICODE_STRING ModuleFileName, ref IntPtr ModuleHandle);

			// Token: 0x02000086 RID: 134
			// (Invoke) Token: 0x06000167 RID: 359
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate void RtlZeroMemory(IntPtr Destination, int length);

			// Token: 0x02000087 RID: 135
			// (Invoke) Token: 0x0600016B RID: 363
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate uint NtQueryInformationProcess(IntPtr processHandle, Native.PROCESSINFOCLASS processInformationClass, IntPtr processInformation, int processInformationLength, ref uint returnLength);

			// Token: 0x02000088 RID: 136
			// (Invoke) Token: 0x0600016F RID: 367
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate uint NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, uint AllocationType, uint Protect);

			// Token: 0x02000089 RID: 137
			// (Invoke) Token: 0x06000173 RID: 371
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate uint NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, IntPtr Buffer, uint BufferLength, ref uint BytesWritten);

			// Token: 0x0200008A RID: 138
			// (Invoke) Token: 0x06000177 RID: 375
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate uint NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, ref uint OldProtect);

			// Token: 0x0200008B RID: 139
			// (Invoke) Token: 0x0600017B RID: 379
			[UnmanagedFunctionPointer(CallingConvention.StdCall)]
			public delegate uint NtFreeVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint FreeType);
		}
	}
}
