using Microsoft.Maui.ApplicationModel.Communication;
using MySqlConnector;
using System;
using System.Threading.Tasks;

namespace EduApp.Services
{
    public class AivenDatabaseService
    {
        // A string de conexão com o SslMode=Required
        private readonly string _stringConexao = "Server=mysql-eduapp-eduapp.g.aivencloud.com;Port=25689;Database=sistema_gamificado;Uid=avnadmin;Pwd=123456;SslMode=Required;";

        // Método para testar se está tudo funcionando
        public async Task<string> TestarConexaoAsync()
        {
            try
            {
                using var connection = new MySqlConnection(_stringConexao);
                await connection.OpenAsync();

                using var command = new MySqlCommand("SELECT VERSION();", connection);
                var versao = await command.ExecuteScalarAsync();

                // Retorna a mensagem de sucesso para quem chamou
                return $"Sucesso! Conectado ao MySQL versão: {versao}";
            }
            catch (Exception ex)
            {
                // Retorna a mensagem de erro
                return $"Erro ao conectar: {ex.Message}";
            }
        }
    }
}
