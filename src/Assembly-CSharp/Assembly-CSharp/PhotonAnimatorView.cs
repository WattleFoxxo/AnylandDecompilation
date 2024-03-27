using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009C RID: 156
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("Photon Networking/Photon Animator View")]
public class PhotonAnimatorView : MonoBehaviour, IPunObservable
{
	// Token: 0x060005C6 RID: 1478 RVA: 0x0001A556 File Offset: 0x00018956
	private void Awake()
	{
		this.m_PhotonView = base.GetComponent<PhotonView>();
		this.m_StreamQueue = new PhotonStreamQueue(120);
		this.m_Animator = base.GetComponent<Animator>();
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0001A580 File Offset: 0x00018980
	private void Update()
	{
		if (this.m_Animator.applyRootMotion && !this.m_PhotonView.isMine && PhotonNetwork.connected)
		{
			this.m_Animator.applyRootMotion = false;
		}
		if (!PhotonNetwork.inRoom || PhotonNetwork.room.PlayerCount <= 1)
		{
			this.m_StreamQueue.Reset();
			return;
		}
		if (this.m_PhotonView.isMine)
		{
			this.SerializeDataContinuously();
			this.CacheDiscreteTriggers();
		}
		else
		{
			this.DeserializeDataContinuously();
		}
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0001A610 File Offset: 0x00018A10
	public void CacheDiscreteTriggers()
	{
		for (int i = 0; i < this.m_SynchronizeParameters.Count; i++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[i];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete && synchronizedParameter.Type == PhotonAnimatorView.ParameterType.Trigger && this.m_Animator.GetBool(synchronizedParameter.Name) && synchronizedParameter.Type == PhotonAnimatorView.ParameterType.Trigger)
			{
				this.m_raisedDiscreteTriggersCache.Add(synchronizedParameter.Name);
				break;
			}
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0001A698 File Offset: 0x00018A98
	public bool DoesLayerSynchronizeTypeExist(int layerIndex)
	{
		return this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex) != -1;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0001A6D0 File Offset: 0x00018AD0
	public bool DoesParameterSynchronizeTypeExist(string name)
	{
		return this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name) != -1;
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0001A707 File Offset: 0x00018B07
	public List<PhotonAnimatorView.SynchronizedLayer> GetSynchronizedLayers()
	{
		return this.m_SynchronizeLayers;
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0001A70F File Offset: 0x00018B0F
	public List<PhotonAnimatorView.SynchronizedParameter> GetSynchronizedParameters()
	{
		return this.m_SynchronizeParameters;
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0001A718 File Offset: 0x00018B18
	public PhotonAnimatorView.SynchronizeType GetLayerSynchronizeType(int layerIndex)
	{
		int num = this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex);
		if (num == -1)
		{
			return PhotonAnimatorView.SynchronizeType.Disabled;
		}
		return this.m_SynchronizeLayers[num].SynchronizeType;
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0001A764 File Offset: 0x00018B64
	public PhotonAnimatorView.SynchronizeType GetParameterSynchronizeType(string name)
	{
		int num = this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name);
		if (num == -1)
		{
			return PhotonAnimatorView.SynchronizeType.Disabled;
		}
		return this.m_SynchronizeParameters[num].SynchronizeType;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0001A7B0 File Offset: 0x00018BB0
	public void SetLayerSynchronized(int layerIndex, PhotonAnimatorView.SynchronizeType synchronizeType)
	{
		if (Application.isPlaying)
		{
			this.m_WasSynchronizeTypeChanged = true;
		}
		int num = this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex);
		if (num == -1)
		{
			this.m_SynchronizeLayers.Add(new PhotonAnimatorView.SynchronizedLayer
			{
				LayerIndex = layerIndex,
				SynchronizeType = synchronizeType
			});
		}
		else
		{
			this.m_SynchronizeLayers[num].SynchronizeType = synchronizeType;
		}
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0001A838 File Offset: 0x00018C38
	public void SetParameterSynchronized(string name, PhotonAnimatorView.ParameterType type, PhotonAnimatorView.SynchronizeType synchronizeType)
	{
		if (Application.isPlaying)
		{
			this.m_WasSynchronizeTypeChanged = true;
		}
		int num = this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name);
		if (num == -1)
		{
			this.m_SynchronizeParameters.Add(new PhotonAnimatorView.SynchronizedParameter
			{
				Name = name,
				Type = type,
				SynchronizeType = synchronizeType
			});
		}
		else
		{
			this.m_SynchronizeParameters[num].SynchronizeType = synchronizeType;
		}
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0001A8C8 File Offset: 0x00018CC8
	private void SerializeDataContinuously()
	{
		if (this.m_Animator == null)
		{
			return;
		}
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				this.m_StreamQueue.SendNext(this.m_Animator.GetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex));
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					this.m_StreamQueue.SendNext(this.m_Animator.GetFloat(synchronizedParameter.Name));
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						this.m_StreamQueue.SendNext(this.m_Animator.GetBool(synchronizedParameter.Name));
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					this.m_StreamQueue.SendNext(this.m_Animator.GetInteger(synchronizedParameter.Name));
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					this.m_StreamQueue.SendNext(this.m_Animator.GetBool(synchronizedParameter.Name));
					break;
				}
			}
		}
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0001AA40 File Offset: 0x00018E40
	private void DeserializeDataContinuously()
	{
		if (!this.m_StreamQueue.HasQueuedObjects())
		{
			return;
		}
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				this.m_Animator.SetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex, (float)this.m_StreamQueue.ReceiveNext());
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					this.m_Animator.SetFloat(synchronizedParameter.Name, (float)this.m_StreamQueue.ReceiveNext());
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						this.m_Animator.SetBool(synchronizedParameter.Name, (bool)this.m_StreamQueue.ReceiveNext());
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					this.m_Animator.SetInteger(synchronizedParameter.Name, (int)this.m_StreamQueue.ReceiveNext());
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					this.m_Animator.SetBool(synchronizedParameter.Name, (bool)this.m_StreamQueue.ReceiveNext());
					break;
				}
			}
		}
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0001ABB8 File Offset: 0x00018FB8
	private void SerializeDataDiscretly(PhotonStream stream)
	{
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				stream.SendNext(this.m_Animator.GetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex));
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					stream.SendNext(this.m_Animator.GetFloat(synchronizedParameter.Name));
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						stream.SendNext(this.m_raisedDiscreteTriggersCache.Contains(synchronizedParameter.Name));
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					stream.SendNext(this.m_Animator.GetInteger(synchronizedParameter.Name));
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					stream.SendNext(this.m_Animator.GetBool(synchronizedParameter.Name));
					break;
				}
			}
		}
		this.m_raisedDiscreteTriggersCache.Clear();
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0001AD10 File Offset: 0x00019110
	private void DeserializeDataDiscretly(PhotonStream stream)
	{
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				this.m_Animator.SetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex, (float)stream.ReceiveNext());
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					if (!(stream.PeekNext() is float))
					{
						return;
					}
					this.m_Animator.SetFloat(synchronizedParameter.Name, (float)stream.ReceiveNext());
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						if (!(stream.PeekNext() is bool))
						{
							return;
						}
						if ((bool)stream.ReceiveNext())
						{
							this.m_Animator.SetTrigger(synchronizedParameter.Name);
						}
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					if (!(stream.PeekNext() is int))
					{
						return;
					}
					this.m_Animator.SetInteger(synchronizedParameter.Name, (int)stream.ReceiveNext());
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					if (!(stream.PeekNext() is bool))
					{
						return;
					}
					this.m_Animator.SetBool(synchronizedParameter.Name, (bool)stream.ReceiveNext());
					break;
				}
			}
		}
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0001AEA8 File Offset: 0x000192A8
	private void SerializeSynchronizationTypeState(PhotonStream stream)
	{
		byte[] array = new byte[this.m_SynchronizeLayers.Count + this.m_SynchronizeParameters.Count];
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			array[i] = (byte)this.m_SynchronizeLayers[i].SynchronizeType;
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			array[this.m_SynchronizeLayers.Count + j] = (byte)this.m_SynchronizeParameters[j].SynchronizeType;
		}
		stream.SendNext(array);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0001AF48 File Offset: 0x00019348
	private void DeserializeSynchronizationTypeState(PhotonStream stream)
	{
		byte[] array = (byte[])stream.ReceiveNext();
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			this.m_SynchronizeLayers[i].SynchronizeType = (PhotonAnimatorView.SynchronizeType)array[i];
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			this.m_SynchronizeParameters[j].SynchronizeType = (PhotonAnimatorView.SynchronizeType)array[this.m_SynchronizeLayers.Count + j];
		}
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0001AFD0 File Offset: 0x000193D0
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.m_Animator == null)
		{
			return;
		}
		if (stream.isWriting)
		{
			if (this.m_WasSynchronizeTypeChanged)
			{
				this.m_StreamQueue.Reset();
				this.SerializeSynchronizationTypeState(stream);
				this.m_WasSynchronizeTypeChanged = false;
			}
			this.m_StreamQueue.Serialize(stream);
			this.SerializeDataDiscretly(stream);
		}
		else
		{
			if (stream.PeekNext() is byte[])
			{
				this.DeserializeSynchronizationTypeState(stream);
			}
			this.m_StreamQueue.Deserialize(stream);
			this.DeserializeDataDiscretly(stream);
		}
	}

	// Token: 0x04000412 RID: 1042
	private Animator m_Animator;

	// Token: 0x04000413 RID: 1043
	private PhotonStreamQueue m_StreamQueue;

	// Token: 0x04000414 RID: 1044
	[HideInInspector]
	[SerializeField]
	private bool ShowLayerWeightsInspector = true;

	// Token: 0x04000415 RID: 1045
	[HideInInspector]
	[SerializeField]
	private bool ShowParameterInspector = true;

	// Token: 0x04000416 RID: 1046
	[HideInInspector]
	[SerializeField]
	private List<PhotonAnimatorView.SynchronizedParameter> m_SynchronizeParameters = new List<PhotonAnimatorView.SynchronizedParameter>();

	// Token: 0x04000417 RID: 1047
	[HideInInspector]
	[SerializeField]
	private List<PhotonAnimatorView.SynchronizedLayer> m_SynchronizeLayers = new List<PhotonAnimatorView.SynchronizedLayer>();

	// Token: 0x04000418 RID: 1048
	private Vector3 m_ReceiverPosition;

	// Token: 0x04000419 RID: 1049
	private float m_LastDeserializeTime;

	// Token: 0x0400041A RID: 1050
	private bool m_WasSynchronizeTypeChanged = true;

	// Token: 0x0400041B RID: 1051
	private PhotonView m_PhotonView;

	// Token: 0x0400041C RID: 1052
	private List<string> m_raisedDiscreteTriggersCache = new List<string>();

	// Token: 0x0200009D RID: 157
	public enum ParameterType
	{
		// Token: 0x0400041E RID: 1054
		Float = 1,
		// Token: 0x0400041F RID: 1055
		Int = 3,
		// Token: 0x04000420 RID: 1056
		Bool,
		// Token: 0x04000421 RID: 1057
		Trigger = 9
	}

	// Token: 0x0200009E RID: 158
	public enum SynchronizeType
	{
		// Token: 0x04000423 RID: 1059
		Disabled,
		// Token: 0x04000424 RID: 1060
		Discrete,
		// Token: 0x04000425 RID: 1061
		Continuous
	}

	// Token: 0x0200009F RID: 159
	[Serializable]
	public class SynchronizedParameter
	{
		// Token: 0x04000426 RID: 1062
		public PhotonAnimatorView.ParameterType Type;

		// Token: 0x04000427 RID: 1063
		public PhotonAnimatorView.SynchronizeType SynchronizeType;

		// Token: 0x04000428 RID: 1064
		public string Name;
	}

	// Token: 0x020000A0 RID: 160
	[Serializable]
	public class SynchronizedLayer
	{
		// Token: 0x04000429 RID: 1065
		public PhotonAnimatorView.SynchronizeType SynchronizeType;

		// Token: 0x0400042A RID: 1066
		public int LayerIndex;
	}
}
