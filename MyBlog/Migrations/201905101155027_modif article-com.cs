namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifarticlecom : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.blog_Comments", name: "Article_ID", newName: "Parent_ID");
            RenameIndex(table: "dbo.blog_Comments", name: "IX_Article_ID", newName: "IX_Parent_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.blog_Comments", name: "IX_Parent_ID", newName: "IX_Article_ID");
            RenameColumn(table: "dbo.blog_Comments", name: "Parent_ID", newName: "Article_ID");
        }
    }
}
