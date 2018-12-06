using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ImageBatch : BaseBatch
{
    public ImageBatch(Transform root) : base(root)
    {
    }

    public override string GetTypeName()
    {
        return "Image";
    }

    public override string GetBatchKey()
    {
        Texture texture = null;
        Image image = m_root.GetComponent<Image>();
        string materialName = string.Empty;
        string shaderName = string.Empty;

        if (image != null)
        {
            texture = image.mainTexture;

            if (image.material != null)
            {
                materialName = image.material.name;
                shaderName = image.material.shader.name;
            }
        }
        else
        {
            RawImage rawImage = m_root.GetComponent<RawImage>();
            if (rawImage != null)
            {
                texture = rawImage.mainTexture;

                if (rawImage.material != null)
                {
                    materialName = rawImage.material.name;
                    shaderName = rawImage.material.shader.name;
                }
            }
        }

        if (texture == null)
        {
            return string.Empty;
        }

        string assetPath = string.Empty;
        string atlasName = string.Empty;

#if UNITY_EDITOR
        assetPath = AssetDatabase.GetAssetPath(texture);

        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            atlasName = importer.spritePackingTag;
        }
#endif

        return assetPath + atlasName + materialName + shaderName;
    }
}