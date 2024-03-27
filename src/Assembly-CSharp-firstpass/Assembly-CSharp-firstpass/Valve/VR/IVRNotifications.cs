using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x020000FD RID: 253
	public struct IVRNotifications
	{
		// Token: 0x040000F3 RID: 243
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRNotifications._CreateNotification CreateNotification;

		// Token: 0x040000F4 RID: 244
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRNotifications._RemoveNotification RemoveNotification;

		// Token: 0x020000FE RID: 254
		// (Invoke) Token: 0x060003CA RID: 970
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRNotificationError _CreateNotification(ulong ulOverlayHandle, ulong ulUserValue, EVRNotificationType type, string pchText, EVRNotificationStyle style, ref NotificationBitmap_t pImage, ref uint pNotificationId);

		// Token: 0x020000FF RID: 255
		// (Invoke) Token: 0x060003CE RID: 974
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRNotificationError _RemoveNotification(uint notificationId);
	}
}
