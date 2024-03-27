using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000122 RID: 290
	public class CVRSettings
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x000041EC File Offset: 0x000023EC
		internal CVRSettings(IntPtr pInterface)
		{
			this.FnTable = (IVRSettings)Marshal.PtrToStructure(pInterface, typeof(IVRSettings));
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00004210 File Offset: 0x00002410
		public string GetSettingsErrorNameFromEnum(EVRSettingsError eError)
		{
			IntPtr intPtr = this.FnTable.GetSettingsErrorNameFromEnum(eError);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00004238 File Offset: 0x00002438
		public bool Sync(bool bForce, ref EVRSettingsError peError)
		{
			return this.FnTable.Sync(bForce, ref peError);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000425C File Offset: 0x0000245C
		public bool GetBool(string pchSection, string pchSettingsKey, bool bDefaultValue, ref EVRSettingsError peError)
		{
			return this.FnTable.GetBool(pchSection, pchSettingsKey, bDefaultValue, ref peError);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00004280 File Offset: 0x00002480
		public void SetBool(string pchSection, string pchSettingsKey, bool bValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetBool(pchSection, pchSettingsKey, bValue, ref peError);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00004298 File Offset: 0x00002498
		public int GetInt32(string pchSection, string pchSettingsKey, int nDefaultValue, ref EVRSettingsError peError)
		{
			return this.FnTable.GetInt32(pchSection, pchSettingsKey, nDefaultValue, ref peError);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000042BC File Offset: 0x000024BC
		public void SetInt32(string pchSection, string pchSettingsKey, int nValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetInt32(pchSection, pchSettingsKey, nValue, ref peError);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000042D4 File Offset: 0x000024D4
		public float GetFloat(string pchSection, string pchSettingsKey, float flDefaultValue, ref EVRSettingsError peError)
		{
			return this.FnTable.GetFloat(pchSection, pchSettingsKey, flDefaultValue, ref peError);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000042F8 File Offset: 0x000024F8
		public void SetFloat(string pchSection, string pchSettingsKey, float flValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetFloat(pchSection, pchSettingsKey, flValue, ref peError);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000430F File Offset: 0x0000250F
		public void GetString(string pchSection, string pchSettingsKey, StringBuilder pchValue, uint unValueLen, string pchDefaultValue, ref EVRSettingsError peError)
		{
			this.FnTable.GetString(pchSection, pchSettingsKey, pchValue, unValueLen, pchDefaultValue, ref peError);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000432A File Offset: 0x0000252A
		public void SetString(string pchSection, string pchSettingsKey, string pchValue, ref EVRSettingsError peError)
		{
			this.FnTable.SetString(pchSection, pchSettingsKey, pchValue, ref peError);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00004341 File Offset: 0x00002541
		public void RemoveSection(string pchSection, ref EVRSettingsError peError)
		{
			this.FnTable.RemoveSection(pchSection, ref peError);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00004355 File Offset: 0x00002555
		public void RemoveKeyInSection(string pchSection, string pchSettingsKey, ref EVRSettingsError peError)
		{
			this.FnTable.RemoveKeyInSection(pchSection, pchSettingsKey, ref peError);
		}

		// Token: 0x04000114 RID: 276
		private IVRSettings FnTable;
	}
}
