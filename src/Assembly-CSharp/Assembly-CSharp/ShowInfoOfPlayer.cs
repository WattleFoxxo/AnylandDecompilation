using System;
using Photon;
using UnityEngine;

// Token: 0x020000D1 RID: 209
[RequireComponent(typeof(PhotonView))]
public class ShowInfoOfPlayer : global::Photon.MonoBehaviour
{
	// Token: 0x060006B8 RID: 1720 RVA: 0x0001F394 File Offset: 0x0001D794
	private void Start()
	{
		if (this.font == null)
		{
			this.font = (Font)Resources.FindObjectsOfTypeAll(typeof(Font))[0];
			Debug.LogWarning("No font defined. Found font: " + this.font);
		}
		if (this.tm == null)
		{
			this.textGo = new GameObject("3d text");
			this.textGo.transform.parent = base.gameObject.transform;
			this.textGo.transform.localPosition = Vector3.zero;
			MeshRenderer meshRenderer = this.textGo.AddComponent<MeshRenderer>();
			meshRenderer.material = this.font.material;
			this.tm = this.textGo.AddComponent<TextMesh>();
			this.tm.font = this.font;
			this.tm.anchor = TextAnchor.MiddleCenter;
			if (this.CharacterSize > 0f)
			{
				this.tm.characterSize = this.CharacterSize;
			}
		}
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x0001F4A0 File Offset: 0x0001D8A0
	private void Update()
	{
		bool flag = !this.DisableOnOwnObjects || base.photonView.isMine;
		if (this.textGo != null)
		{
			this.textGo.SetActive(flag);
		}
		if (!flag)
		{
			return;
		}
		PhotonPlayer owner = base.photonView.owner;
		if (owner != null)
		{
			this.tm.text = ((!string.IsNullOrEmpty(owner.NickName)) ? owner.NickName : ("player" + owner.ID));
		}
		else if (base.photonView.isSceneView)
		{
			this.tm.text = "scn";
		}
		else
		{
			this.tm.text = "n/a";
		}
	}

	// Token: 0x040004E5 RID: 1253
	private GameObject textGo;

	// Token: 0x040004E6 RID: 1254
	private TextMesh tm;

	// Token: 0x040004E7 RID: 1255
	public float CharacterSize;

	// Token: 0x040004E8 RID: 1256
	public Font font;

	// Token: 0x040004E9 RID: 1257
	public bool DisableOnOwnObjects;
}
