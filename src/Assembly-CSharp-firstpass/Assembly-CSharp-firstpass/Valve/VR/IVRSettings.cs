using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000100 RID: 256
	public struct IVRSettings
	{
		// Token: 0x040000F5 RID: 245
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetSettingsErrorNameFromEnum GetSettingsErrorNameFromEnum;

		// Token: 0x040000F6 RID: 246
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._Sync Sync;

		// Token: 0x040000F7 RID: 247
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetBool GetBool;

		// Token: 0x040000F8 RID: 248
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetBool SetBool;

		// Token: 0x040000F9 RID: 249
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetInt32 GetInt32;

		// Token: 0x040000FA RID: 250
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetInt32 SetInt32;

		// Token: 0x040000FB RID: 251
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetFloat GetFloat;

		// Token: 0x040000FC RID: 252
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetFloat SetFloat;

		// Token: 0x040000FD RID: 253
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._GetString GetString;

		// Token: 0x040000FE RID: 254
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._SetString SetString;

		// Token: 0x040000FF RID: 255
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._RemoveSection RemoveSection;

		// Token: 0x04000100 RID: 256
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSettings._RemoveKeyInSection RemoveKeyInSection;

		// Token: 0x02000101 RID: 257
		// (Invoke) Token: 0x060003D2 RID: 978
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetSettingsErrorNameFromEnum(EVRSettingsError eError);

		// Token: 0x02000102 RID: 258
		// (Invoke) Token: 0x060003D6 RID: 982
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _Sync(bool bForce, ref EVRSettingsError peError);

		// Token: 0x02000103 RID: 259
		// (Invoke) Token: 0x060003DA RID: 986
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetBool(string pchSection, string pchSettingsKey, bool bDefaultValue, ref EVRSettingsError peError);

		// Token: 0x02000104 RID: 260
		// (Invoke) Token: 0x060003DE RID: 990
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetBool(string pchSection, string pchSettingsKey, bool bValue, ref EVRSettingsError peError);

		// Token: 0x02000105 RID: 261
		// (Invoke) Token: 0x060003E2 RID: 994
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate int _GetInt32(string pchSection, string pchSettingsKey, int nDefaultValue, ref EVRSettingsError peError);

		// Token: 0x02000106 RID: 262
		// (Invoke) Token: 0x060003E6 RID: 998
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetInt32(string pchSection, string pchSettingsKey, int nValue, ref EVRSettingsError peError);

		// Token: 0x02000107 RID: 263
		// (Invoke) Token: 0x060003EA RID: 1002
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate float _GetFloat(string pchSection, string pchSettingsKey, float flDefaultValue, ref EVRSettingsError peError);

		// Token: 0x02000108 RID: 264
		// (Invoke) Token: 0x060003EE RID: 1006
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetFloat(string pchSection, string pchSettingsKey, float flValue, ref EVRSettingsError peError);

		// Token: 0x02000109 RID: 265
		// (Invoke) Token: 0x060003F2 RID: 1010
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetString(string pchSection, string pchSettingsKey, StringBuilder pchValue, uint unValueLen, string pchDefaultValue, ref EVRSettingsError peError);

		// Token: 0x0200010A RID: 266
		// (Invoke) Token: 0x060003F6 RID: 1014
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetString(string pchSection, string pchSettingsKey, string pchValue, ref EVRSettingsError peError);

		// Token: 0x0200010B RID: 267
		// (Invoke) Token: 0x060003FA RID: 1018
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _RemoveSection(string pchSection, ref EVRSettingsError peError);

		// Token: 0x0200010C RID: 268
		// (Invoke) Token: 0x060003FE RID: 1022
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _RemoveKeyInSection(string pchSection, string pchSettingsKey, ref EVRSettingsError peError);
	}
}
