using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EduApp.Services;

namespace EduApp.ViewModels.Aluno
{
    // INotifyPropertyChanged para a tela saber quando os pontos mudarem
    internal class AlunoViewModel : INotifyPropertyChanged
    {
        private int _pontosAtual;
        public int PontosAtual
        {
            get => _pontosAtual;
            set
            {
                _pontosAtual = value;
                OnPropertyChanged(); // Avisa a tela (XAML) que o número mudou!
            }
        }

        public ICommand LogoutCommand { get; }

        public AlunoViewModel()
        {
            LogoutCommand = new Command(async () => await FazerLogout());
        }

        // NOVO MÉTOD: Carrega os pontos quando a tela abrir
        public async Task CarregarDadosDoHeroiAsync()
        {
            // 1. Pega o crachá do aluno logado
            int idAluno = Preferences.Default.Get("IdUsuario", 0);

            if (idAluno != 0)
            {
                // 2. Vai no banco de dados buscar o XP
                var usuarioService = new UsuarioService();
                PontosAtual = await usuarioService.BuscarPontosDoAlunoAsync(idAluno);
            }
        }

        private async Task FazerLogout()
        {
            bool confirmar = await Application.Current.MainPage.DisplayAlert("Sair", "Tem certeza que deseja sair da conta?", "Sim", "Não");

            if (confirmar)
            {
                Preferences.Default.Remove("IdUsuario");
                Application.Current.MainPage = new NavigationPage(new EduApp.Views.LoginPage());
            }
        }

        // Código obrigatório para a tela se atualizar sozinha
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}