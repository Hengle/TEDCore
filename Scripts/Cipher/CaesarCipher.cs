
namespace TEDCore.Cipher
{
    public class CaesarCipher : ICipher
    {
        private const int MOD = 26;
        private int m_key = 10;

        public void SetKey(int key)
        {
            m_key = key;
        }

        public string Encrypt(string text)
        {
            string output = string.Empty;

            foreach (char c in text)
            {
                output += Cipher(c, m_key);
            }

            return output;
        }

        public string Decrypt(string text)
        {
            string output = string.Empty;

            foreach (char c in text)
            {
                output += Cipher(c, -m_key);
            }

            return output;
        }

        private char Cipher(char c, int key)
        {
            if (!char.IsLetter(c))
            {
                return c;
            }

            char firstChar = char.IsUpper(c) ? 'A' : 'a';
            int x = c - firstChar;
            int result = (x + key) % MOD;
            if (result < 0)
            {
                result += MOD;
            }

            return (char)(firstChar + result);
        }
    }
}
