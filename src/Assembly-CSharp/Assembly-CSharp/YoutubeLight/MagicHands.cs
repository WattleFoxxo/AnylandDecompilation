using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YoutubeLight
{
	// Token: 0x020002DA RID: 730
	internal static class MagicHands
	{
		// Token: 0x06001B09 RID: 6921 RVA: 0x000F4BE0 File Offset: 0x000F2FE0
		private static string ApplyOperation(string cipher, string op)
		{
			switch (op[0])
			{
			case 'r':
				return new string(cipher.ToCharArray().Reverse<char>().ToArray<char>());
			case 's':
			{
				int opIndex = MagicHands.GetOpIndex(op);
				return cipher.Substring(opIndex);
			}
			case 'w':
			{
				int opIndex2 = MagicHands.GetOpIndex(op);
				return MagicHands.SwapFirstChar(cipher, opIndex2);
			}
			}
			throw new NotImplementedException("Couldn't find cipher operation.");
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000F4C59 File Offset: 0x000F3059
		public static string DecipherWithOperations(string cipher, string operations)
		{
			return operations.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Aggregate(cipher, new Func<string, string, string>(MagicHands.ApplyOperation));
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000F4C94 File Offset: 0x000F3094
		private static string GetFunctionFromLine(string currentLine)
		{
			Regex regex = new Regex("\\w+\\.(?<functionID>\\w+)\\(");
			Match match = regex.Match(currentLine);
			return match.Groups["functionID"].Value;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000F4CCC File Offset: 0x000F30CC
		private static int GetOpIndex(string op)
		{
			string text = new Regex(".(\\d+)").Match(op).Result("$1");
			return int.Parse(text);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000F4CFC File Offset: 0x000F30FC
		private static string SwapFirstChar(string cipher, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(cipher);
			stringBuilder[0] = cipher[index];
			stringBuilder[index] = cipher[0];
			return stringBuilder.ToString();
		}
	}
}
