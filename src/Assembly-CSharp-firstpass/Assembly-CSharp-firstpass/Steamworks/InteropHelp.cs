using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Steamworks
{
	// Token: 0x020002C7 RID: 711
	public class InteropHelp
	{
		// Token: 0x06000C76 RID: 3190 RVA: 0x0000C37B File Offset: 0x0000A57B
		public static void TestIfPlatformSupported()
		{
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0000C37D File Offset: 0x0000A57D
		public static void TestIfAvailableClient()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamClient() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0000C3A3 File Offset: 0x0000A5A3
		public static void TestIfAvailableGameServer()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamGameServerClient() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0000C3CC File Offset: 0x0000A5CC
		public static string PtrToStringUTF8(IntPtr nativeUtf8)
		{
			if (nativeUtf8 == IntPtr.Zero)
			{
				return null;
			}
			int num = 0;
			while (Marshal.ReadByte(nativeUtf8, num) != 0)
			{
				num++;
			}
			if (num == 0)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			Marshal.Copy(nativeUtf8, array, 0, array.Length);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x020002C8 RID: 712
		public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x06000C7A RID: 3194 RVA: 0x0000C42C File Offset: 0x0000A62C
			public UTF8StringHandle(string str)
				: base(true)
			{
				if (str == null)
				{
					base.SetHandle(IntPtr.Zero);
					return;
				}
				byte[] array = new byte[Encoding.UTF8.GetByteCount(str) + 1];
				Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				base.SetHandle(intPtr);
			}

			// Token: 0x06000C7B RID: 3195 RVA: 0x0000C495 File Offset: 0x0000A695
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					Marshal.FreeHGlobal(this.handle);
				}
				return true;
			}
		}

		// Token: 0x020002C9 RID: 713
		public class SteamParamStringArray
		{
			// Token: 0x06000C7C RID: 3196 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
			public SteamParamStringArray(IList<string> strings)
			{
				if (strings == null)
				{
					this.m_pSteamParamStringArray = IntPtr.Zero;
					return;
				}
				this.m_Strings = new IntPtr[strings.Count];
				for (int i = 0; i < strings.Count; i++)
				{
					byte[] array = new byte[Encoding.UTF8.GetByteCount(strings[i]) + 1];
					Encoding.UTF8.GetBytes(strings[i], 0, strings[i].Length, array, 0);
					this.m_Strings[i] = Marshal.AllocHGlobal(array.Length);
					Marshal.Copy(array, 0, this.m_Strings[i], array.Length);
				}
				this.m_ptrStrings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * this.m_Strings.Length);
				SteamParamStringArray_t steamParamStringArray_t = new SteamParamStringArray_t
				{
					m_ppStrings = this.m_ptrStrings,
					m_nNumStrings = this.m_Strings.Length
				};
				Marshal.Copy(this.m_Strings, 0, steamParamStringArray_t.m_ppStrings, this.m_Strings.Length);
				this.m_pSteamParamStringArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SteamParamStringArray_t)));
				Marshal.StructureToPtr(steamParamStringArray_t, this.m_pSteamParamStringArray, false);
			}

			// Token: 0x06000C7D RID: 3197 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
			protected override void Finalize()
			{
				try
				{
					foreach (IntPtr intPtr in this.m_Strings)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					if (this.m_ptrStrings != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_ptrStrings);
					}
					if (this.m_pSteamParamStringArray != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x06000C7E RID: 3198 RVA: 0x0000C680 File Offset: 0x0000A880
			public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that)
			{
				return that.m_pSteamParamStringArray;
			}

			// Token: 0x04000CA0 RID: 3232
			private IntPtr[] m_Strings;

			// Token: 0x04000CA1 RID: 3233
			private IntPtr m_ptrStrings;

			// Token: 0x04000CA2 RID: 3234
			private IntPtr m_pSteamParamStringArray;
		}
	}
}
