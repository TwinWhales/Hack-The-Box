using System;

namespace DInvoke.Data
{
	// Token: 0x02000007 RID: 7
	public static class Win32
	{
		// Token: 0x0200002C RID: 44
		public static class Kernel32
		{
			// Token: 0x04000243 RID: 579
			public static uint MEM_COMMIT = 4096U;

			// Token: 0x04000244 RID: 580
			public static uint MEM_RESERVE = 8192U;

			// Token: 0x04000245 RID: 581
			public static uint MEM_RESET = 524288U;

			// Token: 0x04000246 RID: 582
			public static uint MEM_RESET_UNDO = 16777216U;

			// Token: 0x04000247 RID: 583
			public static uint MEM_LARGE_PAGES = 536870912U;

			// Token: 0x04000248 RID: 584
			public static uint MEM_PHYSICAL = 4194304U;

			// Token: 0x04000249 RID: 585
			public static uint MEM_TOP_DOWN = 1048576U;

			// Token: 0x0400024A RID: 586
			public static uint MEM_WRITE_WATCH = 2097152U;

			// Token: 0x0400024B RID: 587
			public static uint MEM_COALESCE_PLACEHOLDERS = 1U;

			// Token: 0x0400024C RID: 588
			public static uint MEM_PRESERVE_PLACEHOLDER = 2U;

			// Token: 0x0400024D RID: 589
			public static uint MEM_DECOMMIT = 16384U;

			// Token: 0x0400024E RID: 590
			public static uint MEM_RELEASE = 32768U;

			// Token: 0x0400024F RID: 591
			public static long BLOCK_NON_MICROSOFT_BINARIES_ALWAYS_ON = 17592186044416L;

			// Token: 0x04000250 RID: 592
			public static uint PROC_THREAD_ATTRIBUTE_PARENT_PROCESS = 131072U;

			// Token: 0x04000251 RID: 593
			public static uint PROC_THREAD_ATTRIBUTE_MITIGATION_POLICY = 131079U;

			// Token: 0x04000252 RID: 594
			public static uint EXTENDED_STARTUPINFO_PRESENT = 524288U;

			// Token: 0x0200008C RID: 140
			[Flags]
			public enum ProcessAccessFlags : uint
			{
				// Token: 0x0400030B RID: 779
				PROCESS_ALL_ACCESS = 2035711U,
				// Token: 0x0400030C RID: 780
				PROCESS_CREATE_PROCESS = 128U,
				// Token: 0x0400030D RID: 781
				PROCESS_CREATE_THREAD = 2U,
				// Token: 0x0400030E RID: 782
				PROCESS_DUP_HANDLE = 64U,
				// Token: 0x0400030F RID: 783
				PROCESS_QUERY_INFORMATION = 1024U,
				// Token: 0x04000310 RID: 784
				PROCESS_QUERY_LIMITED_INFORMATION = 4096U,
				// Token: 0x04000311 RID: 785
				PROCESS_SET_INFORMATION = 512U,
				// Token: 0x04000312 RID: 786
				PROCESS_SET_QUOTA = 256U,
				// Token: 0x04000313 RID: 787
				PROCESS_SUSPEND_RESUME = 2048U,
				// Token: 0x04000314 RID: 788
				PROCESS_TERMINATE = 1U,
				// Token: 0x04000315 RID: 789
				PROCESS_VM_OPERATION = 8U,
				// Token: 0x04000316 RID: 790
				PROCESS_VM_READ = 16U,
				// Token: 0x04000317 RID: 791
				PROCESS_VM_WRITE = 32U,
				// Token: 0x04000318 RID: 792
				SYNCHRONIZE = 1048576U
			}

