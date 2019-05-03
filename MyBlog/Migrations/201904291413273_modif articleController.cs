namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifarticleController : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commentaires",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Pseudo = c.String(maxLength: 75),
                        Contenu = c.String(),
                        Publication = c.DateTime(nullable: false),
                        Article_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.blog_Article", t => t.Article_ID)
                .Index(t => t.Article_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Commentaires", "Article_ID", "dbo.blog_Article");
            DropIndex("dbo.Commentaires", new[] { "Article_ID" });
            DropTable("dbo.Commentaires");
        }
    }
}
