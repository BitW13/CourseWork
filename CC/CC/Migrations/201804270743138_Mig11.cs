namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cafes", "Lat", c => c.String());
            AlterColumn("dbo.Cafes", "Lng", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cafes", "Lng", c => c.Double(nullable: false));
            AlterColumn("dbo.Cafes", "Lat", c => c.Double(nullable: false));
        }
    }
}
