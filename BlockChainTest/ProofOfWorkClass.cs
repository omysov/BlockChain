using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BlockChainTest.BlockClass;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlockChainTest
{
    public class ProofOfWorkClass
    {
        public ProofOfWorkClass() { }

        public const int targetBits = 24;
        public const string dificult_ = "000";

        public struct ProofOfWork
        {
            Block block;
            long target;
            public int nonce_resutl;

            public ProofOfWork NewProofOfWork(Block block)
            {
                long target = 1; //init 
                var target_ = (uint)target << (256 - targetBits);
                // сдвиг на (256 - tatgetBits)
                ProofOfWork pow = new ProofOfWork() { block = block, target = target_};

                return pow;
            }

            public byte[] prepareData(int nonce)
            {
                byte[] timestamp = BitConverter.GetBytes(block.TimeStamp);
                byte[] target = BitConverter.GetBytes(targetBits);
                byte[] _nonce = BitConverter.GetBytes(nonce);

                string headers_str = HashStrig.GetHashString(timestamp)
                    + HashStrig.GetHashString(block.PrevBlockHash)
                    + HashStrig.GetHashString(block.Data)
                    + HashStrig.GetHashString(target)
                    + HashStrig.GetHashString(_nonce);

                byte[] data = Encoding.UTF8.GetBytes(headers_str);

               return data;
            }

            public Dictionary<string, byte[]> Run()
            {
                Dictionary<string, byte[]> keyValuePairs = new();
                long hasInt;
                string dificult = dificult_;
                byte[] hash = new byte[32];
                int nonce;
                SHA256 sHA = SHA256.Create();

                for (nonce = 0; nonce < int.MaxValue; nonce++)
                {
                    byte[] data = prepareData(nonce); 
                    hash = sHA.ComputeHash(data);
                    string hash_str = HashStrig.GetHashString(hash);
                    Console.WriteLine(hash_str);
                    if (hash_str[..dificult.Length] == dificult)
                    {
                        keyValuePairs["nonce"] = BitConverter.GetBytes(nonce);
                        break;
                    } 
                }
                keyValuePairs["hash"] = hash;
                return keyValuePairs;
            }

            public bool Validate()
            {
                SHA256 sHA = SHA256.Create();
                byte[] data =  prepareData(block.Nonce);
                byte[] hash = sHA.ComputeHash(data);
                string hash_str = HashStrig.GetHashString(hash);

                if (hash_str[..dificult_.Length] == dificult_ &&
                    HashStrig.GetHashString(block.Hash) == HashStrig.GetHashString(hash))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
