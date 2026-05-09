using MySqlConnector;
using System;

namespace EduApp.Services
{
    public abstract class BaseDatabaseService
    {
        // String de conexão                                                                                                                           //Alterar senha aqui
        protected readonly string _connectionString = "Server=mysql-eduapp-eduapp.g.aivencloud.com;Port=25689;Database=sistema_gamificado;Uid=avnadmin;Pwd=senha_falsa_tcc;SslMode=Required;";

        // Método que as classes filhas vão chamar para pegar a conexão pronta
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}