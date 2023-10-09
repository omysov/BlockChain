using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainTest
{
    public class BlockClass
    {
        public BlockClass() { }

        public struct Block
        {
            public Int64 TimeStamp;
            public byte[] Data;
            public byte[]? PrevBlockHash;
            public byte[] Hash;
            public int Nonce;

            public void SetHash()
            {
                byte[] timestamp = BitConverter.GetBytes(TimeStamp);

                string headers_str = HashStrig.GetHashString(timestamp) + HashStrig.GetHashString(PrevBlockHash) + HashStrig.GetHashString(Data);
        
                byte[] header_bt = Encoding.UTF8.GetBytes(headers_str);

                Hash = SHA256.HashData(header_bt);

            }

            public Block NewGenesisBlock()
            {
                byte[] bytes_empty = null;
                return NewBlock("Genesis Block", bytes_empty);
            }
        }

        public struct Blockchain
        {
            public LinkedList<Block>? blocks;

            public void AddBlock(string data)
            {
                Block prevBlock = blocks.Last();
                Block newBlock = NewBlock(data, prevBlock.Hash);
                blocks.AddLast(newBlock);
            }

            public Blockchain NewBlockchain()
            {
                Block block = new Block().NewGenesisBlock();
                Blockchain chain = new Blockchain();
                LinkedList<Block> blocks = new LinkedList<Block>();
                blocks.AddLast(block);
                chain.blocks = blocks;

                return chain;
            }
        }


        public static Block NewBlock(string data, byte[] prevBlockHash)
        {
            Block block = new()
            {
                TimeStamp = DateTime.Now.Ticks,
                Data = Encoding.UTF8.GetBytes(data),
                PrevBlockHash = prevBlockHash
            };
            ProofOfWorkClass.ProofOfWork pow = new ProofOfWorkClass.ProofOfWork().NewProofOfWork(block);
            
            
            Dictionary<string, byte[]> keyValues = pow.Run();

            block.Hash = keyValues["hash"];
            block.Nonce = BitConverter.ToInt32(keyValues["nonce"]);

            //block.SetHash();
            return block;
        }

    }
}