			// Token: 0x0200008D RID: 141
			[Flags]
			public enum StandardRights : uint
			{
				// Token: 0x0400031A RID: 794
				Delete = 65536U,
				// Token: 0x0400031B RID: 795
				ReadControl = 131072U,
				// Token: 0x0400031C RID: 796
				WriteDac = 262144U,
				// Token: 0x0400031D RID: 797
				WriteOwner = 524288U,
				// Token: 0x0400031E RID: 798
				Synchronize = 1048576U,
				// Token: 0x0400031F RID: 799
				Required = 983040U,
				// Token: 0x04000320 RID: 800
				Read = 131072U,
				// Token: 0x04000321 RID: 801
				Write = 131072U,
				// Token: 0x04000322 RID: 802
				Execute = 131072U,
				// Token: 0x04000323 RID: 803
				All = 2031616U,
				// Token: 0x04000324 RID: 804
				SpecificRightsAll = 65535U,
				// Token: 0x04000325 RID: 805
				AccessSystemSecurity = 16777216U,
				// Token: 0x04000326 RID: 806
				MaximumAllowed = 33554432U,
				// Token: 0x04000327 RID: 807
				GenericRead = 2147483648U,
				// Token: 0x04000328 RID: 808
				GenericWrite = 1073741824U,
				// Token: 0x04000329 RID: 809
				GenericExecute = 536870912U,
				// Token: 0x0400032A RID: 810
				GenericAll = 268435456U
			}

			// Token: 0x0200008E RID: 142
			[Flags]
			public enum ThreadAccess : uint
			{
				// Token: 0x0400032C RID: 812
				Terminate = 1U,
				// Token: 0x0400032D RID: 813
				SuspendResume = 2U,
				// Token: 0x0400032E RID: 814
				Alert = 4U,
				// Token: 0x0400032F RID: 815
				GetContext = 8U,
				// Token: 0x04000330 RID: 816
				SetContext = 16U,
				// Token: 0x04000331 RID: 817
				SetInformation = 32U,
				// Token: 0x04000332 RID: 818
				QueryInformation = 64U,
				// Token: 0x04000333 RID: 819
				SetThreadToken = 128U,
				// Token: 0x04000334 RID: 820
				Impersonate = 256U,
				// Token: 0x04000335 RID: 821
				DirectImpersonation = 512U,
				// Token: 0x04000336 RID: 822
				SetLimitedInformation = 1024U,
				// Token: 0x04000337 RID: 823
				QueryLimitedInformation = 2048U,
				// Token: 0x04000338 RID: 824
				All = 2032639U
			}

			// Token: 0x0200008F RID: 143
			[Flags]
			public enum STARTF : uint
			{
				// Token: 0x0400033A RID: 826
				STARTF_USESHOWWINDOW = 1U
			}
		}

		// Token: 0x0200002D RID: 45
		public static class Advapi32
		{
			// Token: 0x02000090 RID: 144
			[Flags]
			public enum CREATION_FLAGS : uint
			{
				// Token: 0x0400033C RID: 828
				NONE = 0U,
				// Token: 0x0400033D RID: 829
				DEBUG_PROCESS = 1U,
				// Token: 0x0400033E RID: 830
				DEBUG_ONLY_THIS_PROCESS = 2U,
				// Token: 0x0400033F RID: 831
				CREATE_SUSPENDED = 4U,
				// Token: 0x04000340 RID: 832
				DETACHED_PROCESS = 8U,
				// Token: 0x04000341 RID: 833
				CREATE_NEW_CONSOLE = 16U,
				// Token: 0x04000342 RID: 834
				NORMAL_PRIORITY_CLASS = 32U,
				// Token: 0x04000343 RID: 835
				IDLE_PRIORITY_CLASS = 64U,
				// Token: 0x04000344 RID: 836
				HIGH_PRIORITY_CLASS = 128U,
				// Token: 0x04000345 RID: 837
				REALTIME_PRIORITY_CLASS = 256U,
				// Token: 0x04000346 RID: 838
				CREATE_NEW_PROCESS_GROUP = 512U,
				// Token: 0x04000347 RID: 839
				CREATE_UNICODE_ENVIRONMENT = 1024U,
				// Token: 0x04000348 RID: 840
				CREATE_SEPARATE_WOW_VDM = 2048U,
				// Token: 0x04000349 RID: 841
				CREATE_SHARED_WOW_VDM = 4096U,
				// Token: 0x0400034A RID: 842
				CREATE_FORCEDOS = 8192U,
				// Token: 0x0400034B RID: 843
				BELOW_NORMAL_PRIORITY_CLASS = 16384U,
				// Token: 0x0400034C RID: 844
				ABOVE_NORMAL_PRIORITY_CLASS = 32768U,
				// Token: 0x0400034D RID: 845
				INHERIT_PARENT_AFFINITY = 65536U,
				// Token: 0x0400034E RID: 846
				INHERIT_CALLER_PRIORITY = 131072U,
				// Token: 0x0400034F RID: 847
				CREATE_PROTECTED_PROCESS = 262144U,
				// Token: 0x04000350 RID: 848
				EXTENDED_STARTUPINFO_PRESENT = 524288U,
				// Token: 0x04000351 RID: 849
				PROCESS_MODE_BACKGROUND_BEGIN = 1048576U,
				// Token: 0x04000352 RID: 850
				PROCESS_MODE_BACKGROUND_END = 2097152U,
				// Token: 0x04000353 RID: 851
				CREATE_BREAKAWAY_FROM_JOB = 16777216U,
				// Token: 0x04000354 RID: 852
				CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 33554432U,
				// Token: 0x04000355 RID: 853
				CREATE_DEFAULT_ERROR_MODE = 67108864U,
				// Token: 0x04000356 RID: 854
				CREATE_NO_WINDOW = 134217728U,
				// Token: 0x04000357 RID: 855
				PROFILE_USER = 268435456U,
				// Token: 0x04000358 RID: 856
				PROFILE_KERNEL = 536870912U,
				// Token: 0x04000359 RID: 857
				PROFILE_SERVER = 1073741824U,
				// Token: 0x0400035A RID: 858
				CREATE_IGNORE_SYSTEM_DEFAULT = 2147483648U
			}
		}

