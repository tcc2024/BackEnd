using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class ConnectionFactory
    {
        public static MySqlConnection Build()
        {
            var connectionString = "Server=localhost;Database=TaskSync;Uid=root;Pwd=root;";
            return new MySqlConnection(connectionString);
        }
    }
}
