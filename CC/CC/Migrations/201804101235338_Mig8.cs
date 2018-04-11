namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig8 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Hosts", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Hosts", "UserId", c => c.Guid(nullable: false));
        }
    }
}
