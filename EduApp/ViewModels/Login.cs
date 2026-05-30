using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EduApp.Services;
using EduApp.Views;

namespace EduApp.ViewModels
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
                await App.Current.MainPage.DisplayAlert("Atenção", "Por favor, preencha o e-mail e a senha.", "OK");
                return;
            }

            var usuarioService = new UsuarioService();

            // Puxa a string direta da coluna usuPermissao do MySQL
            string permissao = await usuarioService.ValidarLoginAsync(Email, Senha);

            // Se o banco retornar algo (não nulo e não vazio), o login está correto
            if (!string.IsNullOrEmpty(permissao))
            {
                await App.Current.MainPage.DisplayAlert("Bem-vindo!", "Login realizado com sucesso.", "Continuar");

                // Remove espaços em branco acidentais que possam vir do banco de dados (ex: "Admin ")
                string permissaoLimpa = permissao.Trim();

                // Compara os perfis ignorando maiúsculas/minúsculas (StringComparison.OrdinalIgnoreCase)
                if (permissaoLimpa.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    permissaoLimpa.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.MainPage = new NavigationPage(new PainelAdminPage());
                }
                else if (permissaoLimpa.Equals("Aluno", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.MainPage = new NavigationPage(new ListaAtividadesPage());
                }
                else if (permissaoLimpa.Equals("Professor", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.MainPage = new NavigationPage(new CriarAtividadePage());
                }
                else
                {
                    // Caso o texto seja 'Pendente' ou qualquer outra coisa que não seja os perfis acima
                    await App.Current.MainPage.DisplayAlert("Aviso", "Sua conta está aguardando a aprovação do administrador.", "OK");
                }
            }
            else
            {
                // Se a resposta do banco for nula, o e-mail ou a senha estão incorretos
                await App.Current.MainPage.DisplayAlert("Acesso Negado", "E-mail ou senha incorretos. Tente novamente.", "OK");
            }
        }
    }
}