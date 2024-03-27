using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000121 RID: 289
	public class CVRNotifications
	{
		// Token: 0x06000520 RID: 1312 RVA: 0x00004179 File Offset: 0x00002379
		internal CVRNotifications(IntPtr pInterface)
		{
			this.FnTable = (IVRNotifications)Marshal.PtrToStructure(pInterface, typeof(IVRNotifications));
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000419C File Offset: 0x0000239C
		public EVRNotificationError CreateNotification(ulong ulOverlayHandle, ulong ulUserValue, EVRNotificationType type, string pchText, EVRNotificationStyle style, ref NotificationBitmap_t pImage, ref uint pNotificationId)
		{
			pNotificationId = 0U;
			return this.FnTable.CreateNotification(ulOverlayHandle, ulUserValue, type, pchText, style, ref pImage, ref pNotificationId);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000041CC File Offset: 0x000023CC
		public EVRNotificationError RemoveNotification(uint notificationId)
		{
			return this.FnTable.RemoveNotification(notificationId);
		}

		// Token: 0x04000113 RID: 275
		private IVRNotifications FnTable;
	}
}
