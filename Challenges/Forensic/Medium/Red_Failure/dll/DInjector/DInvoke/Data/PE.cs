using System;
using System.Runtime.InteropServices;

namespace DInvoke.Data
{
	// Token: 0x02000006 RID: 6
	public static class PE
	{
		// Token: 0x02000021 RID: 33
		[Flags]
		public enum DataSectionFlags : uint
		{
			// Token: 0x0400019A RID: 410
			TYPE_NO_PAD = 8U,
			// Token: 0x0400019B RID: 411
			CNT_CODE = 32U,
			// Token: 0x0400019C RID: 412
			CNT_INITIALIZED_DATA = 64U,
			// Token: 0x0400019D RID: 413
			CNT_UNINITIALIZED_DATA = 128U,
			// Token: 0x0400019E RID: 414
			LNK_INFO = 512U,
			// Token: 0x0400019F RID: 415
			LNK_REMOVE = 2048U,
			// Token: 0x040001A0 RID: 416
			LNK_COMDAT = 4096U,
			// Token: 0x040001A1 RID: 417
			NO_DEFER_SPEC_EXC = 16384U,
			// Token: 0x040001A2 RID: 418
			GPREL = 32768U,
			// Token: 0x040001A3 RID: 419
			MEM_FARDATA = 32768U,
			// Token: 0x040001A4 RID: 420
			MEM_PURGEABLE = 131072U,
			// Token: 0x040001A5 RID: 421
			MEM_16BIT = 131072U,
			// Token: 0x040001A6 RID: 422
			MEM_LOCKED = 262144U,
			// Token: 0x040001A7 RID: 423
			MEM_PRELOAD = 524288U,
			// Token: 0x040001A8 RID: 424
			ALIGN_1BYTES = 1048576U,
			// Token: 0x040001A9 RID: 425
			ALIGN_2BYTES = 2097152U,
			// Token: 0x040001AA RID: 426
			ALIGN_4BYTES = 3145728U,
			// Token: 0x040001AB RID: 427
			ALIGN_8BYTES = 4194304U,
			// Token: 0x040001AC RID: 428
			ALIGN_16BYTES = 5242880U,
			// Token: 0x040001AD RID: 429
			ALIGN_32BYTES = 6291456U,
			// Token: 0x040001AE RID: 430
			ALIGN_64BYTES = 7340032U,
			// Token: 0x040001AF RID: 431
			ALIGN_128BYTES = 8388608U,
			// Token: 0x040001B0 RID: 432
			ALIGN_256BYTES = 9437184U,
			// Token: 0x040001B1 RID: 433
			ALIGN_512BYTES = 10485760U,
			// Token: 0x040001B2 RID: 434
			ALIGN_1024BYTES = 11534336U,
			// Token: 0x040001B3 RID: 435
			ALIGN_2048BYTES = 12582912U,
			// Token: 0x040001B4 RID: 436
			ALIGN_4096BYTES = 13631488U,
			// Token: 0x040001B5 RID: 437
			ALIGN_8192BYTES = 14680064U,
			// Token: 0x040001B6 RID: 438
			ALIGN_MASK = 15728640U,
			// Token: 0x040001B7 RID: 439
			LNK_NRELOC_OVFL = 16777216U,
			// Token: 0x040001B8 RID: 440
			MEM_DISCARDABLE = 33554432U,
			// Token: 0x040001B9 RID: 441
			MEM_NOT_CACHED = 67108864U,
			// Token: 0x040001BA RID: 442
			MEM_NOT_PAGED = 134217728U,
			// Token: 0x040001BB RID: 443
			MEM_SHARED = 268435456U,
			// Token: 0x040001BC RID: 444
			MEM_EXECUTE = 536870912U,
			// Token: 0x040001BD RID: 445
			MEM_READ = 1073741824U,
			// Token: 0x040001BE RID: 446
			MEM_WRITE = 2147483648U
		}

		// Token: 0x02000022 RID: 34
		public struct IMAGE_DATA_DIRECTORY
		{
			// Token: 0x040001BF RID: 447
			public uint VirtualAddress;

			// Token: 0x040001C0 RID: 448
			public uint Size;
		}

