using System;
using UnityEngine;

// Token: 0x02000227 RID: 551
public class GetPersonInfo_Response : ServerResponse
{
	// Token: 0x060015DC RID: 5596 RVA: 0x000C0071 File Offset: 0x000BE471
	public GetPersonInfo_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.personInfo = JsonUtility.FromJson<PersonInfo>(www.text);
		}
	}

	// Token: 0x04001313 RID: 4883
	public PersonInfo personInfo;
}
