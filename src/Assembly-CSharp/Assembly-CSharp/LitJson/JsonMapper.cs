using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace LitJson
{
	// Token: 0x02000031 RID: 49
	public class JsonMapper
	{
		// Token: 0x06000194 RID: 404 RVA: 0x000063AC File Offset: 0x000047AC
		static JsonMapper()
		{
			JsonMapper.RegisterBaseExporters();
			JsonMapper.RegisterBaseImporters();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006468 File Offset: 0x00004868
		private static void AddArrayMetadata(Type type)
		{
			if (JsonMapper.array_metadata.ContainsKey(type))
			{
				return;
			}
			ArrayMetadata arrayMetadata = default(ArrayMetadata);
			arrayMetadata.IsArray = type.IsArray;
			if (type.GetInterface("System.Collections.IList") != null)
			{
				arrayMetadata.IsList = true;
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				if (!(propertyInfo.Name != "Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						if (indexParameters[0].ParameterType == typeof(int))
						{
							arrayMetadata.ElementType = propertyInfo.PropertyType;
						}
					}
				}
			}
			object obj = JsonMapper.array_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.array_metadata.Add(type, arrayMetadata);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006578 File Offset: 0x00004978
		private static void AddObjectMetadata(Type type)
		{
			if (JsonMapper.object_metadata.ContainsKey(type))
			{
				return;
			}
			ObjectMetadata objectMetadata = default(ObjectMetadata);
			if (type.GetInterface("System.Collections.IDictionary") != null)
			{
				objectMetadata.IsDictionary = true;
			}
			objectMetadata.Properties = new Dictionary<string, PropertyMetadata>();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				if (propertyInfo.Name == "Item")
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						if (indexParameters[0].ParameterType == typeof(string))
						{
							objectMetadata.ElementType = propertyInfo.PropertyType;
						}
					}
				}
				else
				{
					PropertyMetadata propertyMetadata = default(PropertyMetadata);
					propertyMetadata.Info = propertyInfo;
					propertyMetadata.Type = propertyInfo.PropertyType;
					objectMetadata.Properties.Add(propertyInfo.Name, propertyMetadata);
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				PropertyMetadata propertyMetadata2 = default(PropertyMetadata);
				propertyMetadata2.Info = fieldInfo;
				propertyMetadata2.IsField = true;
				propertyMetadata2.Type = fieldInfo.FieldType;
				objectMetadata.Properties.Add(fieldInfo.Name, propertyMetadata2);
			}
			object obj = JsonMapper.object_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.object_metadata.Add(type, objectMetadata);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000671C File Offset: 0x00004B1C
		private static void AddTypeProperties(Type type)
		{
			if (JsonMapper.type_properties.ContainsKey(type))
			{
				return;
			}
			IList<PropertyMetadata> list = new List<PropertyMetadata>();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				if (!(propertyInfo.Name == "Item"))
				{
					list.Add(new PropertyMetadata
					{
						Info = propertyInfo,
						IsField = false
					});
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				list.Add(new PropertyMetadata
				{
					Info = fieldInfo,
					IsField = true
				});
			}
			object obj = JsonMapper.type_properties_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.type_properties.Add(type, list);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006830 File Offset: 0x00004C30
		private static MethodInfo GetConvOp(Type t1, Type t2)
		{
			object obj = JsonMapper.conv_ops_lock;
			lock (obj)
			{
				if (!JsonMapper.conv_ops.ContainsKey(t1))
				{
					JsonMapper.conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
				}
			}
			if (JsonMapper.conv_ops[t1].ContainsKey(t2))
			{
				return JsonMapper.conv_ops[t1][t2];
			}
			MethodInfo method = t1.GetMethod("op_Implicit", new Type[] { t2 });
			object obj2 = JsonMapper.conv_ops_lock;
			lock (obj2)
			{
				try
				{
					JsonMapper.conv_ops[t1].Add(t2, method);
				}
				catch (ArgumentException)
				{
					return JsonMapper.conv_ops[t1][t2];
				}
			}
			return method;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006928 File Offset: 0x00004D28
		private static object ReadValue(Type inst_type, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd)
			{
				return null;
			}
			Type underlyingType = Nullable.GetUnderlyingType(inst_type);
			Type type = underlyingType ?? inst_type;
			if (reader.Token == JsonToken.Null)
			{
				if (inst_type.IsClass || underlyingType != null)
				{
					return null;
				}
				throw new JsonException(string.Format("Can't assign null to an instance of type {0}", inst_type));
			}
			else
			{
				if (reader.Token != JsonToken.Double && reader.Token != JsonToken.Int && reader.Token != JsonToken.Long && reader.Token != JsonToken.String && reader.Token != JsonToken.Boolean)
				{
					object obj = null;
					if (reader.Token == JsonToken.ArrayStart)
					{
						JsonMapper.AddArrayMetadata(inst_type);
						ArrayMetadata arrayMetadata = JsonMapper.array_metadata[inst_type];
						if (!arrayMetadata.IsArray && !arrayMetadata.IsList)
						{
							throw new JsonException(string.Format("Type {0} can't act as an array", inst_type));
						}
						IList list;
						Type type2;
						if (!arrayMetadata.IsArray)
						{
							list = (IList)Activator.CreateInstance(inst_type);
							type2 = arrayMetadata.ElementType;
						}
						else
						{
							list = new ArrayList();
							type2 = inst_type.GetElementType();
						}
						for (;;)
						{
							object obj2 = JsonMapper.ReadValue(type2, reader);
							if (obj2 == null && reader.Token == JsonToken.ArrayEnd)
							{
								break;
							}
							list.Add(obj2);
						}
						if (arrayMetadata.IsArray)
						{
							int count = list.Count;
							obj = Array.CreateInstance(type2, count);
							for (int i = 0; i < count; i++)
							{
								((Array)obj).SetValue(list[i], i);
							}
						}
						else
						{
							obj = list;
						}
					}
					else if (reader.Token == JsonToken.ObjectStart)
					{
						JsonMapper.AddObjectMetadata(type);
						ObjectMetadata objectMetadata = JsonMapper.object_metadata[type];
						obj = Activator.CreateInstance(type);
						string text;
						for (;;)
						{
							reader.Read();
							if (reader.Token == JsonToken.ObjectEnd)
							{
								break;
							}
							text = (string)reader.Value;
							if (objectMetadata.Properties.ContainsKey(text))
							{
								PropertyMetadata propertyMetadata = objectMetadata.Properties[text];
								if (propertyMetadata.IsField)
								{
									((FieldInfo)propertyMetadata.Info).SetValue(obj, JsonMapper.ReadValue(propertyMetadata.Type, reader));
								}
								else
								{
									PropertyInfo propertyInfo = (PropertyInfo)propertyMetadata.Info;
									if (propertyInfo.CanWrite)
									{
										propertyInfo.SetValue(obj, JsonMapper.ReadValue(propertyMetadata.Type, reader), null);
									}
									else
									{
										JsonMapper.ReadValue(propertyMetadata.Type, reader);
									}
								}
							}
							else if (!objectMetadata.IsDictionary)
							{
								if (!reader.SkipNonMembers)
								{
									goto Block_30;
								}
								JsonMapper.ReadSkip(reader);
							}
							else
							{
								((IDictionary)obj).Add(text, JsonMapper.ReadValue(objectMetadata.ElementType, reader));
							}
						}
						return obj;
						Block_30:
						throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", inst_type, text));
					}
					return obj;
				}
				Type type3 = reader.Value.GetType();
				if (type.IsAssignableFrom(type3))
				{
					return reader.Value;
				}
				if (JsonMapper.custom_importers_table.ContainsKey(type3) && JsonMapper.custom_importers_table[type3].ContainsKey(type))
				{
					ImporterFunc importerFunc = JsonMapper.custom_importers_table[type3][type];
					return importerFunc(reader.Value);
				}
				if (JsonMapper.base_importers_table.ContainsKey(type3) && JsonMapper.base_importers_table[type3].ContainsKey(type))
				{
					ImporterFunc importerFunc2 = JsonMapper.base_importers_table[type3][type];
					return importerFunc2(reader.Value);
				}
				if (type.IsEnum)
				{
					return Enum.ToObject(type, reader.Value);
				}
				MethodInfo convOp = JsonMapper.GetConvOp(type, type3);
				if (convOp != null)
				{
					return convOp.Invoke(null, new object[] { reader.Value });
				}
				throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, type3, inst_type));
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006D1C File Offset: 0x0000511C
		private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null)
			{
				return null;
			}
			IJsonWrapper jsonWrapper = factory();
			if (reader.Token == JsonToken.String)
			{
				jsonWrapper.SetString((string)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Double)
			{
				jsonWrapper.SetDouble((double)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Int)
			{
				jsonWrapper.SetInt((int)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Long)
			{
				jsonWrapper.SetLong((long)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Boolean)
			{
				jsonWrapper.SetBoolean((bool)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.ArrayStart)
			{
				jsonWrapper.SetJsonType(JsonType.Array);
				for (;;)
				{
					IJsonWrapper jsonWrapper2 = JsonMapper.ReadValue(factory, reader);
					if (jsonWrapper2 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					jsonWrapper.Add(jsonWrapper2);
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				jsonWrapper.SetJsonType(JsonType.Object);
				for (;;)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string text = (string)reader.Value;
					jsonWrapper[text] = JsonMapper.ReadValue(factory, reader);
				}
			}
			return jsonWrapper;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006E7E File Offset: 0x0000527E
		private static void ReadSkip(JsonReader reader)
		{
			JsonMapper.ToWrapper(() => new JsonMockWrapper(), reader);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006EA4 File Offset: 0x000052A4
		private static void RegisterBaseExporters()
		{
			JsonMapper.base_exporters_table[typeof(byte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((byte)obj));
			};
			JsonMapper.base_exporters_table[typeof(char)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((char)obj));
			};
			JsonMapper.base_exporters_table[typeof(DateTime)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((DateTime)obj, JsonMapper.datetime_format));
			};
			JsonMapper.base_exporters_table[typeof(decimal)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((decimal)obj);
			};
			JsonMapper.base_exporters_table[typeof(sbyte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((sbyte)obj));
			};
			JsonMapper.base_exporters_table[typeof(short)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((short)obj));
			};
			JsonMapper.base_exporters_table[typeof(ushort)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((ushort)obj));
			};
			JsonMapper.base_exporters_table[typeof(uint)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToUInt64((uint)obj));
			};
			JsonMapper.base_exporters_table[typeof(ulong)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((ulong)obj);
			};
			JsonMapper.base_exporters_table[typeof(float)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToDouble((float)obj));
			};
			JsonMapper.base_exporters_table[typeof(long)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((long)obj);
			};
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000070CC File Offset: 0x000054CC
		private static void RegisterBaseImporters()
		{
			ImporterFunc importerFunc = (object input) => Convert.ToByte((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(byte), importerFunc);
			importerFunc = (object input) => Convert.ToUInt64((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ulong), importerFunc);
			importerFunc = (object input) => Convert.ToSByte((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(sbyte), importerFunc);
			importerFunc = (object input) => Convert.ToInt16((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(short), importerFunc);
			importerFunc = (object input) => Convert.ToUInt16((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ushort), importerFunc);
			importerFunc = (object input) => Convert.ToUInt32((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(uint), importerFunc);
			importerFunc = (object input) => Convert.ToSingle((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(float), importerFunc);
			importerFunc = (object input) => Convert.ToDouble((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(double), importerFunc);
			importerFunc = (object input) => Convert.ToDecimal((double)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(double), typeof(decimal), importerFunc);
			importerFunc = (object input) => Convert.ToSingle((float)((double)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(double), typeof(float), importerFunc);
			importerFunc = (object input) => Convert.ToUInt32((long)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(long), typeof(uint), importerFunc);
			importerFunc = (object input) => Convert.ToChar((string)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(char), importerFunc);
			importerFunc = (object input) => Convert.ToDateTime((string)input, JsonMapper.datetime_format);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(DateTime), importerFunc);
			importerFunc = (object input) => Convert.ToInt64((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(long), importerFunc);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000742F File Offset: 0x0000582F
		private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
		{
			if (!table.ContainsKey(json_type))
			{
				table.Add(json_type, new Dictionary<Type, ImporterFunc>());
			}
			table[json_type][value_type] = importer;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007458 File Offset: 0x00005858
		private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
		{
			if (depth > JsonMapper.max_nesting_depth)
			{
				throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", obj.GetType()));
			}
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj is IJsonWrapper)
			{
				if (writer_is_private)
				{
					writer.TextWriter.Write(((IJsonWrapper)obj).ToJson());
				}
				else
				{
					((IJsonWrapper)obj).ToJson(writer);
				}
				return;
			}
			if (obj is string)
			{
				writer.Write((string)obj);
				return;
			}
			if (obj is double)
			{
				writer.Write((double)obj);
				return;
			}
			if (obj is int)
			{
				writer.Write((int)obj);
				return;
			}
			if (obj is bool)
			{
				writer.Write((bool)obj);
				return;
			}
			if (obj is long)
			{
				writer.Write((long)obj);
				return;
			}
			if (obj is Array)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator = ((Array)obj).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						JsonMapper.WriteValue(obj2, writer, writer_is_private, depth + 1);
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
			if (obj is IList)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator2 = ((IList)obj).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj3 = enumerator2.Current;
						JsonMapper.WriteValue(obj3, writer, writer_is_private, depth + 1);
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
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IDictionary)
			{
				writer.WriteObjectStart();
				IDictionaryEnumerator enumerator3 = ((IDictionary)obj).GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj4 = enumerator3.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj4;
						writer.WritePropertyName((string)dictionaryEntry.Key);
						JsonMapper.WriteValue(dictionaryEntry.Value, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = enumerator3 as IDisposable) != null)
					{
						disposable3.Dispose();
					}
				}
				writer.WriteObjectEnd();
				return;
			}
			Type type = obj.GetType();
			if (JsonMapper.custom_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc = JsonMapper.custom_exporters_table[type];
				exporterFunc(obj, writer);
				return;
			}
			if (JsonMapper.base_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc2 = JsonMapper.base_exporters_table[type];
				exporterFunc2(obj, writer);
				return;
			}
			if (obj is Enum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (underlyingType == typeof(long) || underlyingType == typeof(uint) || underlyingType == typeof(ulong))
				{
					writer.Write((ulong)obj);
				}
				else
				{
					writer.Write((int)obj);
				}
				return;
			}
			JsonMapper.AddTypeProperties(type);
			IList<PropertyMetadata> list = JsonMapper.type_properties[type];
			writer.WriteObjectStart();
			foreach (PropertyMetadata propertyMetadata in list)
			{
				if (propertyMetadata.IsField)
				{
					writer.WritePropertyName(propertyMetadata.Info.Name);
					JsonMapper.WriteValue(((FieldInfo)propertyMetadata.Info).GetValue(obj), writer, writer_is_private, depth + 1);
				}
				else
				{
					PropertyInfo propertyInfo = (PropertyInfo)propertyMetadata.Info;
					if (propertyInfo.CanRead)
					{
						writer.WritePropertyName(propertyMetadata.Info.Name);
						JsonMapper.WriteValue(propertyInfo.GetValue(obj, null), writer, writer_is_private, depth + 1);
					}
				}
			}
			writer.WriteObjectEnd();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007858 File Offset: 0x00005C58
		public static string ToJson(object obj)
		{
			object obj2 = JsonMapper.static_writer_lock;
			string text;
			lock (obj2)
			{
				JsonMapper.static_writer.Reset();
				JsonMapper.WriteValue(obj, JsonMapper.static_writer, true, 0);
				text = JsonMapper.static_writer.ToString();
			}
			return text;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000078B0 File Offset: 0x00005CB0
		public static void ToJson(object obj, JsonWriter writer)
		{
			JsonMapper.WriteValue(obj, writer, false, 0);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000078BB File Offset: 0x00005CBB
		public static JsonData ToObject(JsonReader reader)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), reader);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000078E8 File Offset: 0x00005CE8
		public static JsonData ToObject(TextReader reader)
		{
			JsonReader jsonReader = new JsonReader(reader);
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), jsonReader);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007924 File Offset: 0x00005D24
		public static JsonData ToObject(string json)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), json);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000794E File Offset: 0x00005D4E
		public static T ToObject<T>(JsonReader reader)
		{
			return (T)((object)JsonMapper.ReadValue(typeof(T), reader));
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007968 File Offset: 0x00005D68
		public static T ToObject<T>(TextReader reader)
		{
			JsonReader jsonReader = new JsonReader(reader);
			return (T)((object)JsonMapper.ReadValue(typeof(T), jsonReader));
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007994 File Offset: 0x00005D94
		public static T ToObject<T>(string json)
		{
			JsonReader jsonReader = new JsonReader(json);
			return (T)((object)JsonMapper.ReadValue(typeof(T), jsonReader));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000079BD File Offset: 0x00005DBD
		public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
		{
			return JsonMapper.ReadValue(factory, reader);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000079C8 File Offset: 0x00005DC8
		public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
		{
			JsonReader jsonReader = new JsonReader(json);
			return JsonMapper.ReadValue(factory, jsonReader);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000079E4 File Offset: 0x00005DE4
		public static void RegisterExporter<T>(ExporterFunc<T> exporter)
		{
			ExporterFunc exporterFunc = delegate(object obj, JsonWriter writer)
			{
				exporter((T)((object)obj), writer);
			};
			JsonMapper.custom_exporters_table[typeof(T)] = exporterFunc;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00007A20 File Offset: 0x00005E20
		public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
		{
			ImporterFunc importerFunc = (object input) => importer((TJson)((object)input));
			JsonMapper.RegisterImporter(JsonMapper.custom_importers_table, typeof(TJson), typeof(TValue), importerFunc);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007A66 File Offset: 0x00005E66
		public static void UnregisterExporters()
		{
			JsonMapper.custom_exporters_table.Clear();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007A72 File Offset: 0x00005E72
		public static void UnregisterImporters()
		{
			JsonMapper.custom_importers_table.Clear();
		}

		// Token: 0x04000091 RID: 145
		private static int max_nesting_depth = 100;

		// Token: 0x04000092 RID: 146
		private static IFormatProvider datetime_format = DateTimeFormatInfo.InvariantInfo;

		// Token: 0x04000093 RID: 147
		private static IDictionary<Type, ExporterFunc> base_exporters_table = new Dictionary<Type, ExporterFunc>();

		// Token: 0x04000094 RID: 148
		private static IDictionary<Type, ExporterFunc> custom_exporters_table = new Dictionary<Type, ExporterFunc>();

		// Token: 0x04000095 RID: 149
		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();

		// Token: 0x04000096 RID: 150
		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();

		// Token: 0x04000097 RID: 151
		private static IDictionary<Type, ArrayMetadata> array_metadata = new Dictionary<Type, ArrayMetadata>();

		// Token: 0x04000098 RID: 152
		private static readonly object array_metadata_lock = new object();

		// Token: 0x04000099 RID: 153
		private static IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();

		// Token: 0x0400009A RID: 154
		private static readonly object conv_ops_lock = new object();

		// Token: 0x0400009B RID: 155
		private static IDictionary<Type, ObjectMetadata> object_metadata = new Dictionary<Type, ObjectMetadata>();

		// Token: 0x0400009C RID: 156
		private static readonly object object_metadata_lock = new object();

		// Token: 0x0400009D RID: 157
		private static IDictionary<Type, IList<PropertyMetadata>> type_properties = new Dictionary<Type, IList<PropertyMetadata>>();

		// Token: 0x0400009E RID: 158
		private static readonly object type_properties_lock = new object();

		// Token: 0x0400009F RID: 159
		private static JsonWriter static_writer = new JsonWriter();

		// Token: 0x040000A0 RID: 160
		private static readonly object static_writer_lock = new object();
	}
}
