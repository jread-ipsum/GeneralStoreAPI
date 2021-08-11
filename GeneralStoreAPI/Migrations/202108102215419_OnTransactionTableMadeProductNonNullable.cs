namespace GeneralStoreAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnTransactionTableMadeProductNonNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "ProductSKU", "dbo.Products");
            DropIndex("dbo.Transactions", new[] { "ProductSKU" });
            AlterColumn("dbo.Transactions", "ProductSKU", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Transactions", "ProductSKU");
            AddForeignKey("dbo.Transactions", "ProductSKU", "dbo.Products", "SKU", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "ProductSKU", "dbo.Products");
            DropIndex("dbo.Transactions", new[] { "ProductSKU" });
            AlterColumn("dbo.Transactions", "ProductSKU", c => c.String(maxLength: 128));
            CreateIndex("dbo.Transactions", "ProductSKU");
            AddForeignKey("dbo.Transactions", "ProductSKU", "dbo.Products", "SKU");
        }
    }
}
