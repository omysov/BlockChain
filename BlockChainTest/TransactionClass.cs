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

            public Transaction NewCoinbaseTX(string to, string data)
            {
                if (data == "")
                {
                    Console.WriteLine($"Reward to {0}", to);
                    data = "Reward to " + to;
                }

                TXInput txin = new TXInput() { Txid = null, Vout = -1, ScriptSig = data };
                TXOutput txout = new TXOutput() { Value = subsidy, ScriptPubKey = to};
                Transaction tx = new Transaction() { ID = null, Vin = TXInput{ txin }, Vout = TXOutput{ txout } };
                tx.SetID();
                return tx;
            }
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
