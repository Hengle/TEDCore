
namespace TEDCore.Cipher
{
    public class CipherManager : Singleton<CipherManager>
    {
        public enum CipherType
        {
            Caesar,
            Affine,
        }

        private CipherType m_cipherType = CipherType.Caesar;

        public void SetCipherType(CipherType cipherType)
        {
            m_cipherType = cipherType;
        }

        public string Encipher(string plainText)
        {
            return GetCipher().Encipher(plainText);
        }

        public string Decipher(string cipherText)
        {
            return GetCipher().Decipher(cipherText);
        }

        private ICipher GetCipher()
        {
            ICipher cipher = null;

            switch(m_cipherType)
            {
                case CipherType.Caesar:
                    cipher = new CaesarCipher();
                    break;
                case CipherType.Affine:
                    cipher = new AffineCipher();
                    break;
            }

            return cipher;
        }
    }
}
