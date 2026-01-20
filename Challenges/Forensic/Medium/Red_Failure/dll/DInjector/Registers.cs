using System;
using System.Runtime.InteropServices;

namespace DInjector
{
	// Token: 0x02000014 RID: 20
	internal class Registers
	{
		// Token: 0x0200006E RID: 110
		public struct FLOATING_SAVE_AREA
		{
			// Token: 0x04000295 RID: 661
			public uint ControlWord;

			// Token: 0x04000296 RID: 662
			public uint StatusWord;

			// Token: 0x04000297 RID: 663
			public uint TagWord;

			// Token: 0x04000298 RID: 664
			public uint ErrorOffset;

			// Token: 0x04000299 RID: 665
			public uint ErrorSelector;

			// Token: 0x0400029A RID: 666
			public uint DataOffset;

			// Token: 0x0400029B RID: 667
			public uint DataSelector;

			// Token: 0x0400029C RID: 668
			public uint Cr0NpxState;

			// Token: 0x0400029D RID: 669
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
			public byte[] RegisterArea;
		}

		// Token: 0x0200006F RID: 111
		public struct CONTEXT
		{
			// Token: 0x0400029E RID: 670
			public uint ContextFlags;

			// Token: 0x0400029F RID: 671
			public uint Dr0;

			// Token: 0x040002A0 RID: 672
			public uint Dr1;

			// Token: 0x040002A1 RID: 673
			public uint Dr2;

			// Token: 0x040002A2 RID: 674
			public uint Dr3;

			// Token: 0x040002A3 RID: 675
			public uint Dr6;

			// Token: 0x040002A4 RID: 676
			public uint Dr7;

			// Token: 0x040002A5 RID: 677
			public Registers.FLOATING_SAVE_AREA FloatSave;

			// Token: 0x040002A6 RID: 678
			public uint SegGs;

			// Token: 0x040002A7 RID: 679
			public uint SegFs;

			// Token: 0x040002A8 RID: 680
			public uint SegEs;

			// Token: 0x040002A9 RID: 681
			public uint SegDs;

			// Token: 0x040002AA RID: 682
			public uint Edi;

			// Token: 0x040002AB RID: 683
			public uint Esi;

			// Token: 0x040002AC RID: 684
			public uint Ebx;

			// Token: 0x040002AD RID: 685
			public uint Edx;

			// Token: 0x040002AE RID: 686
			public uint Ecx;

			// Token: 0x040002AF RID: 687
			public uint Eax;

			// Token: 0x040002B0 RID: 688
			public uint Ebp;

			// Token: 0x040002B1 RID: 689
			public uint Eip;

			// Token: 0x040002B2 RID: 690
			public uint SegCs;

			// Token: 0x040002B3 RID: 691
			public uint EFlags;

			// Token: 0x040002B4 RID: 692
			public uint Esp;

			// Token: 0x040002B5 RID: 693
			public uint SegSs;

			// Token: 0x040002B6 RID: 694
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
			public byte[] ExtendedRegisters;
		}

		// Token: 0x02000070 RID: 112
		[StructLayout(LayoutKind.Sequential, Pack = 16)]
		public struct XSAVE_FORMAT64
		{
			// Token: 0x040002B7 RID: 695
			public ushort ControlWord;

			// Token: 0x040002B8 RID: 696
			public ushort StatusWord;

			// Token: 0x040002B9 RID: 697
			public byte TagWord;

			// Token: 0x040002BA RID: 698
			public byte Reserved1;

			// Token: 0x040002BB RID: 699
			public ushort ErrorOpcode;

			// Token: 0x040002BC RID: 700
			public uint ErrorOffset;

			// Token: 0x040002BD RID: 701
			public ushort ErrorSelector;

			// Token: 0x040002BE RID: 702
			public ushort Reserved2;

			// Token: 0x040002BF RID: 703
			public uint DataOffset;

			// Token: 0x040002C0 RID: 704
			public ushort DataSelector;

			// Token: 0x040002C1 RID: 705
			public ushort Reserved3;

			// Token: 0x040002C2 RID: 706
			public uint MxCsr;

			// Token: 0x040002C3 RID: 707
			public uint MxCsr_Mask;

			// Token: 0x040002C4 RID: 708
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public Registers.M128A[] FloatRegisters;

			// Token: 0x040002C5 RID: 709
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public Registers.M128A[] XmmRegisters;

			// Token: 0x040002C6 RID: 710
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
			public byte[] Reserved4;
		}

		// Token: 0x02000071 RID: 113
		[StructLayout(LayoutKind.Sequential, Pack = 16)]
		public struct CONTEXT64
		{
			// Token: 0x040002C7 RID: 711
			public ulong P1Home;

			// Token: 0x040002C8 RID: 712
			public ulong P2Home;

			// Token: 0x040002C9 RID: 713
			public ulong P3Home;

