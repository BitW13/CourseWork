namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cafes", "Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Cafes", "Lng", c => c.Double(nullable: false));
            DropColumn("dbo.Cafes", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cafes", "Location", c => c.String());
            DropColumn("dbo.Cafes", "Lng");
            DropColumn("dbo.Cafes", "Lat");
        }
    }
}
