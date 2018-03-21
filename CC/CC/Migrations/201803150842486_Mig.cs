namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cafes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cafes");
        }
    }
}