		// Token: 0x02000023 RID: 35
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct IMAGE_OPTIONAL_HEADER32
		{
			// Token: 0x040001C1 RID: 449
			public ushort Magic;

			// Token: 0x040001C2 RID: 450
			public byte MajorLinkerVersion;

			// Token: 0x040001C3 RID: 451
			public byte MinorLinkerVersion;

			// Token: 0x040001C4 RID: 452
			public uint SizeOfCode;

			// Token: 0x040001C5 RID: 453
			public uint SizeOfInitializedData;

			// Token: 0x040001C6 RID: 454
			public uint SizeOfUninitializedData;

			// Token: 0x040001C7 RID: 455
			public uint AddressOfEntryPoint;

			// Token: 0x040001C8 RID: 456
			public uint BaseOfCode;

			// Token: 0x040001C9 RID: 457
			public uint BaseOfData;

			// Token: 0x040001CA RID: 458
			public uint ImageBase;

			// Token: 0x040001CB RID: 459
			public uint SectionAlignment;

			// Token: 0x040001CC RID: 460
			public uint FileAlignment;

			// Token: 0x040001CD RID: 461
			public ushort MajorOperatingSystemVersion;

			// Token: 0x040001CE RID: 462
			public ushort MinorOperatingSystemVersion;

			// Token: 0x040001CF RID: 463
			public ushort MajorImageVersion;

			// Token: 0x040001D0 RID: 464
			public ushort MinorImageVersion;

			// Token: 0x040001D1 RID: 465
			public ushort MajorSubsystemVersion;

			// Token: 0x040001D2 RID: 466
			public ushort MinorSubsystemVersion;

			// Token: 0x040001D3 RID: 467
			public uint Win32VersionValue;

			// Token: 0x040001D4 RID: 468
			public uint SizeOfImage;

			// Token: 0x040001D5 RID: 469
			public uint SizeOfHeaders;

			// Token: 0x040001D6 RID: 470
			public uint CheckSum;

			// Token: 0x040001D7 RID: 471
			public ushort Subsystem;

			// Token: 0x040001D8 RID: 472
			public ushort DllCharacteristics;

			// Token: 0x040001D9 RID: 473
			public uint SizeOfStackReserve;

			// Token: 0x040001DA RID: 474
			public uint SizeOfStackCommit;

			// Token: 0x040001DB RID: 475
			public uint SizeOfHeapReserve;

			// Token: 0x040001DC RID: 476
			public uint SizeOfHeapCommit;

			// Token: 0x040001DD RID: 477
			public uint LoaderFlags;

			// Token: 0x040001DE RID: 478
			public uint NumberOfRvaAndSizes;

			// Token: 0x040001DF RID: 479
			public PE.IMAGE_DATA_DIRECTORY ExportTable;

			// Token: 0x040001E0 RID: 480
			public PE.IMAGE_DATA_DIRECTORY ImportTable;

			// Token: 0x040001E1 RID: 481
			public PE.IMAGE_DATA_DIRECTORY ResourceTable;

			// Token: 0x040001E2 RID: 482
			public PE.IMAGE_DATA_DIRECTORY ExceptionTable;

			// Token: 0x040001E3 RID: 483
			public PE.IMAGE_DATA_DIRECTORY CertificateTable;

			// Token: 0x040001E4 RID: 484
			public PE.IMAGE_DATA_DIRECTORY BaseRelocationTable;

			// Token: 0x040001E5 RID: 485
			public PE.IMAGE_DATA_DIRECTORY Debug;

			// Token: 0x040001E6 RID: 486
			public PE.IMAGE_DATA_DIRECTORY Architecture;

			// Token: 0x040001E7 RID: 487
			public PE.IMAGE_DATA_DIRECTORY GlobalPtr;

			// Token: 0x040001E8 RID: 488
			public PE.IMAGE_DATA_DIRECTORY TLSTable;

			// Token: 0x040001E9 RID: 489
			public PE.IMAGE_DATA_DIRECTORY LoadConfigTable;

			// Token: 0x040001EA RID: 490
			public PE.IMAGE_DATA_DIRECTORY BoundImport;

			// Token: 0x040001EB RID: 491
			public PE.IMAGE_DATA_DIRECTORY IAT;

			// Token: 0x040001EC RID: 492
			public PE.IMAGE_DATA_DIRECTORY DelayImportDescriptor;

			// Token: 0x040001ED RID: 493
			public PE.IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

			// Token: 0x040001EE RID: 494
			public PE.IMAGE_DATA_DIRECTORY Reserved;
		}

