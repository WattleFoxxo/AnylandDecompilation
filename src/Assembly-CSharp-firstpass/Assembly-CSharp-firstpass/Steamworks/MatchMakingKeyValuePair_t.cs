using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002BA RID: 698
	public struct MatchMakingKeyValuePair_t
	{
		// Token: 0x06000C3B RID: 3131 RVA: 0x0000BB8D File Offset: 0x00009D8D
		private MatchMakingKeyValuePair_t(string strKey, string strValue)
		{
			this.m_szKey = strKey;
			this.m_szValue = strValue;
		}

		// Token: 0x04000C84 RID: 3204
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szKey;

		// Token: 0x04000C85 RID: 3205
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szValue;
	}
}
