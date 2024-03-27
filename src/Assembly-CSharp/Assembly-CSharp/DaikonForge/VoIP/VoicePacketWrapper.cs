using System;

namespace DaikonForge.VoIP
{
	// Token: 0x02000018 RID: 24
	public struct VoicePacketWrapper
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00003C52 File Offset: 0x00002052
		public VoicePacketWrapper(ulong Index, int Frequency, byte[] RawData)
		{
			this.tempHeaderData = null;
			this.Index = Index;
			this.Frequency = (byte)(Frequency / 1000);
			this.RawData = RawData;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003C77 File Offset: 0x00002077
		public VoicePacketWrapper(ulong Index, byte Frequency, byte[] RawData)
		{
			this.tempHeaderData = null;
			this.Index = Index;
			this.Frequency = Frequency;
			this.RawData = RawData;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003C95 File Offset: 0x00002095
		public VoicePacketWrapper(byte[] headers, byte[] rawData)
		{
			this.tempHeaderData = null;
			this.Index = BitConverter.ToUInt64(headers, 0);
			this.Frequency = headers[8];
			this.RawData = rawData;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003CBC File Offset: 0x000020BC
		public byte[] ObtainHeaders()
		{
			this.tempHeaderData = TempArray<byte>.Obtain(9);
			byte b = (byte)(this.Index & 255UL);
			byte b2 = (byte)((this.Index & 65280UL) >> 8);
			byte b3 = (byte)((this.Index & 16711680UL) >> 16);
			byte b4 = (byte)((this.Index & (ulong)(-16777216)) >> 24);
			byte b5 = (byte)((this.Index & 1095216660480UL) >> 32);
			byte b6 = (byte)((this.Index & 280375465082880UL) >> 40);
			byte b7 = (byte)((this.Index & 71776119061217280UL) >> 48);
			byte b8 = (byte)((this.Index & 18374686479671623680UL) >> 56);
			this.tempHeaderData[0] = b;
			this.tempHeaderData[1] = b2;
			this.tempHeaderData[2] = b3;
			this.tempHeaderData[3] = b4;
			this.tempHeaderData[4] = b5;
			this.tempHeaderData[5] = b6;
			this.tempHeaderData[6] = b7;
			this.tempHeaderData[7] = b8;
			this.tempHeaderData[8] = this.Frequency;
			return this.tempHeaderData;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003DD2 File Offset: 0x000021D2
		public void ReleaseHeaders()
		{
			TempArray<byte>.Release(this.tempHeaderData);
		}

		// Token: 0x04000057 RID: 87
		public ulong Index;

		// Token: 0x04000058 RID: 88
		public byte Frequency;

		// Token: 0x04000059 RID: 89
		public byte[] RawData;

		// Token: 0x0400005A RID: 90
		private byte[] tempHeaderData;
	}
}
