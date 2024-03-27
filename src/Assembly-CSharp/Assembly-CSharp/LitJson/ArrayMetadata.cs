using System;

namespace LitJson
{
	// Token: 0x0200002A RID: 42
	internal struct ArrayMetadata
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00006319 File Offset: 0x00004719
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00006337 File Offset: 0x00004737
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006340 File Offset: 0x00004740
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00006348 File Offset: 0x00004748
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00006351 File Offset: 0x00004751
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00006359 File Offset: 0x00004759
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x0400008B RID: 139
		private Type element_type;

		// Token: 0x0400008C RID: 140
		private bool is_array;

		// Token: 0x0400008D RID: 141
		private bool is_list;
	}
}
