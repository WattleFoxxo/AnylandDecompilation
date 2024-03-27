using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class LogDisplay : MonoBehaviour
{
	// Token: 0x060007C8 RID: 1992 RVA: 0x0002B1B0 File Offset: 0x000295B0
	private void Start()
	{
		base.transform.localPosition = new Vector3(0f, 0f, 0.5f);
		base.transform.localRotation = Quaternion.identity;
		base.gameObject.name = Misc.RemoveCloneFromName(base.gameObject.name);
		this.textMesh = base.GetComponent<TextMesh>();
		this.SetText("(no errors yet)");
		Application.logMessageReceived += this.LogMessageWasAdded;
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0002B22F File Offset: 0x0002962F
	private void OnDestroy()
	{
		Application.logMessageReceived -= this.LogMessageWasAdded;
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0002B244 File Offset: 0x00029644
	private void LogMessageWasAdded(string logString, string stackTrace, LogType type)
	{
		if ((type == LogType.Exception || type == LogType.Error) && !this.ContainsIgnoredString(logString))
		{
			string text = Environment.NewLine + Environment.NewLine;
			string text2 = string.Empty;
			string text3 = text2;
			text2 = string.Concat(new object[]
			{
				text3,
				"[",
				type,
				" at ",
				DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"),
				"ms]",
				text
			});
			text2 = text2 + logString + text;
			text2 += stackTrace;
			GUIUtility.systemCopyBuffer = text2;
			text2 = text2 + text + "(copied to clipboard)";
			this.SetText(text2);
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0002B2F7 File Offset: 0x000296F7
	private void SetText(string text)
	{
		this.textMesh.text = text;
		this.textMesh.alignment = TextAlignment.Center;
		this.textMesh.anchor = TextAnchor.MiddleCenter;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0002B320 File Offset: 0x00029720
	private bool ContainsIgnoredString(string stringToCheck)
	{
		bool flag = false;
		foreach (string text in this.ignoreIfContains)
		{
			if (stringToCheck.Contains(text))
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x040005BA RID: 1466
	private TextMesh textMesh;

	// Token: 0x040005BB RID: 1467
	private string[] ignoreIfContains = new string[] { "Non-convex MeshCollider with non-kinematic Rigidbody is no longer supported", "Triggers on concave MeshColliders are not supported" };
}
