using MySqlConnector;
using EduApp.Services;
using EduApp.Models;

namespace EduApp.Services
{
    public class AtividadeService : BaseDatabaseService
    {
        public async Task<bool> InserirAtividadeAsync(Atividade novaAtividade)
        {
            try
            {
                using var conexao = GetConnection();

                await conexao.OpenAsync();

                string sql = "INSERT INTO Atividade (Titulo, Descricao, Pontos) VALUES (@titulo, @descricao, @pontos)";

                using var comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@titulo", novaAtividade.Titulo);
                comando.Parameters.AddWithValue("@descricao", novaAtividade.Descricao);
                comando.Parameters.AddWithValue("@pontos", novaAtividade.Pontos);

                int linhasAfetadas = await comando.ExecuteNonQueryAsync();

                return linhasAfetadas > 0;
            }
            catch (Exception erro)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao inserir: " + erro.Message);
                return false;
            }
        }
        public async Task<List<Atividade>> BuscarAtividadesAsync()
        {
            var listaDeAtividades = new List<Atividade>();

            try
            {
                using var conexao = GetConnection();
                await conexao.OpenAsync();

 
                string sql = "SELECT * FROM Atividade";
                using var comando = new MySqlCommand(sql, conexao);

                using var leitor = await comando.ExecuteReaderAsync();


                while (await leitor.ReadAsync())
                {
                    var atividadeEncontrada = new Atividade
                    {
                        Titulo = leitor["Titulo"].ToString(),
                        Descricao = leitor["Descricao"].ToString(),
                        Pontos = Convert.ToInt32(leitor["Pontos"])
                    };

                    listaDeAtividades.Add(atividadeEncontrada);
                }
            }
            catch (Exception erro)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao buscar atividades: " + erro.Message);
            }

            return listaDeAtividades;
        }
        public async Task<List<HistoricoAtividade>> BuscarEntregasPendentesAsync()
        {
            var lista = new List<HistoricoAtividade>();

            try
            {
                using var conexao = GetConnection();
                await conexao.OpenAsync();

                string sql = @"SELECT h.Id AS IdHistorico, h.idAluno, a.Nome AS AlunoNome, act.Titulo AS AtividadeTitulo, act.Pontos 
                       FROM Historico_Atividade h
                       INNER JOIN Aluno a ON h.idAluno = a.Id
                       INNER JOIN Atividade act ON h.idAtividade = act.Id
                       WHERE h.Status = 'Pendente'";

                using var comando = new MySqlCommand(sql, conexao);
                using var leitor = await comando.ExecuteReaderAsync();

                while (await leitor.ReadAsync())
                {
                    var entrega = new HistoricoAtividade
                    {
                        IdHistorico = Convert.ToInt32(leitor["IdHistorico"]),
                        IdAluno = Convert.ToInt32(leitor["idAluno"]),
                        AlunoNome = leitor["AlunoNome"].ToString(),
                        AtividadeTitulo = leitor["AtividadeTitulo"].ToString(),
                        Pontos = Convert.ToInt32(leitor["Pontos"])
                    };

                    lista.Add(entrega);
                }
            }
            catch (Exception erro)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao buscar pendentes: " + erro.Message);
            }

            return lista;
        }
        public async Task<bool> EntregarAtividadeAsync(int idAluno, int idAtividade)
        {
            try
            {
                using var conexao = GetConnection();
                await conexao.OpenAsync();

                string sql = "INSERT INTO Historico_Atividade (idAluno, idAtividade, status) VALUES (@idAluno, @idAtividade, 'Pendente')";

                using var comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@idAluno", idAluno);
                comando.Parameters.AddWithValue("@idAtividade", idAtividade);

                int linhasAfetadas = await comando.ExecuteNonQueryAsync();
                return linhasAfetadas > 0;
            }
            catch (Exception erro)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao entregar atividade: " + erro.Message);
                return false;
            }
        }
        public async Task<bool> AprovarAtividadeAsync(int idHistorico, int idAluno, int pontosDaAtividade)
        {
            try
            {
                using var conexao = GetConnection();
                await conexao.OpenAsync();

                string sqlHistorico = "UPDATE Historico_Atividade SET Status = 'Concluída' WHERE Id = @idHistorico";
                using var comando1 = new MySqlCommand(sqlHistorico, conexao);
                comando1.Parameters.AddWithValue("@idHistorico", idHistorico);
                await comando1.ExecuteNonQueryAsync();

                string sqlAluno = "UPDATE Aluno SET Pontos = Pontos + @pontos WHERE Id = @idAluno";
                using var comando2 = new MySqlCommand(sqlAluno, conexao);
                comando2.Parameters.AddWithValue("@pontos", pontosDaAtividade);
                comando2.Parameters.AddWithValue("@idAluno", idAluno);

                int linhasAfetadas = await comando2.ExecuteNonQueryAsync();

                return linhasAfetadas > 0;
            }
            catch (Exception erro)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao aprovar atividade: " + erro.Message);
                return false;
            }
        }
    }
}