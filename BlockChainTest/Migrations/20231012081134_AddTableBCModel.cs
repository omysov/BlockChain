using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockChainTest.Migrations
{
    /// <inheritdoc />
    public partial class AddTableBCModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_blockchain",
                table: "blockchain");

            migrationBuilder.RenameTable(
                name: "blockchain",
                newName: "BlockchainModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockchainModel",
                table: "BlockchainModel",
                column: "key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockchainModel",
                table: "BlockchainModel");

            migrationBuilder.RenameTable(
                name: "BlockchainModel",
                newName: "blockchain");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blockchain",
                table: "blockchain",
                column: "key");
        }
    }
}
