using MySqlConnector;
using System;

namespace EduApp.Services
{
    public abstract class BaseDatabaseService
    {
                                                                                                                              //Alterar senha aqui
        protected readonly string _connectionString = "Server=mysql-eduapp-eduapp.g.aivencloud.com;Port=25689;Database=sistema_gamificado;Uid=avnadmin;Pwd=senha;SslMode=Required;";

       
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
