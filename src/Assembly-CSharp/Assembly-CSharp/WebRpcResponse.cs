using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

// Token: 0x02000086 RID: 134
public class WebRpcResponse
{
	// Token: 0x06000458 RID: 1112 RVA: 0x000148A0 File Offset: 0x00012CA0
	public WebRpcResponse(OperationResponse response)
	{
		object obj;
		response.Parameters.TryGetValue(209, out obj);
		this.Name = obj as string;
		response.Parameters.TryGetValue(207, out obj);
		this.ReturnCode = ((obj == null) ? (-1) : ((int)((byte)obj)));
		response.Parameters.TryGetValue(208, out obj);
		this.Parameters = obj as Dictionary<string, object>;
		response.Parameters.TryGetValue(206, out obj);
		this.DebugMessage = obj as string;
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000459 RID: 1113 RVA: 0x0001493B File Offset: 0x00012D3B
	// (set) Token: 0x0600045A RID: 1114 RVA: 0x00014943 File Offset: 0x00012D43
	public string Name { get; private set; }

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600045B RID: 1115 RVA: 0x0001494C File Offset: 0x00012D4C
	// (set) Token: 0x0600045C RID: 1116 RVA: 0x00014954 File Offset: 0x00012D54
	public int ReturnCode { get; private set; }

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x0600045D RID: 1117 RVA: 0x0001495D File Offset: 0x00012D5D
	// (set) Token: 0x0600045E RID: 1118 RVA: 0x00014965 File Offset: 0x00012D65
	public string DebugMessage { get; private set; }

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x0600045F RID: 1119 RVA: 0x0001496E File Offset: 0x00012D6E
	// (set) Token: 0x06000460 RID: 1120 RVA: 0x00014976 File Offset: 0x00012D76
	public Dictionary<string, object> Parameters { get; private set; }

	// Token: 0x06000461 RID: 1121 RVA: 0x0001497F File Offset: 0x00012D7F
	public string ToStringFull()
	{
		return string.Format("{0}={2}: {1} \"{3}\"", new object[]
		{
			this.Name,
			SupportClass.DictionaryToString(this.Parameters),
			this.ReturnCode,
			this.DebugMessage
		});
	}
}