		// Token: 0x0200002E RID: 46
		public class WinNT
		{
			// Token: 0x04000253 RID: 595
			public const uint PAGE_NOACCESS = 1U;

			// Token: 0x04000254 RID: 596
			public const uint PAGE_READONLY = 2U;

			// Token: 0x04000255 RID: 597
			public const uint PAGE_READWRITE = 4U;

			// Token: 0x04000256 RID: 598
			public const uint PAGE_WRITECOPY = 8U;

			// Token: 0x04000257 RID: 599
			public const uint PAGE_EXECUTE = 16U;

			// Token: 0x04000258 RID: 600
			public const uint PAGE_EXECUTE_READ = 32U;

			// Token: 0x04000259 RID: 601
			public const uint PAGE_EXECUTE_READWRITE = 64U;

			// Token: 0x0400025A RID: 602
			public const uint PAGE_EXECUTE_WRITECOPY = 128U;

			// Token: 0x0400025B RID: 603
			public const uint PAGE_GUARD = 256U;

			// Token: 0x0400025C RID: 604
			public const uint PAGE_NOCACHE = 512U;

			// Token: 0x0400025D RID: 605
			public const uint PAGE_WRITECOMBINE = 1024U;

			// Token: 0x0400025E RID: 606
			public const uint PAGE_TARGETS_INVALID = 1073741824U;

			// Token: 0x0400025F RID: 607
			public const uint PAGE_TARGETS_NO_UPDATE = 1073741824U;

			// Token: 0x04000260 RID: 608
			public const uint SEC_COMMIT = 134217728U;

			// Token: 0x04000261 RID: 609
			public const uint SEC_IMAGE = 16777216U;

			// Token: 0x04000262 RID: 610
			public const uint SEC_IMAGE_NO_EXECUTE = 285212672U;

			// Token: 0x04000263 RID: 611
			public const uint SEC_LARGE_PAGES = 2147483648U;

			// Token: 0x04000264 RID: 612
			public const uint SEC_NOCACHE = 268435456U;

			// Token: 0x04000265 RID: 613
			public const uint SEC_RESERVE = 67108864U;

			// Token: 0x04000266 RID: 614
			public const uint SEC_WRITECOMBINE = 1073741824U;

			// Token: 0x04000267 RID: 615
			public const uint SE_PRIVILEGE_ENABLED = 2U;

			// Token: 0x04000268 RID: 616
			public const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1U;

			// Token: 0x04000269 RID: 617
			public const uint SE_PRIVILEGE_REMOVED = 4U;

