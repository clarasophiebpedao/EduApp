using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls; // Necessário para exibir alertas e navegar
using EduApp.Services;

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

        // 3. A Lógica de Validação
        private async Task RealizarLoginAsync()
        {
            // Valida se o usuário deixou algum campo em branco
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
            {
                await App.Current.MainPage.DisplayAlert("Atenção", "Por favor, preencha o e-mail e a senha.", "OK");
                return;
            }

            // Instancia o serviço de conexão com o banco
            var usuarioService = new UsuarioService();

            // Pede para o serviço validar as credenciais (que vai rodar o SELECT COUNT no banco)
            bool loginValido = await usuarioService.ValidarLoginAsync(Email, Senha);

            // Dá o feedback para o usuário
            if (loginValido)
            {
                await App.Current.MainPage.DisplayAlert("Bem-vindo!", "Login realizado com sucesso.", "Continuar");

                // NOTA PARA O TCC: Quando vocês criarem a tela principal do app (Home),
                // é aqui que vocês vão colocar o código para redirecionar o aluno.
                // Exemplo: await App.Current.MainPage.Navigation.PushAsync(new HomeAppPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Acesso Negado", "E-mail ou senha incorretos. Tente novamente.", "OK");
            }
        }
    }
}