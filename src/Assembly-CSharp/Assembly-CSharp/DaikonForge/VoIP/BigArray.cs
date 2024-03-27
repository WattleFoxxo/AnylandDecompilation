using System;

namespace DaikonForge.VoIP
{
	// Token: 0x0200001C RID: 28
	public class BigArray<T>
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00003EFD File Offset: 0x000022FD
		public BigArray(int capacity, int count)
		{
			this.items = new T[capacity];
			this.count = count;
		}

		// Token: 0x17000015 RID: 21
		public T this[int index]
		{
			get
			{
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException();
				}
				return this.items[index];
			}
			set
			{
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException();
				}
				this.items[index] = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003F59 File Offset: 0x00002359
		public int Length
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003F61 File Offset: 0x00002361
		public T[] Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003F69 File Offset: 0x00002369
		public void Resize(int newSize)
		{
			this.count = newSize;
			if (this.items.Length < newSize)
			{
				Array.Resize<T>(ref this.items, newSize * 2);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003F8E File Offset: 0x0000238E
		public void CopyTo(int startIndex, BigArray<T> destination, int destIndex, int count)
		{
			Buffer.BlockCopy(this.items, startIndex, destination.items, destIndex, count);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003FA5 File Offset: 0x000023A5
		public void CopyTo(int startIndex, T[] destination, int destIndex, int count)
		{
			Buffer.BlockCopy(this.items, startIndex, destination, destIndex, count);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003FB7 File Offset: 0x000023B7
		public void CopyFrom(T[] source, int sourceIndex, int destIndex, int count)
		{
			Buffer.BlockCopy(source, sourceIndex, this.items, destIndex, count);
		}

		// Token: 0x04000061 RID: 97
		private T[] items;

		// Token: 0x04000062 RID: 98
		private int count;
	}
}