			// Token: 0x0400026A RID: 618
			public const uint SE_PRIVILEGE_USED_FOR_ACCESS = 3U;

			// Token: 0x0400026B RID: 619
			public const ulong SE_GROUP_ENABLED = 4UL;

			// Token: 0x0400026C RID: 620
			public const ulong SE_GROUP_ENABLED_BY_DEFAULT = 2UL;

			// Token: 0x0400026D RID: 621
			public const ulong SE_GROUP_INTEGRITY = 32UL;

			// Token: 0x0400026E RID: 622
			public const uint SE_GROUP_INTEGRITY_32 = 32U;

			// Token: 0x0400026F RID: 623
			public const ulong SE_GROUP_INTEGRITY_ENABLED = 64UL;

			// Token: 0x04000270 RID: 624
			public const ulong SE_GROUP_LOGON_ID = 3221225472UL;

			// Token: 0x04000271 RID: 625
			public const ulong SE_GROUP_MANDATORY = 1UL;

			// Token: 0x04000272 RID: 626
			public const ulong SE_GROUP_OWNER = 8UL;

			// Token: 0x04000273 RID: 627
			public const ulong SE_GROUP_RESOURCE = 536870912UL;

			// Token: 0x04000274 RID: 628
			public const ulong SE_GROUP_USE_FOR_DENY_ONLY = 16UL;

			// Token: 0x02000091 RID: 145
			[Flags]
			public enum ACCESS_MASK : uint
			{
				// Token: 0x0400035C RID: 860
				DELETE = 65536U,
				// Token: 0x0400035D RID: 861
				READ_CONTROL = 131072U,
				// Token: 0x0400035E RID: 862
				WRITE_DAC = 262144U,
				// Token: 0x0400035F RID: 863
				WRITE_OWNER = 524288U,
				// Token: 0x04000360 RID: 864
				SYNCHRONIZE = 1048576U,
				// Token: 0x04000361 RID: 865
				STANDARD_RIGHTS_REQUIRED = 983040U,
				// Token: 0x04000362 RID: 866
				STANDARD_RIGHTS_READ = 131072U,
				// Token: 0x04000363 RID: 867
				STANDARD_RIGHTS_WRITE = 131072U,
				// Token: 0x04000364 RID: 868
				STANDARD_RIGHTS_EXECUTE = 131072U,
				// Token: 0x04000365 RID: 869
				STANDARD_RIGHTS_ALL = 2031616U,
				// Token: 0x04000366 RID: 870
				SPECIFIC_RIGHTS_ALL = 4095U,
				// Token: 0x04000367 RID: 871
				ACCESS_SYSTEM_SECURITY = 16777216U,
				// Token: 0x04000368 RID: 872
				MAXIMUM_ALLOWED = 33554432U,
				// Token: 0x04000369 RID: 873
				GENERIC_READ = 2147483648U,
				// Token: 0x0400036A RID: 874
				GENERIC_WRITE = 1073741824U,
				// Token: 0x0400036B RID: 875
				GENERIC_EXECUTE = 536870912U,
				// Token: 0x0400036C RID: 876
				GENERIC_ALL = 268435456U,
				// Token: 0x0400036D RID: 877
				DESKTOP_READOBJECTS = 1U,
				// Token: 0x0400036E RID: 878
				DESKTOP_CREATEWINDOW = 2U,
				// Token: 0x0400036F RID: 879
				DESKTOP_CREATEMENU = 4U,
				// Token: 0x04000370 RID: 880
				DESKTOP_HOOKCONTROL = 8U,
				// Token: 0x04000371 RID: 881
				DESKTOP_JOURNALRECORD = 16U,
				// Token: 0x04000372 RID: 882
				DESKTOP_JOURNALPLAYBACK = 32U,
				// Token: 0x04000373 RID: 883
				DESKTOP_ENUMERATE = 64U,
				// Token: 0x04000374 RID: 884
				DESKTOP_WRITEOBJECTS = 128U,
				// Token: 0x04000375 RID: 885
				DESKTOP_SWITCHDESKTOP = 256U,
				// Token: 0x04000376 RID: 886
				WINSTA_ENUMDESKTOPS = 1U,
				// Token: 0x04000377 RID: 887
				WINSTA_READATTRIBUTES = 2U,
				// Token: 0x04000378 RID: 888
				WINSTA_ACCESSCLIPBOARD = 4U,
				// Token: 0x04000379 RID: 889
				WINSTA_CREATEDESKTOP = 8U,
				// Token: 0x0400037A RID: 890
				WINSTA_WRITEATTRIBUTES = 16U,
				// Token: 0x0400037B RID: 891
				WINSTA_ACCESSGLOBALATOMS = 32U,
				// Token: 0x0400037C RID: 892
				WINSTA_EXITWINDOWS = 64U,
				// Token: 0x0400037D RID: 893
				WINSTA_ENUMERATE = 256U,
				// Token: 0x0400037E RID: 894
				WINSTA_READSCREEN = 512U,
				// Token: 0x0400037F RID: 895
				WINSTA_ALL_ACCESS = 895U,
				// Token: 0x04000380 RID: 896
				SECTION_ALL_ACCESS = 268435456U,
				// Token: 0x04000381 RID: 897
				SECTION_QUERY = 1U,
				// Token: 0x04000382 RID: 898
				SECTION_MAP_WRITE = 2U,
				// Token: 0x04000383 RID: 899
				SECTION_MAP_READ = 4U,
				// Token: 0x04000384 RID: 900
				SECTION_MAP_EXECUTE = 8U,
				// Token: 0x04000385 RID: 901
				SECTION_EXTEND_SIZE = 16U
			}
		}

