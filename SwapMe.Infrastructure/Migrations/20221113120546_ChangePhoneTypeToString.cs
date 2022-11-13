using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwapMe.Infrastructure.Migrations
{
    public partial class ChangePhoneTypeToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "UserContact",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "UserContact",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
