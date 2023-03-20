using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorInvoicing.Migrations
{
    /// <inheritdoc />
    public partial class vendordbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor");

            migrationBuilder.RenameTable(
                name: "Vendor",
                newName: "Vendors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLineItem_InvoiceId",
                table: "InvoiceLineItem",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_VendorId",
                table: "Invoice",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Vendors_VendorId",
                table: "Invoice",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLineItem_Invoice_InvoiceId",
                table: "InvoiceLineItem",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Vendors_VendorId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLineItem_Invoice_InvoiceId",
                table: "InvoiceLineItem");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLineItem_InvoiceId",
                table: "InvoiceLineItem");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_VendorId",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "Vendor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor",
                column: "VendorId");
        }
    }
}
