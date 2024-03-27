using System;

// Token: 0x02000077 RID: 119
public class AuthenticationValues
{
	// Token: 0x06000369 RID: 873 RVA: 0x0000DC40 File Offset: 0x0000C040
	public AuthenticationValues()
	{
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0000DC53 File Offset: 0x0000C053
	public AuthenticationValues(string userId)
	{
		this.UserId = userId;
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600036B RID: 875 RVA: 0x0000DC6D File Offset: 0x0000C06D
	// (set) Token: 0x0600036C RID: 876 RVA: 0x0000DC75 File Offset: 0x0000C075
	public CustomAuthenticationType AuthType
	{
		get
		{
			return this.authType;
		}
		set
		{
			this.authType = value;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600036D RID: 877 RVA: 0x0000DC7E File Offset: 0x0000C07E
	// (set) Token: 0x0600036E RID: 878 RVA: 0x0000DC86 File Offset: 0x0000C086
	public string AuthGetParameters { get; set; }

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600036F RID: 879 RVA: 0x0000DC8F File Offset: 0x0000C08F
	// (set) Token: 0x06000370 RID: 880 RVA: 0x0000DC97 File Offset: 0x0000C097
	public object AuthPostData { get; private set; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000371 RID: 881 RVA: 0x0000DCA0 File Offset: 0x0000C0A0
	// (set) Token: 0x06000372 RID: 882 RVA: 0x0000DCA8 File Offset: 0x0000C0A8
	public string Token { get; set; }

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000373 RID: 883 RVA: 0x0000DCB1 File Offset: 0x0000C0B1
	// (set) Token: 0x06000374 RID: 884 RVA: 0x0000DCB9 File Offset: 0x0000C0B9
	public string UserId { get; set; }

	// Token: 0x06000375 RID: 885 RVA: 0x0000DCC2 File Offset: 0x0000C0C2
	public virtual void SetAuthPostData(string stringData)
	{
		this.AuthPostData = ((!string.IsNullOrEmpty(stringData)) ? stringData : null);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0000DCDC File Offset: 0x0000C0DC
	public virtual void SetAuthPostData(byte[] byteData)
	{
		this.AuthPostData = byteData;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0000DCE8 File Offset: 0x0000C0E8
	public virtual void AddAuthParameter(string key, string value)
	{
		string text = ((!string.IsNullOrEmpty(this.AuthGetParameters)) ? "&" : string.Empty);
		this.AuthGetParameters = string.Format("{0}{1}{2}={3}", new object[]
		{
			this.AuthGetParameters,
			text,
			Uri.EscapeDataString(key),
			Uri.EscapeDataString(value)
		});
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0000DD4A File Offset: 0x0000C14A
	public override string ToString()
	{
		return string.Format("AuthenticationValues UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null);
	}

	// Token: 0x040002E5 RID: 741
	private CustomAuthenticationType authType = CustomAuthenticationType.None;
}
