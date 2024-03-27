using System;

namespace Steamworks
{
	// Token: 0x020001A4 RID: 420
	public static class SteamHTTP
	{
		// Token: 0x0600074D RID: 1869 RVA: 0x00008888 File Offset: 0x00006A88
		public static HTTPRequestHandle CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, string pchAbsoluteURL)
		{
			InteropHelp.TestIfAvailableClient();
			HTTPRequestHandle httprequestHandle;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchAbsoluteURL))
			{
				httprequestHandle = (HTTPRequestHandle)NativeMethods.ISteamHTTP_CreateHTTPRequest(eHTTPRequestMethod, utf8StringHandle);
			}
			return httprequestHandle;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000088D4 File Offset: 0x00006AD4
		public static bool SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestContextValue(hRequest, ulContextValue);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000088E2 File Offset: 0x00006AE2
		public static bool SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(hRequest, unTimeoutSeconds);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000088F0 File Offset: 0x00006AF0
		public static bool SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, string pchHeaderValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchHeaderValue))
				{
					flag = NativeMethods.ISteamHTTP_SetHTTPRequestHeaderValue(hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00008958 File Offset: 0x00006B58
		public static bool SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, string pchParamName, string pchParamValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchParamName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchParamValue))
				{
					flag = NativeMethods.ISteamHTTP_SetHTTPRequestGetOrPostParameter(hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return flag;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000089C0 File Offset: 0x00006BC0
		public static bool SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SendHTTPRequest(hRequest, out pCallHandle);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000089CE File Offset: 0x00006BCE
		public static bool SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SendHTTPRequestAndStreamResponse(hRequest, out pCallHandle);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000089DC File Offset: 0x00006BDC
		public static bool DeferHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_DeferHTTPRequest(hRequest);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000089E9 File Offset: 0x00006BE9
		public static bool PrioritizeHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_PrioritizeHTTPRequest(hRequest);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000089F8 File Offset: 0x00006BF8
		public static bool GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, string pchHeaderName, out uint unResponseHeaderSize)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamHTTP_GetHTTPResponseHeaderSize(hRequest, utf8StringHandle, out unResponseHeaderSize);
			}
			return flag;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00008A40 File Offset: 0x00006C40
		public static bool GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, string pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				flag = NativeMethods.ISteamHTTP_GetHTTPResponseHeaderValue(hRequest, utf8StringHandle, pHeaderValueBuffer, unBufferSize);
			}
			return flag;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00008A88 File Offset: 0x00006C88
		public static bool GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPResponseBodySize(hRequest, out unBodySize);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00008A96 File Offset: 0x00006C96
		public static bool GetHTTPResponseBodyData(HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPResponseBodyData(hRequest, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00008AA5 File Offset: 0x00006CA5
		public static bool GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPStreamingResponseBodyData(hRequest, cOffset, pBodyDataBuffer, unBufferSize);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00008AB5 File Offset: 0x00006CB5
		public static bool ReleaseHTTPRequest(HTTPRequestHandle hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_ReleaseHTTPRequest(hRequest);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00008AC2 File Offset: 0x00006CC2
		public static bool GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPDownloadProgressPct(hRequest, out pflPercentOut);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public static bool SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, string pchContentType, byte[] pubBody, uint unBodyLen)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchContentType))
			{
				flag = NativeMethods.ISteamHTTP_SetHTTPRequestRawPostBody(hRequest, utf8StringHandle, pubBody, unBodyLen);
			}
			return flag;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00008B18 File Offset: 0x00006D18
		public static HTTPCookieContainerHandle CreateCookieContainer(bool bAllowResponsesToModify)
		{
			InteropHelp.TestIfAvailableClient();
			return (HTTPCookieContainerHandle)NativeMethods.ISteamHTTP_CreateCookieContainer(bAllowResponsesToModify);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00008B2A File Offset: 0x00006D2A
		public static bool ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_ReleaseCookieContainer(hCookieContainer);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00008B38 File Offset: 0x00006D38
		public static bool SetCookie(HTTPCookieContainerHandle hCookieContainer, string pchHost, string pchUrl, string pchCookie)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHost))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUrl))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchCookie))
					{
						flag = NativeMethods.ISteamHTTP_SetCookie(hCookieContainer, utf8StringHandle, utf8StringHandle2, utf8StringHandle3);
					}
				}
			}
			return flag;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00008BC0 File Offset: 0x00006DC0
		public static bool SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestCookieContainer(hRequest, hCookieContainer);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00008BD0 File Offset: 0x00006DD0
		public static bool SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, string pchUserAgentInfo)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgentInfo))
			{
				flag = NativeMethods.ISteamHTTP_SetHTTPRequestUserAgentInfo(hRequest, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00008C14 File Offset: 0x00006E14
		public static bool SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, bool bRequireVerifiedCertificate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(hRequest, bRequireVerifiedCertificate);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00008C22 File Offset: 0x00006E22
		public static bool SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(hRequest, unMilliseconds);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00008C30 File Offset: 0x00006E30
		public static bool GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTTP_GetHTTPRequestWasTimedOut(hRequest, out pbWasTimedOut);
		}
	}
}
