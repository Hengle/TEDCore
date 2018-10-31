
namespace TEDCore.Cipher
{
    public class SimpleSubstitutionCipher : ICipher
    {
        private string m_plainAlphabet = "abcdefghijklmnopqrstuvwxyz";
        private string m_cipherAlphabet = "zebrascdfghijklmnopqtuvwxy";
        private string m_key = "zebras";

        public void SetKey(string key)
        {
            m_key = key.ToLower();
            m_cipherAlphabet = string.Empty;

            for (int i = 0; i < m_key.Length; i++)
            {
                if(m_cipherAlphabet.Contains(m_key[i].ToString()))
                {
                    continue;
                }

                if(!m_plainAlphabet.Contains(m_key[i].ToString()))
                {
                    continue;
                }

                m_cipherAlphabet += m_key[i];
            }

            for (int i = 0; i < m_plainAlphabet.Length; i++)
            {
                if (m_cipherAlphabet.Contains(m_plainAlphabet[i].ToString()))
                {
                    continue;
                }

                m_cipherAlphabet += m_plainAlphabet[i];
            }
        }

        public string Encipher(string plainText)
        {
            string output = string.Empty;

            foreach (char c in plainText)
            {
                output += Cipher(c, m_plainAlphabet, m_cipherAlphabet);
            }

            return output;
        }

        public string Decipher(string cipherText)
        {
            string output = string.Empty;

            foreach (char c in cipherText)
            {
                output += Cipher(c, m_cipherAlphabet, m_plainAlphabet);
            }

            return output;
        }

        private char Cipher(char c, string plainAlphabet, string cipherAlphabet)
        {
            if (!char.IsLetter(c))
            {
                return c;
            }

            int index = plainAlphabet.IndexOf(char.ToLower(c));
            return char.IsUpper(c) ? char.ToUpper(cipherAlphabet[index]) : cipherAlphabet[index];
        }
    }
}
