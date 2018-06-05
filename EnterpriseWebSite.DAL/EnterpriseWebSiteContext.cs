using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class EnterpriseWebSiteContext : DbContext
    {
        public EnterpriseWebSiteContext()
                : base("name=ConnectionString")
            {
            }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<EnterpriseInfo> EnterpriseInfo { get; set; }
        public DbSet<HtmlElement> HtmlElement { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<HtmlPage> HtmlPage { get; set; }
        public DbSet<HtmlRegion> HtmlRegion { get; set; }
    }
}
