using System.Data;
using Dapper;
using Npgsql;
using SimpleInjector;
using WebApi.Common.Interfaces;

namespace WebApi.Data
{
    public static class Bootstrapper
    {
        public static Container BootstrapDb(this Container container)
        {
            SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);
            //Registers the sql connection
            container.Register(() =>
            {
                var pgHost = Environment.GetEnvironmentVariable("PGHOST");
                var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");
                var pgPort = Environment.GetEnvironmentVariable("PGPORT");
                var pgUser = Environment.GetEnvironmentVariable("PGUSER");
                var pgDb = Environment.GetEnvironmentVariable("PGDB");

                var connectionString = $"Server={pgHost};Port={pgPort};Database={pgDb};User Id={pgUser};Password={pgPassword};";
                var sqlConn = new NpgsqlConnection(connectionString);
                sqlConn.Open();
                return sqlConn;
            }, Lifestyle.Scoped);

            //register Data Access classes to interfaces to inject into handlers/controllers
            container.Register<IViewDataAccess, ViewModelDataAccess>(Lifestyle.Scoped);
            return container;
        }
    }
}