		// Token: 0x0200002F RID: 47
		public static class WinBase
		{
			// Token: 0x02000092 RID: 146
			public struct SECURITY_ATTRIBUTES
			{
				// Token: 0x04000386 RID: 902
				private uint nLength;

				// Token: 0x04000387 RID: 903
				private IntPtr lpSecurityDescriptor;

				// Token: 0x04000388 RID: 904
				private bool bInheritHandle;
			}
		}

		// Token: 0x02000030 RID: 48
		public class ProcessThreadsAPI
		{
			// Token: 0x02000093 RID: 147
			public struct _STARTUPINFO
			{
				// Token: 0x04000389 RID: 905
				public uint cb;

				// Token: 0x0400038A RID: 906
				public string lpReserved;

				// Token: 0x0400038B RID: 907
				public string lpDesktop;

				// Token: 0x0400038C RID: 908
				public string lpTitle;

				// Token: 0x0400038D RID: 909
				public uint dwX;

				// Token: 0x0400038E RID: 910
				public uint dwY;

				// Token: 0x0400038F RID: 911
				public uint dwXSize;

				// Token: 0x04000390 RID: 912
				public uint dwYSize;

				// Token: 0x04000391 RID: 913
				public uint dwXCountChars;

				// Token: 0x04000392 RID: 914
				public uint dwYCountChars;

				// Token: 0x04000393 RID: 915
				public uint dwFillAttribute;

				// Token: 0x04000394 RID: 916
				public uint dwFlags;

				// Token: 0x04000395 RID: 917
				public ushort wShowWindow;

				// Token: 0x04000396 RID: 918
				public ushort cbReserved2;

				// Token: 0x04000397 RID: 919
				public IntPtr lpReserved2;

				// Token: 0x04000398 RID: 920
				public IntPtr hStdInput;

				// Token: 0x04000399 RID: 921
				public IntPtr hStdOutput;

				// Token: 0x0400039A RID: 922
				public IntPtr hStdError;
			}

			// Token: 0x02000094 RID: 148
			public struct _STARTUPINFOEX
			{
				// Token: 0x0400039B RID: 923
				public Win32.ProcessThreadsAPI._STARTUPINFO StartupInfo;

				// Token: 0x0400039C RID: 924
				public IntPtr lpAttributeList;
			}

			// Token: 0x02000095 RID: 149
			public struct _PROCESS_INFORMATION
			{
				// Token: 0x0400039D RID: 925
				public IntPtr hProcess;

				// Token: 0x0400039E RID: 926
				public IntPtr hThread;

				// Token: 0x0400039F RID: 927
				public uint dwProcessId;

				// Token: 0x040003A0 RID: 928
				public uint dwThreadId;
			}
		}
	}
}
