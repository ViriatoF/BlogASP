namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcomments : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Commentaires", newName: "blog_Comments");
            RenameColumn(table: "dbo.blog_Comments", name: "Contenu", newName: "Content");
            RenameColumn(table: "dbo.blog_Comments", name: "Publication", newName: "Date_publication");
            AlterColumn("dbo.blog_Comments", "Pseudo", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.blog_Comments", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.blog_Comments", "Content", c => c.String());
            AlterColumn("dbo.blog_Comments", "Pseudo", c => c.String(maxLength: 75));
            RenameColumn(table: "dbo.blog_Comments", name: "Date_publication", newName: "Publication");
            RenameColumn(table: "dbo.blog_Comments", name: "Content", newName: "Contenu");
            RenameTable(name: "dbo.blog_Comments", newName: "Commentaires");
        }
    }
}
