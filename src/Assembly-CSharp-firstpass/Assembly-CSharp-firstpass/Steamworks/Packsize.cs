using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002EA RID: 746
	public static class Packsize
	{
		// Token: 0x06000CF7 RID: 3319 RVA: 0x0000CD48 File Offset: 0x0000AF48
		public static bool Test()
		{
			int num = Marshal.SizeOf(typeof(Packsize.ValvePackingSentinel_t));
			int num2 = Marshal.SizeOf(typeof(RemoteStorageEnumerateUserSubscribedFilesResult_t));
			return num == 32 && num2 == 616;
		}

		// Token: 0x04000CC7 RID: 3271
		public const int value = 8;

		// Token: 0x020002EB RID: 747
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		private struct ValvePackingSentinel_t
		{
			// Token: 0x04000CC8 RID: 3272
			private uint m_u32;

			// Token: 0x04000CC9 RID: 3273
			private ulong m_u64;

			// Token: 0x04000CCA RID: 3274
			private ushort m_u16;

			// Token: 0x04000CCB RID: 3275
			private double m_d;
		}
	}
}
