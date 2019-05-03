namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.blog_Article", name: "Publication", newName: "Date_publication");
            AlterColumn("dbo.blog_Comments", "Date_publication", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.blog_Article", "ImageName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.blog_Article", "ImageName", c => c.String());
            AlterColumn("dbo.blog_Comments", "Date_publication", c => c.DateTime(nullable: false));
            RenameColumn(table: "dbo.blog_Article", name: "Date_publication", newName: "Publication");
        }
    }
}
