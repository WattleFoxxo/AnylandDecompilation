using System;

// Token: 0x0200020E RID: 526
public class ResponseError
{
	// Token: 0x060015BF RID: 5567 RVA: 0x000BFBEA File Offset: 0x000BDFEA
	public ResponseError(string httpError, int httpResponseCode, string softErrorReason)
	{
		this.HttpError = httpError;
		this.HttpResponseCode = httpResponseCode;
		this.SoftErrorReason = softErrorReason;
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x060015C0 RID: 5568 RVA: 0x000BFC07 File Offset: 0x000BE007
	public bool IsHttpError
	{
		get
		{
			return !string.IsNullOrEmpty(this.HttpError);
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x060015C1 RID: 5569 RVA: 0x000BFC17 File Offset: 0x000BE017
	public bool IsSoftError
	{
		get
		{
			return !this.IsHttpError;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x060015C2 RID: 5570 RVA: 0x000BFC22 File Offset: 0x000BE022
	public bool IsNetworkOrServerError
	{
		get
		{
			return this.HttpResponseCode >= 500 && this.HttpResponseCode < 600;
		}
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x000BFC44 File Offset: 0x000BE044
	public string GetPublicErrorMessage()
	{
		string text;
		if (this.IsHttpError)
		{
			if (this.HttpResponseCode == 500)
			{
				text = "Server error";
			}
			else
			{
				text = "Problem contacting the server, or server error";
			}
		}
		else
		{
			text = "Operation could not be completed";
		}
		return text;
	}

	// Token: 0x040012D6 RID: 4822
	public string HttpError;

	// Token: 0x040012D7 RID: 4823
	public int HttpResponseCode;

	// Token: 0x040012D8 RID: 4824
	public string SoftErrorReason;
}
