using EduApp.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduApp.Services
{
    // CÓDIGO DA VITÓRIA: Criamos uma classe concreta temporária que herda de Usuario
    // para podermos listar as pessoas que ainda não têm perfil definido.
    public class UsuarioPendente : Usuario { }

    public class UsuarioService : BaseDatabaseService
    {
        // ==========================================
        // C - CREATE (Cadastro de Usuário)
        // ==========================================
        public async Task<bool> CadastrarUsuarioAsync(Usuario novoUsuario)
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                string query = @"INSERT INTO Usuario 
                                (usuNome, usuEmail, usuSenha, usuDataNascimento, usuEscola, usuPermissao) 
                                VALUES 
                                (@nome, @email, @senha, @dataNasc, @escola, @permissao)";

                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nome", novoUsuario.Nome);
                command.Parameters.AddWithValue("@email", novoUsuario.Email);
                command.Parameters.AddWithValue("@senha", novoUsuario.Senha);
                command.Parameters.AddWithValue("@dataNasc", novoUsuario.DataNascimento.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@escola", novoUsuario.Escola);
                command.Parameters.AddWithValue("@permissao", novoUsuario.Permissao.ToString());

                int linhasAfetadas = await command.ExecuteNonQueryAsync();
                return linhasAfetadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar: {ex.Message}");
                return false;
            }
        }

        // ==========================================
        // R - READ (Login do Usuário)
        // ==========================================
        public async Task<string> ValidarLoginAsync(string email, string senha)
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                string query = "SELECT usuPermissao FROM Usuario WHERE usuEmail = @email AND usuSenha = @senha";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@senha", senha);

                var resultado = await command.ExecuteScalarAsync();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return null;
                }

                return resultado.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao fazer login: {ex.Message}");
                return null;
            }
        }

        // ==========================================
        // MÉTODOS DO ADMINISTRADOR
        // ==========================================

        public async Task<List<Usuario>> BuscarUsuariosPendentesAsync()
        {
            var lista = new List<Usuario>();

            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                string query = "SELECT idUsuario, usuNome, usuEmail FROM Usuario WHERE usuPermissao = 'Pendente'";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {

                    lista.Add(new UsuarioPendente
                    {
                        Id = Convert.ToInt32(reader["idUsuario"]),
                        Nome = reader["usuNome"].ToString(),
                        Email = reader["usuEmail"].ToString(),
                        Senha = string.Empty,
                        DataNascimento = DateOnly.MinValue,
                        Escola = string.Empty,
                        Permissao = default // O 'default' resolve o preenchimento automático de Enums
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuários pendentes: {ex.Message}");
            }

            return lista;
        }
        public async Task<bool> AtualizarPerfilUsuarioAsync(int id, string novoPerfil)
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                string query = "UPDATE Usuario SET usuPermissao = @permissao WHERE idUsuario = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@permissao", novoPerfil);
                command.Parameters.AddWithValue("@id", id);

                int linhasAfetadas = await command.ExecuteNonQueryAsync();
                return linhasAfetadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atribuir perfil: {ex.Message}");
                return false;
            }
        }
    }
}