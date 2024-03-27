using System;
using System.Runtime.InteropServices;
using POpusCodec.Enums;

namespace POpusCodec
{
	// Token: 0x02000196 RID: 406
	internal class Wrapper
	{
		// Token: 0x06000593 RID: 1427
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_encoder_get_size(Channels channels);

		// Token: 0x06000594 RID: 1428
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern OpusStatusCode opus_encoder_init(IntPtr st, SamplingRate Fs, Channels channels, OpusApplicationType application);

		// Token: 0x06000595 RID: 1429
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern IntPtr opus_get_version_string();

		// Token: 0x06000596 RID: 1430
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_encode(IntPtr st, short[] pcm, int frame_size, byte[] data, int max_data_bytes);

		// Token: 0x06000597 RID: 1431
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_encode_float(IntPtr st, float[] pcm, int frame_size, byte[] data, int max_data_bytes);

		// Token: 0x06000598 RID: 1432
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_encoder_ctl_set(IntPtr st, OpusCtlSetRequest request, int value);

		// Token: 0x06000599 RID: 1433
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_encoder_ctl_get(IntPtr st, OpusCtlGetRequest request, ref int value);

		// Token: 0x0600059A RID: 1434
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_decoder_ctl_set(IntPtr st, OpusCtlSetRequest request, int value);

		// Token: 0x0600059B RID: 1435
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_decoder_ctl_get(IntPtr st, OpusCtlGetRequest request, ref int value);

		// Token: 0x0600059C RID: 1436
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_decoder_get_size(Channels channels);

		// Token: 0x0600059D RID: 1437
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern OpusStatusCode opus_decoder_init(IntPtr st, SamplingRate Fs, Channels channels);

		// Token: 0x0600059E RID: 1438
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_decode(IntPtr st, byte[] data, int len, short[] pcm, int frame_size, int decode_fec);

		// Token: 0x0600059F RID: 1439
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int opus_decode_float(IntPtr st, byte[] data, int len, float[] pcm, int frame_size, int decode_fec);

		// Token: 0x060005A0 RID: 1440
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int opus_packet_get_bandwidth(byte[] data);

		// Token: 0x060005A1 RID: 1441
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int opus_packet_get_nb_channels(byte[] data);

