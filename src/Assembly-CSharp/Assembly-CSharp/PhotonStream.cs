using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class PhotonStream
{
	// Token: 0x06000441 RID: 1089 RVA: 0x000142FC File Offset: 0x000126FC
	public PhotonStream(bool write, object[] incomingData)
	{
		this.write = write;
		if (incomingData == null)
		{
			this.writeData = new Queue<object>(10);
		}
		else
		{
			this.readData = incomingData;
		}
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0001432A File Offset: 0x0001272A
	public void SetReadStream(object[] incomingData, byte pos = 0)
	{
		this.readData = incomingData;
		this.currentItem = pos;
		this.write = false;
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x00014341 File Offset: 0x00012741
	internal void ResetWriteStream()
	{
		this.writeData.Clear();
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x0001434E File Offset: 0x0001274E
	public bool isWriting
	{
		get
		{
			return this.write;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x00014356 File Offset: 0x00012756
	public bool isReading
	{
		get
		{
			return !this.write;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x00014361 File Offset: 0x00012761
	public int Count
	{
		get
		{
			return (!this.isWriting) ? this.readData.Length : this.writeData.Count;
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00014388 File Offset: 0x00012788
	public object ReceiveNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		object obj = this.readData[(int)this.currentItem];
		this.currentItem += 1;
		return obj;
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x000143CC File Offset: 0x000127CC
	public object PeekNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		return this.readData[(int)this.currentItem];
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x000143FF File Offset: 0x000127FF
	public void SendNext(object obj)
	{
		if (!this.write)
		{
			Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
			return;
		}
		this.writeData.Enqueue(obj);
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00014423 File Offset: 0x00012823
	public object[] ToArray()
	{
		return (!this.isWriting) ? this.readData : this.writeData.ToArray();
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00014448 File Offset: 0x00012848
	public void Serialize(ref bool myBool)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myBool);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			myBool = (bool)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x000144B0 File Offset: 0x000128B0
	public void Serialize(ref int myInt)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myInt);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			myInt = (int)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00014518 File Offset: 0x00012918
	public void Serialize(ref string value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (string)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00014578 File Offset: 0x00012978
	public void Serialize(ref char value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (char)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x000145E0 File Offset: 0x000129E0
	public void Serialize(ref short value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (short)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00014648 File Offset: 0x00012A48
	public void Serialize(ref float obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (float)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x000146B0 File Offset: 0x00012AB0
	public void Serialize(ref PhotonPlayer obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (PhotonPlayer)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00014710 File Offset: 0x00012B10
	public void Serialize(ref Vector3 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Vector3)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00014780 File Offset: 0x00012B80
	public void Serialize(ref Vector2 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Vector2)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x000147F0 File Offset: 0x00012BF0
	public void Serialize(ref Quaternion obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Quaternion)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x04000367 RID: 871
	private bool write;

	// Token: 0x04000368 RID: 872
	private Queue<object> writeData;

	// Token: 0x04000369 RID: 873
	private object[] readData;

	// Token: 0x0400036A RID: 874
	internal byte currentItem;
}
