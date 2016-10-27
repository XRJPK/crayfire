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
                //List<char> CrayfireAdress = new List<char>();
                try
                {
                    data = DatabaseContext.Database.SqlQuery<AdressCount>(command).ToList();
                    //CrayfireAdress = DatabaseContext.Database.SqlQuery<string>(command).FirstOrDefault<string>().ToList();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql">the sql command to execute</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// based on http://stackoverflow.com/questions/26749429/anonymous-type-result-from-sql-query-execution-entity-framework
        public static IEnumerable DynamicSqlQuery (string sql, params object[] parameters)
        {
            
            var database = new db_Entities();
                TypeBuilder builder = createTypeBuilder(
                    "MyDynamicAssembly", "MyDynamicModule", "MyDynamicType");
            
            using (System.Data.IDbCommand command = database.Database.Connection.CreateCommand())
            {
                try
                {
                    database.Database.Connection.Open();
                    command.CommandText = sql;
                    command.CommandTimeout = command.Connection.ConnectionTimeout;
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    using (System.Data.IDataReader reader = command.ExecuteReader())
                    {
                        var schema = reader.GetSchemaTable();

                        foreach (System.Data.DataRow row in schema.Rows)
                        {
                            string name = (string)row["ColumnName"];
                            //var a=row.ItemArray.Select(d=>d.)
                            Type type = (Type)row["DataType"];
                            if (type != typeof(string) && (bool)row.ItemArray[schema.Columns.IndexOf("AllowDbNull")])
                            {
                                type = typeof(Nullable<>).MakeGenericType(type);
                            }
                            createAutoImplementedProperty(builder, name, type);
                        }
                    }
                }
                finally
                {
                    database.Database.Connection.Close();
                    command.Parameters.Clear();
                }
            }

            Type resultType = builder.CreateType();

            return database.Database.SqlQuery(resultType, sql, parameters);
            
        }
        
        private static TypeBuilder createTypeBuilder(
            string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                                       AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private static void createAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName),
                              propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
            }
       public class AdressCount
        {
            public int groupID { get; set; }
            public string groupName { get; set; }
            public int addressCount { get; set; }
        }
    }


}
