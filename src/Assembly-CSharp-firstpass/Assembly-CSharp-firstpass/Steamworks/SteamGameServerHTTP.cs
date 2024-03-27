using System;

namespace Steamworks
{
	// Token: 0x0200019D RID: 413
	public static class SteamGameServerHTTP
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x00006F0C File Offset: 0x0000510C
		public static HTTPRequestHandle CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, string pchAbsoluteURL)
		{
			InteropHelp.TestIfAvailableGameServer();
			HTTPRequestHandle httprequestHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchAbsoluteURL))
			{
				httprequestHandle = (HTTPRequestHandle)NativeMethods.ISteamGameServerHTTP_CreateHTTPRequest(eHTTPRequestMethod, utf8StringHandle);
			}
			return httprequestHandle;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00006F58 File Offset: 0x00005158
		public static bool SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestContextValue(hRequest, ulContextValue);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00006F66 File Offset: 0x00005166
		public static bool SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestNetworkActivityTimeout(hRequest, unTimeoutSeconds);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00006F74 File Offset: 0x00005174
		public static bool SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, string pchHeaderValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchHeaderValue))
				{
					flag = NativeMethods.ISteamGameServerHTTP_SetHTTPRequestHeaderValue(hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00006FDC File Offset: 0x000051DC
		public static bool SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, string pchParamName, string pchParamValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchParamName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchParamValue))
				{
					flag = NativeMethods.ISteamGameServerHTTP_SetHTTPRequestGetOrPostParameter(hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00007044 File Offset: 0x00005244
		public static bool SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_SendHTTPRequest(hRequest, out pCallHandle);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00007052 File Offset: 0x00005252
		public static bool SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_SendHTTPRequestAndStreamResponse(hRequest, out pCallHandle);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00007060 File Offset: 0x00005260
		public static bool DeferHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_DeferHTTPRequest(hRequest);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0000706D File Offset: 0x0000526D
		public static bool PrioritizeHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_PrioritizeHTTPRequest(hRequest);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0000707C File Offset: 0x0000527C
		public static bool GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, string pchHeaderName, out uint unResponseHeaderSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamGameServerHTTP_GetHTTPResponseHeaderSize(hRequest, utf8StringHandle, out unResponseHeaderSize);
			}
			return flag;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000070C4 File Offset: 0x000052C4
		public static bool GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamGameServerHTTP_GetHTTPResponseHeaderValue(hRequest, utf8StringHandle, pHeaderValueBuffer, unBufferSize);
			}
			return flag;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0000710C File Offset: 0x0000530C
		public static bool GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_GetHTTPResponseBodySize(hRequest, out unBodySize);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0000711A File Offset: 0x0000531A
		public static bool GetHTTPResponseBodyData(HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_GetHTTPResponseBodyData(hRequest, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00007129 File Offset: 0x00005329
		public static bool GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_GetHTTPStreamingResponseBodyData(hRequest, cOffset, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00007139 File Offset: 0x00005339
		public static bool ReleaseHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_ReleaseHTTPRequest(hRequest);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00007146 File Offset: 0x00005346
		public static bool GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_GetHTTPDownloadProgressPct(hRequest, out pflPercentOut);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00007154 File Offset: 0x00005354
		public static bool SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, string pchContentType, byte[] pubBody, uint unBodyLen)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchContentType))
			{
				flag = NativeMethods.ISteamGameServerHTTP_SetHTTPRequestRawPostBody(hRequest, utf8StringHandle, pubBody, unBodyLen);
			}
			return flag;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0000719C File Offset: 0x0000539C
		public static HTTPCookieContainerHandle CreateCookieContainer(bool bAllowResponsesToModify)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HTTPCookieContainerHandle)NativeMethods.ISteamGameServerHTTP_CreateCookieContainer(bAllowResponsesToModify);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000071AE File Offset: 0x000053AE
		public static bool ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_ReleaseCookieContainer(hCookieContainer);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000071BC File Offset: 0x000053BC
		public static bool SetCookie(HTTPCookieContainerHandle hCookieContainer, string pchHost, string pchUrl, string pchCookie)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHost))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUrl))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchCookie))
					{
						flag = NativeMethods.ISteamGameServerHTTP_SetCookie(hCookieContainer, utf8StringHandle, utf8StringHandle2, utf8StringHandle3);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00007244 File Offset: 0x00005444
		public static bool SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestCookieContainer(hRequest, hCookieContainer);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00007254 File Offset: 0x00005454
		public static bool SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, string pchUserAgentInfo)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgentInfo))
			{
				flag = NativeMethods.ISteamGameServerHTTP_SetHTTPRequestUserAgentInfo(hRequest, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00007298 File Offset: 0x00005498
		public static bool SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, bool bRequireVerifiedCertificate)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestRequiresVerifiedCertificate(hRequest, bRequireVerifiedCertificate);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000072A6 File Offset: 0x000054A6
		public static bool SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestAbsoluteTimeoutMS(hRequest, unMilliseconds);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000072B4 File Offset: 0x000054B4
		public static bool GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerHTTP_GetHTTPRequestWasTimedOut(hRequest, out pbWasTimedOut);
		}
	}
}
