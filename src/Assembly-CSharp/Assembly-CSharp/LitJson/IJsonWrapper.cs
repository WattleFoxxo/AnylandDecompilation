using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000025 RID: 37
	public interface IJsonWrapper : IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000F3 RID: 243
		bool IsArray { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000F4 RID: 244
		bool IsBoolean { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000F5 RID: 245
		bool IsDouble { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000F6 RID: 246
		bool IsInt { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000F7 RID: 247
		bool IsLong { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000F8 RID: 248
		bool IsObject { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000F9 RID: 249
		bool IsString { get; }

		// Token: 0x060000FA RID: 250
		bool GetBoolean();

		// Token: 0x060000FB RID: 251
		double GetDouble();

		// Token: 0x060000FC RID: 252
		int GetInt();

		// Token: 0x060000FD RID: 253
		JsonType GetJsonType();

		// Token: 0x060000FE RID: 254
		long GetLong();

		// Token: 0x060000FF RID: 255
		string GetString();

		// Token: 0x06000100 RID: 256
		void SetBoolean(bool val);

		// Token: 0x06000101 RID: 257
		void SetDouble(double val);

		// Token: 0x06000102 RID: 258
		void SetInt(int val);

		// Token: 0x06000103 RID: 259
		void SetJsonType(JsonType type);

		// Token: 0x06000104 RID: 260
		void SetLong(long val);

		// Token: 0x06000105 RID: 261
		void SetString(string val);

		// Token: 0x06000106 RID: 262
		string ToJson();

		// Token: 0x06000107 RID: 263
		void ToJson(JsonWriter writer);
	}
}
