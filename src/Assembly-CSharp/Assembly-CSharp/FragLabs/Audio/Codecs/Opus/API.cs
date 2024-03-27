using System;
using System.Runtime.InteropServices;

namespace FragLabs.Audio.Codecs.Opus
{
	// Token: 0x02000002 RID: 2
	internal class API
	{
		// Token: 0x06000002 RID: 2
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr opus_encoder_create(int Fs, int channels, int application, out IntPtr error);

		// Token: 0x06000003 RID: 3
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void opus_encoder_destroy(IntPtr encoder);

		// Token: 0x06000004 RID: 4
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_encode(IntPtr st, byte[] pcm, int frame_size, IntPtr data, int max_data_bytes);

		// Token: 0x06000005 RID: 5
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr opus_decoder_create(int Fs, int channels, out IntPtr error);

		// Token: 0x06000006 RID: 6
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void opus_decoder_destroy(IntPtr decoder);

		// Token: 0x06000007 RID: 7
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_decode(IntPtr st, byte[] data, int len, IntPtr pcm, int frame_size, int decode_fec);

		// Token: 0x06000008 RID: 8
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_encoder_ctl(IntPtr st, Ctl request, int value);

		// Token: 0x06000009 RID: 9
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_encoder_ctl(IntPtr st, Ctl request, out int value);

		// Token: 0x04000001 RID: 1
		private const string importDLL = "opus";
	}
}
