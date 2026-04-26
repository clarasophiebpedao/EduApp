public abstract class Usuario
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Senha { get; set; }
    public required DateOnly DataNascimento { get; set; }
    public required TipoPermissao Permissao { get; set; } //0 --> ADMINISTRADOR || 1 --> PROFESSOR || 2 --> Aluno
    public required string Escola { get; set; }
}

public enum TipoPermissao
{
    Administrador = 0,
    Professor = 1,
    Aluno = 2
}