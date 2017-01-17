
public class AssetBundleExtension
{
	private static string[] _extension =
	{
		".MP3",
		".PREFAB",
		".PNG",
		".JPG",
		".TXT",
		".ASSET",
	};


	public static bool IsLegal(string extension)
	{
		for (int cnt = 0; cnt < _extension.Length; cnt++)
		{
			if (extension.ToUpper() == _extension [cnt])
			{
				return true;
			}
		}

		return false;
	}
}

