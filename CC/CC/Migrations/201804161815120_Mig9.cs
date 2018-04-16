namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cafes", "Address", c => c.String());
            AddColumn("dbo.Cafes", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cafes", "Location");
            DropColumn("dbo.Cafes", "Address");
        }
    }
}
