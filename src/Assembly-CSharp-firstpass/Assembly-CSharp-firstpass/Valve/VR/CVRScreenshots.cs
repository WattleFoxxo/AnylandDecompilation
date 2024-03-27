using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000123 RID: 291
	public class CVRScreenshots
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x0000436A File Offset: 0x0000256A
		internal CVRScreenshots(IntPtr pInterface)
		{
			this.FnTable = (IVRScreenshots)Marshal.PtrToStructure(pInterface, typeof(IVRScreenshots));
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00004390 File Offset: 0x00002590
		public EVRScreenshotError RequestScreenshot(ref uint pOutScreenshotHandle, EVRScreenshotType type, string pchPreviewFilename, string pchVRFilename)
		{
			pOutScreenshotHandle = 0U;
			return this.FnTable.RequestScreenshot(ref pOutScreenshotHandle, type, pchPreviewFilename, pchVRFilename);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000043B8 File Offset: 0x000025B8
		public EVRScreenshotError HookScreenshot(EVRScreenshotType[] pSupportedTypes)
		{
			return this.FnTable.HookScreenshot(pSupportedTypes, pSupportedTypes.Length);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000043DC File Offset: 0x000025DC
		public EVRScreenshotType GetScreenshotPropertyType(uint screenshotHandle, ref EVRScreenshotError pError)
		{
			return this.FnTable.GetScreenshotPropertyType(screenshotHandle, ref pError);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00004400 File Offset: 0x00002600
		public uint GetScreenshotPropertyFilename(uint screenshotHandle, EVRScreenshotPropertyFilenames filenameType, StringBuilder pchFilename, uint cchFilename, ref EVRScreenshotError pError)
		{
			return this.FnTable.GetScreenshotPropertyFilename(screenshotHandle, filenameType, pchFilename, cchFilename, ref pError);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00004428 File Offset: 0x00002628
		public EVRScreenshotError UpdateScreenshotProgress(uint screenshotHandle, float flProgress)
		{
			return this.FnTable.UpdateScreenshotProgress(screenshotHandle, flProgress);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000444C File Offset: 0x0000264C
		public EVRScreenshotError TakeStereoScreenshot(ref uint pOutScreenshotHandle, string pchPreviewFilename, string pchVRFilename)
		{
			pOutScreenshotHandle = 0U;
			return this.FnTable.TakeStereoScreenshot(ref pOutScreenshotHandle, pchPreviewFilename, pchVRFilename);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00004474 File Offset: 0x00002674
		public EVRScreenshotError SubmitScreenshot(uint screenshotHandle, EVRScreenshotType type, string pchSourcePreviewFilename, string pchSourceVRFilename)
		{
			return this.FnTable.SubmitScreenshot(screenshotHandle, type, pchSourcePreviewFilename, pchSourceVRFilename);
		}

		// Token: 0x04000115 RID: 277
		private IVRScreenshots FnTable;
	}
}
