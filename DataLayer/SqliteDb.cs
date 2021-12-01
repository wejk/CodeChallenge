using System.IO;
using Microsoft.Data.Sqlite;

namespace KenTan.DataLayer
{
    public class SqliteDb
    {
        // handle the / and \ for windows and Linux base system
        private static string ConnectionString = $"Data Source=App_Data{Path.DirectorySeparatorChar}products.db";
        public static SqliteConnection SqliteDbConnection() => new SqliteConnection(ConnectionString);
    }
}
