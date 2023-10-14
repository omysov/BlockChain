using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainTest.ModelDb
{
    [Table("BlockchainModel")]
    public class BlockchainModel
    {
        [Key]
        public byte[] key { get; set; }
        [Required]
        public byte[] value { get; set; }
    }
}
