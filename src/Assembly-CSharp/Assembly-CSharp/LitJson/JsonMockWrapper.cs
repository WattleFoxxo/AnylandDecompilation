using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000032 RID: 50
	public class JsonMockWrapper : IJsonWrapper, IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00007CA7 File Offset: 0x000060A7
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00007CAA File Offset: 0x000060AA
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00007CAD File Offset: 0x000060AD
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00007CB0 File Offset: 0x000060B0
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00007CB3 File Offset: 0x000060B3
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00007CB6 File Offset: 0x000060B6
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007CB9 File Offset: 0x000060B9
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007CBC File Offset: 0x000060BC
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007CBF File Offset: 0x000060BF
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00007CCA File Offset: 0x000060CA
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00007CCD File Offset: 0x000060CD
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00007CD0 File Offset: 0x000060D0
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007CD4 File Offset: 0x000060D4
		public string GetString()
		{
			return string.Empty;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007CDB File Offset: 0x000060DB
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00007CDD File Offset: 0x000060DD
		public void SetDouble(double val)
		{
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007CDF File Offset: 0x000060DF
		public void SetInt(int val)
		{
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007CE1 File Offset: 0x000060E1
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007CE3 File Offset: 0x000060E3
		public void SetLong(long val)
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007CE5 File Offset: 0x000060E5
		public void SetString(string val)
		{
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00007CE7 File Offset: 0x000060E7
		public string ToJson()
		{
			return string.Empty;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00007CEE File Offset: 0x000060EE
		public void ToJson(JsonWriter writer)
		{
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00007CF0 File Offset: 0x000060F0
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007CF3 File Offset: 0x000060F3
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000050 RID: 80
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007CFB File Offset: 0x000060FB
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007CFE File Offset: 0x000060FE
		void IList.Clear()
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007D00 File Offset: 0x00006100
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007D03 File Offset: 0x00006103
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00007D06 File Offset: 0x00006106
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007D08 File Offset: 0x00006108
		void IList.Remove(object value)
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007D0A File Offset: 0x0000610A
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00007D0C File Offset: 0x0000610C
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00007D0F File Offset: 0x0000610F
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00007D12 File Offset: 0x00006112
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007D15 File Offset: 0x00006115
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007D17 File Offset: 0x00006117
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00007D1A File Offset: 0x0000611A
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00007D1D File Offset: 0x0000611D
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00007D20 File Offset: 0x00006120
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00007D23 File Offset: 0x00006123
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000058 RID: 88
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007D2B File Offset: 0x0000612B
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007D2D File Offset: 0x0000612D
		void IDictionary.Clear()
		{
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00007D2F File Offset: 0x0000612F
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007D32 File Offset: 0x00006132
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007D34 File Offset: 0x00006134
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x17000059 RID: 89
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00007D3C File Offset: 0x0000613C
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00007D3F File Offset: 0x0000613F
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007D41 File Offset: 0x00006141
		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
