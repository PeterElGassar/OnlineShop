namespace OnlineShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBasketItem_ProductFK : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BasketItems", "ProductId", c => c.String(maxLength: 128));
            CreateIndex("dbo.BasketItems", "ProductId");
            AddForeignKey("dbo.BasketItems", "ProductId", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BasketItems", "ProductId", "dbo.Products");
            DropIndex("dbo.BasketItems", new[] { "ProductId" });
            AlterColumn("dbo.BasketItems", "ProductId", c => c.String());
        }
    }
}
