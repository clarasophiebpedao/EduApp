using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EduApp.ViewModels.Professor
{
    public class ProfessorViewModel
    {

        public ICommand LogoutCommand { get; }

        public ProfessorViewModel()
        {
            // Instancia o comando no construtor
            LogoutCommand = new Command(async () => await FazerLogout());
        }

        private async Task FazerLogout()
        {
            bool confirmar = await Application.Current.MainPage.DisplayAlert("Sair", "Tem certeza que deseja sair da conta?", "Sim", "Não");

            if (confirmar)
            {
                // Limpe as variáveis de sessão aqui (ex: zerar o ID do usuário logado)
                Preferences.Default.Remove("IdUsuario");
                // Troca a tela principal inteira de volta para o Login (Substitua "GeralV" pela pasta correta se necessário)
                Application.Current.MainPage = new NavigationPage(new EduApp.Views.LoginPage());
            }
        }

    }
}
