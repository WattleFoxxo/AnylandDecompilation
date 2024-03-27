using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000120 RID: 288
	public class CVRRenderModels
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x00003F0B File Offset: 0x0000210B
		internal CVRRenderModels(IntPtr pInterface)
		{
			this.FnTable = (IVRRenderModels)Marshal.PtrToStructure(pInterface, typeof(IVRRenderModels));
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00003F30 File Offset: 0x00002130
		public EVRRenderModelError LoadRenderModel_Async(string pchRenderModelName, ref IntPtr ppRenderModel)
		{
			return this.FnTable.LoadRenderModel_Async(pchRenderModelName, ref ppRenderModel);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00003F51 File Offset: 0x00002151
		public void FreeRenderModel(IntPtr pRenderModel)
		{
			this.FnTable.FreeRenderModel(pRenderModel);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00003F64 File Offset: 0x00002164
		public EVRRenderModelError LoadTexture_Async(int textureId, ref IntPtr ppTexture)
		{
			return this.FnTable.LoadTexture_Async(textureId, ref ppTexture);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00003F85 File Offset: 0x00002185
		public void FreeTexture(IntPtr pTexture)
		{
			this.FnTable.FreeTexture(pTexture);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00003F98 File Offset: 0x00002198
		public EVRRenderModelError LoadTextureD3D11_Async(int textureId, IntPtr pD3D11Device, ref IntPtr ppD3D11Texture2D)
		{
			return this.FnTable.LoadTextureD3D11_Async(textureId, pD3D11Device, ref ppD3D11Texture2D);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00003FBC File Offset: 0x000021BC
		public EVRRenderModelError LoadIntoTextureD3D11_Async(int textureId, IntPtr pDstTexture)
		{
			return this.FnTable.LoadIntoTextureD3D11_Async(textureId, pDstTexture);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00003FDD File Offset: 0x000021DD
		public void FreeTextureD3D11(IntPtr pD3D11Texture2D)
		{
			this.FnTable.FreeTextureD3D11(pD3D11Texture2D);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00003FF0 File Offset: 0x000021F0
		public uint GetRenderModelName(uint unRenderModelIndex, StringBuilder pchRenderModelName, uint unRenderModelNameLen)
		{
			return this.FnTable.GetRenderModelName(unRenderModelIndex, pchRenderModelName, unRenderModelNameLen);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00004014 File Offset: 0x00002214
		public uint GetRenderModelCount()
		{
			return this.FnTable.GetRenderModelCount();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00004034 File Offset: 0x00002234
		public uint GetComponentCount(string pchRenderModelName)
		{
			return this.FnTable.GetComponentCount(pchRenderModelName);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00004054 File Offset: 0x00002254
		public uint GetComponentName(string pchRenderModelName, uint unComponentIndex, StringBuilder pchComponentName, uint unComponentNameLen)
		{
			return this.FnTable.GetComponentName(pchRenderModelName, unComponentIndex, pchComponentName, unComponentNameLen);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00004078 File Offset: 0x00002278
		public ulong GetComponentButtonMask(string pchRenderModelName, string pchComponentName)
		{
			return this.FnTable.GetComponentButtonMask(pchRenderModelName, pchComponentName);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000409C File Offset: 0x0000229C
		public uint GetComponentRenderModelName(string pchRenderModelName, string pchComponentName, StringBuilder pchComponentRenderModelName, uint unComponentRenderModelNameLen)
		{
			return this.FnTable.GetComponentRenderModelName(pchRenderModelName, pchComponentName, pchComponentRenderModelName, unComponentRenderModelNameLen);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000040C0 File Offset: 0x000022C0
		public bool GetComponentState(string pchRenderModelName, string pchComponentName, ref VRControllerState_t pControllerState, ref RenderModel_ControllerMode_State_t pState, ref RenderModel_ComponentState_t pComponentState)
		{
			return this.FnTable.GetComponentState(pchRenderModelName, pchComponentName, ref pControllerState, ref pState, ref pComponentState);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000040E8 File Offset: 0x000022E8
		public bool RenderModelHasComponent(string pchRenderModelName, string pchComponentName)
		{
			return this.FnTable.RenderModelHasComponent(pchRenderModelName, pchComponentName);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000410C File Offset: 0x0000230C
		public uint GetRenderModelThumbnailURL(string pchRenderModelName, StringBuilder pchThumbnailURL, uint unThumbnailURLLen, ref EVRRenderModelError peError)
		{
			return this.FnTable.GetRenderModelThumbnailURL(pchRenderModelName, pchThumbnailURL, unThumbnailURLLen, ref peError);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00004130 File Offset: 0x00002330
		public uint GetRenderModelOriginalPath(string pchRenderModelName, StringBuilder pchOriginalPath, uint unOriginalPathLen, ref EVRRenderModelError peError)
		{
			return this.FnTable.GetRenderModelOriginalPath(pchRenderModelName, pchOriginalPath, unOriginalPathLen, ref peError);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00004154 File Offset: 0x00002354
		public string GetRenderModelErrorNameFromEnum(EVRRenderModelError error)
		{
			IntPtr intPtr = this.FnTable.GetRenderModelErrorNameFromEnum(error);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x04000112 RID: 274
		private IVRRenderModels FnTable;
	}
}
