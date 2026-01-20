using System;
using System.Runtime.InteropServices;

namespace DInvoke.Data
{
	// Token: 0x02000005 RID: 5
	public static class Native
	{
		// Token: 0x0200001C RID: 28
		public struct UNICODE_STRING
		{
			// Token: 0x04000004 RID: 4
			public ushort Length;

			// Token: 0x04000005 RID: 5
			public ushort MaximumLength;

			// Token: 0x04000006 RID: 6
			public IntPtr Buffer;
		}

		// Token: 0x0200001D RID: 29
		public struct PROCESS_BASIC_INFORMATION
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000050 RID: 80 RVA: 0x000057F8 File Offset: 0x000039F8
			public int Size
			{
				get
				{
					return Marshal.SizeOf(typeof(Native.PROCESS_BASIC_INFORMATION));
				}
			}

			// Token: 0x04000007 RID: 7
			public IntPtr ExitStatus;

			// Token: 0x04000008 RID: 8
			public IntPtr PebBaseAddress;

			// Token: 0x04000009 RID: 9
			public IntPtr AffinityMask;

			// Token: 0x0400000A RID: 10
			public IntPtr BasePriority;

			// Token: 0x0400000B RID: 11
			public UIntPtr UniqueProcessId;

			// Token: 0x0400000C RID: 12
			public int InheritedFromUniqueProcessId;
		}

		// Token: 0x0200001E RID: 30
		public struct LIST_ENTRY
		{
			// Token: 0x0400000D RID: 13
			public IntPtr Flink;

			// Token: 0x0400000E RID: 14
			public IntPtr Blink;
		}

		// Token: 0x0200001F RID: 31
		public enum PROCESSINFOCLASS
		{
			// Token: 0x04000010 RID: 16
			ProcessBasicInformation,
			// Token: 0x04000011 RID: 17
			ProcessQuotaLimits,
			// Token: 0x04000012 RID: 18
			ProcessIoCounters,
			// Token: 0x04000013 RID: 19
			ProcessVmCounters,
			// Token: 0x04000014 RID: 20
			ProcessTimes,
			// Token: 0x04000015 RID: 21
			ProcessBasePriority,
			// Token: 0x04000016 RID: 22
			ProcessRaisePriority,
			// Token: 0x04000017 RID: 23
			ProcessDebugPort,
			// Token: 0x04000018 RID: 24
			ProcessExceptionPort,
			// Token: 0x04000019 RID: 25
			ProcessAccessToken,
			// Token: 0x0400001A RID: 26
			ProcessLdtInformation,
			// Token: 0x0400001B RID: 27
			ProcessLdtSize,
			// Token: 0x0400001C RID: 28
			ProcessDefaultHardErrorMode,
			// Token: 0x0400001D RID: 29
			ProcessIoPortHandlers,
			// Token: 0x0400001E RID: 30
			ProcessPooledUsageAndLimits,
			// Token: 0x0400001F RID: 31
			ProcessWorkingSetWatch,
			// Token: 0x04000020 RID: 32
			ProcessUserModeIOPL,
			// Token: 0x04000021 RID: 33
			ProcessEnableAlignmentFaultFixup,
			// Token: 0x04000022 RID: 34
			ProcessPriorityClass,
			// Token: 0x04000023 RID: 35
			ProcessWx86Information,
			// Token: 0x04000024 RID: 36
			ProcessHandleCount,
			// Token: 0x04000025 RID: 37
			ProcessAffinityMask,
			// Token: 0x04000026 RID: 38
			ProcessPriorityBoost,
			// Token: 0x04000027 RID: 39
			ProcessDeviceMap,
			// Token: 0x04000028 RID: 40
			ProcessSessionInformation,
			// Token: 0x04000029 RID: 41
			ProcessForegroundInformation,
			// Token: 0x0400002A RID: 42
			ProcessWow64Information,
			// Token: 0x0400002B RID: 43
			ProcessImageFileName,
			// Token: 0x0400002C RID: 44
			ProcessLUIDDeviceMapsEnabled,
			// Token: 0x0400002D RID: 45
			ProcessBreakOnTermination,
			// Token: 0x0400002E RID: 46
			ProcessDebugObjectHandle,
			// Token: 0x0400002F RID: 47
			ProcessDebugFlags,
			// Token: 0x04000030 RID: 48
			ProcessHandleTracing,
			// Token: 0x04000031 RID: 49
			ProcessIoPriority,
			// Token: 0x04000032 RID: 50
			ProcessExecuteFlags,
			// Token: 0x04000033 RID: 51
			ProcessResourceManagement,
			// Token: 0x04000034 RID: 52
			ProcessCookie,
			// Token: 0x04000035 RID: 53
			ProcessImageInformation,
			// Token: 0x04000036 RID: 54
			ProcessCycleTime,
			// Token: 0x04000037 RID: 55
			ProcessPagePriority,
			// Token: 0x04000038 RID: 56
			ProcessInstrumentationCallback,
			// Token: 0x04000039 RID: 57
			ProcessThreadStackAllocation,
			// Token: 0x0400003A RID: 58
			ProcessWorkingSetWatchEx,
			// Token: 0x0400003B RID: 59
			ProcessImageFileNameWin32,
			// Token: 0x0400003C RID: 60
			ProcessImageFileMapping,
			// Token: 0x0400003D RID: 61
			ProcessAffinityUpdateMode,
			// Token: 0x0400003E RID: 62
			ProcessMemoryAllocationMode,
			// Token: 0x0400003F RID: 63
			ProcessGroupInformation,
			// Token: 0x04000040 RID: 64
			ProcessTokenVirtualizationEnabled,
			// Token: 0x04000041 RID: 65
			ProcessConsoleHostProcess,
			// Token: 0x04000042 RID: 66
			ProcessWindowInformation,
			// Token: 0x04000043 RID: 67
			ProcessHandleInformation,
			// Token: 0x04000044 RID: 68
			ProcessMitigationPolicy,
			// Token: 0x04000045 RID: 69
			ProcessDynamicFunctionTableInformation,
			// Token: 0x04000046 RID: 70
			ProcessHandleCheckingMode,
			// Token: 0x04000047 RID: 71
			ProcessKeepAliveCount,
			// Token: 0x04000048 RID: 72
			ProcessRevokeFileHandles,
			// Token: 0x04000049 RID: 73
			MaxProcessInfoClass
		}

