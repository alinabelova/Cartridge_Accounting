namespace CartridgeAccounting.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartridgeMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cartridge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Creation = c.DateTime(nullable: false),
                        DateOfChange = c.DateTime(nullable: false),
                        Model = c.String(),
                        CompatiblePrinter = c.String(),
                        Color = c.String(),
                        Resource = c.String(),
                        WriteOff = c.Boolean(nullable: false),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CartridgeType", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.CartridgeType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cartridge", "Type_Id", "dbo.CartridgeType");
            DropIndex("dbo.Cartridge", new[] { "Type_Id" });
            DropTable("dbo.CartridgeType");
            DropTable("dbo.Cartridge");
        }
    }
}
