
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

        public string Encipher(string text)
        {
            string output = string.Empty;

            foreach (char c in text)
            {
                output += Cipher(c, m_key);
            }

            return output;
        }

        public string Decipher(string text)
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

            char d = char.IsUpper(c) ? 'A' : 'a';
            int value = (c - d + key) % MOD;
            if (value < 0)
            {
                value += MOD;
            }

            return (char)(value + d);
        }
    }
}
