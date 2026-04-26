namespace EduApp.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public required string Nome { get; set; } //Nome da turma, exemplo 8º C
        public required int ProfessorId { get; set; } //Chave estrangeira para o banco de dados
        public Professor? ProfessorResponsavel { get; set; } //Propriedade de Navegação
        public List<Aluno> Alunos { get; set; } = new List<Aluno>(); //Propriedade de Navegação
    }
}
