using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EduApp.Services;
using EduApp.Views;
using EduApp.Views.Aluno;

namespace EduApp.ViewModels.Geral
{
    public class Login
    {
        // 1. Propriedades ligadas às caixas de texto do XAML (Bindings)
        public string Email { get; set; }
        public string Senha { get; set; }

        // 2. O Comando que o botão "Entrar" dispara
        public ICommand LoginCommand { get; }

        // Construtor
        public Login()
        {
            LoginCommand = new Command(async () => await RealizarLoginAsync());
        }

        // 3. Lógica de Validação e Redirecionamento Blindada por Perfil
        private async Task RealizarLoginAsync()
        {
            // Valida se o usuário deixou algum campo em branco
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Por favor, preencha o e-mail e a senha.", "OK");
                return;
            }

            var usuarioService = new UsuarioService();

            // CORREÇÃO 1: Puxa o resultado completo (Id e Permissao) do método atualizado
            var resultado = await usuarioService.ValidarLoginAsync(Email, Senha);

            // Se a permissão não for nula, o login está correto
            if (resultado.Permissao != null)
            {
                await Application.Current.MainPage.DisplayAlert("Bem-vindo!", "Login realizado com sucesso.", "Continuar");

                // Remove espaços em branco acidentais
                string permissaoLimpa = resultado.Permissao.Trim();

                // Compara os perfis ignorando maiúsculas/minúsculas
                if (permissaoLimpa.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    permissaoLimpa.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    // CORREÇÃO 2: Salva o ID usando a chave "IdUsuario" que programamos na tela de missões
                    Preferences.Default.Set("IdUsuario", resultado.Id);
                    Application.Current.MainPage = new NavigationPage(new PainelAdminPage());
                }
                else if (permissaoLimpa.Equals("Aluno", StringComparison.OrdinalIgnoreCase))
                {
                    // CORREÇÃO 2: Salva o ID usando o resultado vindo do banco
                    Preferences.Default.Set("IdUsuario", resultado.Id);
                    Application.Current.MainPage = new NavigationPage(new AlunoView());
                }
                else if (permissaoLimpa.Equals("Professor", StringComparison.OrdinalIgnoreCase))
                {
                    // CORREÇÃO 2: Salva o ID usando o resultado vindo do banco
                    Preferences.Default.Set("IdUsuario", resultado.Id);
                    Application.Current.MainPage = new NavigationPage(new ProfessorView());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Sua conta está aguardando a aprovação do administrador.", "OK");
                }
            }
            else
            {
                // Se a resposta for nula, o e-mail ou a senha estão incorretos
                await Application.Current.MainPage.DisplayAlert("Acesso Negado", "E-mail ou senha incorretos. Tente novamente.", "OK");
            }
        }
    }
}