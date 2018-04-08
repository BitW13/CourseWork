namespace CC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ConfirmPassword", c => c.String());
            AddColumn("dbo.Users", "NewPassword", c => c.String(maxLength: 25));
            AddColumn("dbo.Users", "ConfirmPassword1", c => c.String());
            AddColumn("dbo.Users", "SecretKey", c => c.String());
            AddColumn("dbo.Users", "SecurityCode", c => c.String());
            AddColumn("dbo.Users", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Cafes", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Cafes", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Records", "NickName", c => c.String(maxLength: 20));
            AlterColumn("dbo.Records", "Title", c => c.String(nullable: false, maxLength: 600));
            AlterColumn("dbo.Records", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "NickName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "UserSurname", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String());
            AlterColumn("dbo.Users", "UserSurname", c => c.String());
            AlterColumn("dbo.Users", "UserName", c => c.String());
            AlterColumn("dbo.Users", "NickName", c => c.String());
            AlterColumn("dbo.Records", "Description", c => c.String());
            AlterColumn("dbo.Records", "Title", c => c.String());
            AlterColumn("dbo.Records", "NickName", c => c.String());
            AlterColumn("dbo.Cafes", "Description", c => c.String());
            AlterColumn("dbo.Cafes", "Name", c => c.String());
            DropColumn("dbo.Users", "Discriminator");
            DropColumn("dbo.Users", "SecurityCode");
            DropColumn("dbo.Users", "SecretKey");
            DropColumn("dbo.Users", "ConfirmPassword1");
            DropColumn("dbo.Users", "NewPassword");
            DropColumn("dbo.Users", "ConfirmPassword");
        }
    }
}
