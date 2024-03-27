using System;

// Token: 0x02000098 RID: 152
public class Region
{
	// Token: 0x060005AE RID: 1454 RVA: 0x00019CE0 File Offset: 0x000180E0
	public Region(CloudRegionCode code)
	{
		this.Code = code;
		this.Cluster = code.ToString();
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00019D02 File Offset: 0x00018102
	public Region(CloudRegionCode code, string regionCodeString, string address)
	{
		this.Code = code;
		this.Cluster = regionCodeString;
		this.HostAndPort = address;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00019D20 File Offset: 0x00018120
	public static CloudRegionCode Parse(string codeAsString)
	{
		if (codeAsString == null)
		{
			return CloudRegionCode.none;
		}
		int num = codeAsString.IndexOf('/');
		if (num > 0)
		{
			codeAsString = codeAsString.Substring(0, num);
		}
		codeAsString = codeAsString.ToLower();
		if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
		{
			return (CloudRegionCode)Enum.Parse(typeof(CloudRegionCode), codeAsString);
		}
		return CloudRegionCode.none;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00019D84 File Offset: 0x00018184
	internal static CloudRegionFlag ParseFlag(CloudRegionCode region)
	{
		if (Enum.IsDefined(typeof(CloudRegionFlag), region.ToString()))
		{
			return (CloudRegionFlag)Enum.Parse(typeof(CloudRegionFlag), region.ToString());
		}
		return (CloudRegionFlag)0;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00019DD8 File Offset: 0x000181D8
	[Obsolete]
	internal static CloudRegionFlag ParseFlag(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionFlag cloudRegionFlag = (CloudRegionFlag)0;
		if (Enum.IsDefined(typeof(CloudRegionFlag), codeAsString))
		{
			cloudRegionFlag = (CloudRegionFlag)Enum.Parse(typeof(CloudRegionFlag), codeAsString);
		}
		return cloudRegionFlag;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00019E1B File Offset: 0x0001821B
	public override string ToString()
	{
		return string.Format("'{0}' \t{1}ms \t{2}", this.Cluster, this.Ping, this.HostAndPort);
	}

	// Token: 0x040003F5 RID: 1013
	public CloudRegionCode Code;

	// Token: 0x040003F6 RID: 1014
	public string Cluster;

	// Token: 0x040003F7 RID: 1015
	public string HostAndPort;

	// Token: 0x040003F8 RID: 1016
	public int Ping;
}