		// Token: 0x02000024 RID: 36
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct IMAGE_OPTIONAL_HEADER64
		{
			// Token: 0x040001EF RID: 495
			public ushort Magic;

			// Token: 0x040001F0 RID: 496
			public byte MajorLinkerVersion;

			// Token: 0x040001F1 RID: 497
			public byte MinorLinkerVersion;

			// Token: 0x040001F2 RID: 498
			public uint SizeOfCode;

			// Token: 0x040001F3 RID: 499
			public uint SizeOfInitializedData;

			// Token: 0x040001F4 RID: 500
			public uint SizeOfUninitializedData;

			// Token: 0x040001F5 RID: 501
			public uint AddressOfEntryPoint;

			// Token: 0x040001F6 RID: 502
			public uint BaseOfCode;

			// Token: 0x040001F7 RID: 503
			public ulong ImageBase;

			// Token: 0x040001F8 RID: 504
			public uint SectionAlignment;

			// Token: 0x040001F9 RID: 505
			public uint FileAlignment;

			// Token: 0x040001FA RID: 506
			public ushort MajorOperatingSystemVersion;

			// Token: 0x040001FB RID: 507
			public ushort MinorOperatingSystemVersion;

			// Token: 0x040001FC RID: 508
			public ushort MajorImageVersion;

			// Token: 0x040001FD RID: 509
			public ushort MinorImageVersion;

			// Token: 0x040001FE RID: 510
			public ushort MajorSubsystemVersion;

			// Token: 0x040001FF RID: 511
			public ushort MinorSubsystemVersion;

			// Token: 0x04000200 RID: 512
			public uint Win32VersionValue;

			// Token: 0x04000201 RID: 513
			public uint SizeOfImage;

			// Token: 0x04000202 RID: 514
			public uint SizeOfHeaders;

			// Token: 0x04000203 RID: 515
			public uint CheckSum;

			// Token: 0x04000204 RID: 516
			public ushort Subsystem;

			// Token: 0x04000205 RID: 517
			public ushort DllCharacteristics;

			// Token: 0x04000206 RID: 518
			public ulong SizeOfStackReserve;

			// Token: 0x04000207 RID: 519
			public ulong SizeOfStackCommit;

			// Token: 0x04000208 RID: 520
			public ulong SizeOfHeapReserve;

			// Token: 0x04000209 RID: 521
			public ulong SizeOfHeapCommit;

			// Token: 0x0400020A RID: 522
			public uint LoaderFlags;

			// Token: 0x0400020B RID: 523
			public uint NumberOfRvaAndSizes;

			// Token: 0x0400020C RID: 524
			public PE.IMAGE_DATA_DIRECTORY ExportTable;

			// Token: 0x0400020D RID: 525
			public PE.IMAGE_DATA_DIRECTORY ImportTable;

			// Token: 0x0400020E RID: 526
			public PE.IMAGE_DATA_DIRECTORY ResourceTable;

			// Token: 0x0400020F RID: 527
			public PE.IMAGE_DATA_DIRECTORY ExceptionTable;

			// Token: 0x04000210 RID: 528
			public PE.IMAGE_DATA_DIRECTORY CertificateTable;

			// Token: 0x04000211 RID: 529
			public PE.IMAGE_DATA_DIRECTORY BaseRelocationTable;

			// Token: 0x04000212 RID: 530
			public PE.IMAGE_DATA_DIRECTORY Debug;

			// Token: 0x04000213 RID: 531
			public PE.IMAGE_DATA_DIRECTORY Architecture;

			// Token: 0x04000214 RID: 532
			public PE.IMAGE_DATA_DIRECTORY GlobalPtr;

			// Token: 0x04000215 RID: 533
			public PE.IMAGE_DATA_DIRECTORY TLSTable;

			// Token: 0x04000216 RID: 534
			public PE.IMAGE_DATA_DIRECTORY LoadConfigTable;

			// Token: 0x04000217 RID: 535
			public PE.IMAGE_DATA_DIRECTORY BoundImport;

			// Token: 0x04000218 RID: 536
			public PE.IMAGE_DATA_DIRECTORY IAT;

			// Token: 0x04000219 RID: 537
			public PE.IMAGE_DATA_DIRECTORY DelayImportDescriptor;

			// Token: 0x0400021A RID: 538
			public PE.IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

			// Token: 0x0400021B RID: 539
			public PE.IMAGE_DATA_DIRECTORY Reserved;
		}

		// Token: 0x02000025 RID: 37
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct IMAGE_FILE_HEADER
		{
			// Token: 0x0400021C RID: 540
			public ushort Machine;

			// Token: 0x0400021D RID: 541
			public ushort NumberOfSections;

			// Token: 0x0400021E RID: 542
			public uint TimeDateStamp;

			// Token: 0x0400021F RID: 543
			public uint PointerToSymbolTable;

			// Token: 0x04000220 RID: 544
			public uint NumberOfSymbols;

			// Token: 0x04000221 RID: 545
			public ushort SizeOfOptionalHeader;

			// Token: 0x04000222 RID: 546
			public ushort Characteristics;
		}

