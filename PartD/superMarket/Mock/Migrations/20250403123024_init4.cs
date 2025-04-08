using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProducstsStore",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProducstsStore_UserId",
                table: "ProducstsStore",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProducstsStore_Users_UserId",
                table: "ProducstsStore",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProducstsStore_Users_UserId",
                table: "ProducstsStore");

            migrationBuilder.DropIndex(
                name: "IX_ProducstsStore_UserId",
                table: "ProducstsStore");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProducstsStore");
        }
    }
}
