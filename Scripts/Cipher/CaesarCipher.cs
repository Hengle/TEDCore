
namespace TEDCore.Cipher
{
    public class CaesarCipher : ICipher
    {
        private int m_key = 10;

        public void SetKey(int key)
        {
            m_key = key;
        }

        public string Encipher(string text)
        {
            string output = string.Empty;

            foreach(char c in text)
            {
                output += Cipher(c, m_key);
            }

            return output;
        }

        private string Encipher(string text, int key)
        {
            string output = string.Empty;

            foreach (char c in text)
            {
                output += Cipher(c, key);
            }

            return output;
        }

        public string Decipher(string text)
        {
            return Encipher(text, 26 - m_key);
        }

        private char Cipher(char c, int key)
        {
            if (!char.IsLetter(c))
            {
                return c;
            }

            char d = char.IsUpper(c) ? 'A' : 'a';
            return (char)((((c + key) - d) % 26) + d);
        }
    }
}
