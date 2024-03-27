using System;

namespace Steamworks
{
	// Token: 0x020001AE RID: 430
	public static class SteamUnifiedMessages
	{
		// Token: 0x06000877 RID: 2167 RVA: 0x0000AE04 File Offset: 0x00009004
		public static ClientUnifiedMessageHandle SendMethod(string pchServiceMethod, byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext)
		{
			InteropHelp.TestIfAvailableClient();
			ClientUnifiedMessageHandle clientUnifiedMessageHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchServiceMethod))
			{
				clientUnifiedMessageHandle = (ClientUnifiedMessageHandle)NativeMethods.ISteamUnifiedMessages_SendMethod(utf8StringHandle, pRequestBuffer, unRequestBufferSize, unContext);
			}
			return clientUnifiedMessageHandle;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0000AE50 File Offset: 0x00009050
		public static bool GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0000AE5F File Offset: 0x0000905F
		public static bool GetMethodResponseData(ClientUnifiedMessageHandle hHandle, byte[] pResponseBuffer, uint unResponseBufferSize, bool bAutoRelease)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0000AE6F File Offset: 0x0000906F
		public static bool ReleaseMethod(ClientUnifiedMessageHandle hHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0000AE7C File Offset: 0x0000907C
		public static bool SendNotification(string pchServiceNotification, byte[] pNotificationBuffer, uint unNotificationBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchServiceNotification))
			{
				flag = NativeMethods.ISteamUnifiedMessages_SendNotification(utf8StringHandle, pNotificationBuffer, unNotificationBufferSize);
			}
			return flag;
		}
	}
}