		// Token: 0x060005A2 RID: 1442
		[DllImport("opus_egpv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr opus_strerror(OpusStatusCode error);

		// Token: 0x060005A3 RID: 1443 RVA: 0x00005374 File Offset: 0x00003574
		public static IntPtr opus_encoder_create(SamplingRate Fs, Channels channels, OpusApplicationType application)
		{
			int num = Wrapper.opus_encoder_get_size(channels);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			OpusStatusCode opusStatusCode = Wrapper.opus_encoder_init(intPtr, Fs, channels, application);
			try
			{
				Wrapper.HandleStatusCode(opusStatusCode);
			}
			catch (Exception ex)
			{
				if (intPtr != IntPtr.Zero)
				{
					Wrapper.opus_encoder_destroy(intPtr);
					intPtr = IntPtr.Zero;
				}
				throw ex;
			}
			return intPtr;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000053D4 File Offset: 0x000035D4
		public static int opus_encode(IntPtr st, short[] pcm, int frame_size, byte[] data)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusEncoder");
			}
			int num = Wrapper.opus_encode(st, pcm, frame_size, data, data.Length);
			if (num <= 0)
			{
				Wrapper.HandleStatusCode((OpusStatusCode)num);
			}
			return num;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00005418 File Offset: 0x00003618
		public static int opus_encode(IntPtr st, float[] pcm, int frame_size, byte[] data)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusEncoder");
			}
			int num = Wrapper.opus_encode_float(st, pcm, frame_size, data, data.Length);
			if (num <= 0)
			{
				Wrapper.HandleStatusCode((OpusStatusCode)num);
			}
			return num;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0000545B File Offset: 0x0000365B
		public static void opus_encoder_destroy(IntPtr st)
		{
			Marshal.FreeHGlobal(st);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00005464 File Offset: 0x00003664
		public static int get_opus_encoder_ctl(IntPtr st, OpusCtlGetRequest request)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusEncoder");
			}
			int num = 0;
			OpusStatusCode opusStatusCode = (OpusStatusCode)Wrapper.opus_encoder_ctl_get(st, request, ref num);
			Wrapper.HandleStatusCode(opusStatusCode);
			return num;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000054A0 File Offset: 0x000036A0
		public static void set_opus_encoder_ctl(IntPtr st, OpusCtlSetRequest request, int value)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusEncoder");
			}
			OpusStatusCode opusStatusCode = (OpusStatusCode)Wrapper.opus_encoder_ctl_set(st, request, value);
			Wrapper.HandleStatusCode(opusStatusCode);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000054D8 File Offset: 0x000036D8
		public static int get_opus_decoder_ctl(IntPtr st, OpusCtlGetRequest request)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusDcoder");
			}
			int num = 0;
			OpusStatusCode opusStatusCode = (OpusStatusCode)Wrapper.opus_decoder_ctl_get(st, request, ref num);
			Wrapper.HandleStatusCode(opusStatusCode);
			return num;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00005514 File Offset: 0x00003714
		public static void set_opus_decoder_ctl(IntPtr st, OpusCtlSetRequest request, int value)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusDecoder");
			}
			OpusStatusCode opusStatusCode = (OpusStatusCode)Wrapper.opus_decoder_ctl_set(st, request, value);
			Wrapper.HandleStatusCode(opusStatusCode);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000554C File Offset: 0x0000374C
		public static IntPtr opus_decoder_create(SamplingRate Fs, Channels channels)
		{
			int num = Wrapper.opus_decoder_get_size(channels);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			OpusStatusCode opusStatusCode = Wrapper.opus_decoder_init(intPtr, Fs, channels);
			try
			{
				Wrapper.HandleStatusCode(opusStatusCode);
			}
			catch (Exception ex)
			{
				if (intPtr != IntPtr.Zero)
				{
					Wrapper.opus_decoder_destroy(intPtr);
					intPtr = IntPtr.Zero;
				}
				throw ex;
			}
			return intPtr;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000055AC File Offset: 0x000037AC
		public static void opus_decoder_destroy(IntPtr st)
		{
			Marshal.FreeHGlobal(st);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000055B4 File Offset: 0x000037B4
		public static int opus_decode(IntPtr st, byte[] data, short[] pcm, int decode_fec, int channels)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusDecoder");
			}
			int num;
			if (data != null)
			{
				num = Wrapper.opus_decode(st, data, data.Length, pcm, pcm.Length / channels, decode_fec);
			}
			else
			{
				num = Wrapper.opus_decode(st, null, 0, pcm, pcm.Length / channels, decode_fec);
			}
			if (num == -4)
			{
				return 0;
			}
			if (num <= 0)
			{
				Wrapper.HandleStatusCode((OpusStatusCode)num);
			}
			return num;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00005628 File Offset: 0x00003828
		public static int opus_decode(IntPtr st, byte[] data, float[] pcm, int decode_fec, int channels)
		{
			if (st == IntPtr.Zero)
			{
				throw new ObjectDisposedException("OpusDecoder");
			}
			int num;
			if (data != null)
			{
				num = Wrapper.opus_decode_float(st, data, data.Length, pcm, pcm.Length / channels, decode_fec);
			}
			else
			{
				num = Wrapper.opus_decode_float(st, null, 0, pcm, pcm.Length / channels, decode_fec);
			}
			if (num == -4)
			{
				return 0;
			}
			if (num <= 0)
			{
				Wrapper.HandleStatusCode((OpusStatusCode)num);
			}
			return num;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00005699 File Offset: 0x00003899
		private static void HandleStatusCode(OpusStatusCode statusCode)
		{
			if (statusCode != OpusStatusCode.OK)
			{
				throw new OpusException(statusCode, Marshal.PtrToStringAnsi(Wrapper.opus_strerror(statusCode)));
			}
		}
	}
}
