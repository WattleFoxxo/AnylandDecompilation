using System;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class GetPersonInfoBasic_Response : ServerResponse
{
	// Token: 0x060015DD RID: 5597 RVA: 0x000C0096 File Offset: 0x000BE496
	public GetPersonInfoBasic_Response(WWW www)
		: base(www)
	{
		if (www.error == null)
		{
			this.personInfoBasic = JsonUtility.FromJson<PersonInfoBasic>(www.text);
		}
	}

	// Token: 0x04001314 RID: 4884
	public PersonInfoBasic personInfoBasic;
}
