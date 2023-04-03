using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CandidateWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class resume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte[]>(
                name: "Resume",
                table: "Candidates",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ResumeFileName",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ResumeFileName",
                table: "Candidates");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Candidates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
