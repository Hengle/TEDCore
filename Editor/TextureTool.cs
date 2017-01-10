using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureTool
{
	[MenuItem("TEDTools/Change Texture Format/Truecolor")]
	public static void ChangeTextureFormat_Truecolor()
	{
		SelectedChangeTextureFormatSettings(TextureImporterFormat.AutomaticTruecolor);
	}


	[MenuItem("TEDTools/Change Texture Format/Android/RGBA Compressed ASTC 6x6 block")]
	public static void ChangeTextureFormat_RGBA_ASTC_6x6_block()
	{
		SelectedChangeTextureFormatSettings(TextureImporterFormat.ASTC_RGBA_6x6);
	}
	

	[MenuItem("TEDTools/Change Texture Format/iOS/RGBA Compressed PVRTC 4 bits")]
	public static void ChangeTextureFormat_RGBA_PVRTC_4_bits()
	{
		SelectedChangeTextureFormatSettings(TextureImporterFormat.PVRTC_RGBA4);
	}


	private static void SelectedChangeTextureFormatSettings(TextureImporterFormat newFormat, int quality = 100)
	{ 
		Object[] textures = GetSelectedTextures(); 
		Selection.objects = new Object[0];
		
		foreach (Texture2D texture in textures)
		{
			string path = AssetDatabase.GetAssetPath(texture);
			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter; 
			textureImporter.textureType = TextureImporterType.Advanced;
			textureImporter.npotScale = TextureImporterNPOTScale.None;
			textureImporter.alphaIsTransparency = true;
			textureImporter.spriteImportMode = SpriteImportMode.Single;
			textureImporter.mipmapEnabled = false;
			textureImporter.textureFormat = newFormat;
			textureImporter.compressionQuality = quality;
			AssetDatabase.ImportAsset(path); 
		}
	}


	private static Object[] GetSelectedTextures() 
	{ 
		return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets); 
	}
}
