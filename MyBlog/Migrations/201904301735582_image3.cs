namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.blog_Article", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.blog_Article", "ImageName", c => c.String(nullable: false));
        }
    }
}