		// Token: 0x02000026 RID: 38
		[StructLayout(LayoutKind.Explicit)]
		public struct IMAGE_SECTION_HEADER
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000051 RID: 81 RVA: 0x00005809 File Offset: 0x00003A09
			public string Section
			{
				get
				{
					return new string(this.Name);
				}
			}

			// Token: 0x04000223 RID: 547
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public char[] Name;

			// Token: 0x04000224 RID: 548
			[FieldOffset(8)]
			public uint VirtualSize;

			// Token: 0x04000225 RID: 549
			[FieldOffset(12)]
			public uint VirtualAddress;

			// Token: 0x04000226 RID: 550
			[FieldOffset(16)]
			public uint SizeOfRawData;

			// Token: 0x04000227 RID: 551
			[FieldOffset(20)]
			public uint PointerToRawData;

			// Token: 0x04000228 RID: 552
			[FieldOffset(24)]
			public uint PointerToRelocations;

			// Token: 0x04000229 RID: 553
			[FieldOffset(28)]
			public uint PointerToLinenumbers;

			// Token: 0x0400022A RID: 554
			[FieldOffset(32)]
			public ushort NumberOfRelocations;

			// Token: 0x0400022B RID: 555
			[FieldOffset(34)]
			public ushort NumberOfLinenumbers;

			// Token: 0x0400022C RID: 556
			[FieldOffset(36)]
			public PE.DataSectionFlags Characteristics;
		}

		// Token: 0x02000027 RID: 39
		public struct PE_META_DATA
		{
			// Token: 0x0400022D RID: 557
			public uint Pe;

			// Token: 0x0400022E RID: 558
			public bool Is32Bit;

			// Token: 0x0400022F RID: 559
			public PE.IMAGE_FILE_HEADER ImageFileHeader;

			// Token: 0x04000230 RID: 560
			public PE.IMAGE_OPTIONAL_HEADER32 OptHeader32;

			// Token: 0x04000231 RID: 561
			public PE.IMAGE_OPTIONAL_HEADER64 OptHeader64;

			// Token: 0x04000232 RID: 562
			public PE.IMAGE_SECTION_HEADER[] Sections;
		}

		// Token: 0x02000028 RID: 40
		public struct LDR_DATA_TABLE_ENTRY
		{
			// Token: 0x04000233 RID: 563
			public Native.LIST_ENTRY InLoadOrderLinks;

			// Token: 0x04000234 RID: 564
			public Native.LIST_ENTRY InMemoryOrderLinks;

			// Token: 0x04000235 RID: 565
			public Native.LIST_ENTRY InInitializationOrderLinks;

			// Token: 0x04000236 RID: 566
			public IntPtr DllBase;

			// Token: 0x04000237 RID: 567
			public IntPtr EntryPoint;

			// Token: 0x04000238 RID: 568
			public uint SizeOfImage;

			// Token: 0x04000239 RID: 569
			public Native.UNICODE_STRING FullDllName;

			// Token: 0x0400023A RID: 570
			public Native.UNICODE_STRING BaseDllName;
		}

		// Token: 0x02000029 RID: 41
		[StructLayout(LayoutKind.Explicit)]
		public struct ApiSetNamespace
		{
			// Token: 0x0400023B RID: 571
			[FieldOffset(12)]
			public int Count;

			// Token: 0x0400023C RID: 572
			[FieldOffset(16)]
			public int EntryOffset;
		}

		// Token: 0x0200002A RID: 42
		[StructLayout(LayoutKind.Explicit, Size = 24)]
		public struct ApiSetNamespaceEntry
		{
			// Token: 0x0400023D RID: 573
			[FieldOffset(4)]
			public int NameOffset;

			// Token: 0x0400023E RID: 574
			[FieldOffset(8)]
			public int NameLength;

			// Token: 0x0400023F RID: 575
			[FieldOffset(16)]
			public int ValueOffset;

			// Token: 0x04000240 RID: 576
			[FieldOffset(20)]
			public int ValueLength;
		}

		// Token: 0x0200002B RID: 43
		[StructLayout(LayoutKind.Explicit)]
		public struct ApiSetValueEntry
		{
			// Token: 0x04000241 RID: 577
			[FieldOffset(12)]
			public int ValueOffset;

			// Token: 0x04000242 RID: 578
			[FieldOffset(16)]
			public int ValueCount;
		}
	}
}
