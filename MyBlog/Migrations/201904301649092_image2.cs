namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.blog_Article", "ImageName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.blog_Article", "ImageName");
        }
    }
}
