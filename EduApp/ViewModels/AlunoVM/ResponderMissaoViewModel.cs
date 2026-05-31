using System.ComponentModel;
using System.Windows.Input;
using EduApp.Models;
using EduApp.Services;

namespace EduApp.ViewModels.Aluno
{
    public class ResponderMissaoViewModel : INotifyPropertyChanged
    {
        private readonly AtividadeService _atividadeService;

        // A missão que o aluno está visualizando agora
        public Atividade MissaoAtual { get; set; }

        // O texto que a criança vai digitar (pronto para ser salvo no futuro)
        public string TextoResposta { get; set; }

        public ICommand EnviarRespostaCommand { get; }

        public ResponderMissaoViewModel(Atividade missaoEscolhida)
        {
            _atividadeService = new AtividadeService();
            MissaoAtual = missaoEscolhida;

            EnviarRespostaCommand = new Command(async () => await EnviarMissao());
        }

        private async Task EnviarMissao()
        {
            // 1. Pegar o ID do Aluno logado (Ajuste para a sua lógica de login real)
            // int idAlunoLogado = Preferences.Get("UsuarioId", 0);
            int idAlunoLogado = 1; // Fixo apenas para teste

            // 2. Salva no banco MySQL
            bool sucesso = await _atividadeService.EntregarAtividadeAsync(idAlunoLogado, MissaoAtual.Id);

            if (sucesso)
            {
                // 3. Comemoração e volta para a lista
                await Application.Current.MainPage.DisplayAlert(
                    "Uau! 🎉",
                    "Sua missão foi enviada com sucesso! O professor vai avaliar em breve para te dar os seus XP.",
                    "INCRÍVEL!");

                // Fecha a página de resposta e volta para a lista de missões
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ops!", "Falha na comunicação com a base secreta (banco de dados). Tente novamente.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}