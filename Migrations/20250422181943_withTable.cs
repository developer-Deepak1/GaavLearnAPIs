using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaavLearnAPIs.Migrations
{
    /// <inheritdoc />
    public partial class withTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "T_user_tokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "T_users");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "T_user_roles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "T_user_logins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "T_user_claims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "T_roles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "T_role_claims");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "T_user_roles",
                newName: "IX_T_user_roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "T_user_logins",
                newName: "IX_T_user_logins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "T_user_claims",
                newName: "IX_T_user_claims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "T_role_claims",
                newName: "IX_T_role_claims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_user_tokens",
                table: "T_user_tokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_users",
                table: "T_users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_user_roles",
                table: "T_user_roles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_user_logins",
                table: "T_user_logins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_user_claims",
                table: "T_user_claims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_roles",
                table: "T_roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_role_claims",
                table: "T_role_claims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_role_claims_T_roles_RoleId",
                table: "T_role_claims",
                column: "RoleId",
                principalTable: "T_roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_user_claims_T_users_UserId",
                table: "T_user_claims",
                column: "UserId",
                principalTable: "T_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_user_logins_T_users_UserId",
                table: "T_user_logins",
                column: "UserId",
                principalTable: "T_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_user_roles_T_roles_RoleId",
                table: "T_user_roles",
                column: "RoleId",
                principalTable: "T_roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_user_roles_T_users_UserId",
                table: "T_user_roles",
                column: "UserId",
                principalTable: "T_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_user_tokens_T_users_UserId",
                table: "T_user_tokens",
                column: "UserId",
                principalTable: "T_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_role_claims_T_roles_RoleId",
                table: "T_role_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_T_user_claims_T_users_UserId",
                table: "T_user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_T_user_logins_T_users_UserId",
                table: "T_user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_T_user_roles_T_roles_RoleId",
                table: "T_user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_T_user_roles_T_users_UserId",
                table: "T_user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_T_user_tokens_T_users_UserId",
                table: "T_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_users",
                table: "T_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_user_tokens",
                table: "T_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_user_roles",
                table: "T_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_user_logins",
                table: "T_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_user_claims",
                table: "T_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_roles",
                table: "T_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_role_claims",
                table: "T_role_claims");

            migrationBuilder.RenameTable(
                name: "T_users",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "T_user_tokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "T_user_roles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "T_user_logins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "T_user_claims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "T_roles",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "T_role_claims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_T_user_roles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_T_user_logins_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_T_user_claims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_T_role_claims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
