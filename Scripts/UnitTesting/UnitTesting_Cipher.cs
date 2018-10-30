using UnityEngine;
using TEDCore.Cipher;
using TEDCore.UnitTesting;
using TEDCore;

public class UnitTesting_Cipher : BaseUnitTesting
{
    [SerializeField] private string m_plainText;

    [TestInputField]
    public void SetPlainText(string value)
    {
        m_plainText = value;
    }

    [TestButton]
    public void CaesarCipher()
    {
        if(string.IsNullOrEmpty(m_plainText))
        {
            TEDDebug.LogError("Set test first.");
            return;
        }

        CipherManager.Instance.SetCipherType(CipherManager.CipherType.Caesar);
        string cipherText = CipherManager.Instance.Encipher(m_plainText);
        TEDDebug.Log("CaesarCipher CipherText = " + cipherText);
        TEDDebug.Log("CaesarCipher PlainText = " + CipherManager.Instance.Decipher(cipherText));
    }
}
