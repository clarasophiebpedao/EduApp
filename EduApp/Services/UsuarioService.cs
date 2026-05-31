using EduApp.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduApp.Services
{
    // Classe concreta temporária que herda de Usuario para listar os pendentes
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
        public async Task<(int Id, string Permissao)> ValidarLoginAsync(string email, string senha)
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                // 1. Mudamos o SELECT para pedir o usuID também
                string query = "SELECT usuID, usuPermissao FROM Usuario WHERE usuEmail = @email AND usuSenha = @senha";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@senha", senha);

                // 2. Mudamos para ExecuteReader para conseguir ler várias colunas
                using var leitor = await command.ExecuteReaderAsync();

                // 3. Se ele conseguir ler uma linha (ou seja, login deu certo)
                if (await leitor.ReadAsync())
                {
                    int idEncontrado = Convert.ToInt32(leitor["usuID"]);
                    string permissaoEncontrada = leitor["usuPermissao"].ToString();

                    // Devolve as duas informações juntas
                    return (idEncontrado, permissaoEncontrada);
                }

                // Se não achou usuário ou a senha tá errada
                return (0, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao fazer login: {ex.Message}");
                return (0, null);
            }
        }

        // ==========================================
        // MÉTODOS DO ADMINISTRADOR (CORRIGIDOS)
        // ==========================================

        public async Task<List<Usuario>> BuscarUsuariosPendentesAsync()
        {
            var lista = new List<Usuario>();

            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                // CORREÇÃO: Mudado de 'idUsuario' para 'usuID' para bater com o seu banco
                string query = "SELECT usuID, usuNome, usuEmail FROM Usuario WHERE usuPermissao = 'Pendente'";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new UsuarioPendente
                    {
                        // CORREÇÃO: Puxando o valor correto da coluna 'usuID'
                        Id = Convert.ToInt32(reader["usuID"]),
                        Nome = reader["usuNome"].ToString(),
                        Email = reader["usuEmail"].ToString(),
                        Senha = string.Empty,
                        DataNascimento = DateOnly.MinValue,
                        Escola = string.Empty,
                        Permissao = default
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

                // 1. Peça base: Sempre atualiza o perfil na tabela principal
                string query = "UPDATE Usuario SET usuPermissao = @permissao WHERE usuID = @id;";

                // 2. Peça dinâmica: Cria o registro na tabela específica correspondente
                if (novoPerfil == "Aluno")
                {
                    // O INSERT IGNORE é um escudo: se o aluno já estiver na tabela por algum motivo, ele não trava o app.
                    query += " INSERT IGNORE INTO Aluno (usuID, aluPontos) VALUES (@id, 0);";
                }
                else if (novoPerfil == "Professor")
                {
                    // Cria o registro na tabela Professor
                    query += " INSERT IGNORE INTO Professor (usuID) VALUES (@id);";
                }

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@permissao", novoPerfil);
                command.Parameters.AddWithValue("@id", id);

                int linhasAfetadas = await command.ExecuteNonQueryAsync();

                // Retorna verdadeiro se o comando funcionou (update ou insert)
                return linhasAfetadas > 0;
            }
            catch (Exception ex)
            {
                // Debug para facilitar a visualização de erros no Visual Studio
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar perfil: {ex.Message}");
                return false;
            }
        }
    }
}