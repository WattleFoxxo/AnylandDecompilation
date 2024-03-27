using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x0200011B RID: 283
	public class CVRApplications
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x000028AD File Offset: 0x00000AAD
		internal CVRApplications(IntPtr pInterface)
		{
			this.FnTable = (IVRApplications)Marshal.PtrToStructure(pInterface, typeof(IVRApplications));
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000028D0 File Offset: 0x00000AD0
		public EVRApplicationError AddApplicationManifest(string pchApplicationManifestFullPath, bool bTemporary)
		{
			return this.FnTable.AddApplicationManifest(pchApplicationManifestFullPath, bTemporary);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000028F4 File Offset: 0x00000AF4
		public EVRApplicationError RemoveApplicationManifest(string pchApplicationManifestFullPath)
		{
			return this.FnTable.RemoveApplicationManifest(pchApplicationManifestFullPath);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00002914 File Offset: 0x00000B14
		public bool IsApplicationInstalled(string pchAppKey)
		{
			return this.FnTable.IsApplicationInstalled(pchAppKey);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00002934 File Offset: 0x00000B34
		public uint GetApplicationCount()
		{
			return this.FnTable.GetApplicationCount();
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00002954 File Offset: 0x00000B54
		public EVRApplicationError GetApplicationKeyByIndex(uint unApplicationIndex, string pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetApplicationKeyByIndex(unApplicationIndex, pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00002978 File Offset: 0x00000B78
		public EVRApplicationError GetApplicationKeyByProcessId(uint unProcessId, string pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetApplicationKeyByProcessId(unProcessId, pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000299C File Offset: 0x00000B9C
		public EVRApplicationError LaunchApplication(string pchAppKey)
		{
			return this.FnTable.LaunchApplication(pchAppKey);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000029BC File Offset: 0x00000BBC
		public EVRApplicationError LaunchTemplateApplication(string pchTemplateAppKey, string pchNewAppKey, AppOverrideKeys_t[] pKeys)
		{
			return this.FnTable.LaunchTemplateApplication(pchTemplateAppKey, pchNewAppKey, pKeys, (uint)pKeys.Length);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000029E4 File Offset: 0x00000BE4
		public EVRApplicationError LaunchApplicationFromMimeType(string pchMimeType, string pchArgs)
		{
			return this.FnTable.LaunchApplicationFromMimeType(pchMimeType, pchArgs);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00002A08 File Offset: 0x00000C08
		public EVRApplicationError LaunchDashboardOverlay(string pchAppKey)
		{
			return this.FnTable.LaunchDashboardOverlay(pchAppKey);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00002A28 File Offset: 0x00000C28
		public bool CancelApplicationLaunch(string pchAppKey)
		{
			return this.FnTable.CancelApplicationLaunch(pchAppKey);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00002A48 File Offset: 0x00000C48
		public EVRApplicationError IdentifyApplication(uint unProcessId, string pchAppKey)
		{
			return this.FnTable.IdentifyApplication(unProcessId, pchAppKey);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00002A6C File Offset: 0x00000C6C
		public uint GetApplicationProcessId(string pchAppKey)
		{
			return this.FnTable.GetApplicationProcessId(pchAppKey);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00002A8C File Offset: 0x00000C8C
		public string GetApplicationsErrorNameFromEnum(EVRApplicationError error)
		{
			IntPtr intPtr = this.FnTable.GetApplicationsErrorNameFromEnum(error);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public uint GetApplicationPropertyString(string pchAppKey, EVRApplicationProperty eProperty, string pchPropertyValueBuffer, uint unPropertyValueBufferLen, ref EVRApplicationError peError)
		{
			return this.FnTable.GetApplicationPropertyString(pchAppKey, eProperty, pchPropertyValueBuffer, unPropertyValueBufferLen, ref peError);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00002ADC File Offset: 0x00000CDC
		public bool GetApplicationPropertyBool(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError)
		{
			return this.FnTable.GetApplicationPropertyBool(pchAppKey, eProperty, ref peError);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00002B00 File Offset: 0x00000D00
		public ulong GetApplicationPropertyUint64(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError)
		{
			return this.FnTable.GetApplicationPropertyUint64(pchAppKey, eProperty, ref peError);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00002B24 File Offset: 0x00000D24
		public EVRApplicationError SetApplicationAutoLaunch(string pchAppKey, bool bAutoLaunch)
		{
			return this.FnTable.SetApplicationAutoLaunch(pchAppKey, bAutoLaunch);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00002B48 File Offset: 0x00000D48
		public bool GetApplicationAutoLaunch(string pchAppKey)
		{
			return this.FnTable.GetApplicationAutoLaunch(pchAppKey);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00002B68 File Offset: 0x00000D68
		public EVRApplicationError SetDefaultApplicationForMimeType(string pchAppKey, string pchMimeType)
		{
			return this.FnTable.SetDefaultApplicationForMimeType(pchAppKey, pchMimeType);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00002B8C File Offset: 0x00000D8C
		public bool GetDefaultApplicationForMimeType(string pchMimeType, string pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetDefaultApplicationForMimeType(pchMimeType, pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00002BB0 File Offset: 0x00000DB0
		public bool GetApplicationSupportedMimeTypes(string pchAppKey, string pchMimeTypesBuffer, uint unMimeTypesBuffer)
		{
			return this.FnTable.GetApplicationSupportedMimeTypes(pchAppKey, pchMimeTypesBuffer, unMimeTypesBuffer);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public uint GetApplicationsThatSupportMimeType(string pchMimeType, string pchAppKeysThatSupportBuffer, uint unAppKeysThatSupportBuffer)
		{
			return this.FnTable.GetApplicationsThatSupportMimeType(pchMimeType, pchAppKeysThatSupportBuffer, unAppKeysThatSupportBuffer);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public uint GetApplicationLaunchArguments(uint unHandle, string pchArgs, uint unArgs)
		{
			return this.FnTable.GetApplicationLaunchArguments(unHandle, pchArgs, unArgs);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00002C1C File Offset: 0x00000E1C
		public EVRApplicationError GetStartingApplication(string pchAppKeyBuffer, uint unAppKeyBufferLen)
		{
			return this.FnTable.GetStartingApplication(pchAppKeyBuffer, unAppKeyBufferLen);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00002C40 File Offset: 0x00000E40
		public EVRApplicationTransitionState GetTransitionState()
		{
			return this.FnTable.GetTransitionState();
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00002C60 File Offset: 0x00000E60
		public EVRApplicationError PerformApplicationPrelaunchCheck(string pchAppKey)
		{
			return this.FnTable.PerformApplicationPrelaunchCheck(pchAppKey);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00002C80 File Offset: 0x00000E80
		public string GetApplicationsTransitionStateNameFromEnum(EVRApplicationTransitionState state)
		{
			IntPtr intPtr = this.FnTable.GetApplicationsTransitionStateNameFromEnum(state);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public bool IsQuitUserPromptRequested()
		{
			return this.FnTable.IsQuitUserPromptRequested();
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public EVRApplicationError LaunchInternalProcess(string pchBinaryPath, string pchArguments, string pchWorkingDirectory)
		{
			return this.FnTable.LaunchInternalProcess(pchBinaryPath, pchArguments, pchWorkingDirectory);
		}

		// Token: 0x0400010D RID: 269
		private IVRApplications FnTable;
	}
}
