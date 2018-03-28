namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "RecordDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "RecordDate");
        }
    }
}
