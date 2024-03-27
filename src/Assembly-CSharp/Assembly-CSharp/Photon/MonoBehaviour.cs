using System;
using UnityEngine;

namespace Photon
{
	// Token: 0x02000080 RID: 128
	public class MonoBehaviour : MonoBehaviour
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00014235 File Offset: 0x00012635
		public PhotonView photonView
		{
			get
			{
				if (this.pvCache == null)
				{
					this.pvCache = PhotonView.Get(this);
				}
				return this.pvCache;
			}
		}

		// Token: 0x04000357 RID: 855
		private PhotonView pvCache;
	}
}
