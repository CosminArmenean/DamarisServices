using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Damaris.Officer.Migrations
{
    /// <inheritdoc />
    public partial class InitialAspNetIdentityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsJointAccount",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsJointAccount",
                table: "Users");
        }
    }
}
