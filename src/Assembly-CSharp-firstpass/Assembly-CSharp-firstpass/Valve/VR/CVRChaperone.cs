using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200011C RID: 284
	public class CVRChaperone
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x00002CEA File Offset: 0x00000EEA
		internal CVRChaperone(IntPtr pInterface)
		{
			this.FnTable = (IVRChaperone)Marshal.PtrToStructure(pInterface, typeof(IVRChaperone));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00002D10 File Offset: 0x00000F10
		public ChaperoneCalibrationState GetCalibrationState()
		{
			return this.FnTable.GetCalibrationState();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00002D30 File Offset: 0x00000F30
		public bool GetPlayAreaSize(ref float pSizeX, ref float pSizeZ)
		{
			pSizeX = 0f;
			pSizeZ = 0f;
			return this.FnTable.GetPlayAreaSize(ref pSizeX, ref pSizeZ);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00002D60 File Offset: 0x00000F60
		public bool GetPlayAreaRect(ref HmdQuad_t rect)
		{
			return this.FnTable.GetPlayAreaRect(ref rect);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00002D80 File Offset: 0x00000F80
		public void ReloadInfo()
		{
			this.FnTable.ReloadInfo();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00002D92 File Offset: 0x00000F92
		public void SetSceneColor(HmdColor_t color)
		{
			this.FnTable.SetSceneColor(color);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00002DA5 File Offset: 0x00000FA5
		public void GetBoundsColor(ref HmdColor_t pOutputColorArray, int nNumOutputColors, float flCollisionBoundsFadeDistance, ref HmdColor_t pOutputCameraColor)
		{
			this.FnTable.GetBoundsColor(ref pOutputColorArray, nNumOutputColors, flCollisionBoundsFadeDistance, ref pOutputCameraColor);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00002DBC File Offset: 0x00000FBC
		public bool AreBoundsVisible()
		{
			return this.FnTable.AreBoundsVisible();
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00002DDB File Offset: 0x00000FDB
		public void ForceBoundsVisible(bool bForce)
		{
			this.FnTable.ForceBoundsVisible(bForce);
		}

		// Token: 0x0400010E RID: 270
		private IVRChaperone FnTable;
	}
}
