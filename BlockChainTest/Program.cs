// See https://aka.ms/new-console-template for more information
using BlockChainTest;
using System.Security.Cryptography;
using System.Text;

Console.WriteLine("Hello, World!");

BlockClass.Blockchain blockchain = new BlockClass.Blockchain().NewBlockchain();

blockchain.AddBlock("I ncn ikn ");
blockchain.AddBlock("fdfffffff");
blockchain.AddBlock("I think is ugly!");
blockchain.AddBlock("I rethink about this");
blockchain.AddBlock("I am soo tired!");


foreach (var block in blockchain.blocks)
{
    Console.WriteLine($"Prev. hash: " + HashStrig.GetHashString(block.PrevBlockHash));
    Console.WriteLine($"Data: " + Encoding.Default.GetString(block.Data));
    Console.WriteLine($"Hash: " + HashStrig.GetHashString(block.Hash));
    ProofOfWorkClass.ProofOfWork proofOfWork = new ProofOfWorkClass.ProofOfWork().NewProofOfWork(block);
    Console.WriteLine("Validate: " + proofOfWork.Validate().ToString());

}

