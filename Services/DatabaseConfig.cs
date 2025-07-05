using DotNetEnv;

namespace TodoList.Services
{
    public class DatabaseConfig
    {
        public static string MySqlStringConnection()
        {
            Env.Load();
            var host = Env.GetString("MYSQL_HOST");
            var port = Env.GetString("MYSQL_PORT");
            var database = Env.GetString("MYSQL_DATABASE");
            var user = Env.GetString("MYSQL_USER");
            var password = Env.GetString("MYSQL_PASSWORD");
            return $"Server={host}; Port={port}; Database={database}; User={user}; Password={password}!;"; 
        }
    }
}