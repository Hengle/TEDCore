
namespace TEDCore.Cipher
{
    public interface ICipher
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
