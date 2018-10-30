
namespace TEDCore.Cipher
{
    public interface ICipher
    {
        string Encipher(string plainText);
        string Decipher(string cipherText);
    }
}
