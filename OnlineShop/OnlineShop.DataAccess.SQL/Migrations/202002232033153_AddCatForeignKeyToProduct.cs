namespace OnlineShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCatForeignKeyToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Slug", c => c.String());
            AddColumn("dbo.Products", "ProductCategoryId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "ProductCategoryId");
            AddForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropColumn("dbo.Products", "ProductCategoryId");
            DropColumn("dbo.Products", "Slug");
        }
    }
}
