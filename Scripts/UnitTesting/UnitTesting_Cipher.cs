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
    public void TestCaesarCipher()
    {
        if(string.IsNullOrEmpty(m_plainText))
        {
            TEDDebug.LogError("Set test first.");
            return;
        }

        CaesarCipher caesarCipher = new CaesarCipher();
        string cipherText = caesarCipher.Encrypt(m_plainText);
        TEDDebug.Log("CaesarCipher CipherText = " + cipherText);
        TEDDebug.Log("CaesarCipher PlainText = " + caesarCipher.Decrypt(cipherText));
    }

    [TestButton]
    public void TestAffineCipher()
    {
        if (string.IsNullOrEmpty(m_plainText))
        {
            TEDDebug.LogError("Set test first.");
            return;
        }

        AffineCipher affineCipher = new AffineCipher();
        string cipherText = affineCipher.Encrypt(m_plainText);
        TEDDebug.Log("AffinCipher CipherText = " + cipherText);
        TEDDebug.Log("AffinCipher PlainText = " + affineCipher.Decrypt(cipherText));
    }

    [TestButton]
    public void TestSimpleSubstitutionCipher()
    {
        if (string.IsNullOrEmpty(m_plainText))
        {
            TEDDebug.LogError("Set test first.");
            return;
        }

        SimpleSubstitutionCipher simpleSubstitutionCipher = new SimpleSubstitutionCipher();
        string cipherText = simpleSubstitutionCipher.Encrypt(m_plainText);
        TEDDebug.Log("SimpleSubstitutionCipher CipherText = " + cipherText);
        TEDDebug.Log("SimpleSubstitutionCipher PlainText = " + simpleSubstitutionCipher.Decrypt(cipherText));
    }
}
