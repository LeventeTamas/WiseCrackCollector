using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiseCrackCollector.Migrations
{
    /// <inheritdoc />
    public partial class create_AppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroupPermissions", 
                table: "UserGroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Wisecracks_Groups_GroupId",
                table: "Wisecracks");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "UserGroupPermissions",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserGroupPermissions",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupPermissions_GroupId",
                table: "UserGroupPermissions",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupPermissions_AspNetUsers_UserId",
                table: "UserGroupPermissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupPermissions_Groups_GroupId",
                table: "UserGroupPermissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Wisecracks_Groups_GroupId",
                table: "Wisecracks",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroupPermissions",
                table: "UserGroupPermissions",
                columns: new[] { "UserId", "GroupId" }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroupPermissions",
                table: "UserGroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupPermissions_AspNetUsers_UserId",
                table: "UserGroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupPermissions_Groups_GroupId",
                table: "UserGroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Wisecracks_Groups_GroupId",
                table: "Wisecracks");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupPermissions_GroupId",
                table: "UserGroupPermissions");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "UserGroupPermissions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserGroupPermissions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AddForeignKey(
                name: "FK_Wisecracks_Groups_GroupId",
                table: "Wisecracks",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroupPermissions",
                table: "UserGroupPermissions",
                columns: new[] { "UserId", "GroupId" }
                );
        }
    }
}
