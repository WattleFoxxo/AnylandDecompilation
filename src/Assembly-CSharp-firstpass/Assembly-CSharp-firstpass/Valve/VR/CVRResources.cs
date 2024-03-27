using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000124 RID: 292
	public class CVRResources
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x00004498 File Offset: 0x00002698
		internal CVRResources(IntPtr pInterface)
		{
			this.FnTable = (IVRResources)Marshal.PtrToStructure(pInterface, typeof(IVRResources));
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000044BC File Offset: 0x000026BC
		public uint LoadSharedResource(string pchResourceName, string pchBuffer, uint unBufferLen)
		{
			return this.FnTable.LoadSharedResource(pchResourceName, pchBuffer, unBufferLen);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x000044E0 File Offset: 0x000026E0
		public uint GetResourceFullPath(string pchResourceName, string pchResourceTypeDirectory, string pchPathBuffer, uint unBufferLen)
		{
			return this.FnTable.GetResourceFullPath(pchResourceName, pchResourceTypeDirectory, pchPathBuffer, unBufferLen);
		}

		// Token: 0x04000116 RID: 278
		private IVRResources FnTable;
	}
}
