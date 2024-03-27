using System;
using System.IO;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class TextureLoader : MonoBehaviour
{
	// Token: 0x06000EB8 RID: 3768 RVA: 0x00081AD4 File Offset: 0x0007FED4
	public static Texture2D LoadTGA(string fileName)
	{
		Texture2D texture2D;
		using (FileStream fileStream = File.OpenRead(fileName))
		{
			texture2D = TextureLoader.LoadTGA(fileStream);
		}
		return texture2D;
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x00081B14 File Offset: 0x0007FF14
	public static Texture2D LoadDDSManual(string ddsPath)
	{
		Texture2D texture2D2;
		try
		{
			byte[] array = File.ReadAllBytes(ddsPath);
			byte b = array[4];
			if (b != 124)
			{
				throw new Exception("Invalid DDS DXTn texture. Unable to read");
			}
			int num = (int)array[13] * 256 + (int)array[12];
			int num2 = (int)array[17] * 256 + (int)array[16];
			byte b2 = array[87];
			TextureFormat textureFormat = TextureFormat.DXT5;
			if (b2 == 49)
			{
				textureFormat = TextureFormat.DXT1;
			}
			if (b2 == 53)
			{
				textureFormat = TextureFormat.DXT5;
			}
			int num3 = 128;
			byte[] array2 = new byte[array.Length - num3];
			Buffer.BlockCopy(array, num3, array2, 0, array.Length - num3);
			FileInfo fileInfo = new FileInfo(ddsPath);
			Texture2D texture2D = new Texture2D(num2, num, textureFormat, false);
			texture2D.LoadRawTextureData(array2);
			texture2D.Apply();
			texture2D.name = fileInfo.Name;
			texture2D2 = texture2D;
		}
		catch (Exception ex)
		{
			Debug.LogError("Error: Could not load DDS");
			texture2D2 = new Texture2D(8, 8);
		}
		return texture2D2;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x00081C10 File Offset: 0x00080010
	public static void SetNormalMap(ref Texture2D tex)
	{
		Color[] pixels = tex.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			Color color = pixels[i];
			color.r = pixels[i].g;
			color.a = pixels[i].r;
			pixels[i] = color;
		}
		tex.SetPixels(pixels);
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x00081C84 File Offset: 0x00080084
	public static Texture2D LoadTexture(string fn, bool normalMap = false)
	{
		if (!File.Exists(fn))
		{
			return null;
		}
		string text = Path.GetExtension(fn).ToLower();
		if (text == ".png" || text == ".jpg")
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.LoadImage(File.ReadAllBytes(fn));
			if (normalMap)
			{
				TextureLoader.SetNormalMap(ref texture2D);
			}
			return texture2D;
		}
		if (text == ".dds")
		{
			Texture2D texture2D2 = TextureLoader.LoadDDSManual(fn);
			if (normalMap)
			{
				TextureLoader.SetNormalMap(ref texture2D2);
			}
			return texture2D2;
		}
		if (text == ".tga")
		{
			Texture2D texture2D3 = TextureLoader.LoadTGA(fn);
			if (normalMap)
			{
				TextureLoader.SetNormalMap(ref texture2D3);
			}
			return texture2D3;
		}
		Debug.Log("texture not supported : " + fn);
		return null;
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x00081D4C File Offset: 0x0008014C
	public static Texture2D LoadTGA(Stream TGAStream)
	{
		Texture2D texture2D2;
		using (BinaryReader binaryReader = new BinaryReader(TGAStream))
		{
			binaryReader.BaseStream.Seek(12L, SeekOrigin.Begin);
			short num = binaryReader.ReadInt16();
			short num2 = binaryReader.ReadInt16();
			int num3 = (int)binaryReader.ReadByte();
			binaryReader.BaseStream.Seek(1L, SeekOrigin.Current);
			Texture2D texture2D = new Texture2D((int)num, (int)num2);
			Color32[] array = new Color32[(int)(num * num2)];
			if (num3 == 32)
			{
				for (int i = 0; i < (int)(num * num2); i++)
				{
					byte b = binaryReader.ReadByte();
					byte b2 = binaryReader.ReadByte();
					byte b3 = binaryReader.ReadByte();
					byte b4 = binaryReader.ReadByte();
					array[i] = new Color32(b3, b2, b, b4);
				}
			}
			else
			{
				if (num3 != 24)
				{
					throw new Exception("TGA texture had non 32/24 bit depth.");
				}
				for (int j = 0; j < (int)(num * num2); j++)
				{
					byte b5 = binaryReader.ReadByte();
					byte b6 = binaryReader.ReadByte();
					byte b7 = binaryReader.ReadByte();
					array[j] = new Color32(b7, b6, b5, 1);
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			texture2D2 = texture2D;
		}
		return texture2D2;
	}
}
