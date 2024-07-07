using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class submissionRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ProblemId",
                schema: "omiiasi",
                table: "Submissions",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_UserId",
                schema: "omiiasi",
                table: "Submissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Problems_ProblemId",
                schema: "omiiasi",
                table: "Submissions",
                column: "ProblemId",
                principalSchema: "omiiasi",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Users_UserId",
                schema: "omiiasi",
                table: "Submissions",
                column: "UserId",
                principalSchema: "omiiasi",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Problems_ProblemId",
                schema: "omiiasi",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_UserId",
                schema: "omiiasi",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ProblemId",
                schema: "omiiasi",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_UserId",
                schema: "omiiasi",
                table: "Submissions");
        }
    }
}
