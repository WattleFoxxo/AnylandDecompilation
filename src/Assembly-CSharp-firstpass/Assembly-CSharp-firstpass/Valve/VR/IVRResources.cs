using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000115 RID: 277
	public struct IVRResources
	{
		// Token: 0x04000108 RID: 264
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRResources._LoadSharedResource LoadSharedResource;

		// Token: 0x04000109 RID: 265
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRResources._GetResourceFullPath GetResourceFullPath;

		// Token: 0x02000116 RID: 278
		// (Invoke) Token: 0x0600041E RID: 1054
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _LoadSharedResource(string pchResourceName, string pchBuffer, uint unBufferLen);

		// Token: 0x02000117 RID: 279
		// (Invoke) Token: 0x06000422 RID: 1058
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetResourceFullPath(string pchResourceName, string pchResourceTypeDirectory, string pchPathBuffer, uint unBufferLen);
	}
}
