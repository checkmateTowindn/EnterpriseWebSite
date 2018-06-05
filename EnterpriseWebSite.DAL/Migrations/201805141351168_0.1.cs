namespace EnterpriseWebSite.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(unicode: false),
                        PassWord = c.String(unicode: false),
                        PassWord2 = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Mobile = c.String(unicode: false),
                        Sex = c.String(unicode: false),
                        VerifyCode = c.String(unicode: false),
                        Authority = c.Int(nullable: false),
                        CreateAdminId = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        AddDate = c.String(unicode: false),
                        Remark = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EnterpriseInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Content = c.String(unicode: false),
                        AddDate = c.String(unicode: false),
                        Admin_Id = c.Int(),
                        HtmlRegion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.Admin_Id)
                .ForeignKey("dbo.HtmlRegions", t => t.HtmlRegion_Id)
                .Index(t => t.Admin_Id)
                .Index(t => t.HtmlRegion_Id);
            
            CreateTable(
                "dbo.HtmlRegions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionName = c.String(unicode: false),
                        HtmlPage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HtmlPages", t => t.HtmlPage_Id)
                .Index(t => t.HtmlPage_Id);
            
            CreateTable(
                "dbo.HtmlPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HtmlElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Width = c.String(unicode: false),
                        Height = c.String(unicode: false),
                        Remark = c.String(unicode: false),
                        AddDate = c.String(unicode: false),
                        Admin_Id = c.Int(),
                        HtmlRegion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.Admin_Id)
                .ForeignKey("dbo.HtmlRegions", t => t.HtmlRegion_Id)
                .Index(t => t.Admin_Id)
                .Index(t => t.HtmlRegion_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Alt = c.String(unicode: false),
                        Url = c.String(unicode: false),
                        Sort = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        HtmlElement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HtmlElements", t => t.HtmlElement_Id)
                .Index(t => t.HtmlElement_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mobile = c.String(maxLength: 32, storeType: "nvarchar"),
                        Nick = c.String(unicode: false),
                        IdentifyingCode = c.String(maxLength: 6, storeType: "nvarchar"),
                        MessageContent = c.String(maxLength: 1000, storeType: "nvarchar"),
                        UpperLeve = c.Int(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        AddDate = c.String(maxLength: 32, storeType: "nvarchar"),
                        Admin_Id = c.Int(),
                        HtmlPage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.Admin_Id)
                .ForeignKey("dbo.HtmlPages", t => t.HtmlPage_Id)
                .Index(t => t.Admin_Id)
                .Index(t => t.HtmlPage_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "HtmlPage_Id", "dbo.HtmlPages");
            DropForeignKey("dbo.Messages", "Admin_Id", "dbo.Admins");
            DropForeignKey("dbo.Images", "HtmlElement_Id", "dbo.HtmlElements");
            DropForeignKey("dbo.HtmlElements", "HtmlRegion_Id", "dbo.HtmlRegions");
            DropForeignKey("dbo.HtmlElements", "Admin_Id", "dbo.Admins");
            DropForeignKey("dbo.EnterpriseInfoes", "HtmlRegion_Id", "dbo.HtmlRegions");
            DropForeignKey("dbo.HtmlRegions", "HtmlPage_Id", "dbo.HtmlPages");
            DropForeignKey("dbo.EnterpriseInfoes", "Admin_Id", "dbo.Admins");
            DropIndex("dbo.Messages", new[] { "HtmlPage_Id" });
            DropIndex("dbo.Messages", new[] { "Admin_Id" });
            DropIndex("dbo.Images", new[] { "HtmlElement_Id" });
            DropIndex("dbo.HtmlElements", new[] { "HtmlRegion_Id" });
            DropIndex("dbo.HtmlElements", new[] { "Admin_Id" });
            DropIndex("dbo.HtmlRegions", new[] { "HtmlPage_Id" });
            DropIndex("dbo.EnterpriseInfoes", new[] { "HtmlRegion_Id" });
            DropIndex("dbo.EnterpriseInfoes", new[] { "Admin_Id" });
            DropTable("dbo.Messages");
            DropTable("dbo.Images");
            DropTable("dbo.HtmlElements");
            DropTable("dbo.HtmlPages");
            DropTable("dbo.HtmlRegions");
            DropTable("dbo.EnterpriseInfoes");
            DropTable("dbo.Admins");
        }
    }
}