		// Token: 0x02000020 RID: 32
		public enum NTSTATUS : uint
		{
			// Token: 0x0400004B RID: 75
			Success,
			// Token: 0x0400004C RID: 76
			Wait0 = 0U,
			// Token: 0x0400004D RID: 77
			Wait1,
			// Token: 0x0400004E RID: 78
			Wait2,
			// Token: 0x0400004F RID: 79
			Wait3,
			// Token: 0x04000050 RID: 80
			Wait63 = 63U,
			// Token: 0x04000051 RID: 81
			Abandoned = 128U,
			// Token: 0x04000052 RID: 82
			AbandonedWait0 = 128U,
			// Token: 0x04000053 RID: 83
			AbandonedWait1,
			// Token: 0x04000054 RID: 84
			AbandonedWait2,
			// Token: 0x04000055 RID: 85
			AbandonedWait3,
			// Token: 0x04000056 RID: 86
			AbandonedWait63 = 191U,
			// Token: 0x04000057 RID: 87
			UserApc,
			// Token: 0x04000058 RID: 88
			KernelApc = 256U,
			// Token: 0x04000059 RID: 89
			Alerted,
			// Token: 0x0400005A RID: 90
			Timeout,
			// Token: 0x0400005B RID: 91
			Pending,
			// Token: 0x0400005C RID: 92
			Reparse,
			// Token: 0x0400005D RID: 93
			MoreEntries,
			// Token: 0x0400005E RID: 94
			NotAllAssigned,
			// Token: 0x0400005F RID: 95
			SomeNotMapped,
			// Token: 0x04000060 RID: 96
			OpLockBreakInProgress,
			// Token: 0x04000061 RID: 97
			VolumeMounted,
			// Token: 0x04000062 RID: 98
			RxActCommitted,
			// Token: 0x04000063 RID: 99
			NotifyCleanup,
			// Token: 0x04000064 RID: 100
			NotifyEnumDir,
			// Token: 0x04000065 RID: 101
			NoQuotasForAccount,
			// Token: 0x04000066 RID: 102
			PrimaryTransportConnectFailed,
			// Token: 0x04000067 RID: 103
			PageFaultTransition = 272U,
			// Token: 0x04000068 RID: 104
			PageFaultDemandZero,
			// Token: 0x04000069 RID: 105
			PageFaultCopyOnWrite,
			// Token: 0x0400006A RID: 106
			PageFaultGuardPage,
			// Token: 0x0400006B RID: 107
			PageFaultPagingFile,
			// Token: 0x0400006C RID: 108
			CrashDump = 278U,
			// Token: 0x0400006D RID: 109
			ReparseObject = 280U,
			// Token: 0x0400006E RID: 110
			NothingToTerminate = 290U,
			// Token: 0x0400006F RID: 111
			ProcessNotInJob,
			// Token: 0x04000070 RID: 112
			ProcessInJob,
			// Token: 0x04000071 RID: 113
			ProcessCloned = 297U,
			// Token: 0x04000072 RID: 114
			FileLockedWithOnlyReaders,
			// Token: 0x04000073 RID: 115
			FileLockedWithWriters,
			// Token: 0x04000074 RID: 116
			Informational = 1073741824U,
			// Token: 0x04000075 RID: 117
			ObjectNameExists = 1073741824U,
			// Token: 0x04000076 RID: 118
			ThreadWasSuspended,
			// Token: 0x04000077 RID: 119
			WorkingSetLimitRange,
			// Token: 0x04000078 RID: 120
			ImageNotAtBase,
			// Token: 0x04000079 RID: 121
			RegistryRecovered = 1073741833U,
			// Token: 0x0400007A RID: 122
			Warning = 2147483648U,
			// Token: 0x0400007B RID: 123
			GuardPageViolation,
			// Token: 0x0400007C RID: 124
			DatatypeMisalignment,
			// Token: 0x0400007D RID: 125
			Breakpoint,
			// Token: 0x0400007E RID: 126
			SingleStep,
			// Token: 0x0400007F RID: 127
			BufferOverflow,
			// Token: 0x04000080 RID: 128
			NoMoreFiles,
			// Token: 0x04000081 RID: 129
			HandlesClosed = 2147483658U,
			// Token: 0x04000082 RID: 130
			PartialCopy = 2147483661U,
			// Token: 0x04000083 RID: 131
			DeviceBusy = 2147483665U,
			// Token: 0x04000084 RID: 132
			InvalidEaName = 2147483667U,
			// Token: 0x04000085 RID: 133
			EaListInconsistent,
			// Token: 0x04000086 RID: 134
			NoMoreEntries = 2147483674U,
			// Token: 0x04000087 RID: 135
			LongJump = 2147483686U,
			// Token: 0x04000088 RID: 136
			DllMightBeInsecure = 2147483691U,
			// Token: 0x04000089 RID: 137
			Error = 3221225472U,
			// Token: 0x0400008A RID: 138
			Unsuccessful,
			// Token: 0x0400008B RID: 139
			NotImplemented,
			// Token: 0x0400008C RID: 140
			InvalidInfoClass,
			// Token: 0x0400008D RID: 141
			InfoLengthMismatch,
			// Token: 0x0400008E RID: 142
			AccessViolation,
			// Token: 0x0400008F RID: 143
			InPageError,
			// Token: 0x04000090 RID: 144
			PagefileQuota,
			// Token: 0x04000091 RID: 145
			InvalidHandle,
			// Token: 0x04000092 RID: 146
			BadInitialStack,
			// Token: 0x04000093 RID: 147
			BadInitialPc,
			// Token: 0x04000094 RID: 148
			InvalidCid,
			// Token: 0x04000095 RID: 149
			TimerNotCanceled,
			// Token: 0x04000096 RID: 150
			InvalidParameter,
			// Token: 0x04000097 RID: 151
			NoSuchDevice,
			// Token: 0x04000098 RID: 152
			NoSuchFile,
			// Token: 0x04000099 RID: 153
			InvalidDeviceRequest,
			// Token: 0x0400009A RID: 154
			EndOfFile,
			// Token: 0x0400009B RID: 155
			WrongVolume,
			// Token: 0x0400009C RID: 156
			NoMediaInDevice,
			// Token: 0x0400009D RID: 157
			NoMemory = 3221225495U,
			// Token: 0x0400009E RID: 158
			ConflictingAddresses,
			// Token: 0x0400009F RID: 159
			NotMappedView,
			// Token: 0x040000A0 RID: 160
			UnableToFreeVm,
			// Token: 0x040000A1 RID: 161
			UnableToDeleteSection,
			// Token: 0x040000A2 RID: 162
			IllegalInstruction = 3221225501U,
			// Token: 0x040000A3 RID: 163
			AlreadyCommitted = 3221225505U,
			// Token: 0x040000A4 RID: 164
			AccessDenied,
			// Token: 0x040000A5 RID: 165
			BufferTooSmall,
			// Token: 0x040000A6 RID: 166
			ObjectTypeMismatch,
			// Token: 0x040000A7 RID: 167
			NonContinuableException,
			// Token: 0x040000A8 RID: 168
			BadStack = 3221225512U,
			// Token: 0x040000A9 RID: 169
			NotLocked = 3221225514U,
			// Token: 0x040000AA RID: 170
			NotCommitted = 3221225517U,
			// Token: 0x040000AB RID: 171
			InvalidParameterMix = 3221225520U,
			// Token: 0x040000AC RID: 172
			ObjectNameInvalid = 3221225523U,
			// Token: 0x040000AD RID: 173
			ObjectNameNotFound,
			// Token: 0x040000AE RID: 174
			ObjectNameCollision,
			// Token: 0x040000AF RID: 175
			ObjectPathInvalid = 3221225529U,
			// Token: 0x040000B0 RID: 176
			ObjectPathNotFound,
			// Token: 0x040000B1 RID: 177
			ObjectPathSyntaxBad,
			// Token: 0x040000B2 RID: 178
			DataOverrun,
			// Token: 0x040000B3 RID: 179
			DataLate,
			// Token: 0x040000B4 RID: 180
			DataError,
			// Token: 0x040000B5 RID: 181
			CrcError,
			// Token: 0x040000B6 RID: 182
			SectionTooBig,
			// Token: 0x040000B7 RID: 183
			PortConnectionRefused,
			// Token: 0x040000B8 RID: 184
			InvalidPortHandle,
			// Token: 0x040000B9 RID: 185
			SharingViolation,
			// Token: 0x040000BA RID: 186
			QuotaExceeded,
			// Token: 0x040000BB RID: 187
			InvalidPageProtection,
			// Token: 0x040000BC RID: 188
			MutantNotOwned,
			// Token: 0x040000BD RID: 189
			SemaphoreLimitExceeded,
			// Token: 0x040000BE RID: 190
			PortAlreadySet,
			// Token: 0x040000BF RID: 191
			SectionNotImage,
			// Token: 0x040000C0 RID: 192
			SuspendCountExceeded,
			// Token: 0x040000C1 RID: 193
			ThreadIsTerminating,
			// Token: 0x040000C2 RID: 194
			BadWorkingSetLimit,
			// Token: 0x040000C3 RID: 195
			IncompatibleFileMap,
			// Token: 0x040000C4 RID: 196
			SectionProtection,
			// Token: 0x040000C5 RID: 197
			EasNotSupported,
			// Token: 0x040000C6 RID: 198
			EaTooLarge,
			// Token: 0x040000C7 RID: 199
			NonExistentEaEntry,
			// Token: 0x040000C8 RID: 200
			NoEasOnFile,
			// Token: 0x040000C9 RID: 201
			EaCorruptError,
			// Token: 0x040000CA RID: 202
			FileLockConflict,
			// Token: 0x040000CB RID: 203
			LockNotGranted,
			// Token: 0x040000CC RID: 204
			DeletePending,
			// Token: 0x040000CD RID: 205
			CtlFileNotSupported,
			// Token: 0x040000CE RID: 206
			UnknownRevision,
			// Token: 0x040000CF RID: 207
			RevisionMismatch,
			// Token: 0x040000D0 RID: 208
			InvalidOwner,
			// Token: 0x040000D1 RID: 209
			InvalidPrimaryGroup,
			// Token: 0x040000D2 RID: 210
			NoImpersonationToken,
			// Token: 0x040000D3 RID: 211
			CantDisableMandatory,
			// Token: 0x040000D4 RID: 212
			NoLogonServers,
			// Token: 0x040000D5 RID: 213
			NoSuchLogonSession,
			// Token: 0x040000D6 RID: 214
			NoSuchPrivilege,
			// Token: 0x040000D7 RID: 215
			PrivilegeNotHeld,
			// Token: 0x040000D8 RID: 216
			InvalidAccountName,
			// Token: 0x040000D9 RID: 217
			UserExists,
			// Token: 0x040000DA RID: 218
			NoSuchUser,
			// Token: 0x040000DB RID: 219
			GroupExists,
			// Token: 0x040000DC RID: 220
			NoSuchGroup,
			// Token: 0x040000DD RID: 221
			MemberInGroup,
			// Token: 0x040000DE RID: 222
			MemberNotInGroup,
			// Token: 0x040000DF RID: 223
			LastAdmin,
			// Token: 0x040000E0 RID: 224
			WrongPassword,
			// Token: 0x040000E1 RID: 225
			IllFormedPassword,
			// Token: 0x040000E2 RID: 226
			PasswordRestriction,
			// Token: 0x040000E3 RID: 227
			LogonFailure,
			// Token: 0x040000E4 RID: 228
			AccountRestriction,
			// Token: 0x040000E5 RID: 229
			InvalidLogonHours,
			// Token: 0x040000E6 RID: 230
			InvalidWorkstation,
			// Token: 0x040000E7 RID: 231
			PasswordExpired,
			// Token: 0x040000E8 RID: 232
			AccountDisabled,
			// Token: 0x040000E9 RID: 233
			NoneMapped,
			// Token: 0x040000EA RID: 234
			TooManyLuidsRequested,
			// Token: 0x040000EB RID: 235
			LuidsExhausted,
			// Token: 0x040000EC RID: 236
			InvalidSubAuthority,
			// Token: 0x040000ED RID: 237
			InvalidAcl,
			// Token: 0x040000EE RID: 238
			InvalidSid,
			// Token: 0x040000EF RID: 239
			InvalidSecurityDescr,
			// Token: 0x040000F0 RID: 240
			ProcedureNotFound,
			// Token: 0x040000F1 RID: 241
			InvalidImageFormat,
			// Token: 0x040000F2 RID: 242
			NoToken,
			// Token: 0x040000F3 RID: 243
			BadInheritanceAcl,
			// Token: 0x040000F4 RID: 244
			RangeNotLocked,
			// Token: 0x040000F5 RID: 245
			DiskFull,
			// Token: 0x040000F6 RID: 246
			ServerDisabled,
			// Token: 0x040000F7 RID: 247
			ServerNotDisabled,
			// Token: 0x040000F8 RID: 248
			TooManyGuidsRequested,
			// Token: 0x040000F9 RID: 249
			GuidsExhausted,
			// Token: 0x040000FA RID: 250
			InvalidIdAuthority,
			// Token: 0x040000FB RID: 251
			AgentsExhausted,
			// Token: 0x040000FC RID: 252
			InvalidVolumeLabel,
			// Token: 0x040000FD RID: 253
			SectionNotExtended,
			// Token: 0x040000FE RID: 254
			NotMappedData,
			// Token: 0x040000FF RID: 255
			ResourceDataNotFound,
			// Token: 0x04000100 RID: 256
			ResourceTypeNotFound,
			// Token: 0x04000101 RID: 257
			ResourceNameNotFound,
			// Token: 0x04000102 RID: 258
			ArrayBoundsExceeded,
			// Token: 0x04000103 RID: 259
			FloatDenormalOperand,
			// Token: 0x04000104 RID: 260
			FloatDivideByZero,
			// Token: 0x04000105 RID: 261
			FloatInexactResult,
			// Token: 0x04000106 RID: 262
			FloatInvalidOperation,
			// Token: 0x04000107 RID: 263
			FloatOverflow,
			// Token: 0x04000108 RID: 264
			FloatStackCheck,
			// Token: 0x04000109 RID: 265
			FloatUnderflow,
			// Token: 0x0400010A RID: 266
			IntegerDivideByZero,
			// Token: 0x0400010B RID: 267
			IntegerOverflow,
			// Token: 0x0400010C RID: 268
			PrivilegedInstruction,
			// Token: 0x0400010D RID: 269
			TooManyPagingFiles,
			// Token: 0x0400010E RID: 270
			FileInvalid,
			// Token: 0x0400010F RID: 271
			InsufficientResources = 3221225626U,
			// Token: 0x04000110 RID: 272
			InstanceNotAvailable = 3221225643U,
			// Token: 0x04000111 RID: 273
			PipeNotAvailable,
			// Token: 0x04000112 RID: 274
			InvalidPipeState,
			// Token: 0x04000113 RID: 275
			PipeBusy,
			// Token: 0x04000114 RID: 276
			IllegalFunction,
			// Token: 0x04000115 RID: 277
			PipeDisconnected,
			// Token: 0x04000116 RID: 278
			PipeClosing,
			// Token: 0x04000117 RID: 279
			PipeConnected,
			// Token: 0x04000118 RID: 280
			PipeListening,
			// Token: 0x04000119 RID: 281
			InvalidReadMode,
			// Token: 0x0400011A RID: 282
			IoTimeout,
			// Token: 0x0400011B RID: 283
			FileForcedClosed,
			// Token: 0x0400011C RID: 284
			ProfilingNotStarted,
			// Token: 0x0400011D RID: 285
			ProfilingNotStopped,
			// Token: 0x0400011E RID: 286
			NotSameDevice = 3221225684U,
			// Token: 0x0400011F RID: 287
			FileRenamed,
			// Token: 0x04000120 RID: 288
			CantWait = 3221225688U,
			// Token: 0x04000121 RID: 289
			PipeEmpty,
			// Token: 0x04000122 RID: 290
			CantTerminateSelf = 3221225691U,
			// Token: 0x04000123 RID: 291
			InternalError = 3221225701U,
			// Token: 0x04000124 RID: 292
			InvalidParameter1 = 3221225711U,
			// Token: 0x04000125 RID: 293
			InvalidParameter2,
			// Token: 0x04000126 RID: 294
			InvalidParameter3,
			// Token: 0x04000127 RID: 295
			InvalidParameter4,
			// Token: 0x04000128 RID: 296
			InvalidParameter5,
			// Token: 0x04000129 RID: 297
			InvalidParameter6,
			// Token: 0x0400012A RID: 298
			InvalidParameter7,
			// Token: 0x0400012B RID: 299
			InvalidParameter8,
			// Token: 0x0400012C RID: 300
			InvalidParameter9,
			// Token: 0x0400012D RID: 301
			InvalidParameter10,
			// Token: 0x0400012E RID: 302
			InvalidParameter11,
			// Token: 0x0400012F RID: 303
			InvalidParameter12,
			// Token: 0x04000130 RID: 304
			ProcessIsTerminating = 3221225738U,
			// Token: 0x04000131 RID: 305
			MappedFileSizeZero = 3221225758U,
			// Token: 0x04000132 RID: 306
			TooManyOpenedFiles,
			// Token: 0x04000133 RID: 307
			Cancelled,
			// Token: 0x04000134 RID: 308
			CannotDelete,
			// Token: 0x04000135 RID: 309
			InvalidComputerName,
			// Token: 0x04000136 RID: 310
			FileDeleted,
			// Token: 0x04000137 RID: 311
			SpecialAccount,
			// Token: 0x04000138 RID: 312
			SpecialGroup,
			// Token: 0x04000139 RID: 313
			SpecialUser,
			// Token: 0x0400013A RID: 314
			MembersPrimaryGroup,
			// Token: 0x0400013B RID: 315
			FileClosed,
			// Token: 0x0400013C RID: 316
			TooManyThreads,
			// Token: 0x0400013D RID: 317
			ThreadNotInProcess,
			// Token: 0x0400013E RID: 318
			TokenAlreadyInUse,
			// Token: 0x0400013F RID: 319
			PagefileQuotaExceeded,
			// Token: 0x04000140 RID: 320
			CommitmentLimit,
			// Token: 0x04000141 RID: 321
			InvalidImageLeFormat,
			// Token: 0x04000142 RID: 322
			InvalidImageNotMz,
			// Token: 0x04000143 RID: 323
			InvalidImageProtect,
			// Token: 0x04000144 RID: 324
			InvalidImageWin16,
			// Token: 0x04000145 RID: 325
			LogonServer,
			// Token: 0x04000146 RID: 326
			DifferenceAtDc,
			// Token: 0x04000147 RID: 327
			SynchronizationRequired,
			// Token: 0x04000148 RID: 328
			DllNotFound,
			// Token: 0x04000149 RID: 329
			IoPrivilegeFailed = 3221225783U,
			// Token: 0x0400014A RID: 330
			OrdinalNotFound,
			// Token: 0x0400014B RID: 331
			EntryPointNotFound,
			// Token: 0x0400014C RID: 332
			ControlCExit,
			// Token: 0x0400014D RID: 333
			InvalidAddress = 3221225793U,
			// Token: 0x0400014E RID: 334
			PortNotSet = 3221226323U,
			// Token: 0x0400014F RID: 335
			DebuggerInactive,
			// Token: 0x04000150 RID: 336
			CallbackBypass = 3221226755U,
			// Token: 0x04000151 RID: 337
			PortClosed = 3221227264U,
			// Token: 0x04000152 RID: 338
			MessageLost,
			// Token: 0x04000153 RID: 339
			InvalidMessage,
			// Token: 0x04000154 RID: 340
			RequestCanceled,
			// Token: 0x04000155 RID: 341
			RecursiveDispatch,
			// Token: 0x04000156 RID: 342
			LpcReceiveBufferExpected,
			// Token: 0x04000157 RID: 343
			LpcInvalidConnectionUsage,
			// Token: 0x04000158 RID: 344
			LpcRequestsNotAllowed,
			// Token: 0x04000159 RID: 345
			ResourceInUse,
			// Token: 0x0400015A RID: 346
			ProcessIsProtected = 3221227282U,
			// Token: 0x0400015B RID: 347
			VolumeDirty = 3221227526U,
			// Token: 0x0400015C RID: 348
			FileCheckedOut = 3221227777U,
			// Token: 0x0400015D RID: 349
			CheckOutRequired,
			// Token: 0x0400015E RID: 350
			BadFileType,
			// Token: 0x0400015F RID: 351
			FileTooLarge,
			// Token: 0x04000160 RID: 352
			FormsAuthRequired,
			// Token: 0x04000161 RID: 353
			VirusInfected,
			// Token: 0x04000162 RID: 354
			VirusDeleted,
			// Token: 0x04000163 RID: 355
			TransactionalConflict = 3222863873U,
			// Token: 0x04000164 RID: 356
			InvalidTransaction,
			// Token: 0x04000165 RID: 357
			TransactionNotActive,
			// Token: 0x04000166 RID: 358
			TmInitializationFailed,
			// Token: 0x04000167 RID: 359
			RmNotActive,
			// Token: 0x04000168 RID: 360
			RmMetadataCorrupt,
			// Token: 0x04000169 RID: 361
			TransactionNotJoined,
			// Token: 0x0400016A RID: 362
			DirectoryNotRm,
			// Token: 0x0400016B RID: 363
			CouldNotResizeLog,
			// Token: 0x0400016C RID: 364
			TransactionsUnsupportedRemote,
			// Token: 0x0400016D RID: 365
			LogResizeInvalidSize,
			// Token: 0x0400016E RID: 366
			RemoteFileVersionMismatch,
			// Token: 0x0400016F RID: 367
			CrmProtocolAlreadyExists = 3222863887U,
			// Token: 0x04000170 RID: 368
			TransactionPropagationFailed,
			// Token: 0x04000171 RID: 369
			CrmProtocolNotFound,
			// Token: 0x04000172 RID: 370
			TransactionSuperiorExists,
			// Token: 0x04000173 RID: 371
			TransactionRequestNotValid,
			// Token: 0x04000174 RID: 372
			TransactionNotRequested,
			// Token: 0x04000175 RID: 373
			TransactionAlreadyAborted,
			// Token: 0x04000176 RID: 374
			TransactionAlreadyCommitted,
			// Token: 0x04000177 RID: 375
			TransactionInvalidMarshallBuffer,
			// Token: 0x04000178 RID: 376
			CurrentTransactionNotValid,
			// Token: 0x04000179 RID: 377
			LogGrowthFailed,
			// Token: 0x0400017A RID: 378
			ObjectNoLongerExists = 3222863905U,
			// Token: 0x0400017B RID: 379
			StreamMiniversionNotFound,
			// Token: 0x0400017C RID: 380
			StreamMiniversionNotValid,
			// Token: 0x0400017D RID: 381
			MiniversionInaccessibleFromSpecifiedTransaction,
			// Token: 0x0400017E RID: 382
			CantOpenMiniversionWithModifyIntent,
			// Token: 0x0400017F RID: 383
			CantCreateMoreStreamMiniversions,
			// Token: 0x04000180 RID: 384
			HandleNoLongerValid = 3222863912U,
			// Token: 0x04000181 RID: 385
			NoTxfMetadata,
			// Token: 0x04000182 RID: 386
			LogCorruptionDetected = 3222863920U,
			// Token: 0x04000183 RID: 387
			CantRecoverWithHandleOpen,
			// Token: 0x04000184 RID: 388
			RmDisconnected,
			// Token: 0x04000185 RID: 389
			EnlistmentNotSuperior,
			// Token: 0x04000186 RID: 390
			RecoveryNotNeeded,
			// Token: 0x04000187 RID: 391
			RmAlreadyStarted,
			// Token: 0x04000188 RID: 392
			FileIdentityNotPersistent,
			// Token: 0x04000189 RID: 393
			CantBreakTransactionalDependency,
			// Token: 0x0400018A RID: 394
			CantCrossRmBoundary,
			// Token: 0x0400018B RID: 395
			TxfDirNotEmpty,
			// Token: 0x0400018C RID: 396
			IndoubtTransactionsExist,
			// Token: 0x0400018D RID: 397
			TmVolatile,
			// Token: 0x0400018E RID: 398
			RollbackTimerExpired,
			// Token: 0x0400018F RID: 399
			TxfAttributeCorrupt,
			// Token: 0x04000190 RID: 400
			EfsNotAllowedInTransaction,
			// Token: 0x04000191 RID: 401
			TransactionalOpenNotAllowed,
			// Token: 0x04000192 RID: 402
			TransactedMappingUnsupportedRemote,
			// Token: 0x04000193 RID: 403
			TxfMetadataAlreadyPresent,
			// Token: 0x04000194 RID: 404
			TransactionScopeCallbacksNotSet,
			// Token: 0x04000195 RID: 405
			TransactionRequiredPromotion,
			// Token: 0x04000196 RID: 406
			CannotExecuteFileInTransaction,
			// Token: 0x04000197 RID: 407
			TransactionsNotFrozen,
			// Token: 0x04000198 RID: 408
			MaximumNtStatus = 4294967295U
		}
	}
}
