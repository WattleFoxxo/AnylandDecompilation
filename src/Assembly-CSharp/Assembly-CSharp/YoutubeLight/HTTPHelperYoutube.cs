using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace YoutubeLight
{
	// Token: 0x020002D9 RID: 729
	internal static class HTTPHelperYoutube
	{
		// Token: 0x06001B04 RID: 6916 RVA: 0x000F490C File Offset: 0x000F2D0C
		public static string HtmlDecode(string value)
		{
			return value.DecodeHtmlChars();
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000F4914 File Offset: 0x000F2D14
		public static string DecodeHtmlChars(this string source)
		{
			string[] array = source.Split(new string[] { "&#x" }, StringSplitOptions.None);
			for (int i = 1; i < array.Length; i++)
			{
				int num = array[i].IndexOf(';');
				string text = array[i].Substring(0, num);
				try
				{
					int num2 = Convert.ToInt32(text, 16);
					array[i] = (char)num2 + array[i].Substring(num + 1);
				}
				catch
				{
				}
			}
			return string.Join(string.Empty, array);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000F49AC File Offset: 0x000F2DAC
		public static IDictionary<string, string> ParseQueryString(string s)
		{
			if (s.StartsWith("http") && s.Contains("?"))
			{
				s = s.Substring(s.IndexOf('?') + 1);
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string text in Regex.Split(s, "&"))
			{
				string[] array2 = Regex.Split(text, "=");
				string text2 = array2[0];
				string text3 = string.Empty;
				if (array2.Length == 2)
				{
					text3 = array2[1];
				}
				else if (array2.Length > 2)
				{
					text3 = string.Join("=", array2.Skip(1).Take(array2.Length).ToArray<string>());
				}
				dictionary.Add(text2, text3);
			}
			return dictionary;
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000F4A7C File Offset: 0x000F2E7C
		public static string ReplaceQueryStringParameter(string currentPageUrl, string paramToReplace, string newValue, string lsig)
		{
			IDictionary<string, string> dictionary = HTTPHelperYoutube.ParseQueryString(currentPageUrl);
			dictionary[paramToReplace] = newValue;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				if (!flag)
				{
					stringBuilder.Append("&");
				}
				if (keyValuePair.Key == "lsig")
				{
					if (keyValuePair.Value == string.Empty || keyValuePair.Value == string.Empty)
					{
						stringBuilder.Append(keyValuePair.Key);
						stringBuilder.Append("=");
						stringBuilder.Append(lsig);
					}
					else
					{
						stringBuilder.Append(keyValuePair.Key);
						stringBuilder.Append("=");
						stringBuilder.Append(keyValuePair.Value);
					}
				}
				else
				{
					stringBuilder.Append(keyValuePair.Key);
					stringBuilder.Append("=");
					stringBuilder.Append(keyValuePair.Value);
				}
				flag = false;
			}
			UriBuilder uriBuilder = new UriBuilder(currentPageUrl)
			{
				Query = stringBuilder.ToString()
			};
			return uriBuilder.ToString();
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000F4BD8 File Offset: 0x000F2FD8
		public static string UrlDecode(string url)
		{
			return UnityWebRequest.UnEscapeURL(url);
		}
	}
}
