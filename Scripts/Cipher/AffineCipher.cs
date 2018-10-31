
namespace TEDCore.Cipher
{
    public class AffineCipher : ICipher
    {
        private const int MOD = 26;

        //a, b, a^(-1)
        private int[] m_keys = {5, 8, 21};

        public void SetKeys(params int[] keys)
        {
            if(keys.Length < 2)
            {
                TEDDebug.LogError("There should be two keys for AffineCipher.");
                return;
            }

            if (keys[0] >= MOD)
            {
                TEDDebug.LogError("key[0] should be 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 25.");
                return;
            }

            if (GetGCD(keys[0], MOD) != 1)
            {
                TEDDebug.LogErrorFormat("The gcd({0}, {1}) is not equal to 1, should be 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 25.", keys[0], MOD);
                return;
            }

            m_keys[0] = keys[0];
            m_keys[1] = keys[1];
            m_keys[2] = GetModularMultiplicativeInverse(keys[0]);
        }

        private int GetGCD(int m, int n)
        {
            if(m % n == 0)
            {
                return n;
            }

            return GetGCD(n, m % n);
        }

        public string Encipher(string plainText)
        {
            string cipherText = string.Empty;

            foreach(char c in plainText)
            {
                cipherText += Encipher(c);
            }

            return cipherText;
        }

        private char Encipher(char c)
        {
            if (!char.IsLetter(c))
            {
                return c;
            }

            char d = char.IsUpper(c) ? 'A' : 'a';
            return (char)((m_keys[0] * (c - d) + m_keys[1]) % MOD + d);
        }

        public string Decipher(string cipherText)
        {
            string plainText = string.Empty;

            foreach (char c in cipherText)
            {
                plainText += Decipher(c);
            }

            return plainText;
        }

        private char Decipher(char c)
        {
            if (!char.IsLetter(c))
            {
                return c;
            }

            char d = char.IsUpper(c) ? 'A' : 'a';
            int dx = (m_keys[2] * (c - d - m_keys[1])) % MOD;
            dx = dx < 0 ? dx + MOD : dx;

            return (char)(dx + d);
        }

        private int GetModularMultiplicativeInverse(int a)
        {
            int count = 0;
            int result = (1 + MOD * count) / a;

            while((a * result) % MOD != 1)
            {
                count++;
                result = (1 + MOD * count) / a;
            }

            return result;
        }
    }
}
