using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000027 RID: 39
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06000166 RID: 358 RVA: 0x000061F5 File Offset: 0x000045F5
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00006204 File Offset: 0x00004604
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00006214 File Offset: 0x00004614
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006240 File Offset: 0x00004640
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00006260 File Offset: 0x00004660
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006280 File Offset: 0x00004680
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000628D File Offset: 0x0000468D
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04000087 RID: 135
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