			// Token: 0x040002CA RID: 714
			public ulong P4Home;

			// Token: 0x040002CB RID: 715
			public ulong P5Home;

			// Token: 0x040002CC RID: 716
			public ulong P6Home;

			// Token: 0x040002CD RID: 717
			public Registers.CONTEXT_FLAGS ContextFlags;

			// Token: 0x040002CE RID: 718
			public uint MxCsr;

			// Token: 0x040002CF RID: 719
			public ushort SegCs;

			// Token: 0x040002D0 RID: 720
			public ushort SegDs;

			// Token: 0x040002D1 RID: 721
			public ushort SegEs;

			// Token: 0x040002D2 RID: 722
			public ushort SegFs;

			// Token: 0x040002D3 RID: 723
			public ushort SegGs;

			// Token: 0x040002D4 RID: 724
			public ushort SegSs;

			// Token: 0x040002D5 RID: 725
			public uint EFlags;

			// Token: 0x040002D6 RID: 726
			public ulong Dr0;

			// Token: 0x040002D7 RID: 727
			public ulong Dr1;

			// Token: 0x040002D8 RID: 728
			public ulong Dr2;

			// Token: 0x040002D9 RID: 729
			public ulong Dr3;

			// Token: 0x040002DA RID: 730
			public ulong Dr6;

			// Token: 0x040002DB RID: 731
			public ulong Dr7;

			// Token: 0x040002DC RID: 732
			public ulong Rax;

			// Token: 0x040002DD RID: 733
			public ulong Rcx;

			// Token: 0x040002DE RID: 734
			public ulong Rdx;

			// Token: 0x040002DF RID: 735
			public ulong Rbx;

			// Token: 0x040002E0 RID: 736
			public ulong Rsp;

			// Token: 0x040002E1 RID: 737
			public ulong Rbp;

			// Token: 0x040002E2 RID: 738
			public ulong Rsi;

			// Token: 0x040002E3 RID: 739
			public ulong Rdi;

			// Token: 0x040002E4 RID: 740
			public ulong R8;

			// Token: 0x040002E5 RID: 741
			public ulong R9;

			// Token: 0x040002E6 RID: 742
			public ulong R10;

			// Token: 0x040002E7 RID: 743
			public ulong R11;

			// Token: 0x040002E8 RID: 744
			public ulong R12;

			// Token: 0x040002E9 RID: 745
			public ulong R13;

			// Token: 0x040002EA RID: 746
			public ulong R14;

			// Token: 0x040002EB RID: 747
			public ulong R15;

			// Token: 0x040002EC RID: 748
			public ulong Rip;

			// Token: 0x040002ED RID: 749
			public Registers.XSAVE_FORMAT64 DUMMYUNIONNAME;

			// Token: 0x040002EE RID: 750
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
			public Registers.M128A[] VectorRegister;

			// Token: 0x040002EF RID: 751
			public ulong VectorControl;

			// Token: 0x040002F0 RID: 752
			public ulong DebugControl;

			// Token: 0x040002F1 RID: 753
			public ulong LastBranchToRip;

			// Token: 0x040002F2 RID: 754
			public ulong LastBranchFromRip;

			// Token: 0x040002F3 RID: 755
			public ulong LastExceptionToRip;

			// Token: 0x040002F4 RID: 756
			public ulong LastExceptionFromRip;
		}

		// Token: 0x02000072 RID: 114
		public enum CONTEXT_FLAGS : uint
		{
			// Token: 0x040002F6 RID: 758
			CONTEXT_i386 = 65536U,
			// Token: 0x040002F7 RID: 759
			CONTEXT_i486 = 65536U,
			// Token: 0x040002F8 RID: 760
			CONTEXT_CONTROL,
			// Token: 0x040002F9 RID: 761
			CONTEXT_INTEGER,
			// Token: 0x040002FA RID: 762
			CONTEXT_SEGMENTS = 65540U,
			// Token: 0x040002FB RID: 763
			CONTEXT_FLOATING_POINT = 65544U,
			// Token: 0x040002FC RID: 764
			CONTEXT_DEBUG_REGISTERS = 65552U,
			// Token: 0x040002FD RID: 765
			CONTEXT_EXTENDED_REGISTERS = 65568U,
			// Token: 0x040002FE RID: 766
			CONTEXT_FULL = 65543U,
			// Token: 0x040002FF RID: 767
			CONTEXT_ALL = 65599U
		}

		// Token: 0x02000073 RID: 115
		public struct M128A
		{
			// Token: 0x06000129 RID: 297 RVA: 0x000058D1 File Offset: 0x00003AD1
			public override string ToString()
			{
				return string.Format("High:{0}, Low:{1}", this.High, this.Low);
			}

			// Token: 0x04000300 RID: 768
			public ulong High;

			// Token: 0x04000301 RID: 769
			public long Low;
		}
	}
}
