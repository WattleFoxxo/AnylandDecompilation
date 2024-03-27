using System;

// Token: 0x02000073 RID: 115
public class TypedLobby
{
	// Token: 0x06000362 RID: 866 RVA: 0x0000DB71 File Offset: 0x0000BF71
	public TypedLobby()
	{
		this.Name = string.Empty;
		this.Type = LobbyType.Default;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0000DB8B File Offset: 0x0000BF8B
	public TypedLobby(string name, LobbyType type)
	{
		this.Name = name;
		this.Type = type;
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000364 RID: 868 RVA: 0x0000DBA1 File Offset: 0x0000BFA1
	public bool IsDefault
	{
		get
		{
			return this.Type == LobbyType.Default && string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0000DBBC File Offset: 0x0000BFBC
	public override string ToString()
	{
		return string.Format("lobby '{0}'[{1}]", this.Name, this.Type);
	}

	// Token: 0x040002D4 RID: 724
	public string Name;

	// Token: 0x040002D5 RID: 725
	public LobbyType Type;

	// Token: 0x040002D6 RID: 726
	public static readonly TypedLobby Default = new TypedLobby();
}
