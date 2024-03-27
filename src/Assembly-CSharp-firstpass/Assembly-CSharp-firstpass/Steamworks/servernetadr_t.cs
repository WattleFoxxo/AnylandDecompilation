using System;

namespace Steamworks
{
	// Token: 0x020002F1 RID: 753
	[Serializable]
	public struct servernetadr_t
	{
		// Token: 0x06000D1D RID: 3357 RVA: 0x0000D0AD File Offset: 0x0000B2AD
		public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
		{
			this.m_unIP = ip;
			this.m_usQueryPort = usQueryPort;
			this.m_usConnectionPort = usConnectionPort;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0000D0C4 File Offset: 0x0000B2C4
		public ushort GetQueryPort()
		{
			return this.m_usQueryPort;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0000D0CC File Offset: 0x0000B2CC
		public void SetQueryPort(ushort usPort)
		{
			this.m_usQueryPort = usPort;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0000D0D5 File Offset: 0x0000B2D5
		public ushort GetConnectionPort()
		{
			return this.m_usConnectionPort;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0000D0DD File Offset: 0x0000B2DD
		public void SetConnectionPort(ushort usPort)
		{
			this.m_usConnectionPort = usPort;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0000D0E6 File Offset: 0x0000B2E6
		public uint GetIP()
		{
			return this.m_unIP;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0000D0EE File Offset: 0x0000B2EE
		public void SetIP(uint unIP)
		{
			this.m_unIP = unIP;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0000D0F7 File Offset: 0x0000B2F7
		public string GetConnectionAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0000D10A File Offset: 0x0000B30A
		public string GetQueryAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0000D120 File Offset: 0x0000B320
		public static string ToString(uint unIP, ushort usPort)
		{
			return string.Format("{0}.{1}.{2}.{3}:{4}", new object[]
			{
				(ulong)(unIP >> 24) & 255UL,
				(ulong)(unIP >> 16) & 255UL,
				(ulong)(unIP >> 8) & 255UL,
				(ulong)unIP & 255UL,
				usPort
			});
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0000D192 File Offset: 0x0000B392
		public static bool operator <(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP < y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort < y.m_usQueryPort);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		public static bool operator >(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP > y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort > y.m_usQueryPort);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0000D20E File Offset: 0x0000B40E
		public override bool Equals(object other)
		{
			return other is servernetadr_t && this == (servernetadr_t)other;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0000D22F File Offset: 0x0000B42F
		public override int GetHashCode()
		{
			return this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0000D266 File Offset: 0x0000B466
		public static bool operator ==(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP == y.m_unIP && x.m_usQueryPort == y.m_usQueryPort && x.m_usConnectionPort == y.m_usConnectionPort;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0000D2A1 File Offset: 0x0000B4A1
		public static bool operator !=(servernetadr_t x, servernetadr_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0000D2AD File Offset: 0x0000B4AD
		public bool Equals(servernetadr_t other)
		{
			return this.m_unIP == other.m_unIP && this.m_usQueryPort == other.m_usQueryPort && this.m_usConnectionPort == other.m_usConnectionPort;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0000D2E5 File Offset: 0x0000B4E5
		public int CompareTo(servernetadr_t other)
		{
			return this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
		}

		// Token: 0x04000CE3 RID: 3299
		private ushort m_usConnectionPort;

		// Token: 0x04000CE4 RID: 3300
		private ushort m_usQueryPort;

		// Token: 0x04000CE5 RID: 3301
		private uint m_unIP;
	}
}
