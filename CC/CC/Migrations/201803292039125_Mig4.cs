namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cafes", "UserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cafes", "UserId", c => c.String());
        }
    }
}
