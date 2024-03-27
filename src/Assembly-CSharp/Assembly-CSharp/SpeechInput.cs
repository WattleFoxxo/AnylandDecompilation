using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Windows.Speech;

// Token: 0x02000250 RID: 592
public static class SpeechInput
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06001608 RID: 5640 RVA: 0x000C108C File Offset: 0x000BF48C
	// (remove) Token: 0x06001609 RID: 5641 RVA: 0x000C10C0 File Offset: 0x000BF4C0
	[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event SpeechInput.SpeechRecognizedHandler SpeechRecognizedEvent;

	// Token: 0x0600160A RID: 5642 RVA: 0x000C10F4 File Offset: 0x000BF4F4
	public static void InitListener(string[] words, SpeechInput.SpeechRecognizedHandler handler)
	{
		if (words.Length == 0 || SpeechInput.AlreadyActiveWithSameWords(words))
		{
			return;
		}
		SpeechInput.DisposeListener();
		SpeechInput.SpeechRecognizedEvent += handler;
		if (SpeechInput.keywordRecognizer == null)
		{
			try
			{
				SpeechInput.activeWords = words;
				SpeechInput.keywordRecognizer = new KeywordRecognizer(words, ConfidenceLevel.Low);
				SpeechInput.keywordRecognizer.OnPhraseRecognized += SpeechInput.OnPhraseRecognized;
				SpeechInput.keywordRecognizer.Start();
			}
			catch (Exception ex)
			{
				Log.Debug(ex.Message);
				SpeechInput.keywordRecognizer = null;
			}
		}
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x000C119C File Offset: 0x000BF59C
	public static void DisposeListener()
	{
		SpeechInput.SpeechRecognizedEvent = null;
		if (SpeechInput.keywordRecognizer != null)
		{
			SpeechInput.keywordRecognizer.Stop();
			SpeechInput.keywordRecognizer.Dispose();
			SpeechInput.keywordRecognizer = null;
		}
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x000C11C8 File Offset: 0x000BF5C8
	private static void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		SpeechRecognizedEventArgs speechRecognizedEventArgs = new SpeechRecognizedEventArgs(args.text);
		SpeechInput.SpeechRecognizedEvent(speechRecognizedEventArgs);
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x000C11ED File Offset: 0x000BF5ED
	private static bool AlreadyActiveWithSameWords(string[] words)
	{
		return SpeechInput.keywordRecognizer != null && SpeechInput.keywordRecognizer.IsRunning && SpeechInput.activeWords.Length == words.Length && SpeechInput.activeWords.SequenceEqual(words);
	}

	// Token: 0x040013CC RID: 5068
	private static KeywordRecognizer keywordRecognizer;

	// Token: 0x040013CD RID: 5069
	private static string[] activeWords;

	// Token: 0x02000251 RID: 593
	// (Invoke) Token: 0x0600160F RID: 5647
	public delegate void SpeechRecognizedHandler(SpeechRecognizedEventArgs e);
}
