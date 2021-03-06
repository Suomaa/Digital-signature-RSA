using System;
using System.Security.Cryptography;

namespace ECP
{
    class ECP
    {
        
        private readonly BigInteger n;
        /// <summary>
        /// ������ n
        /// </summary>
        public string N
        {
            get { return n.ToString(); }
        }

        private readonly BigInteger e;
        /// <summary>
        /// �������� ����
        /// </summary>
        public string E
        {
            get { return e.ToString(); }
        }
   
        private readonly BigInteger d;
        /// <summary>
        /// �������� ����
        /// </summary>
        public string D
        {
            get { return d.ToString(); }
        }

        public ECP()
        {
            Random uy = new Random();

            BigInteger p = BigInteger.genPseudoPrime(129, 13, uy);
            BigInteger q = BigInteger.genPseudoPrime(128, 13, uy);

            n = p * q;
            p--;
            q--;
            BigInteger w = p * q;

            e = new BigInteger();
            BigInteger one = new BigInteger(1);
            do
            {
            ar:
                e.genRandomBits(256, uy);
                if (e > n) goto ar;
            }
            while (BigInteger.NOD(e, w) != one);
            // d - �������� ���� (�������� � � �� ������ w)
            d = BigInteger.Inverse(e, w);
        }

        /// <summary>
        /// ��������� ��� �������� ������
        /// </summary>
        /// <param name="data">�������� ������</param>
        /// <returns>160-������ ���</returns>
        private static byte[] SHA1hash(byte[] data)
        {
            // This is one implementation of the abstract class SHA1.
            SHA1 sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(data);
        }   
        /// <summary>
        /// ������������ ��� ���������-Text
        /// </summary>
        /// <param name="Text">���������</param>
        /// <returns>�������</returns>
        public byte[] CreateSignature(byte[] Text)
        {
            byte[] hash = SHA1hash(Text);
            BigInteger BI_Text = new BigInteger(hash);
            return BI_Text.modPow(d, n).getBytes();
        }

        /// <summary>
        /// �������� �������� ������� Signature ������ Text
        /// </summary>
        /// <param name="Text">�����</param>
        /// <param name="Signature">�������</param>
        /// <param name="e">�������� ����</param>
        /// <param name="n">������</param>
        /// <returns>true ���� ������� �����, false � �������� ������</returns>
        public bool VerifySignature(byte[] Text, byte[] Signature, byte[] e, byte[] n)
        {
            BigInteger R = new BigInteger(SHA1hash(Text));
            BigInteger S = new BigInteger(Signature).modPow(new BigInteger(e),new BigInteger(n));
            if (R==S)
                return true;
            return false;
        }
    }
}
