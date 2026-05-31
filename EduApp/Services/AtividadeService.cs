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

                string sql = "INSERT INTO Atividade (atiTitulo, atiDescricao, atiPontos) VALUES (@titulo, @descricao, @pontos)";

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
                        Titulo = leitor["atiTitulo"].ToString(),
                        Descricao = leitor["atiDescricao"].ToString(),
                        Pontos = Convert.ToInt32(leitor["atiPontos"])
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

                string sql = @"SELECT h.histID AS IdHistorico, h.usuID_Aluno AS idAluno, u.usuNome AS AlunoNome, act.atiTitulo AS AtividadeTitulo, act.atiPontos AS Pontos 
                    FROM HistoricoAtividade h
                    INNER JOIN Aluno a ON h.usuID_Aluno = a.usuID
                    INNER JOIN Usuario u ON a.usuID = u.usuID
                    INNER JOIN Atividade act ON h.atiID = act.atiID
                    WHERE h.status = 'Pendente'";

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

                string sql = "INSERT INTO HistoricoAtividade (usuID_Aluno, atiID, status) VALUES (@idAluno, @idAtividade, 'Pendente')";

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

                string sqlHistorico = "UPDATE HistoricoAtividade SET status = 'Concluída' WHERE histID = @idHistorico";
                using var comando1 = new MySqlCommand(sqlHistorico, conexao);
                comando1.Parameters.AddWithValue("@idHistorico", idHistorico);
                await comando1.ExecuteNonQueryAsync();

                string sqlAluno = "UPDATE Aluno SET aluPontos = aluPontos + @pontos WHERE usuID = @idAluno";
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