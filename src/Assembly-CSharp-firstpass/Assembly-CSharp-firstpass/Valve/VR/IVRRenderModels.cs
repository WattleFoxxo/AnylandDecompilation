using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x020000EA RID: 234
	public struct IVRRenderModels
	{
		// Token: 0x040000E1 RID: 225
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadRenderModel_Async LoadRenderModel_Async;

		// Token: 0x040000E2 RID: 226
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._FreeRenderModel FreeRenderModel;

		// Token: 0x040000E3 RID: 227
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadTexture_Async LoadTexture_Async;

		// Token: 0x040000E4 RID: 228
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._FreeTexture FreeTexture;

		// Token: 0x040000E5 RID: 229
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadTextureD3D11_Async LoadTextureD3D11_Async;

		// Token: 0x040000E6 RID: 230
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._LoadIntoTextureD3D11_Async LoadIntoTextureD3D11_Async;

		// Token: 0x040000E7 RID: 231
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._FreeTextureD3D11 FreeTextureD3D11;

		// Token: 0x040000E8 RID: 232
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelName GetRenderModelName;

		// Token: 0x040000E9 RID: 233
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelCount GetRenderModelCount;

		// Token: 0x040000EA RID: 234
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentCount GetComponentCount;

		// Token: 0x040000EB RID: 235
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentName GetComponentName;

		// Token: 0x040000EC RID: 236
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentButtonMask GetComponentButtonMask;

		// Token: 0x040000ED RID: 237
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentRenderModelName GetComponentRenderModelName;

		// Token: 0x040000EE RID: 238
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetComponentState GetComponentState;

		// Token: 0x040000EF RID: 239
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._RenderModelHasComponent RenderModelHasComponent;

		// Token: 0x040000F0 RID: 240
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelThumbnailURL GetRenderModelThumbnailURL;

		// Token: 0x040000F1 RID: 241
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelOriginalPath GetRenderModelOriginalPath;

		// Token: 0x040000F2 RID: 242
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRRenderModels._GetRenderModelErrorNameFromEnum GetRenderModelErrorNameFromEnum;

		// Token: 0x020000EB RID: 235
		// (Invoke) Token: 0x06000382 RID: 898
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadRenderModel_Async(string pchRenderModelName, ref IntPtr ppRenderModel);

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x06000386 RID: 902
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FreeRenderModel(IntPtr pRenderModel);

		// Token: 0x020000ED RID: 237
		// (Invoke) Token: 0x0600038A RID: 906
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadTexture_Async(int textureId, ref IntPtr ppTexture);

		// Token: 0x020000EE RID: 238
		// (Invoke) Token: 0x0600038E RID: 910
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FreeTexture(IntPtr pTexture);

		// Token: 0x020000EF RID: 239
		// (Invoke) Token: 0x06000392 RID: 914
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadTextureD3D11_Async(int textureId, IntPtr pD3D11Device, ref IntPtr ppD3D11Texture2D);

		// Token: 0x020000F0 RID: 240
		// (Invoke) Token: 0x06000396 RID: 918
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRRenderModelError _LoadIntoTextureD3D11_Async(int textureId, IntPtr pDstTexture);

		// Token: 0x020000F1 RID: 241
		// (Invoke) Token: 0x0600039A RID: 922
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FreeTextureD3D11(IntPtr pD3D11Texture2D);

		// Token: 0x020000F2 RID: 242
		// (Invoke) Token: 0x0600039E RID: 926
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelName(uint unRenderModelIndex, StringBuilder pchRenderModelName, uint unRenderModelNameLen);

		// Token: 0x020000F3 RID: 243
		// (Invoke) Token: 0x060003A2 RID: 930
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelCount();

		// Token: 0x020000F4 RID: 244
		// (Invoke) Token: 0x060003A6 RID: 934
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetComponentCount(string pchRenderModelName);

		// Token: 0x020000F5 RID: 245
		// (Invoke) Token: 0x060003AA RID: 938
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetComponentName(string pchRenderModelName, uint unComponentIndex, StringBuilder pchComponentName, uint unComponentNameLen);

		// Token: 0x020000F6 RID: 246
		// (Invoke) Token: 0x060003AE RID: 942
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetComponentButtonMask(string pchRenderModelName, string pchComponentName);

		// Token: 0x020000F7 RID: 247
		// (Invoke) Token: 0x060003B2 RID: 946
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetComponentRenderModelName(string pchRenderModelName, string pchComponentName, StringBuilder pchComponentRenderModelName, uint unComponentRenderModelNameLen);

		// Token: 0x020000F8 RID: 248
		// (Invoke) Token: 0x060003B6 RID: 950
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetComponentState(string pchRenderModelName, string pchComponentName, ref VRControllerState_t pControllerState, ref RenderModel_ControllerMode_State_t pState, ref RenderModel_ComponentState_t pComponentState);

		// Token: 0x020000F9 RID: 249
		// (Invoke) Token: 0x060003BA RID: 954
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _RenderModelHasComponent(string pchRenderModelName, string pchComponentName);

		// Token: 0x020000FA RID: 250
		// (Invoke) Token: 0x060003BE RID: 958
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelThumbnailURL(string pchRenderModelName, StringBuilder pchThumbnailURL, uint unThumbnailURLLen, ref EVRRenderModelError peError);

		// Token: 0x020000FB RID: 251
		// (Invoke) Token: 0x060003C2 RID: 962
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetRenderModelOriginalPath(string pchRenderModelName, StringBuilder pchOriginalPath, uint unOriginalPathLen, ref EVRRenderModelError peError);

		// Token: 0x020000FC RID: 252
		// (Invoke) Token: 0x060003C6 RID: 966
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetRenderModelErrorNameFromEnum(EVRRenderModelError error);
	}
}
