using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x0200002B RID: 43
	internal struct ObjectMetadata
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00006362 File Offset: 0x00004762
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00006380 File Offset: 0x00004780
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00006389 File Offset: 0x00004789
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00006391 File Offset: 0x00004791
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000639A File Offset: 0x0000479A
		// (set) Token: 0x0600017F RID: 383 RVA: 0x000063A2 File Offset: 0x000047A2
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x0400008E RID: 142
		private Type element_type;

		// Token: 0x0400008F RID: 143
		private bool is_dictionary;

		// Token: 0x04000090 RID: 144
		private IDictionary<string, PropertyMetadata> properties;
	}
}
