using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using DInvoke.Data;
using DInvoke.ManualMap;

namespace DInvoke.DynamicInvoke
{
	// Token: 0x02000003 RID: 3
	public class Generic
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000209A File Offset: 0x0000029A
		public static object DynamicAPIInvoke(string DLLName, string FunctionName, Type FunctionDelegateType, ref object[] Parameters, bool CanLoadFromDisk = false, bool ResolveForwards = true)
		{
			return Generic.DynamicFunctionInvoke(Generic.GetLibraryAddress(DLLName, FunctionName, CanLoadFromDisk, ResolveForwards), FunctionDelegateType, ref Parameters);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020AE File Offset: 0x000002AE
		public static object DynamicFunctionInvoke(IntPtr FunctionPointer, Type FunctionDelegateType, ref object[] Parameters)
		{
			return Marshal.GetDelegateForFunctionPointer(FunctionPointer, FunctionDelegateType).DynamicInvoke(Parameters);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C0 File Offset: 0x000002C0
		public static IntPtr LoadModuleFromDisk(string DLLPath)
		{
			Native.UNICODE_STRING unicode_STRING = default(Native.UNICODE_STRING);
			Native.RtlInitUnicodeString(ref unicode_STRING, DLLPath);
			IntPtr zero = IntPtr.Zero;
			if (Native.LdrLoadDll(IntPtr.Zero, 0U, ref unicode_STRING, ref zero) != Native.NTSTATUS.Success || zero == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			return zero;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002108 File Offset: 0x00000308
		public static IntPtr GetLibraryAddress(string DLLName, string FunctionName, bool CanLoadFromDisk = false, bool ResolveForwards = true)
		{
			IntPtr intPtr = Generic.GetLoadedModuleAddress(DLLName);
			if (intPtr == IntPtr.Zero && CanLoadFromDisk)
			{
				intPtr = Generic.LoadModuleFromDisk(DLLName);
				if (intPtr == IntPtr.Zero)
				{
					throw new FileNotFoundException(DLLName + ", unable to find the specified file.");
				}
			}
			else if (intPtr == IntPtr.Zero)
			{
				throw new DllNotFoundException(DLLName + ", Dll was not found.");
			}
			return Generic.GetExportAddress(intPtr, FunctionName, ResolveForwards);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002178 File Offset: 0x00000378
		public static IntPtr GetLoadedModuleAddress(string DLLName)
		{
			foreach (object obj in Process.GetCurrentProcess().Modules)
			{
				ProcessModule processModule = (ProcessModule)obj;
				if (processModule.FileName.ToLower().EndsWith(DLLName.ToLower()))
				{
					return processModule.BaseAddress;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021F8 File Offset: 0x000003F8
		public static IntPtr GetPebLdrModuleEntry(string DLLName)
		{
			ref Native.PROCESS_BASIC_INFORMATION ptr = Native.NtQueryInformationProcessBasicInformation((IntPtr)(-1));
			uint num;
			uint num2;
			if (IntPtr.Size == 4)
			{
				num = 12U;
				num2 = 12U;
			}
			else
			{
				num = 24U;
				num2 = 16U;
			}
			Native.LIST_ENTRY list_ENTRY = (Native.LIST_ENTRY)Marshal.PtrToStructure((IntPtr)((long)Marshal.ReadIntPtr((IntPtr)((long)ptr.PebBaseAddress + (long)((ulong)num))) + (long)((ulong)num2)), typeof(Native.LIST_ENTRY));
			IntPtr flink = list_ENTRY.Flink;
			IntPtr result = IntPtr.Zero;
			PE.LDR_DATA_TABLE_ENTRY ldr_DATA_TABLE_ENTRY = (PE.LDR_DATA_TABLE_ENTRY)Marshal.PtrToStructure(flink, typeof(PE.LDR_DATA_TABLE_ENTRY));
			while (ldr_DATA_TABLE_ENTRY.InLoadOrderLinks.Flink != list_ENTRY.Blink)
			{
				if (Marshal.PtrToStringUni(ldr_DATA_TABLE_ENTRY.FullDllName.Buffer).EndsWith(DLLName, StringComparison.OrdinalIgnoreCase))
				{
					result = ldr_DATA_TABLE_ENTRY.DllBase;
				}
				ldr_DATA_TABLE_ENTRY = (PE.LDR_DATA_TABLE_ENTRY)Marshal.PtrToStructure(ldr_DATA_TABLE_ENTRY.InLoadOrderLinks.Flink, typeof(PE.LDR_DATA_TABLE_ENTRY));
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022E8 File Offset: 0x000004E8
		public static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName, bool ResolveForwards = true)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int num = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 60L));
				Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + (long)num + 20L));
				long num2 = ModuleBase.ToInt64() + (long)num + 24L;
				long value;
				if (Marshal.ReadInt16((IntPtr)num2) == 267)
				{
					value = num2 + 96L;
				}
				else
				{
					value = num2 + 112L;
				}
				int num3 = Marshal.ReadInt32((IntPtr)value);
				int num4 = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num3 + 16L));
				Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num3 + 20L));
				int num5 = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num3 + 24L));
				int num6 = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num3 + 28L));
				int num7 = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num3 + 32L));
				int num8 = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num3 + 36L));
				long num9 = ModuleBase.ToInt64() + (long)Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num7));
				int i = 0;
				while (i < num5)
				{
					if (Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + (long)Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num7 + (long)(i * 4))))).Equals(ExportName, StringComparison.OrdinalIgnoreCase))
					{
						int num10 = (int)Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + (long)num8 + (long)(i * 2))) + num4;
						int num11 = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + (long)num6 + (long)(4 * (num10 - num4))));
						intPtr = (IntPtr)((long)ModuleBase + (long)num11);
						if (ResolveForwards)
						{
							intPtr = Generic.GetForwardAddress(intPtr, false);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			catch
			{
				throw new InvalidOperationException("Failed to parse module exports.");
			}
			if (intPtr == IntPtr.Zero)
			{
				throw new MissingMethodException(ExportName + ", export not found.");
			}
			return intPtr;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002520 File Offset: 0x00000720
		public static IntPtr GetForwardAddress(IntPtr ExportAddress, bool CanLoadFromDisk = false)
		{
			IntPtr intPtr = ExportAddress;
			try
			{
				string[] array = Marshal.PtrToStringAnsi(intPtr).Split(new char[]
				{
					'.'
				});
				if (array.Length > 1)
				{
					string text = array[0];
					string exportName = array[1];
					Dictionary<string, string> apiSetMapping = Generic.GetApiSetMapping();
					string key = text.Substring(0, text.Length - 2) + ".dll";
					if (apiSetMapping.ContainsKey(key))
					{
						text = apiSetMapping[key];
					}
					else
					{
						text += ".dll";
					}
					IntPtr intPtr2 = Generic.GetPebLdrModuleEntry(text);
					if (intPtr2 == IntPtr.Zero && CanLoadFromDisk)
					{
						intPtr2 = Generic.LoadModuleFromDisk(text);
					}
					if (intPtr2 != IntPtr.Zero)
					{
						intPtr = Generic.GetExportAddress(intPtr2, exportName, true);
					}
				}
			}
			catch
			{
			}
			return intPtr;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000025EC File Offset: 0x000007EC
		public static PE.PE_META_DATA GetPeMetaData(IntPtr pModule)
		{
			PE.PE_META_DATA pe_META_DATA = default(PE.PE_META_DATA);
			try
			{
				uint num = (uint)Marshal.ReadInt32((IntPtr)((long)pModule + 60L));
				pe_META_DATA.Pe = (uint)Marshal.ReadInt32((IntPtr)((long)pModule + (long)((ulong)num)));
				if (pe_META_DATA.Pe != 17744U)
				{
					throw new InvalidOperationException("Invalid PE signature.");
				}
				pe_META_DATA.ImageFileHeader = (PE.IMAGE_FILE_HEADER)Marshal.PtrToStructure((IntPtr)((long)pModule + (long)((ulong)num) + 4L), typeof(PE.IMAGE_FILE_HEADER));
				IntPtr intPtr = (IntPtr)((long)pModule + (long)((ulong)num) + 24L);
				ushort num2 = (ushort)Marshal.ReadInt16(intPtr);
				if (num2 == 267)
				{
					pe_META_DATA.Is32Bit = true;
					pe_META_DATA.OptHeader32 = (PE.IMAGE_OPTIONAL_HEADER32)Marshal.PtrToStructure(intPtr, typeof(PE.IMAGE_OPTIONAL_HEADER32));
				}
				else
				{
					if (num2 != 523)
					{
						throw new InvalidOperationException("Invalid magic value (PE32/PE32+).");
					}
					pe_META_DATA.Is32Bit = false;
					pe_META_DATA.OptHeader64 = (PE.IMAGE_OPTIONAL_HEADER64)Marshal.PtrToStructure(intPtr, typeof(PE.IMAGE_OPTIONAL_HEADER64));
				}
				PE.IMAGE_SECTION_HEADER[] array = new PE.IMAGE_SECTION_HEADER[(int)pe_META_DATA.ImageFileHeader.NumberOfSections];
				for (int i = 0; i < (int)pe_META_DATA.ImageFileHeader.NumberOfSections; i++)
				{
					IntPtr ptr = (IntPtr)((long)intPtr + (long)((ulong)pe_META_DATA.ImageFileHeader.SizeOfOptionalHeader) + (long)((ulong)(i * 40)));
					array[i] = (PE.IMAGE_SECTION_HEADER)Marshal.PtrToStructure(ptr, typeof(PE.IMAGE_SECTION_HEADER));
				}
				pe_META_DATA.Sections = array;
			}
			catch
			{
				throw new InvalidOperationException("Invalid module base specified.");
			}
			return pe_META_DATA;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002798 File Offset: 0x00000998
		public static Dictionary<string, string> GetApiSetMapping()
		{
			ref Native.PROCESS_BASIC_INFORMATION ptr = Native.NtQueryInformationProcessBasicInformation((IntPtr)(-1));
			uint num = (IntPtr.Size == 4) ? 56U : 104U;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			IntPtr intPtr = Marshal.ReadIntPtr((IntPtr)((long)ptr.PebBaseAddress + (long)((ulong)num)));
			PE.ApiSetNamespace apiSetNamespace = (PE.ApiSetNamespace)Marshal.PtrToStructure(intPtr, typeof(PE.ApiSetNamespace));
			for (int i = 0; i < apiSetNamespace.Count; i++)
			{
				PE.ApiSetNamespaceEntry apiSetNamespaceEntry = default(PE.ApiSetNamespaceEntry);
				apiSetNamespaceEntry = (PE.ApiSetNamespaceEntry)Marshal.PtrToStructure((IntPtr)((long)intPtr + (long)apiSetNamespace.EntryOffset + (long)(i * Marshal.SizeOf(apiSetNamespaceEntry))), typeof(PE.ApiSetNamespaceEntry));
				string text = Marshal.PtrToStringUni((IntPtr)((long)intPtr + (long)apiSetNamespaceEntry.NameOffset), apiSetNamespaceEntry.NameLength / 2);
				string key = text.Substring(0, text.Length - 2) + ".dll";
				PE.ApiSetValueEntry apiSetValueEntry = default(PE.ApiSetValueEntry);
				IntPtr intPtr2 = IntPtr.Zero;
				if (apiSetNamespaceEntry.ValueLength == 1)
				{
					intPtr2 = (IntPtr)((long)intPtr + (long)apiSetNamespaceEntry.ValueOffset);
				}
				else if (apiSetNamespaceEntry.ValueLength > 1)
				{
					for (int j = 0; j < apiSetNamespaceEntry.ValueLength; j++)
					{
						if (Marshal.PtrToStringUni((IntPtr)((long)intPtr + (long)apiSetNamespaceEntry.ValueOffset + (long)Marshal.SizeOf(apiSetValueEntry) * (long)j)) != text)
						{
							intPtr2 = (IntPtr)((long)intPtr + (long)apiSetNamespaceEntry.ValueOffset + (long)Marshal.SizeOf(apiSetValueEntry) * (long)j);
						}
					}
					if (intPtr2 == IntPtr.Zero)
					{
						intPtr2 = (IntPtr)((long)intPtr + (long)apiSetNamespaceEntry.ValueOffset);
					}
				}
				apiSetValueEntry = (PE.ApiSetValueEntry)Marshal.PtrToStructure(intPtr2, typeof(PE.ApiSetValueEntry));
				string value = string.Empty;
				if (apiSetValueEntry.ValueCount != 0)
				{
					value = Marshal.PtrToStringUni((IntPtr)((long)intPtr + (long)apiSetValueEntry.ValueOffset), apiSetValueEntry.ValueCount / 2);
				}
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000029C0 File Offset: 0x00000BC0
		public static IntPtr GetSyscallStub(string FunctionName)
		{
			bool flag = Native.NtQueryInformationProcessWow64Information((IntPtr)(-1));
			if (IntPtr.Size == 4 && flag)
			{
				throw new InvalidOperationException("Generating Syscall stubs is not supported for WOW64.");
			}
			string filePath = string.Empty;
			foreach (object obj in Process.GetCurrentProcess().Modules)
			{
				ProcessModule processModule = (ProcessModule)obj;
				if (processModule.FileName.EndsWith("ntdll.dll", StringComparison.OrdinalIgnoreCase))
				{
					filePath = processModule.FileName;
				}
			}
			IntPtr intPtr = Map.AllocateFileToMemory(filePath);
			PE.PE_META_DATA peMetaData = Generic.GetPeMetaData(intPtr);
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr2 = peMetaData.Is32Bit ? ((IntPtr)((long)((ulong)peMetaData.OptHeader32.SizeOfImage))) : ((IntPtr)((long)((ulong)peMetaData.OptHeader64.SizeOfImage)));
			uint bufferLength = peMetaData.Is32Bit ? peMetaData.OptHeader32.SizeOfHeaders : peMetaData.OptHeader64.SizeOfHeaders;
			IntPtr intPtr3 = Native.NtAllocateVirtualMemory((IntPtr)(-1), ref zero, IntPtr.Zero, ref intPtr2, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			Native.NtWriteVirtualMemory((IntPtr)(-1), intPtr3, intPtr, bufferLength);
			foreach (PE.IMAGE_SECTION_HEADER image_SECTION_HEADER in peMetaData.Sections)
			{
				IntPtr baseAddress = (IntPtr)((long)intPtr3 + (long)((ulong)image_SECTION_HEADER.VirtualAddress));
				IntPtr buffer = (IntPtr)((long)intPtr + (long)((ulong)image_SECTION_HEADER.PointerToRawData));
				if (Native.NtWriteVirtualMemory((IntPtr)(-1), baseAddress, buffer, image_SECTION_HEADER.SizeOfRawData) != image_SECTION_HEADER.SizeOfRawData)
				{
					throw new InvalidOperationException("Failed to write to memory.");
				}
			}
			IntPtr exportAddress = Generic.GetExportAddress(intPtr3, FunctionName, true);
			if (exportAddress == IntPtr.Zero)
			{
				throw new InvalidOperationException("Failed to resolve ntdll export.");
			}
			zero = IntPtr.Zero;
			intPtr2 = (IntPtr)80;
			IntPtr intPtr4 = Native.NtAllocateVirtualMemory((IntPtr)(-1), ref zero, IntPtr.Zero, ref intPtr2, Win32.Kernel32.MEM_COMMIT | Win32.Kernel32.MEM_RESERVE, 4U);
			if (Native.NtWriteVirtualMemory((IntPtr)(-1), intPtr4, exportAddress, 80U) != 80U)
			{
				throw new InvalidOperationException("Failed to write to memory.");
			}
			Native.NtProtectVirtualMemory((IntPtr)(-1), ref intPtr4, ref intPtr2, 32U);
			Marshal.FreeHGlobal(intPtr);
			intPtr2 = (peMetaData.Is32Bit ? ((IntPtr)((long)((ulong)peMetaData.OptHeader32.SizeOfImage))) : ((IntPtr)((long)((ulong)peMetaData.OptHeader64.SizeOfImage))));
			Native.NtFreeVirtualMemory((IntPtr)(-1), ref intPtr3, ref intPtr2, Win32.Kernel32.MEM_RELEASE);
			return intPtr4;
		}
	}
}
