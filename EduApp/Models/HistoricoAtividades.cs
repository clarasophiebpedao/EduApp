namespace EduApp.Models
{
    public class HistoricoAtividade
    {
        public int IdHistorico { get; set; }
        public int IdAluno { get; set; }

        public string AlunoNome { get; set; }
        public string AtividadeTitulo { get; set; }
        public int Pontos { get; set; }
    }
}