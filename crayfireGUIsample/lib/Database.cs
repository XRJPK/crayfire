using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Crayfire.Console.DatabaseLibary;


namespace crayfireGUIsample.lib
{
    /// <summary>
    /// Haupt Databankklasse, stellt alle Basis Datebank Objekte bereit.
    /// </summary>
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
            //Tables of Version 0.0.3
            public DbSet<crayfire_address> Adress { get; set; }
            public DbSet<crayfire_address_contact> AdressContact { get; set; }
            public DbSet<crayfire_address_group> AdressGroup { get; set; }
            public DbSet<crayfire_residual> Residual { get; set; }
            public DbSet<crayfire_residual_calculation> ResidualCalculation { get; set; }
            public DbSet<crayfire_residual_config> ResidualConfig { get; set; }
            public DbSet<crayfire_residual_data> ResidualData { get; set; }
            public DbSet<crayfire_residual_images> ResidualImages { get; set; }
            public DbSet<crayfire_vehicle> Vehicle { get; set; }
            // public DbSet<IEnumerable> AdressQuery { get; set; }

            // Try of generic DbSet
            // public DbSet<Crayfire.Console.DatabaseLibary.Database> MySets { get; set; }
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
                    throw;
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
                    throw;
                }
            }
        }
        public List<crayfire_menu_item> GetCrayfireMenu()
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
                    throw;
                }

                return CrayfireMenu;
            }
        }

        /// <summary>
        /// Used to execute a single SQL query for entity crayfire_menu_item
        /// </summary>
        /// <returns>List from type of own entity</returns>
        public List<crayfire_menu_item> GetCrayfireMenu(string command)
        {
            using (var DatabaseContext = new db_Entities())
            {
                List<crayfire_menu_item> CrayfireMenu = new List<crayfire_menu_item>();
                try
                {
                    CrayfireMenu = DatabaseContext.crayfire_menu_item.SqlQuery(command).ToList();
                }
                catch (Exception Ex )
                {
                    DatabaseContext.Database.Connection.Close();
                    log4.Error(Ex);
                    throw;
                }
                return CrayfireMenu;
            }
        }

        /// <summary>
        /// Get the whole Table GroupList and stores them in a List of its own type 
        /// </summary>
        /// <returns>List from type of own entity</returns>
        public List<crayfire_address_group> getGroupList()
        {
            using (var DatabaseContext = new db_Entities())
            {
                List<crayfire_address_group> crayfireAddressGroup = new List<crayfire_address_group>();

                try
                {
                    InitializeDatabase();
                    crayfireAddressGroup = DatabaseContext.AdressGroup.ToList();
                    ShutdownDatabase();
                }
                catch (Exception Ex)
                {
                    DatabaseContext.Database.Connection.Close();
                    log4.Error(Ex);
                    throw;
                }

                return crayfireAddressGroup;
            }
        }

        public List<crayfire_address> GetCrayfireAdress()
        {
            using (var DatabaseContext = new db_Entities())
            {
                List<crayfire_address> crayfireAdress = new List<crayfire_address>();
                try
                {
                    InitializeDatabase();
                    crayfireAdress = DatabaseContext.Adress.ToList();
                    ShutdownDatabase();
                }
                catch (Exception Ex)
                {
                    DatabaseContext.Database.Connection.Close();
                    log4.Error(Ex);
                    throw;
                }
                return crayfireAdress;
            }
        }

        /// <summary>
        /// Uses the tutorial of http://www.binaryintellect.net/articles/fbc96859-8a31-4735-baeb-7adcbc521b30.aspx 
        /// </summary>
        /// <param name="command">Includes the plain SQL Statement</param>
        /// <returns>a List of special type for the explicite Statement</returns>
        public List<AdressCount> GetCrayfireAdress(string command)
        {
            List<AdressCount> data;
            using (var DatabaseContext = new db_Entities()) 
            {
                try
                {
                    data = DatabaseContext.Database.SqlQuery<AdressCount>(command).ToList();
                }
                catch (Exception Ex)
                {
                    DatabaseContext.Database.Connection.Close();
                    log4.Error(Ex);
                    throw;
                }
                return data;
            }
        }

        public class AdressCount
        {
            public int groupID { get; set; }
            public string groupName { get; set; }
            public int addressCount { get; set; }
        }
    }


}
