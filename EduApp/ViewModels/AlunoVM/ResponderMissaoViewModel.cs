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
            // 1. Pega o ID dinamicamente do celular!
            // O '0' ali no final é o valor padrão caso o aplicativo não ache o ID por algum erro.
            int idAlunoLogado = Preferences.Default.Get("IdUsuario", 0);

            // 1.5 Validação de segurança (Se o ID for 0, o aluno não está logado direito)
            if (idAlunoLogado == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ops!", "Seu crachá de herói sumiu! Por favor, faça login novamente.", "OK");
                return; // Para a execução do código aqui
            }

            // 2. Salva no banco MySQL (agora usando o ID dinâmico real!)
            bool sucesso = await _atividadeService.EntregarAtividadeAsync(idAlunoLogado, MissaoAtual.Id);

            if (sucesso)
            {
                // 3. Comemoração
                await Application.Current.MainPage.DisplayAlert(
                    "Uau! 🎉",
                    "Sua missão foi enviada com sucesso! O professor vai avaliar em breve para te dar os seus XP.",
                    "INCRÍVEL!");

                // Fecha a página de resposta
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ops!", "Falha na comunicação com a base secreta. Tente novamente.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}