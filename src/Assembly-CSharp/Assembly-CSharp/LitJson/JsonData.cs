using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
	// Token: 0x02000026 RID: 38
	public class JsonData : IJsonWrapper, IEquatable<JsonData>, IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		// Token: 0x06000108 RID: 264 RVA: 0x000052CE File Offset: 0x000036CE
		public JsonData()
		{
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000052D6 File Offset: 0x000036D6
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000052EC File Offset: 0x000036EC
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005302 File Offset: 0x00003702
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005318 File Offset: 0x00003718
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005330 File Offset: 0x00003730
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000053E8 File Offset: 0x000037E8
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000053FE File Offset: 0x000037FE
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000540B File Offset: 0x0000380B
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005416 File Offset: 0x00003816
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005421 File Offset: 0x00003821
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000542C File Offset: 0x0000382C
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005437 File Offset: 0x00003837
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005442 File Offset: 0x00003842
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000544D File Offset: 0x0000384D
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005458 File Offset: 0x00003858
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000546C File Offset: 0x0000386C
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005474 File Offset: 0x00003874
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005481 File Offset: 0x00003881
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000548E File Offset: 0x0000388E
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000549B File Offset: 0x0000389B
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000054A8 File Offset: 0x000038A8
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000551C File Offset: 0x0000391C
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00005590 File Offset: 0x00003990
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005598 File Offset: 0x00003998
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000055A0 File Offset: 0x000039A0
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000055A8 File Offset: 0x000039A8
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000123 RID: 291 RVA: 0x000055B0 File Offset: 0x000039B0
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000055B8 File Offset: 0x000039B8
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000055C0 File Offset: 0x000039C0
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000055C8 File Offset: 0x000039C8
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000055D5 File Offset: 0x000039D5
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x17000036 RID: 54
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData jsonData = this.ToJsonData(value);
				this[(string)key] = jsonData;
			}
		}

		// Token: 0x17000037 RID: 55
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData jsonData = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = jsonData;
				KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, jsonData);
				this.object_list[idx] = keyValuePair2;
			}
		}

		// Token: 0x17000038 RID: 56
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData jsonData = this.ToJsonData(value);
				this[index] = jsonData;
			}
		}

		// Token: 0x17000042 RID: 66
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x17000043 RID: 67
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = keyValuePair2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000584D File Offset: 0x00003C4D
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005855 File Offset: 0x00003C55
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000585D File Offset: 0x00003C5D
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005865 File Offset: 0x00003C65
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000586D File Offset: 0x00003C6D
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005875 File Offset: 0x00003C75
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005894 File Offset: 0x00003C94
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000058B3 File Offset: 0x00003CB3
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000058D2 File Offset: 0x00003CD2
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000058F1 File Offset: 0x00003CF1
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005910 File Offset: 0x00003D10
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005920 File Offset: 0x00003D20
		void IDictionary.Add(object key, object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, jsonData);
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>((string)key, jsonData);
			this.object_list.Add(keyValuePair);
			this.json = null;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005963 File Offset: 0x00003D63
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005982 File Offset: 0x00003D82
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005990 File Offset: 0x00003D90
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005998 File Offset: 0x00003D98
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005A09 File Offset: 0x00003E09
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005A16 File Offset: 0x00003E16
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005A35 File Offset: 0x00003E35
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005A54 File Offset: 0x00003E54
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005A73 File Offset: 0x00003E73
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005A92 File Offset: 0x00003E92
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005AB1 File Offset: 0x00003EB1
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005AC8 File Offset: 0x00003EC8
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005ADF File Offset: 0x00003EDF
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005AF6 File Offset: 0x00003EF6
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005B0D File Offset: 0x00003F0D
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005B24 File Offset: 0x00003F24
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005B2C File Offset: 0x00003F2C
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005B35 File Offset: 0x00003F35
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005B3E File Offset: 0x00003F3E
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005B52 File Offset: 0x00003F52
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005B60 File Offset: 0x00003F60
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005B6E File Offset: 0x00003F6E
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005B84 File Offset: 0x00003F84
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005B99 File Offset: 0x00003F99
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005BAE File Offset: 0x00003FAE
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005BC8 File Offset: 0x00003FC8
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData jsonData = this.ToJsonData(value);
			this[text] = jsonData;
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(text, jsonData);
			this.object_list.Insert(idx, keyValuePair);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005C04 File Offset: 0x00004004
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005C44 File Offset: 0x00004044
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005C80 File Offset: 0x00004080
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005CE4 File Offset: 0x000040E4
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005D3C File Offset: 0x0000413C
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005D60 File Offset: 0x00004160
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator = obj.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						JsonData.WriteJson((JsonData)obj2, writer);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = enumerator as IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				IDictionaryEnumerator enumerator2 = obj.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj3 = enumerator2.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
						writer.WritePropertyName((string)dictionaryEntry.Key);
						JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = enumerator2 as IDisposable) != null)
					{
						disposable2.Dispose();
					}
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005EE8 File Offset: 0x000042E8
		public int Add(object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(jsonData);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005F10 File Offset: 0x00004310
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005F38 File Offset: 0x00004338
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006013 File Offset: 0x00004413
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000601C File Offset: 0x0000441C
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000060E0 File Offset: 0x000044E0
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000612C File Offset: 0x0000452C
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006158 File Offset: 0x00004558
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x0400007D RID: 125
		private IList<JsonData> inst_array;

		// Token: 0x0400007E RID: 126
		private bool inst_boolean;

		// Token: 0x0400007F RID: 127
		private double inst_double;

		// Token: 0x04000080 RID: 128
		private int inst_int;

		// Token: 0x04000081 RID: 129
		private long inst_long;

		// Token: 0x04000082 RID: 130
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x04000083 RID: 131
		private string inst_string;

		// Token: 0x04000084 RID: 132
		private string json;

		// Token: 0x04000085 RID: 133
		private JsonType type;

		// Token: 0x04000086 RID: 134
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
