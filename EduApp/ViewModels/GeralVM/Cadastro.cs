using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls; // Necessário para os comandos do MAUI (DisplayAlert, Navigation)
using EduApp.Services;
using EduApp.Models;

namespace EduApp.ViewModels.Geral
{
    public class Cadastro
    {
        // 1. Propriedades que estão conectadas às caixas de texto do XAML (Bindings)
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }

        // 2. O Comando que o botão "Criar Conta" dispara
        public ICommand CadastrarCommand { get; }

        // Construtor
        public Cadastro()
        {
            CadastrarCommand = new Command(async () => await RealizarCadastroAsync());
        }

        // 3. A Lógica de Validação e Cadastro
        private async Task RealizarCadastroAsync()
        {
            // Valida se deixou algum campo em branco
            if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Por favor, preencha todos os campos da tela.", "Entendido");
                return;
            }

            // Valida se as senhas são iguais
            if (Senha != ConfirmaSenha)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "As senhas não coincidem!", "Tentar novamente");
                return;
            }

            // =================================================================
            // EMPACOTANDO NO MODEL
            // Aqui criamos o Aluno e preenchemos os dados obrigatórios (required)
            // =================================================================
            EduApp.Models.Aluno alunoParaSalvar = new EduApp.Models.Aluno
            {
                Nome = Nome,
                Email = Email,
                Senha = Senha,

                // Como não temos esses campos na tela ainda, mandamos valores fixos
                // para o C# não dar erro e conseguirmos salvar no banco
                DataNascimento = new DateOnly(2010, 1, 1),
                Escola = "Nome da Escola Base",
                Permissao = TipoPermissao.Aluno,
                TurmaId = 1 // Simulando a turma de ID 1
            };

            // Instancia o Serviço
            var usuarioService = new UsuarioService();

            // Chama o banco passando o objeto Aluno
            bool sucesso = await usuarioService.CadastrarUsuarioAsync(alunoParaSalvar);

            // Resposta para o usuário
            if (sucesso)
            {
                await Application.Current.MainPage.DisplayAlert("Sucesso!", $"Aluno(a) {Nome} cadastrado(a) com sucesso!", "OK");

                // Volta para a tela de login
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Falha", "Não foi possível cadastrar no banco. Verifique sua conexão.", "OK");
            }
        }
    }
}