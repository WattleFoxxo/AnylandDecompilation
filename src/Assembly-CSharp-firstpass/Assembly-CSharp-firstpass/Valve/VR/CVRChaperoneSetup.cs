using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200011D RID: 285
	public class CVRChaperoneSetup
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x00002DEE File Offset: 0x00000FEE
		internal CVRChaperoneSetup(IntPtr pInterface)
		{
			this.FnTable = (IVRChaperoneSetup)Marshal.PtrToStructure(pInterface, typeof(IVRChaperoneSetup));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00002E14 File Offset: 0x00001014
		public bool CommitWorkingCopy(EChaperoneConfigFile configFile)
		{
			return this.FnTable.CommitWorkingCopy(configFile);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00002E34 File Offset: 0x00001034
		public void RevertWorkingCopy()
		{
			this.FnTable.RevertWorkingCopy();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00002E48 File Offset: 0x00001048
		public bool GetWorkingPlayAreaSize(ref float pSizeX, ref float pSizeZ)
		{
			pSizeX = 0f;
			pSizeZ = 0f;
			return this.FnTable.GetWorkingPlayAreaSize(ref pSizeX, ref pSizeZ);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00002E78 File Offset: 0x00001078
		public bool GetWorkingPlayAreaRect(ref HmdQuad_t rect)
		{
			return this.FnTable.GetWorkingPlayAreaRect(ref rect);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00002E98 File Offset: 0x00001098
		public bool GetWorkingCollisionBoundsInfo(out HmdQuad_t[] pQuadsBuffer)
		{
			uint num = 0U;
			bool flag = this.FnTable.GetWorkingCollisionBoundsInfo(null, ref num);
			pQuadsBuffer = new HmdQuad_t[num];
			return this.FnTable.GetWorkingCollisionBoundsInfo(pQuadsBuffer, ref num);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00002EDC File Offset: 0x000010DC
		public bool GetLiveCollisionBoundsInfo(out HmdQuad_t[] pQuadsBuffer)
		{
			uint num = 0U;
			bool flag = this.FnTable.GetLiveCollisionBoundsInfo(null, ref num);
			pQuadsBuffer = new HmdQuad_t[num];
			return this.FnTable.GetLiveCollisionBoundsInfo(pQuadsBuffer, ref num);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00002F20 File Offset: 0x00001120
		public bool GetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose)
		{
			return this.FnTable.GetWorkingSeatedZeroPoseToRawTrackingPose(ref pmatSeatedZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00002F40 File Offset: 0x00001140
		public bool GetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatStandingZeroPoseToRawTrackingPose)
		{
			return this.FnTable.GetWorkingStandingZeroPoseToRawTrackingPose(ref pmatStandingZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00002F60 File Offset: 0x00001160
		public void SetWorkingPlayAreaSize(float sizeX, float sizeZ)
		{
			this.FnTable.SetWorkingPlayAreaSize(sizeX, sizeZ);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00002F74 File Offset: 0x00001174
		public void SetWorkingCollisionBoundsInfo(HmdQuad_t[] pQuadsBuffer)
		{
			this.FnTable.SetWorkingCollisionBoundsInfo(pQuadsBuffer, (uint)pQuadsBuffer.Length);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00002F8A File Offset: 0x0000118A
		public void SetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatSeatedZeroPoseToRawTrackingPose)
		{
			this.FnTable.SetWorkingSeatedZeroPoseToRawTrackingPose(ref pMatSeatedZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00002F9D File Offset: 0x0000119D
		public void SetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatStandingZeroPoseToRawTrackingPose)
		{
			this.FnTable.SetWorkingStandingZeroPoseToRawTrackingPose(ref pMatStandingZeroPoseToRawTrackingPose);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00002FB0 File Offset: 0x000011B0
		public void ReloadFromDisk(EChaperoneConfigFile configFile)
		{
			this.FnTable.ReloadFromDisk(configFile);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00002FC4 File Offset: 0x000011C4
		public bool GetLiveSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose)
		{
			return this.FnTable.GetLiveSeatedZeroPoseToRawTrackingPose(ref pmatSeatedZeroPoseToRawTrackingPose);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00002FE4 File Offset: 0x000011E4
		public void SetWorkingCollisionBoundsTagsInfo(byte[] pTagsBuffer)
		{
			this.FnTable.SetWorkingCollisionBoundsTagsInfo(pTagsBuffer, (uint)pTagsBuffer.Length);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00002FFC File Offset: 0x000011FC
		public bool GetLiveCollisionBoundsTagsInfo(out byte[] pTagsBuffer)
		{
			uint num = 0U;
			bool flag = this.FnTable.GetLiveCollisionBoundsTagsInfo(null, ref num);
			pTagsBuffer = new byte[num];
			return this.FnTable.GetLiveCollisionBoundsTagsInfo(pTagsBuffer, ref num);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00003040 File Offset: 0x00001240
		public bool SetWorkingPhysicalBoundsInfo(HmdQuad_t[] pQuadsBuffer)
		{
			return this.FnTable.SetWorkingPhysicalBoundsInfo(pQuadsBuffer, (uint)pQuadsBuffer.Length);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00003064 File Offset: 0x00001264
		public bool GetLivePhysicalBoundsInfo(out HmdQuad_t[] pQuadsBuffer)
		{
			uint num = 0U;
			bool flag = this.FnTable.GetLivePhysicalBoundsInfo(null, ref num);
			pQuadsBuffer = new HmdQuad_t[num];
			return this.FnTable.GetLivePhysicalBoundsInfo(pQuadsBuffer, ref num);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000030A8 File Offset: 0x000012A8
		public bool ExportLiveToBuffer(StringBuilder pBuffer, ref uint pnBufferLength)
		{
			pnBufferLength = 0U;
			return this.FnTable.ExportLiveToBuffer(pBuffer, ref pnBufferLength);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000030CC File Offset: 0x000012CC
		public bool ImportFromBufferToWorking(string pBuffer, uint nImportFlags)
		{
			return this.FnTable.ImportFromBufferToWorking(pBuffer, nImportFlags);
		}

		// Token: 0x0400010F RID: 271
		private IVRChaperoneSetup FnTable;
	}
}
