using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002F4 RID: 756
	// (Invoke) Token: 0x06000D44 RID: 3396
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}
