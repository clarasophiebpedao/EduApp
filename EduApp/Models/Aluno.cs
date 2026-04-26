namespace EduApp.Models
{
    public class Aluno : Usuario
    {
        public int Pontos { get; set; } = 0;
        public required int TurmaId { get; set; } //Chave Estrangeira para turma
        public Turma? TurmaAtual { get; set; } //Propriedade de Navegação para turma

    }
}
