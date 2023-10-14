using BlockChainTest.ModelDb;
using Microsoft.EntityFrameworkCore;
using ProtoBuf;
using ProtoBuf.Serializers;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BlockChainTest.TransactionClass;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlockChainTest
{
    public class BlockClass
    {
        public BlockClass() { }

        [Serializable]
        [ProtoContract]
        public struct Block
        {
            [ProtoMember(1)]
            public Int64 TimeStamp;
            [ProtoMember(2)]
            public byte[] Data;
            [ProtoMember(3)]
            public byte[]? PrevBlockHash;
            [ProtoMember(4)]
            public byte[] Hash;
            [ProtoMember(5)]
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
                return NewBlock("Genesis block", bytes_empty);
            }

        }


        public struct Blockchain
        {

            // Сохраняем только верхушку блокчейна. byte[] tip
            public LinkedList<Block>? blocks;

            public byte[] tip;
            public string db;

            public void AddBlock(string data)
            {
                byte[] lastHash;

                var optionsBuilder = new DbContextOptionsBuilder<AppBlockChainContext>();
                var options = optionsBuilder.UseSqlite("Data Source= NewBlockChain.db").Options;

                using (AppBlockChainContext db = new AppBlockChainContext())
                {
                    lastHash = db.blockchains.First(u => u.key == Encoding.UTF8.GetBytes("1")).value;
                }

                Block newBlock = NewBlock(data, lastHash);

               

                using (AppBlockChainContext db = new AppBlockChainContext())
                {
                    db.blockchains.Add(new BlockchainModel()
                    {
                        key = newBlock.Hash,
                        value = Serializing(newBlock)
                    });
                    db.SaveChanges();

                    /*
                    db.blockchains.Update(new BlockchainModel()
                    {
                        key = Encoding.UTF8.GetBytes("1"),
                        value = newBlock.Hash
                    });
                    */

                    var result_1_key = db.blockchainModels.SingleOrDefault(u => u.key == Encoding.UTF8.GetBytes("1"));
                    if(result_1_key != null)
                    {
                        result_1_key.value = newBlock.Hash;
                    }

                    tip = newBlock.Hash;

                    db.SaveChanges();
                }

                //blocks.AddLast(newBlock);
            }

            public Blockchain NewBlockchain()
            {


                /*
                 * Открыть базу данных.
                 * Проверить существует ли блокчейн.
                 * Если существует, то установите для вершины экземпляра блокчейна
                 * значение хэша последнего блока, хранящегося в базе данных.
                 * Если не блокчейна не существует.
                 * Создать генезис блок.
                 * Сохранить в базу данных.
                 * Сохранение хэша генезис блока как хэш последнего блока.
                 * Создать новый блокчейн с вершиной с генезис блоком.
                 */
                
                var optionsBuilder = new DbContextOptionsBuilder<AppBlockChainContext>();
                var options = optionsBuilder.UseSqlite("Data Source= NewBlockChain.db").Options;

                using(AppBlockChainContext db = new AppBlockChainContext())
                {
                    bool isAvalaible = db.Database.CanConnect();
                    if (db.blockchainModels.Count() == 0)
                    {
                        Block genesis = new Block().NewGenesisBlock();
                        db.blockchainModels.Add(new BlockchainModel()
                        {
                            key = genesis.Hash,
                            value = Serializing(genesis)
                        });
                        db.blockchainModels.Add(new BlockchainModel()
                        {
                            key = Encoding.UTF8.GetBytes("1"),
                            value = genesis.Hash
                        });
                        tip = genesis.Hash;

                        db.SaveChanges();
                    } else
                    {
                        tip = db.blockchainModels.First(u => u.key == Encoding.UTF8.GetBytes("1")).value;
                    }

                    var _tip = tip;

                    Blockchain bc = new Blockchain()
                    {
                        tip = _tip
                    };
                    return bc;
                }

                Block block = new Block().NewGenesisBlock();
                Blockchain chain = new Blockchain();
                LinkedList<Block> blocks = new LinkedList<Block>();
                blocks.AddLast(block);
                chain.blocks = blocks;

                return chain;
            }

        }

        public struct BlockchainIterator
        {
            byte[] currentHash;

            public BlockchainIterator Iterator(Blockchain bc)
            {
                BlockchainIterator iter = new BlockchainIterator()
                {
                    currentHash = bc.tip
                };
                return iter;
            }

            public Block Next()
            {
                Block block = new Block();

                var optionsBuilder = new DbContextOptionsBuilder<AppBlockChainContext>();
                var options = optionsBuilder.UseSqlite("Data Source= NewBlockChain.db").Options;

                using (AppBlockChainContext db = new AppBlockChainContext())
                {
                   byte[] CurHash = currentHash;
                   byte[] endcodeBlock = db.blockchainModels.First(u => u.key == CurHash).value;
                   block = DeserializingBlock(endcodeBlock);
                }

                currentHash = block.PrevBlockHash;
                return block;
            }
        }

        public static byte[] Serializing(Block b)
        {
            if(!b.GetType().IsSerializable)
            {
                return null;
            }


            using(MemoryStream  ms = new MemoryStream())
            {
                Serializer.Serialize(ms, b);
                var result = ms.ToArray();
                return ms.ToArray();
            }

        }

        public static Block DeserializingBlock(byte[] data)
        {
            Block block;
            using(MemoryStream ms = new MemoryStream(data))
            {
                block = Serializer.Deserialize<Block>(ms);
            }
            return block;
        }


        public static Block NewBlock(string data, byte[] prevBlockHash)
        {
            Block block = new()
            {
                TimeStamp = DateTime.Now.Ticks,
                Data =  Encoding.UTF8.GetBytes(data),
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
