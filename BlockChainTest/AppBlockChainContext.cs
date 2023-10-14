using BlockChainTest.ModelDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainTest
{
    public class AppBlockChainContext : DbContext
    {
        public DbSet<BlockchainModel> blockchains { get; set; }

        public DbSet<BlockchainModel> blockchainModels { get; set; }

        public AppBlockChainContext() => Database.EnsureCreated();

        /*
        public AppBlockChainContext(DbContextOptions<AppBlockChainContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        */


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source= NewBlockChain.db");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
