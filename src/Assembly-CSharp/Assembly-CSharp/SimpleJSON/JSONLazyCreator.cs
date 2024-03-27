using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020001D0 RID: 464
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06000E81 RID: 3713 RVA: 0x00080A0D File Offset: 0x0007EE0D
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00080A23 File Offset: 0x0007EE23
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00080A39 File Offset: 0x0007EE39
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.None;
			}
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00080A3C File Offset: 0x0007EE3C
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x170001A1 RID: 417
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray { value });
			}
		}

		// Token: 0x170001A2 RID: 418
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONObject { { aKey, value } });
			}
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00080ACC File Offset: 0x0007EECC
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray { aItem });
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00080AF0 File Offset: 0x0007EEF0
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONObject { { aKey, aItem } });
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00080B12 File Offset: 0x0007EF12
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || object.ReferenceEquals(a, b);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00080B23 File Offset: 0x0007EF23
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00080B2F File Offset: 0x0007EF2F
		public override bool Equals(object obj)
		{
			return obj == null || object.ReferenceEquals(this, obj);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00080B40 File Offset: 0x0007EF40
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x00080B44 File Offset: 0x0007EF44
		// (set) Token: 0x06000E90 RID: 3728 RVA: 0x00080B68 File Offset: 0x0007EF68
		public override int AsInt
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber((double)value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00080B84 File Offset: 0x0007EF84
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x00080BAC File Offset: 0x0007EFAC
		public override float AsFloat
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0f;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber((double)value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x00080BC8 File Offset: 0x0007EFC8
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x00080BF4 File Offset: 0x0007EFF4
		public override double AsDouble
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0.0;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber(value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00080C10 File Offset: 0x0007F010
		// (set) Token: 0x06000E96 RID: 3734 RVA: 0x00080C2C File Offset: 0x0007F02C
		public override bool AsBool
		{
			get
			{
				JSONBool jsonbool = new JSONBool(false);
				this.Set(jsonbool);
				return false;
			}
			set
			{
				JSONBool jsonbool = new JSONBool(value);
				this.Set(jsonbool);
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00080C48 File Offset: 0x0007F048
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x00080C64 File Offset: 0x0007F064
		public override JSONObject AsObject
		{
			get
			{
				JSONObject jsonobject = new JSONObject();
				this.Set(jsonobject);
				return jsonobject;
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00080C7F File Offset: 0x0007F07F
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000F6E RID: 3950
		private JSONNode m_Node;

		// Token: 0x04000F6F RID: 3951
		private string m_Key;
	}
}
