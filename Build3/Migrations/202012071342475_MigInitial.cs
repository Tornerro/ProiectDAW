namespace Build3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        IdComment = c.Int(nullable: false, identity: true),
                        ContinutComment = c.String(nullable: false),
                        DataComment = c.DateTime(nullable: false),
                        IdTask = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdComment)
                .ForeignKey("dbo.Tasks", t => t.IdTask, cascadeDelete: true)
                .Index(t => t.IdTask);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        IdTask = c.Int(nullable: false, identity: true),
                        NumeTask = c.String(nullable: false, maxLength: 20),
                        DescriereTask = c.String(),
                        DataTask = c.DateTime(nullable: false),
                        DataSfarsitTask = c.DateTime(),
                        Status = c.String(nullable: false),
                        IdProiect = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdTask)
                .ForeignKey("dbo.Proiects", t => t.IdProiect, cascadeDelete: true)
                .Index(t => t.IdProiect);
            
            CreateTable(
                "dbo.Proiects",
                c => new
                    {
                        IdProiect = c.Int(nullable: false, identity: true),
                        NumeProiect = c.String(nullable: false, maxLength: 20),
                        DataProiect = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdProiect);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "IdProiect", "dbo.Proiects");
            DropForeignKey("dbo.Comments", "IdTask", "dbo.Tasks");
            DropIndex("dbo.Tasks", new[] { "IdProiect" });
            DropIndex("dbo.Comments", new[] { "IdTask" });
            DropTable("dbo.Proiects");
            DropTable("dbo.Tasks");
            DropTable("dbo.Comments");
        }
    }
}
