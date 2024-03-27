using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001DF RID: 479
public static class Log
{
	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0008376C File Offset: 0x00081B6C
	// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x00083773 File Offset: 0x00081B73
	public static bool showVerboseMessages { get; set; } = DebugHelper.VerboseLogging;

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0008377B File Offset: 0x00081B7B
	public static void Debug(object message = null)
	{
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0008377D File Offset: 0x00081B7D
	public static void Info(string message, bool emphasize = false)
	{
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x0008377F File Offset: 0x00081B7F
	public static void InfoEmphasized(string message)
	{
		Log.Info(message, true);
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x00083788 File Offset: 0x00081B88
	public static void Warning(string message)
	{
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0008378A File Offset: 0x00081B8A
	public static void Error(string message)
	{
		global::UnityEngine.Debug.LogError(message);
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x00083792 File Offset: 0x00081B92
	public static void Error(string message, global::UnityEngine.Object context)
	{
		global::UnityEngine.Debug.LogError(message);
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0008379A File Offset: 0x00081B9A
	public static void Error(Exception e)
	{
		global::UnityEngine.Debug.LogError(e);
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x000837A2 File Offset: 0x00081BA2
	public static void StartPerf(string key)
	{
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x000837A4 File Offset: 0x00081BA4
	public static void EndPerf(string key)
	{
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x000837A8 File Offset: 0x00081BA8
	private static void registerMessageHash(string message)
	{
		int hashCode = message.GetHashCode();
		Log.remotelyLoggedMessageHashes.Add(hashCode);
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x000837C8 File Offset: 0x00081BC8
	private static bool wasSentBefore(string message)
	{
		int hashCode = message.GetHashCode();
		return Log.remotelyLoggedMessageHashes.Contains(hashCode);
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x000837E7 File Offset: 0x00081BE7
	private static bool maxRemoteMessagesReached()
	{
		return Log.nubmerOfRemoveLogsSent >= Log.MAX_REMOTE_LOGS;
	}

	// Token: 0x04000FC1 RID: 4033
	private static List<int> remotelyLoggedMessageHashes;

	// Token: 0x04000FC2 RID: 4034
	private static int nubmerOfRemoveLogsSent;

	// Token: 0x04000FC3 RID: 4035
	private static readonly int MAX_REMOTE_LOGS = 10;

	// Token: 0x04000FC4 RID: 4036
	private static bool logRemotely = true;

	// Token: 0x04000FC5 RID: 4037
	private static Dictionary<string, DateTime> perfStartTimes = new Dictionary<string, DateTime>();

	// Token: 0x04000FC6 RID: 4038
	private static DateTime sessionStartTime = DateTime.Now;
}
