using System;
using System.Collections;
using System.Collections.Generic;

namespace DaikonForge.VoIP
{
	// Token: 0x0200001E RID: 30
	public class FastList<T> : IList<T>, IDisposable, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00004095 File Offset: 0x00002495
		internal FastList()
		{
			this.isElementTypeValueType = typeof(T).IsValueType;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000040C2 File Offset: 0x000024C2
		internal FastList(IList<T> listToClone)
			: this()
		{
			this.AddRange(listToClone);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000040D1 File Offset: 0x000024D1
		internal FastList(int capacity)
			: this()
		{
			this.EnsureCapacity(capacity);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000040E0 File Offset: 0x000024E0
		public static void ClearPool()
		{
			object obj = FastList<T>.pool;
			lock (obj)
			{
				FastList<T>.pool.Clear();
				FastList<T>.pool.TrimExcess();
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000412C File Offset: 0x0000252C
		public static FastList<T> Obtain()
		{
			object obj = FastList<T>.pool;
			FastList<T> fastList;
			lock (obj)
			{
				if (FastList<T>.pool.Count == 0)
				{
					fastList = new FastList<T>();
				}
				else
				{
					fastList = (FastList<T>)FastList<T>.pool.Dequeue();
				}
			}
			return fastList;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000418C File Offset: 0x0000258C
		public static FastList<T> Obtain(int capacity)
		{
			FastList<T> fastList = FastList<T>.Obtain();
			fastList.EnsureCapacity(capacity);
			return fastList;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000041A8 File Offset: 0x000025A8
		public void Release()
		{
			this.Clear();
			object obj = FastList<T>.pool;
			lock (obj)
			{
				FastList<T>.pool.Enqueue(this);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000041F0 File Offset: 0x000025F0
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000041F8 File Offset: 0x000025F8
		internal int Capacity
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004202 File Offset: 0x00002602
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001B RID: 27
		public T this[int index]
		{
			get
			{
				if (index < 0 || index > this.count - 1)
				{
					throw new IndexOutOfRangeException();
				}
				return this.items[index];
			}
			set
			{
				if (index < 0 || index > this.count - 1)
				{
					throw new IndexOutOfRangeException();
				}
				this.items[index] = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004258 File Offset: 0x00002658
		internal T[] Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004260 File Offset: 0x00002660
		public void Enqueue(T item)
		{
			object obj = this.items;
			lock (obj)
			{
				this.Add(item);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000042A0 File Offset: 0x000026A0
		public T Dequeue()
		{
			object obj = this.items;
			T t2;
			lock (obj)
			{
				if (this.count == 0)
				{
					throw new IndexOutOfRangeException();
				}
				T t = this.items[0];
				this.RemoveAt(0);
				t2 = t;
			}
			return t2;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004300 File Offset: 0x00002700
		public T Pop()
		{
			object obj = this.items;
			T t2;
			lock (obj)
			{
				if (this.count == 0)
				{
					throw new IndexOutOfRangeException();
				}
				T t = this.items[this.count - 1];
				this.count--;
				t2 = t;
			}
			return t2;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000436C File Offset: 0x0000276C
		public FastList<T> Clone()
		{
			FastList<T> fastList = FastList<T>.Obtain(this.count);
			Array.Copy(this.items, 0, fastList.items, 0, this.count);
			fastList.count = this.count;
			return fastList;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000043AB File Offset: 0x000027AB
		public void Reverse()
		{
			Array.Reverse(this.items, 0, this.count);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000043BF File Offset: 0x000027BF
		public void Sort()
		{
			Array.Sort<T>(this.items, 0, this.count, null);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000043D4 File Offset: 0x000027D4
		public void Sort(IComparer<T> comparer)
		{
			Array.Sort<T>(this.items, 0, this.count, comparer);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000043EC File Offset: 0x000027EC
		public void Sort(Comparison<T> comparison)
		{
			if (comparison == null)
			{
				throw new ArgumentNullException("comparison");
			}
			if (this.count > 0)
			{
				using (FastList<T>.FunctorComparer functorComparer = FastList<T>.FunctorComparer.Obtain(comparison))
				{
					Array.Sort<T>(this.items, 0, this.count, functorComparer);
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004454 File Offset: 0x00002854
		public void EnsureCapacity(int Size)
		{
			if (this.items.Length < Size)
			{
				int num = Size / 128 * 128 + 128;
				Array.Resize<T>(ref this.items, num);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000448F File Offset: 0x0000288F
		public void ForceCount(int count)
		{
			this.count = count;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004498 File Offset: 0x00002898
		public void AddRange(FastList<T> list)
		{
			int num = list.count;
			this.EnsureCapacity(this.count + num);
			Array.Copy(list.items, 0, this.items, this.count, num);
			this.count += num;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000044E4 File Offset: 0x000028E4
		public void AddRange(IList<T> list)
		{
			int num = list.Count;
			this.EnsureCapacity(this.count + num);
			for (int i = 0; i < num; i++)
			{
				this.items[this.count++] = list[i];
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000453C File Offset: 0x0000293C
		public void AddRange(T[] list)
		{
			int num = list.Length;
			this.EnsureCapacity(this.count + num);
			Array.Copy(list, 0, this.items, this.count, num);
			this.count += num;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000457D File Offset: 0x0000297D
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.items, item, 0, this.count);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004594 File Offset: 0x00002994
		public void Insert(int index, T item)
		{
			this.EnsureCapacity(this.count + 1);
			if (index < this.count)
			{
				Array.Copy(this.items, index, this.items, index + 1, this.count - index);
			}
			this.items[index] = item;
			this.count++;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000045F4 File Offset: 0x000029F4
		public void InsertRange(int index, T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("items");
			}
			if (index < 0 || index > this.count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.EnsureCapacity(this.count + array.Length);
			if (index < this.count)
			{
				Array.Copy(this.items, index, this.items, index + array.Length, this.count - index);
			}
			array.CopyTo(this.items, index);
			this.count += array.Length;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004688 File Offset: 0x00002A88
		public void InsertRange(int index, FastList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("items");
			}
			if (index < 0 || index > this.count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.EnsureCapacity(this.count + list.count);
			if (index < this.count)
			{
				Array.Copy(this.items, index, this.items, index + list.count, this.count - index);
			}
			Array.Copy(list.items, 0, this.items, index, list.count);
			this.count += list.count;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004734 File Offset: 0x00002B34
		public void RemoveAll(Predicate<T> predicate)
		{
			int i = 0;
			while (i < this.count)
			{
				if (predicate(this.items[i]))
				{
					this.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000477C File Offset: 0x00002B7C
		public void RemoveAt(int index)
		{
			if (index >= this.count)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.count--;
			if (index < this.count)
			{
				Array.Copy(this.items, index + 1, this.items, index, this.count - index);
			}
			this.items[this.count] = default(T);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000047EC File Offset: 0x00002BEC
		public void RemoveRange(int index, int length)
		{
			if (index < 0 || length < 0 || this.count - index < length)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (this.count > 0)
			{
				this.count -= length;
				if (index < this.count)
				{
					Array.Copy(this.items, index + length, this.items, index, this.count - index);
				}
				Array.Clear(this.items, this.count, length);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004870 File Offset: 0x00002C70
		public void Add(T item)
		{
			this.EnsureCapacity(this.count + 1);
			this.items[this.count++] = item;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000048A8 File Offset: 0x00002CA8
		public void Add(T item0, T item1)
		{
			this.EnsureCapacity(this.count + 2);
			this.items[this.count++] = item0;
			this.items[this.count++] = item1;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004900 File Offset: 0x00002D00
		public void Add(T item0, T item1, T item2)
		{
			this.EnsureCapacity(this.count + 3);
			this.items[this.count++] = item0;
			this.items[this.count++] = item1;
			this.items[this.count++] = item2;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004974 File Offset: 0x00002D74
		public void Add(T item0, T item1, T item2, T item3)
		{
			this.EnsureCapacity(this.count + 3);
			this.items[this.count++] = item0;
			this.items[this.count++] = item1;
			this.items[this.count++] = item2;
			this.items[this.count++] = item3;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004A04 File Offset: 0x00002E04
		public void Clear()
		{
			if (!this.isElementTypeValueType)
			{
				Array.Clear(this.items, 0, this.items.Length);
			}
			this.count = 0;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004A2C File Offset: 0x00002E2C
		public void TrimExcess()
		{
			Array.Resize<T>(ref this.items, this.count);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004A40 File Offset: 0x00002E40
		public bool Contains(T item)
		{
			if (item == null)
			{
				for (int i = 0; i < this.count; i++)
				{
					if (this.items[i] == null)
					{
						return true;
					}
				}
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int j = 0; j < this.count; j++)
			{
				if (@default.Equals(this.items[j], item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004AC1 File Offset: 0x00002EC1
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004ACB File Offset: 0x00002ECB
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.items, 0, array, arrayIndex, this.count);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004AE4 File Offset: 0x00002EE4
		public void CopyTo(int sourceIndex, T[] dest, int destIndex, int length)
		{
			if (sourceIndex + length > this.count)
			{
				throw new IndexOutOfRangeException("sourceIndex");
			}
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (destIndex + length > dest.Length)
			{
				throw new IndexOutOfRangeException("destIndex");
			}
			Array.Copy(this.items, sourceIndex, dest, destIndex, length);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004B44 File Offset: 0x00002F44
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num == -1)
			{
				return false;
			}
			this.RemoveAt(num);
			return true;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004B6C File Offset: 0x00002F6C
		public List<T> ToList()
		{
			List<T> list = new List<T>(this.count);
			list.AddRange(this.ToArray());
			return list;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004B94 File Offset: 0x00002F94
		internal T[] ToTempArray()
		{
			T[] array = TempArray<T>.Obtain(this.count);
			Array.Copy(this.items, 0, array, 0, this.count);
			return array;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004BC4 File Offset: 0x00002FC4
		public T[] ToArray()
		{
			T[] array = new T[this.count];
			Array.Copy(this.items, 0, array, 0, this.count);
			return array;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004BF4 File Offset: 0x00002FF4
		public T[] ToArray(int index, int length)
		{
			T[] array = new T[this.count];
			if (this.count > 0)
			{
				this.CopyTo(index, array, 0, length);
			}
			return array;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004C24 File Offset: 0x00003024
		public FastList<T> GetRange(int index, int length)
		{
			FastList<T> fastList = FastList<T>.Obtain(length);
			this.CopyTo(0, fastList.items, index, length);
			return fastList;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004C48 File Offset: 0x00003048
		public bool Any(Func<T, bool> predicate)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (predicate(this.items[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004C86 File Offset: 0x00003086
		public T First()
		{
			if (this.count == 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this.items[0];
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004CA8 File Offset: 0x000030A8
		public T FirstOrDefault()
		{
			if (this.count > 0)
			{
				return this.items[0];
			}
			return default(T);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004CD8 File Offset: 0x000030D8
		public T FirstOrDefault(Func<T, bool> predicate)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (predicate(this.items[i]))
				{
					return this.items[i];
				}
			}
			return default(T);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004D29 File Offset: 0x00003129
		public T Last()
		{
			if (this.count == 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this.items[this.count - 1];
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004D50 File Offset: 0x00003150
		public T LastOrDefault()
		{
			if (this.count == 0)
			{
				return default(T);
			}
			return this.items[this.count - 1];
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004D88 File Offset: 0x00003188
		public T LastOrDefault(Func<T, bool> predicate)
		{
			T t = default(T);
			for (int i = 0; i < this.count; i++)
			{
				if (predicate(this.items[i]))
				{
					t = this.items[i];
				}
			}
			return t;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004DDC File Offset: 0x000031DC
		public FastList<T> Where(Func<T, bool> predicate)
		{
			FastList<T> fastList = FastList<T>.Obtain(this.count);
			for (int i = 0; i < this.count; i++)
			{
				if (predicate(this.items[i]))
				{
					fastList.Add(this.items[i]);
				}
			}
			return fastList;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004E38 File Offset: 0x00003238
		public int Matching(Func<T, bool> predicate)
		{
			int num = 0;
			for (int i = 0; i < this.count; i++)
			{
				if (predicate(this.items[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004E7C File Offset: 0x0000327C
		public FastList<TResult> Select<TResult>(Func<T, TResult> selector)
		{
			FastList<TResult> fastList = FastList<TResult>.Obtain(this.count);
			for (int i = 0; i < this.count; i++)
			{
				fastList.Add(selector(this.items[i]));
			}
			return fastList;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004EC8 File Offset: 0x000032C8
		public FastList<T> Concat(FastList<T> list)
		{
			FastList<T> fastList = FastList<T>.Obtain(this.count + list.count);
			fastList.AddRange(this);
			fastList.AddRange(list);
			return fastList;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004EF8 File Offset: 0x000032F8
		public FastList<TResult> Convert<TResult>()
		{
			FastList<TResult> fastList = FastList<TResult>.Obtain(this.count);
			for (int i = 0; i < this.count; i++)
			{
				fastList.Add((TResult)((object)global::System.Convert.ChangeType(this.items[i], typeof(TResult))));
			}
			return fastList;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004F54 File Offset: 0x00003354
		public void ForEach(Action<T> action)
		{
			int i = 0;
			while (i < this.Count)
			{
				action(this.items[i++]);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004F8A File Offset: 0x0000338A
		public IEnumerator<T> GetEnumerator()
		{
			return FastList<T>.PooledEnumerator.Obtain(this, null);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004F93 File Offset: 0x00003393
		IEnumerator IEnumerable.GetEnumerator()
		{
			return FastList<T>.PooledEnumerator.Obtain(this, null);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004F9C File Offset: 0x0000339C
		public void Dispose()
		{
			this.Release();
		}

		// Token: 0x04000064 RID: 100
		private static Queue<object> pool = new Queue<object>(1024);

		// Token: 0x04000065 RID: 101
		private const int DEFAULT_CAPACITY = 128;

		// Token: 0x04000066 RID: 102
		private T[] items = new T[128];

		// Token: 0x04000067 RID: 103
		private int count;

		// Token: 0x04000068 RID: 104
		private bool isElementTypeValueType;

		// Token: 0x0200001F RID: 31
		private class PooledEnumerator : IEnumerator<T>, IEnumerable<T>, IEnumerator, IDisposable, IEnumerable
		{
			// Token: 0x060000D4 RID: 212 RVA: 0x00004FC0 File Offset: 0x000033C0
			public static FastList<T>.PooledEnumerator Obtain(FastList<T> list, Func<T, bool> predicate)
			{
				FastList<T>.PooledEnumerator pooledEnumerator = ((FastList<T>.PooledEnumerator.pool.Count <= 0) ? new FastList<T>.PooledEnumerator() : FastList<T>.PooledEnumerator.pool.Dequeue());
				pooledEnumerator.ResetInternal(list, predicate);
				return pooledEnumerator;
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x00004FFB File Offset: 0x000033FB
			public void Release()
			{
				if (this.isValid)
				{
					this.isValid = false;
					FastList<T>.PooledEnumerator.pool.Enqueue(this);
				}
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000501A File Offset: 0x0000341A
			public T Current
			{
				get
				{
					if (!this.isValid)
					{
						throw new InvalidOperationException("The enumerator is no longer valid");
					}
					return this.currentValue;
				}
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x00005038 File Offset: 0x00003438
			private void ResetInternal(FastList<T> list, Func<T, bool> predicate)
			{
				this.isValid = true;
				this.list = list;
				this.predicate = predicate;
				this.currentIndex = 0;
				this.currentValue = default(T);
			}

			// Token: 0x060000D8 RID: 216 RVA: 0x00005070 File Offset: 0x00003470
			public void Dispose()
			{
				this.Release();
			}

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005078 File Offset: 0x00003478
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060000DA RID: 218 RVA: 0x00005088 File Offset: 0x00003488
			public bool MoveNext()
			{
				if (!this.isValid)
				{
					throw new InvalidOperationException("The enumerator is no longer valid");
				}
				while (this.currentIndex < this.list.Count)
				{
					T t = this.list[this.currentIndex++];
					if (this.predicate == null || this.predicate(t))
					{
						this.currentValue = t;
						return true;
					}
				}
				this.Release();
				this.currentValue = default(T);
				return false;
			}

			// Token: 0x060000DB RID: 219 RVA: 0x00005123 File Offset: 0x00003523
			public void Reset()
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000DC RID: 220 RVA: 0x0000512A File Offset: 0x0000352A
			public IEnumerator<T> GetEnumerator()
			{
				return this;
			}

			// Token: 0x060000DD RID: 221 RVA: 0x0000512D File Offset: 0x0000352D
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this;
			}

			// Token: 0x04000069 RID: 105
			private static Queue<FastList<T>.PooledEnumerator> pool = new Queue<FastList<T>.PooledEnumerator>();

			// Token: 0x0400006A RID: 106
			private FastList<T> list;

			// Token: 0x0400006B RID: 107
			private Func<T, bool> predicate;

			// Token: 0x0400006C RID: 108
			private int currentIndex;

			// Token: 0x0400006D RID: 109
			private T currentValue;

			// Token: 0x0400006E RID: 110
			private bool isValid;
		}

		// Token: 0x02000020 RID: 32
		private class FunctorComparer : IComparer<T>, IDisposable
		{
			// Token: 0x060000E0 RID: 224 RVA: 0x00005144 File Offset: 0x00003544
			public static FastList<T>.FunctorComparer Obtain(Comparison<T> comparison)
			{
				FastList<T>.FunctorComparer functorComparer = ((FastList<T>.FunctorComparer.pool.Count <= 0) ? new FastList<T>.FunctorComparer() : FastList<T>.FunctorComparer.pool.Dequeue());
				functorComparer.comparison = comparison;
				return functorComparer;
			}

			// Token: 0x060000E1 RID: 225 RVA: 0x0000517E File Offset: 0x0000357E
			public void Release()
			{
				this.comparison = null;
				if (!FastList<T>.FunctorComparer.pool.Contains(this))
				{
					FastList<T>.FunctorComparer.pool.Enqueue(this);
				}
			}

			// Token: 0x060000E2 RID: 226 RVA: 0x000051A2 File Offset: 0x000035A2
			public int Compare(T x, T y)
			{
				return this.comparison(x, y);
			}

			// Token: 0x060000E3 RID: 227 RVA: 0x000051B1 File Offset: 0x000035B1
			public void Dispose()
			{
				this.Release();
			}

			// Token: 0x0400006F RID: 111
			private static Queue<FastList<T>.FunctorComparer> pool = new Queue<FastList<T>.FunctorComparer>();

			// Token: 0x04000070 RID: 112
			private Comparison<T> comparison;
		}
	}
}
