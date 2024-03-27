using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class PhotonStreamQueue
{
	// Token: 0x06000539 RID: 1337 RVA: 0x00017F4F File Offset: 0x0001634F
	public PhotonStreamQueue(int sampleRate)
	{
		this.m_SampleRate = sampleRate;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00017F8C File Offset: 0x0001638C
	private void BeginWritePackage()
	{
		if (Time.realtimeSinceStartup < this.m_LastSampleTime + 1f / (float)this.m_SampleRate)
		{
			this.m_IsWriting = false;
			return;
		}
		if (this.m_SampleCount == 1)
		{
			this.m_ObjectsPerSample = this.m_Objects.Count;
		}
		else if (this.m_SampleCount > 1 && this.m_Objects.Count / this.m_SampleCount != this.m_ObjectsPerSample)
		{
			Debug.LogWarning("The number of objects sent via a PhotonStreamQueue has to be the same each frame");
			Debug.LogWarning(string.Concat(new object[]
			{
				"Objects in List: ",
				this.m_Objects.Count,
				" / Sample Count: ",
				this.m_SampleCount,
				" = ",
				this.m_Objects.Count / this.m_SampleCount,
				" != ",
				this.m_ObjectsPerSample
			}));
		}
		this.m_IsWriting = true;
		this.m_SampleCount++;
		this.m_LastSampleTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x000180AD File Offset: 0x000164AD
	public void Reset()
	{
		this.m_SampleCount = 0;
		this.m_ObjectsPerSample = -1;
		this.m_LastSampleTime = float.NegativeInfinity;
		this.m_LastFrameCount = -1;
		this.m_Objects.Clear();
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x000180DA File Offset: 0x000164DA
	public void SendNext(object obj)
	{
		if (Time.frameCount != this.m_LastFrameCount)
		{
			this.BeginWritePackage();
		}
		this.m_LastFrameCount = Time.frameCount;
		if (!this.m_IsWriting)
		{
			return;
		}
		this.m_Objects.Add(obj);
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00018115 File Offset: 0x00016515
	public bool HasQueuedObjects()
	{
		return this.m_NextObjectIndex != -1;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00018124 File Offset: 0x00016524
	public object ReceiveNext()
	{
		if (this.m_NextObjectIndex == -1)
		{
			return null;
		}
		if (this.m_NextObjectIndex >= this.m_Objects.Count)
		{
			this.m_NextObjectIndex -= this.m_ObjectsPerSample;
		}
		return this.m_Objects[this.m_NextObjectIndex++];
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00018184 File Offset: 0x00016584
	public void Serialize(PhotonStream stream)
	{
		if (this.m_Objects.Count > 0 && this.m_ObjectsPerSample < 0)
		{
			this.m_ObjectsPerSample = this.m_Objects.Count;
		}
		stream.SendNext(this.m_SampleCount);
		stream.SendNext(this.m_ObjectsPerSample);
		for (int i = 0; i < this.m_Objects.Count; i++)
		{
			stream.SendNext(this.m_Objects[i]);
		}
		this.m_Objects.Clear();
		this.m_SampleCount = 0;
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00018224 File Offset: 0x00016624
	public void Deserialize(PhotonStream stream)
	{
		this.m_Objects.Clear();
		this.m_SampleCount = (int)stream.ReceiveNext();
		this.m_ObjectsPerSample = (int)stream.ReceiveNext();
		for (int i = 0; i < this.m_SampleCount * this.m_ObjectsPerSample; i++)
		{
			this.m_Objects.Add(stream.ReceiveNext());
		}
		if (this.m_Objects.Count > 0)
		{
			this.m_NextObjectIndex = 0;
		}
		else
		{
			this.m_NextObjectIndex = -1;
		}
	}

	// Token: 0x040003AF RID: 943
	private int m_SampleRate;

	// Token: 0x040003B0 RID: 944
	private int m_SampleCount;

	// Token: 0x040003B1 RID: 945
	private int m_ObjectsPerSample = -1;

	// Token: 0x040003B2 RID: 946
	private float m_LastSampleTime = float.NegativeInfinity;

	// Token: 0x040003B3 RID: 947
	private int m_LastFrameCount = -1;

	// Token: 0x040003B4 RID: 948
	private int m_NextObjectIndex = -1;

	// Token: 0x040003B5 RID: 949
	private List<object> m_Objects = new List<object>();

	// Token: 0x040003B6 RID: 950
	private bool m_IsWriting;
}
