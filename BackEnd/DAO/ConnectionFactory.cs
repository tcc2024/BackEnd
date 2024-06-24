using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class ConnectionFactory
    {
        public static MySqlConnection Build()
        {
            var connectionString = "Server=tasksync.mysql.database.azure.com;Database=TaskSync;Uid=tasksync;Pwd=Semlogin0011#;";
            //var connectionString = "Server=localhost;Database=TaskSync;Uid=tasksync;Pwd=Semlogin0011#;";

            return new MySqlConnection(connectionString);
        }
    }
}
