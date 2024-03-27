using System;

namespace Steamworks
{
	// Token: 0x020002C6 RID: 710
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
	internal class CallbackIdentityAttribute : Attribute
	{
		// Token: 0x06000C72 RID: 3186 RVA: 0x0000C353 File Offset: 0x0000A553
		public CallbackIdentityAttribute(int callbackNum)
		{
			this.Identity = callbackNum;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0000C362 File Offset: 0x0000A562
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0000C36A File Offset: 0x0000A56A
		public int Identity { get; set; }
	}
}
