using System;
using System.Collections.Generic;
using System.Reflection;
using Photon;
using UnityEngine;

// Token: 0x02000092 RID: 146
[AddComponentMenu("Photon Networking/Photon View &v")]
public class PhotonView : global::Photon.MonoBehaviour
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000542 RID: 1346 RVA: 0x000182E1 File Offset: 0x000166E1
	// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001830F File Offset: 0x0001670F
	public int prefix
	{
		get
		{
			if (this.prefixBackup == -1 && PhotonNetwork.networkingPeer != null)
			{
				this.prefixBackup = (int)PhotonNetwork.networkingPeer.currentLevelPrefix;
			}
			return this.prefixBackup;
		}
		set
		{
			this.prefixBackup = value;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x00018318 File Offset: 0x00016718
	// (set) Token: 0x06000545 RID: 1349 RVA: 0x00018341 File Offset: 0x00016741
	public object[] instantiationData
	{
		get
		{
			if (!this.didAwake)
			{
				this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
			}
			return this.instantiationDataField;
		}
		set
		{
			this.instantiationDataField = value;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001834A File Offset: 0x0001674A
	// (set) Token: 0x06000547 RID: 1351 RVA: 0x00018354 File Offset: 0x00016754
	public int viewID
	{
		get
		{
			return this.viewIdField;
		}
		set
		{
			bool flag = this.didAwake && this.viewIdField == 0;
			this.ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
			this.viewIdField = value;
			if (flag)
			{
				PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			}
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001839E File Offset: 0x0001679E
	public bool isSceneView
	{
		get
		{
			return this.CreatorActorNr == 0;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000549 RID: 1353 RVA: 0x000183A9 File Offset: 0x000167A9
	public PhotonPlayer owner
	{
		get
		{
			return PhotonPlayer.Find(this.ownerId);
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x0600054A RID: 1354 RVA: 0x000183B6 File Offset: 0x000167B6
	public int OwnerActorNr
	{
		get
		{
			return this.ownerId;
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x0600054B RID: 1355 RVA: 0x000183BE File Offset: 0x000167BE
	public bool isOwnerActive
	{
		get
		{
			return this.ownerId != 0 && PhotonNetwork.networkingPeer.mActors.ContainsKey(this.ownerId);
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x0600054C RID: 1356 RVA: 0x000183E3 File Offset: 0x000167E3
	public int CreatorActorNr
	{
		get
		{
			return this.viewIdField / PhotonNetwork.MAX_VIEW_IDS;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x0600054D RID: 1357 RVA: 0x000183F1 File Offset: 0x000167F1
	public bool isMine
	{
		get
		{
			return this.ownerId == PhotonNetwork.player.ID || (!this.isOwnerActive && PhotonNetwork.isMasterClient);
		}
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0001841E File Offset: 0x0001681E
	protected internal void Awake()
	{
		if (this.viewID != 0)
		{
			PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
		}
		this.didAwake = true;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00018453 File Offset: 0x00016853
	public void RequestOwnership()
	{
		PhotonNetwork.networkingPeer.RequestOwnership(this.viewID, this.ownerId);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0001846B File Offset: 0x0001686B
	public void TransferOwnership(PhotonPlayer newOwner)
	{
		this.TransferOwnership(newOwner.ID);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00018479 File Offset: 0x00016879
	public void TransferOwnership(int newOwnerId)
	{
		PhotonNetwork.networkingPeer.TransferOwnership(this.viewID, newOwnerId);
		this.ownerId = newOwnerId;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00018494 File Offset: 0x00016894
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (this.CreatorActorNr == 0 && !this.OwnerShipWasTransfered && (this.currentMasterID == -1 || this.ownerId == this.currentMasterID))
		{
			this.ownerId = newMasterClient.ID;
		}
		this.currentMasterID = newMasterClient.ID;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x000184EC File Offset: 0x000168EC
	protected internal void OnDestroy()
	{
		if (!this.removedFromLocalViewList)
		{
			bool flag = PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
			bool flag2 = false;
			if (flag && !flag2 && this.instantiationId > 0 && !PhotonHandler.AppQuits && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("PUN-instantiated '" + base.gameObject.name + "' got destroyed by engine. This is OK when loading levels. Otherwise use: PhotonNetwork.Destroy().");
			}
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00018560 File Offset: 0x00016960
	public void SerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.SerializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x000185BC File Offset: 0x000169BC
	public void DeserializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.DeserializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00018618 File Offset: 0x00016A18
	protected internal void DeserializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is global::UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transform = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
			case OnSerializeTransform.OnlyPosition:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeTransform.OnlyRotation:
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				break;
			case OnSerializeTransform.OnlyScale:
				transform.localScale = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeTransform.PositionAndRotation:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				break;
			case OnSerializeTransform.All:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				transform.localScale = (Vector3)stream.ReceiveNext();
				break;
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			OnSerializeRigidBody onSerializeRigidBody = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody == OnSerializeRigidBody.OnlyVelocity)
					{
						rigidbody.velocity = (Vector3)stream.ReceiveNext();
					}
				}
				else
				{
					rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
				}
			}
			else
			{
				rigidbody.velocity = (Vector3)stream.ReceiveNext();
				rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
			}
		}
		else if (component is Rigidbody2D)
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			OnSerializeRigidBody onSerializeRigidBody2 = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody2 != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody2 != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody2 == OnSerializeRigidBody.OnlyVelocity)
					{
						rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
					}
				}
				else
				{
					rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
				}
			}
			else
			{
				rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
				rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
			}
		}
		else
		{
			Debug.LogError("Type of observed is unknown when receiving.");
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00018850 File Offset: 0x00016C50
	protected internal void SerializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is global::UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transform = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
			case OnSerializeTransform.OnlyPosition:
				stream.SendNext(transform.localPosition);
				break;
			case OnSerializeTransform.OnlyRotation:
				stream.SendNext(transform.localRotation);
				break;
			case OnSerializeTransform.OnlyScale:
				stream.SendNext(transform.localScale);
				break;
			case OnSerializeTransform.PositionAndRotation:
				stream.SendNext(transform.localPosition);
				stream.SendNext(transform.localRotation);
				break;
			case OnSerializeTransform.All:
				stream.SendNext(transform.localPosition);
				stream.SendNext(transform.localRotation);
				stream.SendNext(transform.localScale);
				break;
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			OnSerializeRigidBody onSerializeRigidBody = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody == OnSerializeRigidBody.OnlyVelocity)
					{
						stream.SendNext(rigidbody.velocity);
					}
				}
				else
				{
					stream.SendNext(rigidbody.angularVelocity);
				}
			}
			else
			{
				stream.SendNext(rigidbody.velocity);
				stream.SendNext(rigidbody.angularVelocity);
			}
		}
		else if (component is Rigidbody2D)
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			OnSerializeRigidBody onSerializeRigidBody2 = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody2 != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody2 != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody2 == OnSerializeRigidBody.OnlyVelocity)
					{
						stream.SendNext(rigidbody2D.velocity);
					}
				}
				else
				{
					stream.SendNext(rigidbody2D.angularVelocity);
				}
			}
			else
			{
				stream.SendNext(rigidbody2D.velocity);
				stream.SendNext(rigidbody2D.angularVelocity);
			}
		}
		else
		{
			Debug.LogError("Observed type is not serializable: " + component.GetType());
		}
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x00018A94 File Offset: 0x00016E94
	protected internal void ExecuteComponentOnSerialize(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		IPunObservable punObservable = component as IPunObservable;
		if (punObservable != null)
		{
			punObservable.OnPhotonSerializeView(stream, info);
		}
		else if (component != null)
		{
			MethodInfo methodInfo = null;
			if (!this.m_OnSerializeMethodInfos.TryGetValue(component, out methodInfo))
			{
				if (!NetworkingPeer.GetMethod(component as global::UnityEngine.MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out methodInfo))
				{
					Debug.LogError("The observed monobehaviour (" + component.name + ") of this PhotonView does not implement OnPhotonSerializeView()!");
					methodInfo = null;
				}
				this.m_OnSerializeMethodInfos.Add(component, methodInfo);
			}
			if (methodInfo != null)
			{
				methodInfo.Invoke(component, new object[] { stream, info });
			}
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00018B4D File Offset: 0x00016F4D
	public void RefreshRpcMonoBehaviourCache()
	{
		this.RpcMonoBehaviours = base.GetComponents<global::UnityEngine.MonoBehaviour>();
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x00018B5B File Offset: 0x00016F5B
	public void RPC(string methodName, PhotonTargets target, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, false, parameters);
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00018B67 File Offset: 0x00016F67
	public void RpcSecure(string methodName, PhotonTargets target, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, encrypt, parameters);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00018B74 File Offset: 0x00016F74
	public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, false, parameters);
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00018B80 File Offset: 0x00016F80
	public void RpcSecure(string methodName, PhotonPlayer targetPlayer, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, encrypt, parameters);
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x00018B8D File Offset: 0x00016F8D
	public static PhotonView Get(Component component)
	{
		return component.GetComponent<PhotonView>();
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00018B95 File Offset: 0x00016F95
	public static PhotonView Get(GameObject gameObj)
	{
		return gameObj.GetComponent<PhotonView>();
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00018B9D File Offset: 0x00016F9D
	public static PhotonView Find(int viewID)
	{
		return PhotonNetwork.networkingPeer.GetPhotonView(viewID);
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00018BAC File Offset: 0x00016FAC
	public override string ToString()
	{
		return string.Format("View ({3}){0} on {1} {2}", new object[]
		{
			this.viewID,
			(!(base.gameObject != null)) ? "GO==null" : base.gameObject.name,
			(!this.isSceneView) ? string.Empty : "(scene)",
			this.prefix
		});
	}

	// Token: 0x040003CA RID: 970
	public int ownerId;

	// Token: 0x040003CB RID: 971
	public byte group;

	// Token: 0x040003CC RID: 972
	protected internal bool mixedModeIsReliable;

	// Token: 0x040003CD RID: 973
	public bool OwnerShipWasTransfered;

	// Token: 0x040003CE RID: 974
	public int prefixBackup = -1;

	// Token: 0x040003CF RID: 975
	internal object[] instantiationDataField;

	// Token: 0x040003D0 RID: 976
	protected internal object[] lastOnSerializeDataSent;

	// Token: 0x040003D1 RID: 977
	protected internal object[] lastOnSerializeDataReceived;

	// Token: 0x040003D2 RID: 978
	public ViewSynchronization synchronization;

	// Token: 0x040003D3 RID: 979
	public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;

	// Token: 0x040003D4 RID: 980
	public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;

	// Token: 0x040003D5 RID: 981
	public OwnershipOption ownershipTransfer;

	// Token: 0x040003D6 RID: 982
	public List<Component> ObservedComponents;

	// Token: 0x040003D7 RID: 983
	private Dictionary<Component, MethodInfo> m_OnSerializeMethodInfos = new Dictionary<Component, MethodInfo>(3);

	// Token: 0x040003D8 RID: 984
	[SerializeField]
	private int viewIdField;

	// Token: 0x040003D9 RID: 985
	public int instantiationId;

	// Token: 0x040003DA RID: 986
	public int currentMasterID = -1;

	// Token: 0x040003DB RID: 987
	protected internal bool didAwake;

	// Token: 0x040003DC RID: 988
	[SerializeField]
	protected internal bool isRuntimeInstantiated;

	// Token: 0x040003DD RID: 989
	protected internal bool removedFromLocalViewList;

	// Token: 0x040003DE RID: 990
	internal global::UnityEngine.MonoBehaviour[] RpcMonoBehaviours;

	// Token: 0x040003DF RID: 991
	private MethodInfo OnSerializeMethodInfo;

	// Token: 0x040003E0 RID: 992
	private bool failedToFindOnSerialize;
}
