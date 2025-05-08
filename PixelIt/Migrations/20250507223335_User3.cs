using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelIt.Migrations
{
    /// <inheritdoc />
    public partial class User3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageCollections");

            migrationBuilder.AlterColumn<string>(
                name: "NewPostImage",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "VerificationImage1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationImage2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationImage1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VerificationImage2",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "NewPostImage",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ImageCollections",
                columns: table => new
                {
                    IdImageCollection = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCollections", x => x.IdImageCollection);
                    table.ForeignKey(
                        name: "FK_ImageCollections_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageCollections_IdUser",
                table: "ImageCollections",
                column: "IdUser");
        }
    }
}
