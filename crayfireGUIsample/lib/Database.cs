using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Crayfire.Console.DatabaseLibary;


namespace crayfireGUIsample.lib
{
    class Database
    {
        private static readonly log4net.ILog log4 =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public partial class db_Entities : DbContext
        {
            public db_Entities() : base(nameOrConnectionString: "WCF_Database") { }

            public DbSet<wcf1_user> wcf1_user { get; set; }
            public DbSet<wcf1_template> wcf1_template { get; set; }
            public DbSet<crayfire_menu_item> crayfire_menu_item { get; set; }
        }

        static void InitializeDatabase()
        {
            using (var DatabaseContext = new db_Entities())
            {
                try
                {
                    DatabaseContext.Database.Connection.Open();
                    log4.Info("Datenbankverbindung: " + DatabaseContext.Database.Connection.State.ToString());
                }
                catch (Exception Ex)
                {
                    DatabaseContext.Database.Connection.Close();
                    log4.Error(Ex);
                }
            }
        }
        static void ShutdownDatabase()
        {
            using (var DatabaseContext = new db_Entities())
            {
                try
                {
                    DatabaseContext.Database.Connection.Close();
                }
                catch (Exception Ex)
                {
                    DatabaseContext.Database.Connection.Close();
                    log4.Error(Ex);
                }
            }
        }
        public List<crayfire_menu_item> GetCrayfireMenu ()
        {
            using (var DatabaseContext = new db_Entities())
            {
                List<crayfire_menu_item> CrayfireMenu = new List<crayfire_menu_item>();

                try
                {
                    InitializeDatabase();
                    CrayfireMenu = DatabaseContext.crayfire_menu_item.ToList();
                    ShutdownDatabase();
                }
                catch (Exception Ex)
                {
                    DatabaseContext.Database.Connection.Close();
                    log4.Error(Ex);
                }

                return CrayfireMenu;
            }
        }
    }
}
