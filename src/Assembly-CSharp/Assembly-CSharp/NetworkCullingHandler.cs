using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BA RID: 186
[RequireComponent(typeof(PhotonView))]
public class NetworkCullingHandler : MonoBehaviour, IPunObservable
{
	// Token: 0x06000646 RID: 1606 RVA: 0x0001D4A8 File Offset: 0x0001B8A8
	private void OnEnable()
	{
		if (this.pView == null)
		{
			this.pView = base.GetComponent<PhotonView>();
			if (!this.pView.isMine)
			{
				return;
			}
		}
		if (this.cullArea == null)
		{
			this.cullArea = global::UnityEngine.Object.FindObjectOfType<CullArea>();
		}
		this.previousActiveCells = new List<byte>(0);
		this.activeCells = new List<byte>(0);
		this.currentPosition = (this.lastPosition = base.transform.position);
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0001D534 File Offset: 0x0001B934
	private void Start()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		if (PhotonNetwork.inRoom)
		{
			if (this.cullArea.NumberOfSubdivisions == 0)
			{
				this.pView.group = this.cullArea.FIRST_GROUP_ID;
				PhotonNetwork.SetInterestGroups(this.cullArea.FIRST_GROUP_ID, true);
			}
			else
			{
				this.pView.ObservedComponents.Add(this);
			}
		}
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0001D5AC File Offset: 0x0001B9AC
	private void Update()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		this.lastPosition = this.currentPosition;
		this.currentPosition = base.transform.position;
		if (this.currentPosition != this.lastPosition && this.HaveActiveCellsChanged())
		{
			this.UpdateInterestGroups();
		}
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0001D610 File Offset: 0x0001BA10
	private void OnGUI()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		string text = "Inside cells:\n";
		string text2 = "Subscribed cells:\n";
		for (int i = 0; i < this.activeCells.Count; i++)
		{
			if (i <= this.cullArea.NumberOfSubdivisions)
			{
				text = text + this.activeCells[i] + " | ";
			}
			text2 = text2 + this.activeCells[i] + " | ";
		}
		GUI.Label(new Rect(20f, (float)Screen.height - 120f, 200f, 40f), "<color=white>PhotonView Group: " + this.pView.group + "</color>", new GUIStyle
		{
			alignment = TextAnchor.UpperLeft,
			fontSize = 16
		});
		GUI.Label(new Rect(20f, (float)Screen.height - 100f, 200f, 40f), "<color=white>" + text + "</color>", new GUIStyle
		{
			alignment = TextAnchor.UpperLeft,
			fontSize = 16
		});
		GUI.Label(new Rect(20f, (float)Screen.height - 60f, 200f, 40f), "<color=white>" + text2 + "</color>", new GUIStyle
		{
			alignment = TextAnchor.UpperLeft,
			fontSize = 16
		});
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x0001D794 File Offset: 0x0001BB94
	private bool HaveActiveCellsChanged()
	{
		if (this.cullArea.NumberOfSubdivisions == 0)
		{
			return false;
		}
		this.previousActiveCells = new List<byte>(this.activeCells);
		this.activeCells = this.cullArea.GetActiveCells(base.transform.position);
		while (this.activeCells.Count <= this.cullArea.NumberOfSubdivisions)
		{
			this.activeCells.Add(this.cullArea.FIRST_GROUP_ID);
		}
		return this.activeCells.Count != this.previousActiveCells.Count || this.activeCells[this.cullArea.NumberOfSubdivisions] != this.previousActiveCells[this.cullArea.NumberOfSubdivisions];
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0001D868 File Offset: 0x0001BC68
	private void UpdateInterestGroups()
	{
		List<byte> list = new List<byte>(0);
		foreach (byte b in this.previousActiveCells)
		{
			if (!this.activeCells.Contains(b))
			{
				list.Add(b);
			}
		}
		PhotonNetwork.SetInterestGroups(list.ToArray(), this.activeCells.ToArray());
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0001D8F4 File Offset: 0x0001BCF4
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		while (this.activeCells.Count <= this.cullArea.NumberOfSubdivisions)
		{
			this.activeCells.Add(this.cullArea.FIRST_GROUP_ID);
		}
		if (this.cullArea.NumberOfSubdivisions == 1)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER[this.orderIndex]];
		}
		else if (this.cullArea.NumberOfSubdivisions == 2)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER[this.orderIndex]];
		}
		else if (this.cullArea.NumberOfSubdivisions == 3)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER[this.orderIndex]];
		}
	}

	// Token: 0x040004B1 RID: 1201
	private int orderIndex;

	// Token: 0x040004B2 RID: 1202
	private CullArea cullArea;

	// Token: 0x040004B3 RID: 1203
	private List<byte> previousActiveCells;

	// Token: 0x040004B4 RID: 1204
	private List<byte> activeCells;

	// Token: 0x040004B5 RID: 1205
	private PhotonView pView;

	// Token: 0x040004B6 RID: 1206
	private Vector3 lastPosition;

	// Token: 0x040004B7 RID: 1207
	private Vector3 currentPosition;
}
