using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlockChainTest.TransactionClass;

namespace BlockChainTest
{
    public class TransactionClass
    {

        public struct Transaction
        {
            public byte[] ID;
            public TXInput[] Vin;
            public TXOutput[] Vout;

        }

        public struct TXOutput
        {
            public int Value;
            public string ScriptPubKey;
        }

        public struct TXInput
        {
            public byte[] Txid;
            public int Vout;
            public string ScriptSig;
        }
    }
}
