using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class JsonHelper
{
	// Token: 0x06000D77 RID: 3447 RVA: 0x0007846C File Offset: 0x0007686C
	public static string GetJson(Vector3 v)
	{
		return string.Concat(new object[] { "[", v.x, ",", v.y, ",", v.z, "]" });
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x000784D4 File Offset: 0x000768D4
	public static string GetJsonNoBrackets(Vector3 v)
	{
		return string.Concat(new object[] { v.x, ",", v.y, ",", v.z });
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x00078529 File Offset: 0x00076929
	public static string GetJson(string s)
	{
		return "\"" + JsonHelper.Escape(s) + "\"";
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x00078540 File Offset: 0x00076940
	public static string GetJson(bool b)
	{
		return (!b) ? "false" : "true";
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x00078558 File Offset: 0x00076958
	public static string GetJson(Color v)
	{
		return string.Concat(new object[] { "[", v.r, ",", v.g, ",", v.b, "]" });
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x000785C0 File Offset: 0x000769C0
	public static string GetJson(BoolVector3 v)
	{
		return string.Concat(new string[]
		{
			"[",
			JsonHelper.BoolStringNumber(v.x),
			",",
			JsonHelper.BoolStringNumber(v.y),
			",",
			JsonHelper.BoolStringNumber(v.z),
			"]"
		});
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00078622 File Offset: 0x00076A22
	private static string BoolStringNumber(bool b)
	{
		return (!b) ? "0" : "1";
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x00078639 File Offset: 0x00076A39
	public static Vector3 GetVector3(JSONNode node)
	{
		return new Vector3(node[0].AsFloat, node[1].AsFloat, node[2].AsFloat);
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x00078664 File Offset: 0x00076A64
	public static Color GetColor(JSONNode node)
	{
		return new Color(node[0].AsFloat, node[1].AsFloat, node[2].AsFloat);
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x0007868F File Offset: 0x00076A8F
	public static BoolVector3 GetBoolVector3(JSONNode node)
	{
		return new BoolVector3(node[0].AsInt == 1, node[1].AsInt == 1, node[2].AsInt == 1);
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x000786C4 File Offset: 0x00076AC4
	public static string GetStringDictionaryAsArray(Dictionary<string, string> dictionary)
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, string> keyValuePair in dictionary)
		{
			if (text != string.Empty)
			{
				text += ",";
			}
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"[",
				JsonHelper.GetJson(keyValuePair.Key),
				",",
				JsonHelper.GetJson(keyValuePair.Value),
				"]"
			});
		}
		return text;
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x00078780 File Offset: 0x00076B80
	private static string Escape(string s)
	{
		if (s != null)
		{
			s = s.Replace(Environment.NewLine, "[newline]");
			s = s.Replace("\\", string.Empty);
			s = s.Replace("\"", "\\\"");
			s = s.Replace("[newline]", "\\n");
		}
		return s;
	}
}
