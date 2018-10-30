
namespace TEDCore.Cipher
{
    public class SimpleSubstitutionCipher : ICipher
    {
        private string m_plainAlphabet = "abcdefghijklmnopqrstuvwxyz";
        private string m_cipherAlphabet = "yhkqgvxfoluapwmtzecjdbsnri";

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
