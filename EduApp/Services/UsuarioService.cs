using EduApp.Models;
using MySqlConnector;
using System;
using System.Threading.Tasks;

namespace EduApp.Services
{
    public class UsuarioService : BaseDatabaseService
    {
        // ==========================================
        // C - CREATE (Cadastrar Usuário)
        // ==========================================
        // O método recebe a classe abstrata Usuario, então aceita tanto Aluno quanto Professor!
        public async Task<bool> CadastrarUsuarioAsync(Usuario novoUsuario)
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                // Query atualizada com todas as colunas que constam no seu banco de dados
                string query = @"INSERT INTO Usuario 
                                (usuNome, usuEmail, usuSenha, usuDataNascimento, usuEscola, usuPermissao) 
                                VALUES 
                                (@nome, @email, @senha, @dataNasc, @escola, @permissao)";

                using var command = new MySqlCommand(query, connection);

                // Extraindo os dados de dentro do objeto Model para proteger contra SQL Injection
                command.Parameters.AddWithValue("@nome", novoUsuario.Nome);
                command.Parameters.AddWithValue("@email", novoUsuario.Email);
                command.Parameters.AddWithValue("@senha", novoUsuario.Senha);

                // O MySQL precisa da data no formato "yyyy-MM-dd"
                command.Parameters.AddWithValue("@dataNasc", novoUsuario.DataNascimento.ToString("yyyy-MM-dd"));

                command.Parameters.AddWithValue("@escola", novoUsuario.Escola);

                // Convertendo o Enum TipoPermissao para o texto que vai salvar no banco (ex: "Aluno")
                command.Parameters.AddWithValue("@permissao", novoUsuario.Permissao.ToString());

                int linhasAfetadas = await command.ExecuteNonQueryAsync();

                // Retorna true se conseguiu salvar no banco
                return linhasAfetadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar: {ex.Message}");
                return false;
            }
        }

        // ==========================================
        // R - READ (Login do Usuario)
        // ==========================================
        public async Task<bool> ValidarLoginAsync(string email, string senha)
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                string query = "SELECT COUNT(1) FROM Usuario WHERE usuEmail = @email AND usuSenha = @senha";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@senha", senha);

                var resultado = await command.ExecuteScalarAsync();
                int quantidadeEncontrada = Convert.ToInt32(resultado);

                return quantidadeEncontrada > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao fazer login: {ex.Message}");
                return false;
            }
        }
    }
}