using System;
using System.Collections.Generic;

// Token: 0x0200026A RID: 618
public class Sound
{
	// Token: 0x060016AB RID: 5803 RVA: 0x000CBA2C File Offset: 0x000C9E2C
	public void SetByStringData(string data)
	{
		string[] array = Misc.Split(data, "/", StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array)
		{
			string[] array3 = Misc.Split(text, ":", StringSplitOptions.RemoveEmptyEntries);
			int num2;
			if (array3.Length == 2)
			{
				string text2 = array3[0];
				string text3 = array3[1];
				switch (text2)
				{
				case "v":
					this.volume = this.GetFloatFromString(text3);
					break;
				case "p":
					this.pitch = this.GetFloatFromString(text3);
					break;
				case "n":
					this.pitchVariance = this.GetFloatFromString(text3);
					break;
				case "l":
					this.lowPass = this.GetBoolFromString(text3);
					break;
				case "h":
					this.highPass = this.GetBoolFromString(text3);
					break;
				case "s":
					this.stretch = this.GetBoolFromString(text3);
					break;
				case "e":
					this.echo = this.GetBoolFromString(text3);
					break;
				case "r":
					this.reverse = this.GetBoolFromString(text3);
					break;
				case "u":
					this.surround = this.GetBoolFromString(text3);
					break;
				case "a":
					this.repeatCount = this.GetIntFromString(text3);
					break;
				case "d":
					this.secondsDuration = this.GetFloatFromString(text3);
					break;
				case "k":
					this.secondsToSkip = this.GetFloatFromString(text3);
					break;
				case "y":
					this.secondsDelay = this.GetFloatFromString(text3);
					break;
				}
			}
			else if (int.TryParse(text, out num2))
			{
				this.name = Managers.soundLibraryManager.GetNameById(num2);
			}
		}
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x000CBCC0 File Offset: 0x000CA0C0
	private float GetFloatFromString(string s)
	{
		return Misc.GetDecompressedFloatFromString(s);
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x000CBCC8 File Offset: 0x000CA0C8
	private bool GetBoolFromString(string s)
	{
		return s == "1";
	}

	// Token: 0x060016AE RID: 5806 RVA: 0x000CBCD8 File Offset: 0x000CA0D8
	private int GetIntFromString(string s)
	{
		int num = 0;
		int num2;
		if (int.TryParse(s, out num2))
		{
			num = num2;
		}
		return num;
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x000CBCF8 File Offset: 0x000CA0F8
	public string GetStringData()
	{
		List<string> list = new List<string>();
		list.Add(Managers.soundLibraryManager.GetIdByName(this.name).ToString());
		if (this.volume != 1f)
		{
			list.Add("v:" + this.GetFloatString(this.volume));
		}
		if (this.pitch != 1f)
		{
			list.Add("p:" + this.GetFloatString(this.pitch));
		}
		if (this.pitchVariance != 0f)
		{
			list.Add("n:" + this.GetFloatString(this.pitchVariance));
		}
		if (this.lowPass)
		{
			list.Add("l:" + this.GetBoolString(this.lowPass));
		}
		if (this.highPass)
		{
			list.Add("h:" + this.GetBoolString(this.highPass));
		}
		if (this.stretch)
		{
			list.Add("s:" + this.GetBoolString(this.stretch));
		}
		if (this.echo)
		{
			list.Add("e:" + this.GetBoolString(this.echo));
		}
		if (this.reverse)
		{
			list.Add("r:" + this.GetBoolString(this.reverse));
		}
		if (this.surround)
		{
			list.Add("u:" + this.GetBoolString(this.surround));
		}
		if (this.repeatCount != 0)
		{
			list.Add("a:" + this.repeatCount);
		}
		if (this.secondsDuration != 0f)
		{
			list.Add("d:" + this.GetFloatString(this.secondsDuration));
		}
		if (this.secondsToSkip != 0f)
		{
			list.Add("k:" + this.GetFloatString(this.secondsToSkip));
		}
		if (this.secondsDelay != 0f)
		{
			list.Add("y:" + this.GetFloatString(this.secondsDelay));
		}
		return string.Join("/", list.ToArray());
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x000CBF58 File Offset: 0x000CA358
	public bool HasModulators()
	{
		return this.volume != 1f || this.pitch != 1f || this.pitchVariance != 0f || this.lowPass || this.highPass || this.stretch || this.echo || this.reverse || this.surround || this.repeatCount != 0 || this.secondsDuration != 0f || this.secondsToSkip != 0f || this.secondsDelay != 0f;
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x000CC015 File Offset: 0x000CA415
	private string GetFloatString(float value)
	{
		return Misc.GetCompressedStringFromFloat(value, 4, false);
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x000CC01F File Offset: 0x000CA41F
	private string GetBoolString(bool value)
	{
		return (!value) ? "0" : "1";
	}

	// Token: 0x04001457 RID: 5207
	public string name;

	// Token: 0x04001458 RID: 5208
	public float volume = 1f;

	// Token: 0x04001459 RID: 5209
	public float pitch = 1f;

	// Token: 0x0400145A RID: 5210
	public float pitchVariance;

	// Token: 0x0400145B RID: 5211
	public bool lowPass;

	// Token: 0x0400145C RID: 5212
	public bool highPass;

	// Token: 0x0400145D RID: 5213
	public bool stretch;

	// Token: 0x0400145E RID: 5214
	public bool echo;

	// Token: 0x0400145F RID: 5215
	public bool reverse;

	// Token: 0x04001460 RID: 5216
	public bool surround;

	// Token: 0x04001461 RID: 5217
	public int repeatCount;

	// Token: 0x04001462 RID: 5218
	public float secondsDuration;

	// Token: 0x04001463 RID: 5219
	public float secondsToSkip;

	// Token: 0x04001464 RID: 5220
	public float secondsDelay;

	// Token: 0x04001465 RID: 5221
	public float secondsToWaitAfter;
}
