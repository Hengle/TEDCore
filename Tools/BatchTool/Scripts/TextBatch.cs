using UnityEngine;
using UnityEngine.UI;

public class TextBatch : BaseBatch
{
    public TextBatch(Transform root) : base(root)
    {
    }

    public override string GetTypeName()
    {
        return "Text";
    }

    public override string GetBatchKey()
    {
        Text text = m_root.GetComponent<Text>();
        if (text == null)
        {
            return string.Empty;
        }

        string fontName = text.font.name;
        string materialName = string.Empty;
        string shaderName = string.Empty;

        if (text.material != null)
        {
            materialName = text.material.name;
            shaderName = text.material.shader.name;
        }

        return fontName + materialName + shaderName;
    }
}