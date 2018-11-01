
namespace TEDCore.Cipher
{
    public class CipherManager : Singleton<CipherManager>, ICipher
    {
        private const int CAESAR_CIPHER_KEY = 5;
        private const int AFFINE_CIPHER_KEY_A = 21;
        private const int AFFINE_CIPHER_KEY_B = 10;
        private const string SIMPLE_SUBSTITUTION_CIPHER_KEY = "Don't try to hack this because you can't make it easily.";

        public string Encrypt(string plainText)
        {
            string cipherText = plainText;

            CaesarCipher caesarCipher = new CaesarCipher();
            caesarCipher.SetKey(CAESAR_CIPHER_KEY);
            cipherText = caesarCipher.Encrypt(cipherText);

            AffineCipher affineCipher = new AffineCipher();
            affineCipher.SetKeys(new int[] { AFFINE_CIPHER_KEY_A, AFFINE_CIPHER_KEY_B });
            cipherText = affineCipher.Encrypt(cipherText);

            SimpleSubstitutionCipher simpleSubstitutionCipher = new SimpleSubstitutionCipher();
            simpleSubstitutionCipher.SetKey(SIMPLE_SUBSTITUTION_CIPHER_KEY);
            cipherText = simpleSubstitutionCipher.Encrypt(cipherText);

            return cipherText;
        }

        public string Decrypt(string cipherText)
        {
            string plainText = cipherText;

            SimpleSubstitutionCipher simpleSubstitutionCipher = new SimpleSubstitutionCipher();
            simpleSubstitutionCipher.SetKey(SIMPLE_SUBSTITUTION_CIPHER_KEY);
            plainText = simpleSubstitutionCipher.Decrypt(plainText);

            AffineCipher affineCipher = new AffineCipher();
            affineCipher.SetKeys(new int[] { AFFINE_CIPHER_KEY_A, AFFINE_CIPHER_KEY_B });
            plainText = affineCipher.Decrypt(plainText);

            CaesarCipher caesarCipher = new CaesarCipher();
            caesarCipher.SetKey(CAESAR_CIPHER_KEY);
            plainText = caesarCipher.Decrypt(plainText);

            return plainText;
        }
    }
}
