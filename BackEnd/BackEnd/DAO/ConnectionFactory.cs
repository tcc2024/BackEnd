using MySql.Data.MySqlClient;

namespace TaskSync.DAO
{
    public class ConnectionFactory
    {
        public static MySqlConnection Build()
        {
            var connectionString = "Server=localhost;Database=Autenticacao;Uid=root;Pwd=root;";
            return new MySqlConnection(connectionString);
        }
    }
}
