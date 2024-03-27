using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class WindowsVoice : MonoBehaviour
{
	// Token: 0x06000ED2 RID: 3794
	[DllImport("WindowsVoice")]
	public static extern void initSpeech();

	// Token: 0x06000ED3 RID: 3795
	[DllImport("WindowsVoice")]
	public static extern void destroySpeech();

	// Token: 0x06000ED4 RID: 3796
	[DllImport("WindowsVoice")]
	public static extern void addToSpeechQueue(string s);

	// Token: 0x06000ED5 RID: 3797
	[DllImport("WindowsVoice")]
	public static extern void clearSpeechQueue();

	// Token: 0x06000ED6 RID: 3798
	[DllImport("WindowsVoice")]
	public static extern void statusMessage(StringBuilder str, int length);

	// Token: 0x06000ED7 RID: 3799 RVA: 0x000828FF File Offset: 0x00080CFF
	public void Init()
	{
		if (WindowsVoice.thisVoice == null)
		{
			WindowsVoice.thisVoice = this;
			WindowsVoice.initSpeech();
		}
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0008291C File Offset: 0x00080D1C
	public void Speak(string text, VoiceProperties properties)
	{
		text = text.Replace("<", " ");
		text = text.Replace(">", " ");
		text = Misc.Truncate(text, 10000, true);
		string text2 = string.Concat(new object[]
		{
			"<voice required=\"Gender=",
			properties.gender.ToString(),
			"\"><lang langid=\"",
			409,
			"\"><volume level=\"",
			properties.volume,
			"\"><pitch middle=\"",
			properties.pitch,
			"\"><rate speed=\"",
			properties.speed,
			"\">",
			text,
			"</rate></pitch></volume></lang></voice>"
		});
		WindowsVoice.addToSpeechQueue(text2);
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x000829F9 File Offset: 0x00080DF9
	private void OnDestroy()
	{
		if (WindowsVoice.thisVoice == this)
		{
			WindowsVoice.destroySpeech();
			WindowsVoice.thisVoice = null;
		}
	}

	// Token: 0x04000FA7 RID: 4007
	public static WindowsVoice thisVoice;
}
