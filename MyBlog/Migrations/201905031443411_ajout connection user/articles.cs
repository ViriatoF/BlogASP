namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajoutconnectionuserarticles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.blog_Article", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.blog_Article", "ApplicationUser_Id");
            AddForeignKey("dbo.blog_Article", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.blog_Article", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.blog_Article", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.blog_Article", "ApplicationUser_Id");
        }
    }
}
