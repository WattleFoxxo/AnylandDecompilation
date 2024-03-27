using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200010D RID: 269
	public struct IVRScreenshots
	{
		// Token: 0x04000101 RID: 257
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._RequestScreenshot RequestScreenshot;

		// Token: 0x04000102 RID: 258
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._HookScreenshot HookScreenshot;

		// Token: 0x04000103 RID: 259
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._GetScreenshotPropertyType GetScreenshotPropertyType;

		// Token: 0x04000104 RID: 260
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._GetScreenshotPropertyFilename GetScreenshotPropertyFilename;

		// Token: 0x04000105 RID: 261
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._UpdateScreenshotProgress UpdateScreenshotProgress;

		// Token: 0x04000106 RID: 262
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._TakeStereoScreenshot TakeStereoScreenshot;

		// Token: 0x04000107 RID: 263
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRScreenshots._SubmitScreenshot SubmitScreenshot;

		// Token: 0x0200010E RID: 270
		// (Invoke) Token: 0x06000402 RID: 1026
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _RequestScreenshot(ref uint pOutScreenshotHandle, EVRScreenshotType type, string pchPreviewFilename, string pchVRFilename);

		// Token: 0x0200010F RID: 271
		// (Invoke) Token: 0x06000406 RID: 1030
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _HookScreenshot([In] [Out] EVRScreenshotType[] pSupportedTypes, int numTypes);

		// Token: 0x02000110 RID: 272
		// (Invoke) Token: 0x0600040A RID: 1034
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotType _GetScreenshotPropertyType(uint screenshotHandle, ref EVRScreenshotError pError);

		// Token: 0x02000111 RID: 273
		// (Invoke) Token: 0x0600040E RID: 1038
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetScreenshotPropertyFilename(uint screenshotHandle, EVRScreenshotPropertyFilenames filenameType, StringBuilder pchFilename, uint cchFilename, ref EVRScreenshotError pError);

		// Token: 0x02000112 RID: 274
		// (Invoke) Token: 0x06000412 RID: 1042
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _UpdateScreenshotProgress(uint screenshotHandle, float flProgress);

		// Token: 0x02000113 RID: 275
		// (Invoke) Token: 0x06000416 RID: 1046
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _TakeStereoScreenshot(ref uint pOutScreenshotHandle, string pchPreviewFilename, string pchVRFilename);

		// Token: 0x02000114 RID: 276
		// (Invoke) Token: 0x0600041A RID: 1050
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRScreenshotError _SubmitScreenshot(uint screenshotHandle, EVRScreenshotType type, string pchSourcePreviewFilename, string pchSourceVRFilename);
	}
}
