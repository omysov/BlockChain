// See https://aka.ms/new-console-template for more information
using BlockChainTest;
using BlockChainTest.ModelDb;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using static BlockChainTest.BlockClass;

Console.WriteLine("Hello, World!");

/*
using (AppBlockChainContext db = new AppBlockChainContext())
{
    bool isAvaible = db.Database.CanConnect();
    db.blockchainModels.Add(new BlockchainModel
    {
        key = Encoding.UTF8.GetBytes("1"),
        value = Encoding.UTF8.GetBytes("Hello world")
    });
    db.SaveChanges();
}
*/



Blockchain blockchain = new Blockchain().NewBlockchain();

/*
blockchain.AddBlock("I ncn ikn ");
blockchain.AddBlock("fdfffffff");
blockchain.AddBlock("I think is ugly!");
blockchain.AddBlock("I rethink about this");
blockchain.AddBlock("I am soo tired!");
*/


using(AppBlockChainContext db = new AppBlockChainContext())
{
    BlockchainIterator iterator = new BlockchainIterator().Iterator(blockchain);

    
    foreach (var block in db.blockchainModels)
    {
        Block block_it = iterator.Next();

        Console.WriteLine($"Prev. hash: " + HashStrig.GetHashString(block_it.PrevBlockHash));
        Console.WriteLine($"Data: " + HashStrig.GetHashString(block_it.Data));
        Console.WriteLine($"Hash: " + HashStrig.GetHashString(block_it.Hash));    
        Console.WriteLine();
        if(block_it.PrevBlockHash == null)
        {
            break;
        }

        /*
        if(HashStrig.GetHashString(block.key) == "31")
        {
            Console.WriteLine($"key 1: " + HashStrig.GetHashString(block.key));
        }
        else
        {
            Console.WriteLine($"Hash key: " + HashStrig.GetHashString(block.key));
        }
        Console.WriteLine($"Hash value: " + HashStrig.GetHashString(block.value));
        Console.WriteLine();
        */
    }
    

}

