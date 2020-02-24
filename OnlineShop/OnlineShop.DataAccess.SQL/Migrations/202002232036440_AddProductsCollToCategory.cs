namespace OnlineShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductsCollToCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategories", "Slug", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductCategories", "Slug");
        }
    }
}
