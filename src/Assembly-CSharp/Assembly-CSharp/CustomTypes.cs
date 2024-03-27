using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000053 RID: 83
internal static class CustomTypes
{
	// Token: 0x060002FE RID: 766 RVA: 0x0000C04C File Offset: 0x0000A44C
	internal static void Register()
	{
		PhotonPeer.RegisterType(typeof(Vector2), 87, new SerializeStreamMethod(CustomTypes.SerializeVector2), new DeserializeStreamMethod(CustomTypes.DeserializeVector2));
		PhotonPeer.RegisterType(typeof(Vector3), 86, new SerializeStreamMethod(CustomTypes.SerializeVector3), new DeserializeStreamMethod(CustomTypes.DeserializeVector3));
		PhotonPeer.RegisterType(typeof(Quaternion), 81, new SerializeStreamMethod(CustomTypes.SerializeQuaternion), new DeserializeStreamMethod(CustomTypes.DeserializeQuaternion));
		PhotonPeer.RegisterType(typeof(PhotonPlayer), 80, new SerializeStreamMethod(CustomTypes.SerializePhotonPlayer), new DeserializeStreamMethod(CustomTypes.DeserializePhotonPlayer));
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0000C18C File Offset: 0x0000A58C
	private static short SerializeVector3(StreamBuffer outStream, object customobject)
	{
		Vector3 vector = (Vector3)customobject;
		int num = 0;
		object obj = CustomTypes.memVector3;
		lock (obj)
		{
			byte[] array = CustomTypes.memVector3;
			Protocol.Serialize(vector.x, array, ref num);
			Protocol.Serialize(vector.y, array, ref num);
			Protocol.Serialize(vector.z, array, ref num);
			outStream.Write(array, 0, 12);
		}
		return 12;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0000C20C File Offset: 0x0000A60C
	private static object DeserializeVector3(StreamBuffer inStream, short length)
	{
		Vector3 vector = default(Vector3);
		object obj = CustomTypes.memVector3;
		lock (obj)
		{
			inStream.Read(CustomTypes.memVector3, 0, 12);
			int num = 0;
			Protocol.Deserialize(out vector.x, CustomTypes.memVector3, ref num);
			Protocol.Deserialize(out vector.y, CustomTypes.memVector3, ref num);
			Protocol.Deserialize(out vector.z, CustomTypes.memVector3, ref num);
		}
		return vector;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0000C29C File Offset: 0x0000A69C
	private static short SerializeVector2(StreamBuffer outStream, object customobject)
	{
		Vector2 vector = (Vector2)customobject;
		object obj = CustomTypes.memVector2;
		lock (obj)
		{
			byte[] array = CustomTypes.memVector2;
			int num = 0;
			Protocol.Serialize(vector.x, array, ref num);
			Protocol.Serialize(vector.y, array, ref num);
			outStream.Write(array, 0, 8);
		}
		return 8;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0000C308 File Offset: 0x0000A708
	private static object DeserializeVector2(StreamBuffer inStream, short length)
	{
		Vector2 vector = default(Vector2);
		object obj = CustomTypes.memVector2;
		lock (obj)
		{
			inStream.Read(CustomTypes.memVector2, 0, 8);
			int num = 0;
			Protocol.Deserialize(out vector.x, CustomTypes.memVector2, ref num);
			Protocol.Deserialize(out vector.y, CustomTypes.memVector2, ref num);
		}
		return vector;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0000C384 File Offset: 0x0000A784
	private static short SerializeQuaternion(StreamBuffer outStream, object customobject)
	{
		Quaternion quaternion = (Quaternion)customobject;
		object obj = CustomTypes.memQuarternion;
		lock (obj)
		{
			byte[] array = CustomTypes.memQuarternion;
			int num = 0;
			Protocol.Serialize(quaternion.w, array, ref num);
			Protocol.Serialize(quaternion.x, array, ref num);
			Protocol.Serialize(quaternion.y, array, ref num);
			Protocol.Serialize(quaternion.z, array, ref num);
			outStream.Write(array, 0, 16);
		}
		return 16;
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0000C410 File Offset: 0x0000A810
	private static object DeserializeQuaternion(StreamBuffer inStream, short length)
	{
		Quaternion quaternion = default(Quaternion);
		object obj = CustomTypes.memQuarternion;
		lock (obj)
		{
			inStream.Read(CustomTypes.memQuarternion, 0, 16);
			int num = 0;
			Protocol.Deserialize(out quaternion.w, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.x, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.y, CustomTypes.memQuarternion, ref num);
			Protocol.Deserialize(out quaternion.z, CustomTypes.memQuarternion, ref num);
		}
		return quaternion;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0000C4B0 File Offset: 0x0000A8B0
	private static short SerializePhotonPlayer(StreamBuffer outStream, object customobject)
	{
		int id = ((PhotonPlayer)customobject).ID;
		object obj = CustomTypes.memPlayer;
		short num2;
		lock (obj)
		{
			byte[] array = CustomTypes.memPlayer;
			int num = 0;
			Protocol.Serialize(id, array, ref num);
			outStream.Write(array, 0, 4);
			num2 = 4;
		}
		return num2;
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0000C510 File Offset: 0x0000A910
	private static object DeserializePhotonPlayer(StreamBuffer inStream, short length)
	{
		object obj = CustomTypes.memPlayer;
		int num2;
		lock (obj)
		{
			inStream.Read(CustomTypes.memPlayer, 0, (int)length);
			int num = 0;
			Protocol.Deserialize(out num2, CustomTypes.memPlayer, ref num);
		}
		if (PhotonNetwork.networkingPeer.mActors.ContainsKey(num2))
		{
			return PhotonNetwork.networkingPeer.mActors[num2];
		}
		return null;
	}

	// Token: 0x04000194 RID: 404
	public static readonly byte[] memVector3 = new byte[12];

	// Token: 0x04000195 RID: 405
	public static readonly byte[] memVector2 = new byte[8];

	// Token: 0x04000196 RID: 406
	public static readonly byte[] memQuarternion = new byte[16];

	// Token: 0x04000197 RID: 407
	public static readonly byte[] memPlayer = new byte[4];
}
