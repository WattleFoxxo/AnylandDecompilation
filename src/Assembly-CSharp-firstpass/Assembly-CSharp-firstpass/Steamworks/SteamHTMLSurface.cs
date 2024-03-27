using System;

namespace Steamworks
{
	// Token: 0x020001A3 RID: 419
	public static class SteamHTMLSurface
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x0000846F File Offset: 0x0000666F
		public static bool Init()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Init();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0000847B File Offset: 0x0000667B
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Shutdown();
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00008488 File Offset: 0x00006688
		public static SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgent))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUserCSS))
				{
					steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamHTMLSurface_CreateBrowser(utf8StringHandle, utf8StringHandle2);
				}
			}
			return steamAPICall_t;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000084F4 File Offset: 0x000066F4
		public static void RemoveBrowser(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_RemoveBrowser(unBrowserHandle);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00008504 File Offset: 0x00006704
		public static void LoadURL(HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchURL))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchPostData))
				{
					NativeMethods.ISteamHTMLSurface_LoadURL(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000856C File Offset: 0x0000676C
		public static void SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetSize(unBrowserHandle, unWidth, unHeight);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000857B File Offset: 0x0000677B
		public static void StopLoad(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopLoad(unBrowserHandle);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00008588 File Offset: 0x00006788
		public static void Reload(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_Reload(unBrowserHandle);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00008595 File Offset: 0x00006795
		public static void GoBack(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoBack(unBrowserHandle);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x000085A2 File Offset: 0x000067A2
		public static void GoForward(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoForward(unBrowserHandle);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x000085B0 File Offset: 0x000067B0
		public static void AddHeader(HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					NativeMethods.ISteamHTMLSurface_AddHeader(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00008618 File Offset: 0x00006818
		public static void ExecuteJavascript(HHTMLBrowser unBrowserHandle, string pchScript)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchScript))
			{
				NativeMethods.ISteamHTMLSurface_ExecuteJavascript(unBrowserHandle, utf8StringHandle);
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0000865C File Offset: 0x0000685C
		public static void MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseUp(unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0000866A File Offset: 0x0000686A
		public static void MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDown(unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00008678 File Offset: 0x00006878
		public static void MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDoubleClick(unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00008686 File Offset: 0x00006886
		public static void MouseMove(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseMove(unBrowserHandle, x, y);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00008695 File Offset: 0x00006895
		public static void MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseWheel(unBrowserHandle, nDelta);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000086A3 File Offset: 0x000068A3
		public static void KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyDown(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000086B2 File Offset: 0x000068B2
		public static void KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyUp(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000086C1 File Offset: 0x000068C1
		public static void KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyChar(unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000086D0 File Offset: 0x000068D0
		public static void SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000086DE File Offset: 0x000068DE
		public static void SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetVerticalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000086EC File Offset: 0x000068EC
		public static void SetKeyFocus(HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetKeyFocus(unBrowserHandle, bHasKeyFocus);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000086FA File Offset: 0x000068FA
		public static void ViewSource(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_ViewSource(unBrowserHandle);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00008707 File Offset: 0x00006907
		public static void CopyToClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_CopyToClipboard(unBrowserHandle);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00008714 File Offset: 0x00006914
		public static void PasteFromClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_PasteFromClipboard(unBrowserHandle);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00008724 File Offset: 0x00006924
		public static void Find(HHTMLBrowser unBrowserHandle, string pchSearchStr, bool bCurrentlyInFind, bool bReverse)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchSearchStr))
			{
				NativeMethods.ISteamHTMLSurface_Find(unBrowserHandle, utf8StringHandle, bCurrentlyInFind, bReverse);
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00008768 File Offset: 0x00006968
		public static void StopFind(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopFind(unBrowserHandle);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00008775 File Offset: 0x00006975
		public static void GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(unBrowserHandle, x, y);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00008784 File Offset: 0x00006984
		public static void SetCookie(string pchHostname, string pchKey, string pchValue, string pchPath = "/", uint nExpires = 0U, bool bSecure = false, bool bHTTPOnly = false)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHostname))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchValue))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchPath))
						{
							NativeMethods.ISteamHTMLSurface_SetCookie(utf8StringHandle, utf8StringHandle2, utf8StringHandle3, utf8StringHandle4, nExpires, bSecure, bHTTPOnly);
						}
					}
				}
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00008840 File Offset: 0x00006A40
		public static void SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(unBrowserHandle, flZoom, nPointX, nPointY);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00008850 File Offset: 0x00006A50
		public static void SetBackgroundMode(HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetBackgroundMode(unBrowserHandle, bBackgroundMode);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0000885E File Offset: 0x00006A5E
		public static void AllowStartRequest(HHTMLBrowser unBrowserHandle, bool bAllowed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_AllowStartRequest(unBrowserHandle, bAllowed);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0000886C File Offset: 0x00006A6C
		public static void JSDialogResponse(HHTMLBrowser unBrowserHandle, bool bResult)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_JSDialogResponse(unBrowserHandle, bResult);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0000887A File Offset: 0x00006A7A
		public static void FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(unBrowserHandle, pchSelectedFiles);
		}
	}
}
