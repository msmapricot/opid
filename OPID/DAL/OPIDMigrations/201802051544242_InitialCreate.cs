namespace MSM.DAL.OPIDMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnresolvedChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecordID = c.Int(nullable: false),
                        InterviewRecordID = c.Int(nullable: false),
                        Name = c.String(),
                        Num = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Service = c.String(),
                        Disposition = c.String(),
                        Matched = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UnresolvedChecks");
        }
    }
}
