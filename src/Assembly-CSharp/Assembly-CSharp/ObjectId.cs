using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

// Token: 0x020001BF RID: 447
public struct ObjectId
{
	// Token: 0x06000DB7 RID: 3511 RVA: 0x0007A78A File Offset: 0x00078B8A
	private ObjectId(int timestamp, int machine, short pid, int increment)
	{
		this._a = timestamp;
		this._b = (machine << 8) | ((pid >> 8) & 255);
		this._c = ((int)pid << 24) | increment;
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0007A7B3 File Offset: 0x00078BB3
	public static ObjectId GenerateNewId()
	{
		return ObjectId.GenerateNewId(ObjectId.GetTimestampFromDateTime(DateTime.UtcNow));
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0007A7C4 File Offset: 0x00078BC4
	public static ObjectId GenerateNewId(int timestamp)
	{
		int num = Interlocked.Increment(ref ObjectId.__staticIncrement) & 16777215;
		return new ObjectId(timestamp, ObjectId.__staticMachine, ObjectId.__staticPid, num);
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0007A7F3 File Offset: 0x00078BF3
	private static int GetAppDomainId()
	{
		return AppDomain.CurrentDomain.Id;
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0007A7FF File Offset: 0x00078BFF
	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int GetCurrentProcessId()
	{
		return Process.GetCurrentProcess().Id;
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0007A80C File Offset: 0x00078C0C
	private static int GetMachineHash()
	{
		string machineName = ObjectId.GetMachineName();
		return 16777215 & machineName.GetHashCode();
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x0007A82B File Offset: 0x00078C2B
	private static string GetMachineName()
	{
		return Environment.MachineName;
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x0007A834 File Offset: 0x00078C34
	private static short GetPid()
	{
		short num;
		try
		{
			num = (short)ObjectId.GetCurrentProcessId();
		}
		catch (SecurityException)
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0007A868 File Offset: 0x00078C68
	private static int GetTimestampFromDateTime(DateTime timestamp)
	{
		long num = (long)Math.Floor((ObjectId.ToUniversalTime(timestamp) - ObjectId.__unixEpoch).TotalSeconds);
		if (num < -2147483648L || num > 2147483647L)
		{
			throw new ArgumentOutOfRangeException("timestamp");
		}
		return (int)num;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0007A8BC File Offset: 0x00078CBC
	public override string ToString()
	{
		return new string(new char[]
		{
			ObjectId.ToHexChar((this._a >> 28) & 15),
			ObjectId.ToHexChar((this._a >> 24) & 15),
			ObjectId.ToHexChar((this._a >> 20) & 15),
			ObjectId.ToHexChar((this._a >> 16) & 15),
			ObjectId.ToHexChar((this._a >> 12) & 15),
			ObjectId.ToHexChar((this._a >> 8) & 15),
			ObjectId.ToHexChar((this._a >> 4) & 15),
			ObjectId.ToHexChar(this._a & 15),
			ObjectId.ToHexChar((this._b >> 28) & 15),
			ObjectId.ToHexChar((this._b >> 24) & 15),
			ObjectId.ToHexChar((this._b >> 20) & 15),
			ObjectId.ToHexChar((this._b >> 16) & 15),
			ObjectId.ToHexChar((this._b >> 12) & 15),
			ObjectId.ToHexChar((this._b >> 8) & 15),
			ObjectId.ToHexChar((this._b >> 4) & 15),
			ObjectId.ToHexChar(this._b & 15),
			ObjectId.ToHexChar((this._c >> 28) & 15),
			ObjectId.ToHexChar((this._c >> 24) & 15),
			ObjectId.ToHexChar((this._c >> 20) & 15),
			ObjectId.ToHexChar((this._c >> 16) & 15),
			ObjectId.ToHexChar((this._c >> 12) & 15),
			ObjectId.ToHexChar((this._c >> 8) & 15),
			ObjectId.ToHexChar((this._c >> 4) & 15),
			ObjectId.ToHexChar(this._c & 15)
		});
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0007AAB7 File Offset: 0x00078EB7
	private static char ToHexChar(int value)
	{
		return (char)(value + ((value >= 10) ? 87 : 48));
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x0007AAD0 File Offset: 0x00078ED0
	private static DateTime ToUniversalTime(DateTime dateTime)
	{
		if (dateTime == DateTime.MinValue)
		{
			return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
		}
		if (dateTime == DateTime.MaxValue)
		{
			return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
		}
		return dateTime.ToUniversalTime();
	}

	// Token: 0x04000F35 RID: 3893
	private static readonly ObjectId __emptyInstance = default(ObjectId);

	// Token: 0x04000F36 RID: 3894
	private static readonly int __staticMachine = (ObjectId.GetMachineHash() + ObjectId.GetAppDomainId()) & 16777215;

	// Token: 0x04000F37 RID: 3895
	private static readonly short __staticPid = ObjectId.GetPid();

	// Token: 0x04000F38 RID: 3896
	private static int __staticIncrement = new Random().Next();

	// Token: 0x04000F39 RID: 3897
	private static readonly DateTime __unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	// Token: 0x04000F3A RID: 3898
	private readonly int _a;

	// Token: 0x04000F3B RID: 3899
	private readonly int _b;

	// Token: 0x04000F3C RID: 3900
	private readonly int _c;
}
