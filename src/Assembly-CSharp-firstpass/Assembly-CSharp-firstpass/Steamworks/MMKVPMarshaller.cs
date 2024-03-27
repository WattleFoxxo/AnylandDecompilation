using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002CA RID: 714
	public class MMKVPMarshaller
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x0000C688 File Offset: 0x0000A888
		public MMKVPMarshaller(MatchMakingKeyValuePair_t[] filters)
		{
			if (filters == null)
			{
				return;
			}
			int num = Marshal.SizeOf(typeof(MatchMakingKeyValuePair_t));
			this.m_pNativeArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * filters.Length);
			this.m_pArrayEntries = Marshal.AllocHGlobal(num * filters.Length);
			for (int i = 0; i < filters.Length; i++)
			{
				Marshal.StructureToPtr(filters[i], new IntPtr(this.m_pArrayEntries.ToInt64() + (long)(i * num)), false);
			}
			Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0000C734 File Offset: 0x0000A934
		~MMKVPMarshaller()
		{
			if (this.m_pArrayEntries != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pArrayEntries);
			}
			if (this.m_pNativeArray != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pNativeArray);
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0000C7A0 File Offset: 0x0000A9A0
		public static implicit operator IntPtr(MMKVPMarshaller that)
		{
			return that.m_pNativeArray;
		}

		// Token: 0x04000CA3 RID: 3235
		private IntPtr m_pNativeArray;

		// Token: 0x04000CA4 RID: 3236
		private IntPtr m_pArrayEntries;
	}
}
