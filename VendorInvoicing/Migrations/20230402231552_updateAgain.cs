using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorInvoicing.Migrations
{
    /// <inheritdoc />
    public partial class updateAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaymentTermsId",
                table: "Invoices",
                column: "PaymentTermsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_PaymentTerms_PaymentTermsId",
                table: "Invoices",
                column: "PaymentTermsId",
                principalTable: "PaymentTerms",
                principalColumn: "PaymentTermsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_PaymentTerms_PaymentTermsId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_PaymentTermsId",
                table: "Invoices");
        }
    }
}
