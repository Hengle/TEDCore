
namespace TEDCore.Cipher
{
    public class AffineCipher : ICipher
    {
        private int m_mod = 26;

        private int m_a = 5;
        private int m_b = 8;
        private int m_a1 = 21;

        public string Encipher(string plainText)
        {
            string cipherText = string.Empty;

            foreach(char c in plainText)
            {
                cipherText += Encipher(c, m_a, m_b);
            }

            return cipherText;
        }

        private char Encipher(char c, int a, int b)
        {
            if (!char.IsLetter(c))
            {
                return c;
            }

            char d = char.IsUpper(c) ? 'A' : 'a';
            return (char)((a * (c - d) + b) % m_mod + d);
        }

        public string Decipher(string cipherText)
        {
            string plainText = string.Empty;

            foreach (char c in cipherText)
            {
                plainText += Decipher(c, m_a1, m_b);
            }

            return plainText;
        }

        private char Decipher(char c, int a1, int b)
        {
            if (!char.IsLetter(c))
            {
                return c;
            }

            char d = char.IsUpper(c) ? 'A' : 'a';
            int y = c - d;
            int dx = (a1 * (c - d - b)) % m_mod;
            dx = dx < 0 ? dx + m_mod : dx;

            return (char)(dx + d);
        }
    }
}
