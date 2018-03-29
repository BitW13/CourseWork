namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Cafes");
            DropPrimaryKey("dbo.Records");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Cafes", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Records", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Records", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Users", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Cafes", "Id");
            AddPrimaryKey("dbo.Records", "Id");
            AddPrimaryKey("dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Users");
            DropPrimaryKey("dbo.Records");
            DropPrimaryKey("dbo.Cafes");
            AlterColumn("dbo.Users", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Records", "UserId", c => c.String());
            AlterColumn("dbo.Records", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Cafes", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "Id");
            AddPrimaryKey("dbo.Records", "Id");
            AddPrimaryKey("dbo.Cafes", "Id");
        }
    }
}
