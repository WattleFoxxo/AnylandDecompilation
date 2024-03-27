using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000119 RID: 281
	public class CVRExtendedDisplay
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00002620 File Offset: 0x00000820
		internal CVRExtendedDisplay(IntPtr pInterface)
		{
			this.FnTable = (IVRExtendedDisplay)Marshal.PtrToStructure(pInterface, typeof(IVRExtendedDisplay));
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00002643 File Offset: 0x00000843
		public void GetWindowBounds(ref int pnX, ref int pnY, ref uint pnWidth, ref uint pnHeight)
		{
			pnX = 0;
			pnY = 0;
			pnWidth = 0U;
			pnHeight = 0U;
			this.FnTable.GetWindowBounds(ref pnX, ref pnY, ref pnWidth, ref pnHeight);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00002667 File Offset: 0x00000867
		public void GetEyeOutputViewport(EVREye eEye, ref uint pnX, ref uint pnY, ref uint pnWidth, ref uint pnHeight)
		{
			pnX = 0U;
			pnY = 0U;
			pnWidth = 0U;
			pnHeight = 0U;
			this.FnTable.GetEyeOutputViewport(eEye, ref pnX, ref pnY, ref pnWidth, ref pnHeight);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000268E File Offset: 0x0000088E
		public void GetDXGIOutputInfo(ref int pnAdapterIndex, ref int pnAdapterOutputIndex)
		{
			pnAdapterIndex = 0;
			pnAdapterOutputIndex = 0;
			this.FnTable.GetDXGIOutputInfo(ref pnAdapterIndex, ref pnAdapterOutputIndex);
		}

		// Token: 0x0400010B RID: 267
		private IVRExtendedDisplay FnTable;
	}
}
