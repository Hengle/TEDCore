using UnityEngine;
using TEDCore.Cipher;
using TEDCore.UnitTesting;
using TEDCore;

public class UnitTesting_Cipher : BaseUnitTesting
{
    [SerializeField] private string m_text;

    [TestInputField]
    public void SetText(string value)
    {
        m_text = value;
    }

    [TestInputField]
    public void CaesarCipher(string value)
    {
        if(string.IsNullOrEmpty(m_text))
        {
            TEDDebug.LogError("Set test first.");
            return;
        }

        int key = int.Parse(value);
        CaesarCipher cipher = new CaesarCipher(key);
        string encipher = cipher.Encipher(m_text);
        TEDDebug.Log("CaesarEncipher = " + encipher);
        TEDDebug.Log("CaesarDecipher = " + cipher.Decipher(encipher));
    }
}
